using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Security.Cryptography;
using System.Threading;
using System.Timers;

namespace TSP
{
    class ProblemAndSolver
    {
        public static Boolean LiveUpdating = true;
        private static double BBTime = 59000;
        private static double twoChangeInterval = 1000;

        public class TSPSolution
        {
            /// <summary>
            /// we use the representation [cityB,cityA,cityC] 
            /// to mean that cityB is the first city in the solution, cityA is the second, cityC is the third 
            /// and the edge from cityC to cityB is the final edge in the path.  
            /// you are, of course, free to use a different representation if it would be more convenient or efficient 
            /// for your node data structure and search algorithm. 
            /// </summary>
            public ArrayList 
                Route;

            public TSPSolution(ArrayList iroute)
            {
                Route = new ArrayList(iroute);
            }


            /// <summary>
            ///  compute the cost of the current route.  does not check that the route is complete, btw.
            /// assumes that the route passes from the last city back to the first city. 
            /// </summary>
            /// <returns></returns>
            public double costOfRoute()
            {
                // go through each edge in the route and add up the cost. 
                int x;
                City here; 
                double cost = 0D;
                
                for (x = 0; x < Route.Count-1; x++)
                {
                    here = Route[x] as City;
                    cost += here.costToGetTo(Route[x + 1] as City);
                }
                // go from the last city to the first. 
                here = Route[Route.Count - 1] as City;
                cost += here.costToGetTo(Route[0] as City);
                return cost; 
            }

            public BBState toBBState()
            {

                double[,] costMatrix = new double[Route.Count, Route.Count];
                for (int x = 0; x < Route.Count; ++x)
                {
                    for (int y = 0; y < Route.Count; ++y)
                    {
                        costMatrix[x, y] = x == y ? double.PositiveInfinity : ( (City)Route[x] ).costToGetTo( (City)Route[y] );
                    }
                }
                BBState newState = new BBState(costMatrix);
                newState.bound = this.costOfRoute();
                newState.depth = Route.Count;
                newState.excludeCount = 0;
                newState.numPoints = Route.Count;

                return newState;
            }
        }
        
        #region private members
        private const int DEFAULT_SIZE = 25;
        
        private const int CITY_ICON_SIZE = 5;

        /// <summary>
        /// the cities in the current problem.
        /// </summary>
        private City[] Cities;
        /// <summary>
        /// a route through the current problem, useful as a temporary variable. 
        /// </summary>
        private ArrayList Route;
        /// <summary>
        /// best solution so far. 
        /// </summary>
        private TSPSolution bssf; 

        /// <summary>
        /// how to color various things. 
        /// </summary>
        private Brush cityBrushStartStyle;
        private Brush cityBrushStyle;
        private Pen routePenStyle;

        static Random rng = new Random();


        /// <summary>
        /// keep track of the seed value so that the same sequence of problems can be 
        /// regenerated next time the generator is run. 
        /// </summary>
        private int _seed;
        /// <summary>
        /// number of cities to include in a problem. 
        /// </summary>
        private int _size;

        /// <summary>
        /// random number generator. 
        /// </summary>
        private Random rnd;
        #endregion

        #region public members.
        public int Size
        {
            get { return _size; }
        }

        public int Seed
        {
            get { return _seed; }
        }
        #endregion

        public const int DEFAULT_SEED = -1;

        #region Constructors
        public ProblemAndSolver()
        {
            initialize(DEFAULT_SEED, DEFAULT_SIZE);
        }

        public ProblemAndSolver(int seed)
        {
            initialize(seed, DEFAULT_SIZE);
        }

        public ProblemAndSolver(int seed, int size)
        {
            initialize(seed, size);
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// reset the problem instance. 
        /// </summary>
        private void resetData()
        {
            Cities = new City[_size];
            Route = new ArrayList(_size);
            bssf = null; 

            for (int i = 0; i < _size; i++)
                Cities[i] = new City(rnd.NextDouble(), rnd.NextDouble());

            cityBrushStyle = new SolidBrush(Color.Black);
            cityBrushStartStyle = new SolidBrush(Color.Red);
            routePenStyle = new Pen(Color.LightGray,1);
            routePenStyle.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
        }

        private void initialize(int seed, int size)
        {
            this._seed = seed;
            this._size = size;
            if (seed != DEFAULT_SEED)
                this.rnd = new Random(seed);
            else
                this.rnd = new Random();
            this.resetData();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// make a new problem with the given size.
        /// </summary>
        /// <param name="size">number of cities</param>
        public void GenerateProblem(int size)
        {
            this._size = size;
            resetData(); 
        }

        /// <summary>
        /// return a copy of the cities in this problem. 
        /// </summary>
        /// <returns>array of cities</returns>
        public City[] GetCities()
        {
            City[] retCities = new City[Cities.Length];
            Array.Copy(Cities, retCities, Cities.Length);
            return retCities;
        }

        /// <summary>
        /// draw the cities in the problem.  if the bssf member is defined, then
        /// draw that too. 
        /// </summary>
        /// <param name="g">where to draw the stuff</param>
        public void Draw(Graphics g)
        {
            float width  = g.VisibleClipBounds.Width-45F;
            float height = g.VisibleClipBounds.Height-15F;
            Font labelFont = new Font("Arial", 10);

            g.DrawString("n(c) means this node is the nth node in the current solution and incurs cost c to travel to the next node.", labelFont, cityBrushStartStyle, new PointF(0F, 0F)); 

            // Draw lines
            if (bssf != null)
            {
                // make a list of points. 
                Point[] ps = new Point[bssf.Route.Count];
                int index = 0;
                foreach (City c in bssf.Route)
                {
                    if (index < bssf.Route.Count -1)
                        g.DrawString(" " + index +"("+c.costToGetTo(bssf.Route[index+1]as City)+")", labelFont, cityBrushStartStyle, new PointF((float)c.X * width + 3F, (float)c.Y * height));
                    else 
                        g.DrawString(" " + index +"("+c.costToGetTo(bssf.Route[0]as City)+")", labelFont, cityBrushStartStyle, new PointF((float)c.X * width + 3F, (float)c.Y * height));
                    ps[index++] = new Point((int)(c.X * width) + CITY_ICON_SIZE / 2, (int)(c.Y * height) + CITY_ICON_SIZE / 2);
                }

                if (ps.Length > 0)
                {
                    g.DrawLines(routePenStyle, ps);
                    g.FillEllipse(cityBrushStartStyle, (float)Cities[0].X * width - 1, (float)Cities[0].Y * height - 1, CITY_ICON_SIZE + 2, CITY_ICON_SIZE + 2);
                }

                // draw the last line. 
                g.DrawLine(routePenStyle, ps[0], ps[ps.Length - 1]);
            }

            // Draw city dots
            foreach (City c in Cities)
            {
                g.FillEllipse(cityBrushStyle, (float)c.X * width, (float)c.Y * height, CITY_ICON_SIZE, CITY_ICON_SIZE);
            }

        }

        /// <summary>
        ///  return the cost of the best solution so far. 
        /// </summary>
        /// <returns></returns>
        public double costOfBssf ()
        {
            if (bssf != null)
                return (bssf.costOfRoute());
            else
                return -1D; 
        }

        private TSPSolution diagonal()
        {
            int x;
            ArrayList Route = new ArrayList();
            for (x = 0; x < Cities.Length; x++)
            {
                Route.Add(Cities[Cities.Length - x - 1]);
            }
            TSPSolution s = new TSPSolution(Route);
            return s;
        }

        public static void Shuffle(IList list)
        { 
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                Object value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static void Shuffle2(IList list)  
        {  
            var provider = new RNGCryptoServiceProvider();  
            int n = list.Count;  
            while (n > 1)  
            {  
                var box = new byte[1];  
                do provider.GetBytes(box);  
                while (!(box[0] < n * (Byte.MaxValue / n)));  
                var k = (box[0] % n);  
                n--;  
                var value = list[k];  
                list[k] = list[n];  
                list[n] = value;  
            }  
        }

        private TSPSolution random()
        {
            int x;
            ArrayList Route = new ArrayList();
            for (x = 0; x < Cities.Length; x++)
            {
                Route.Add(Cities[Cities.Length - x - 1]);
            }
            Shuffle(Route);
            TSPSolution s = new TSPSolution(Route);
            //Console.WriteLine(s.costOfRoute() + " shuffle");
            return s;
        }

        private TSPSolution TwoChangeOnInterval(BBState state, double interval)
        {
            bool done = false;

            System.Timers.Timer timer = new System.Timers.Timer(interval);
            timer.Elapsed += new ElapsedEventHandler(delegate(Object source, ElapsedEventArgs e)
            {
                done = true;
                timer.Enabled = false;
            });

            timer.Enabled = true;
            TSPSolution s = ThreeChange(state);
            Console.WriteLine(s.costOfRoute());
            while (!done)
            {
                s = TwoChange(ref s);
                Console.WriteLine(s.costOfRoute());
            }
            return s;
        }

        /// <summary>s
        /// Runs TwoChange on a random route
        /// </summary>
        /// <returns>The best solution for choose 2</returns>
        private TSPSolution TwoChange(BBState state)
        {
            ArrayList Route = state.getRoute(GetCities());
            TSPSolution s = new TSPSolution(Route);
            return this.TwoChange(ref s);
        }

        private TSPSolution TwoChange(ref TSPSolution s)
        {
            double cost = s.costOfRoute();
            ArrayList route = s.Route;

            for (int first = 0; first < route.Count - 1; ++first)
            {
                for (int last = first + 1; last < route.Count; ++last)
                {
                    City firstCity = route[first] as City;
                    City lastCity = route[last] as City;
                    route[first] = lastCity;
                    route[last] = firstCity;

                    double newCost = s.costOfRoute();
                    if (newCost < cost)
                    {
                        cost = newCost;
                    }
                    else
                    {
                        route[first] = firstCity;
                        route[last] = lastCity;
                    }
                }
            }
            Console.WriteLine("TwoChange: New Cost " + cost);
            return s;
        }

        private TSPSolution ThreeChange(BBState state)
        {
            ArrayList Route = state.getRoute(GetCities());
            TSPSolution s = new TSPSolution(Route);
            return this.ThreeChange(ref s);
        }

        private TSPSolution ThreeChange(ref TSPSolution s)
        {
            double cost = s.costOfRoute();
            ArrayList route = s.Route;

            for (int first = 0; first < route.Count - 2; ++first)
            {
                for (int middle = first + 1; middle < route.Count - 1; ++middle)
                {
                    for (int last = middle + 1; last < route.Count; ++last)
                    {
                        City firstCity = route[first] as City;
                        City middleCity = route[middle] as City;
                        City lastCity = route[last] as City;
                        City tempCity = null;

                        double newCost = 0;

                        // MIDDLE LAST
                        route[first] = firstCity;
                        route[middle] = lastCity;
                        route[last] = middleCity;
                        newCost = s.costOfRoute();
                        if (newCost < cost)
                        {
                            cost = newCost;
                            tempCity = middleCity;
                            middleCity = lastCity;
                            lastCity = tempCity;
                        }
                        else
                        {
                            route[first] = firstCity;
                            route[middle] = middleCity;
                            route[last] = lastCity;
                        }

                        // FIRST MIDDLE
                        route[first] = middleCity;
                        route[middle] = firstCity;
                        route[last] = lastCity;
                        newCost = s.costOfRoute();
                        if (newCost < cost)
                        {
                            cost = newCost;
                            tempCity = firstCity;
                            firstCity = middleCity;
                            middleCity = tempCity;
                        }
                        else
                        {
                            route[first] = firstCity;
                            route[middle] = middleCity;
                            route[last] = lastCity;
                        }

                        // FIRST LAST
                        route[first] = lastCity;
                        route[middle] = middleCity;
                        route[last] = firstCity;
                        newCost = s.costOfRoute();
                        if (newCost < cost)
                        {
                            cost = newCost;
                            tempCity = firstCity;
                            firstCity = lastCity;
                            lastCity = tempCity;
                        }
                        else
                        {
                            route[first] = firstCity;
                            route[middle] = middleCity;
                            route[last] = lastCity;
                        }

                        // SHIFT LEFT
                        route[first] = middleCity;
                        route[middle] = lastCity;
                        route[last] = firstCity;
                        newCost = s.costOfRoute();
                        if (newCost < cost)
                        {
                            cost = newCost;
                            tempCity = firstCity;
                            firstCity = middleCity;
                            middleCity = lastCity;
                            lastCity = tempCity;
                        }
                        else
                        {
                            route[first] = firstCity;
                            route[middle] = middleCity;
                            route[last] = lastCity;
                        }

                        // SHIFT RIGHT
                        route[first] = lastCity;
                        route[middle] = firstCity;
                        route[last] = middleCity;
                        newCost = s.costOfRoute();
                        if (newCost < cost)
                        {
                            cost = newCost;
                            tempCity = lastCity;
                            lastCity = middleCity;
                            middleCity = firstCity;
                            firstCity = tempCity;
                        }
                        else
                        {
                            route[first] = firstCity;
                            route[middle] = middleCity;
                            route[last] = lastCity;
                        }

                        
                    }
                }
            }

            Console.WriteLine("ThreeChange: New Cost " + cost);
            return s;
        }

        private ArrayList brute(ArrayList Route)
        {
            for (int x = 0; x < Cities.Length; x++)
            {
                Route.Add(Cities[Cities.Length - x - 1]);
                if (Route.Count < Cities.Length)
                    brute(Route);
                else
                {
                    double cost = new TSPSolution(Route).costOfRoute();
                    Console.WriteLine(cost);
                }
                Route.RemoveAt(Route.Count - 1);
            }
            
            return null;
        }

        private BBState CreateInitialState()
        {
            City[] cities = this.GetCities();
            double[,] costMatrix = new double[cities.Length, cities.Length];
            for (int x = 0; x < cities.Length; ++x)
            {
                for (int y = 0; y < cities.Length; ++y)
                {
                    costMatrix[x, y] = x == y ? double.PositiveInfinity : cities[x].costToGetTo(cities[y]);
                }
            }

            return new BBState(costMatrix);
        }

        delegate void UpdateAction();

        private void onTimedEvent(Object source, ElapsedEventArgs e)
        {
            BBWorker.timeAvailable = false;
            Console.WriteLine("-----------");
            Console.WriteLine("The Elapsed event was raised at {0}", e.SignalTime);
            UpdateAction done = showDone;
            Program.MainForm.Invoke(done);
        }

        public void showDone() {
            Program.MainForm.tbElapsedTime.Text = sw.Elapsed + " done";
        }

        public void newSolutionCallback(bool done)
       {
            //bssf = TwoChange(BBWorker.BSSF);
            ////BBWorker.BSSF = bssf.toBBState();
            //BBWorker.setBSSF(bssf.costOfRoute());
            if (LiveUpdating || done)
                updateHUD(done);
            if (done)
            {
                UpdateAction doneHandler = showDone;
                Program.MainForm.Invoke(doneHandler);
            }
        }

        public void updateHUD(bool done)
        {
            BBState BSSFState = BBWorker.BSSF;
            if (done)
            {
                bssf = TwoChangeOnInterval(BSSFState, twoChangeInterval);
                //bssf = ThreeChange(ref bssf);
            }
            else
            {
                bssf = new TSPSolution(BSSFState.getRoute(GetCities()));
            }
            if (bssf.Route.Count != Cities.Length)
            {
                throw new Exception();
            }
            UpdateAction action = updateHUDHandler;
            Program.MainForm.Invoke(action);
        }

        public void updateHUDHandler()
        {
            // update the cost of the tour.
            Program.MainForm.tbCostOfTour.Text = " " + bssf.costOfRoute();
            Program.MainForm.tbElapsedTime.Text = " " + sw.Elapsed;

            Program.MainForm.tbSearched.Text = "" + BBWorker.expansions;
            Program.MainForm.tbPruned.Text = "" + BBWorker.pruned;
            Program.MainForm.tbMaxAgenda.Text = "" + BBWorker.maxAgenda;
            Program.MainForm.tbRam.Text = "" + BBWorker.MaxRam / 1e6 + " MB";

            // do a refresh. 
            Program.MainForm.Invalidate();
        }

        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

        /// <summary>
        ///  solve the problem.  This is the entry point for the solver when the run button is clicked
        /// right now it just picks a simple solution. 
        /// </summary>
        public void solveProblem()
        {
            /*Route = brute(new ArrayList());
            TSPSolution s = new TSPSolution(Route);
            bssf = s;*/

            sw.Reset();
            sw.Start();

            double best = double.MaxValue;
            double worst = 0;

            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = BBTime;
            timer.AutoReset = false;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(this.onTimedEvent);
            timer.Enabled = true;

            Console.WriteLine("Starting Branch and Bound " + sw.ElapsedMilliseconds);

            BBState initialState = CreateInitialState();
            BBWorker.updateGUI = this.newSolutionCallback;
            BBWorker worker = new BBWorker(initialState, initialState.getCostMatrix(), Cities.Length);
            //best = double.MaxValue;
            BBWorker.setBSSF(best);
            Program.MainForm.tbInitial.Text = "" + best;
            Program.MainForm.tbBound.Text = "" + initialState.bound;

            //worker.run();
            Thread nThread = new Thread(new ThreadStart(worker.run));
            nThread.Start();
        }
        #endregion
    }
}
