using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TSP
{
    class BBWorker
    {
        C5.IntervalHeap<BBState> Agenda;
        double initial_bound;
        double BSSF_cost;
        BBState BSSF;
        int numPoints;
        bool timeAvailable;

        public BBWorker(BBState initial, int numPoints)
        {
            Agenda = new C5.IntervalHeap<BBState>();
            BSSF_cost = double.PositiveInfinity;

            initial_bound = initial.bound;
            this.numPoints = numPoints;
            Agenda.Add(initial);
            timeAvailable = true;
        }

        public void run()
        {
            while(!Agenda.IsEmpty && timeAvailable && BSSF_cost != initial_bound) {
                BBState u = Agenda.DeleteMin();

                int x,y;
                if ( u.chooseNextEdge(out x, out y) )
                {
                    BBState exclude = u;
                    BBState include = new BBState(u);

                    exclude.exclude(x, y);
                    include.include(x, y);

                    expand(exclude);
                    expand(include);
                }

                if (!timeAvailable)
                    break;
            }
        }

        public BBState GetBSSFState()
        {
            return BSSF;
        }

        public void setBSSF(double BSSF_cost)
        {
            this.BSSF_cost = BSSF_cost;
        }

        private void expand(BBState w) {
            if (w.bound < BSSF_cost) // See if bound is within cost
            {
                if(criterion(w)) { // If full solution
                    BSSF = w;
                    BSSF_cost = BSSF.bound;
                    Console.WriteLine("Found solution with cost " + BSSF.bound + " depth=" + BSSF.depth);
                }
                else { // Otherwise add to agenda
                    //Console.WriteLine(w.bound);
                    Agenda.Add(w);
                }
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

        
    }

    class BBState : IComparable
    {
        private static double lambda = .05;

        private double[,] cost;

        public double bound;

        public double depth;
        public double excludeCount;

        public int numPoints;


        public BBState(int numPoints)
            : this(new double[numPoints, numPoints])
        {
        }

        public BBState(double[,] cost)
        {
            this.cost = cost;
            this.numPoints = cost.GetLength(1);
        }

        /// Copy Constructor
        public BBState(BBState other)
        {
            this.cost = new double[other.numPoints, other.numPoints];
            Array.Copy(other.cost, this.cost, other.numPoints * other.numPoints);
            this.numPoints = other.numPoints;
            this.depth = other.depth;
            this.bound = other.bound;
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
        public bool chooseNextEdge(out int chosenX, out int chosenY)
        {
            for (int y = 0; y < numPoints; y++) {
 	            for (int x = (depth == (numPoints - 1)) ? 0 : 1; x < numPoints; x++) {
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
            for (int i = 0; i < numPoints; i++)
            {
                cost[i, y] = double.PositiveInfinity;
                cost[x, i] = double.PositiveInfinity;
            }
            cost[x, y] = double.NaN;
            cost[y, x] = double.PositiveInfinity;
            ++depth;
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
                            return false;

                        
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
