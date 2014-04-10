using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;

namespace TSP
{
    class BBWorker
    {
        public static int MaxWorkerCount = 4;
        public static int MinAgendaSplitSize = 50;

        public static int workerCount = 0;

        private static int MaxAgendaCount = 200000;

        public delegate void UpdateAction(bool done);
        public static UpdateAction updateGUI;

        C5.IntervalHeap<BBState> Agenda;
        static double initial_bound;
        double[,] trueCosts;
        int numPoints;

        static double BSSF_cost;
        public static BBState BSSF;
        public static bool timeAvailable;
        BBState initial;

        public static int invalidSolutionsCount;
        public static int failedCount;
        public static int expansions;
        public static int pruned;
        public static int prunedChild;
        public static int maxAgenda;

        public BBWorker(BBState initial, double[,] trueCosts, int numPoints)
        {
            invalidSolutionsCount = failedCount = expansions = pruned = prunedChild = maxAgenda = 0;
            workerCount = 0;

            workerCount++;
            this.initial = initial;

            Agenda = new C5.IntervalHeap<BBState>();
            BSSF_cost = double.PositiveInfinity;
            this.trueCosts = trueCosts;

            initial_bound = initial.bound;
            this.numPoints = numPoints;
            Agenda.Add(initial);
            timeAvailable = true;
        }

        public BBWorker(double[,] trueCosts, int numPoints)
        {
            workerCount++;

            this.trueCosts = trueCosts;
            this.numPoints = numPoints;
        }

        public void setAgenda(C5.IntervalHeap<BBState> Agenda)
        {
            this.Agenda = Agenda;
        }

        public void run()
        {
            while(!Agenda.IsEmpty && timeAvailable && BSSF_cost != initial_bound) {

                BBState u = Agenda.DeleteMin();
                //Console.WriteLine(u.bound);

                //Pruning
                if (u.bound > BSSF_cost)
                {
                    pruned++;
                    continue;
                }

                int x,y;
                if (u.chooseNextEdge(out x, out y))
                {
                    BBState exclude = u;
                    BBState include = new BBState(u);

                    exclude.exclude(x, y);
                    include.include(x, y);

                    expand(exclude);
                    expand(include);
                    //expansions++;
                    Interlocked.Increment(ref expansions);
                }
                else
                {
                    failedCount++;
                    if (failedCount % 100 == 0)
                        Console.WriteLine("failed " + failedCount);
                }

                splitCheck();

                if (!timeAvailable)
                    break;
            }

            Console.WriteLine(Agenda.Count + " on agenda when ending");

            workerCount--;

            if (workerCount == 0)
            {
                updateGUI(true);
            }
        }

        public BBState GetBSSFState()
        {
            return BSSF;
        }

        public void setBSSF(double BSSF_cost)
        {
            BBWorker.BSSF_cost = BSSF_cost;
        }

        private void expand(BBState w) {
            if (w.bound < BSSF_cost || BSSF == null) // See if bound is within cost
            {
                if (criterion(w))
                { // If full solution
                    BSSF = w;
                    BSSF_cost = BSSF.bound;
                    updateGUI(false);
                    Console.WriteLine("Found solution with cost " + BSSF.bound + " depth=" + BSSF.depth);
                }
                else if (w.depth < numPoints)
                { // Otherwise add to agenda
                    //Console.WriteLine(w.bound);
                    Agenda.Add(w);
                }
                else
                {
                    invalidSolutionsCount++;

                    //if (triedCount % 50 == 0)
                    //{
                    Console.WriteLine("Failed Solution " + invalidSolutionsCount);
                    //}
                }
            }
            else
            {
                prunedChild++;
            }
        }

        private bool criterion(BBState w)
        {
            if (w.depth != numPoints)
            {
                return false;
            }
            return w.validateCycle();
        }


        public void splitCheck()
        {
            if (Agenda.Count > maxAgenda)
            {
                maxAgenda = Agenda.Count;
            }

            //Console.WriteLine(Agenda.Count);
            if (workerCount < MaxWorkerCount && Agenda.Count > MinAgendaSplitSize)
            {
                Console.WriteLine("Splitting Agenda cost=" + BSSF_cost + " size=" + Agenda.Count);
                C5.IntervalHeap<BBState> a = new C5.IntervalHeap<BBState>();
                C5.IntervalHeap<BBState> b = new C5.IntervalHeap<BBState>();

                //Split agenda into two
                int counter = 0;
                foreach (BBState s in Agenda)
                {
                    if (counter++ % 2 == 0)
                    {
                        a.Add(s);
                    }
                    else
                    {
                        b.Add(s);
                    }
                }

                BBWorker child = new BBWorker(trueCosts, numPoints);
                child.setAgenda(b);
                setAgenda(a);

                Thread nThread = new Thread(new ThreadStart(child.run));
                nThread.Start();
            }

        }
    }

    class BBState : IComparable
    {
        private static double lambda = .0005;

        private double[,] cost;

        public double bound;

        public double depth;
        public double excludeCount;

        public int numPoints;

        List<Tuple<int, int>> segments;

        public BBState(int numPoints)
            : this(new double[numPoints, numPoints])
        {
        }

        public BBState(double[,] cost)
        {
            this.cost = cost;
            this.numPoints = cost.GetLength(1);
            this.segments = new List<Tuple<int, int>>();
        }

        /// Copy Constructor
        public BBState(BBState other)
        {
            this.cost = new double[other.numPoints, other.numPoints];
            Array.Copy(other.cost, this.cost, other.numPoints * other.numPoints);
            this.numPoints = other.numPoints;
            this.depth = other.depth;
            this.bound = other.bound;
            this.segments = other.segments.ToList(); //deep clone is not necessary since we don't modify the tuples
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public double getPriority() 
        {
            return lambda * bound + (1 - lambda) * (numPoints - depth);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            return (int)(getPriority() - ((BBState)obj).getPriority());
        }
    
        /// <summary>
        /// 
        /// </summary>
        /// <param name="chosenX"></param>
        /// <param name="chosenY"></param>
        public bool chooseNextEdgeOld(out int chosenX, out int chosenY)
        {
            for (int y = 0; y < numPoints; y++) {
 	            for (int x = (depth >= 1) ? 0 : 1; x < numPoints; x++) {
                    if (!double.IsNaN(cost[x, y]) && !double.IsPositiveInfinity(cost[x,y]) )
                    {
                        chosenX = x;
                        chosenY = y;
                        return true;
                    }
                }
            }
            chosenX = chosenY = 0;
            return false;
        }

        public bool chooseNextEdge(out int chosenX, out int chosenY)
        {
            double min = double.MaxValue;
            for (int y = 0; y < numPoints; y++)
                for (int x = (depth >= 1) ? 0 : 1; x < numPoints; x++)
                    if (cost[x, y] < min)
                    {
                        min = cost[x, y];
                    }

            for (int y = 0; y < numPoints; y++)
            {
                for (int x = (depth >= 1) ? 0 : 1; x < numPoints; x++)
                {
                    if (min == cost[x, y])
                    {
                        chosenX = x;
                        chosenY = y;
                        return true;
                    }
                }
            }
            chosenX = chosenY = 0;
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void exclude(int x, int y)
        {
            cost[x, y] = double.PositiveInfinity;
            excludeCount++;
            reduce();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void include(int x, int y)
        {
            if (Double.IsNaN(cost[x, y]) || Double.IsPositiveInfinity(cost[x, y]))
            {
                throw new Exception();
            }
            bound += cost[x, y];
            for (int i = 0; i < numPoints; i++)
            {
                cost[i, y] = double.PositiveInfinity;
                cost[x, i] = double.PositiveInfinity;
            }
            cost[x, y] = double.NaN;
            cost[y, x] = double.PositiveInfinity;


            ++depth;

            //prevent cycles unless this is one edge away
            if (depth < numPoints - 1)
            {
                int segmentX = x;
                int segmentY = y;
                for (int i = segments.Count - 1; i >= 0; i--)
                {
                    Tuple<int, int> edge = segments[i];
                    if (edge.Item2 == segmentX)
                    {
                        segments.RemoveAt(i);
                        segmentX = edge.Item1;
                    }
                    else if (edge.Item1 == segmentY)
                    {
                        segments.RemoveAt(i);
                        segmentY = edge.Item2;
                    }
                }
                segments.Add(new Tuple<int, int>(segmentX, segmentY));
                if (cost[segmentY, segmentX] != double.NaN)
                    cost[segmentY, segmentX] = double.PositiveInfinity;
            }

            reduce();
        }

        public ArrayList getRoute(City[] cities)
        {
            ArrayList cityList = null;
            if (depth == numPoints)
            {
                cityList = new ArrayList();
                int row = 0;
                for (int i = 0; i < numPoints; ++i)
                {
                    for (int x = 0; x < numPoints; ++x)
                    {
                        if ( Double.IsNaN(cost[x, row]) )
                        {
                            cityList.Add(cities[x]);
                            row = x;
                            break;
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("STAAAHP!!!");
            }

            return cityList;
        }

        public double[,] getCostMatrix()
        {
            return cost;
        }

        /// <summary>
        /// 
        /// </summary>
        private void reduce()
        {
            for (int x = 0; x < numPoints; x++) // Column reduction
            {
                double min = double.PositiveInfinity;
                for (int y = 0; y < numPoints; y++) // Find minimum in column
                {
                    if (cost[x, y] < min)
                    {
                        min = cost[x, y];
                    }
                }
                if (min > 0 && !double.IsPositiveInfinity(min))
                {
                    for (int y = 0; y < numPoints; y++)
                    {
                        cost[x, y] -= min;
                    }
                    bound += min;
                }
            }

            for (int y = 0; y < numPoints; y++) // Row reduction
            {
                double min = double.PositiveInfinity;
                for (int x = 0; x < numPoints; x++) // Find minimum in row
                {
                    if (cost[x, y] < min)
                    {
                        min = cost[x, y];
                    }
                }
                if (min > 0 && !double.IsPositiveInfinity(min) )
                {
                    for (int x = 0; x < numPoints; x++)
                    {
                        cost[x, y] -= min;
                    }
                    bound += min;
                }
            }
        }

        public bool validateCycle()
        {
            int row = 0;
            bool[] visited = new bool[numPoints];
            for (int i = 0; i < numPoints; ++i)
            {
                for (int x = 0; x < numPoints; ++x)
                {
                    if (Double.IsNaN(cost[x, row]))
                    {
                        if (visited[x])
                        {
                            Console.WriteLine(x + " cannot be used twice");
                            return false;
                        }

                        visited[x] = true;
                        row = x;
                        break;
                        
                    }
                }
            }

            return true;
        }
    }

}
