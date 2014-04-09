using System;
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
        int numPoints;
        bool timeAvailable;

        public BBWorker()
        {
            Agenda = new C5.IntervalHeap<BBState>();
            BSSF_cost = double.MaxValue;
            numPoints = 5;

            BBState initial = new BBState(numPoints);
            initial_bound = initial.bound;
            Agenda.Add(initial);
            timeAvailable = true;
        }

        public void run()
        {
            while(!Agenda.IsEmpty && timeAvailable && BSSF_cost != initial_bound) {
                BBState u = Agenda.DeleteMin();

                int x,y;
                u.chooseNextEdge(out x, out y);

                BBState exclude = u;
                BBState include = new BBState(u);

                expand(exclude);
                expand(include);

                if (!timeAvailable)
                    break;
            }
        }

        public void expand(BBState w) {
            if (w.bound < BSSF_cost)
            {
                if(criterion(w)) {
                    //BSSF = w
                    //Agenda.prune(BSSF.cost)
                }
                else {
                    Agenda.Add(w);
                }
            }
        }

        bool criterion(BBState w)
        {
            if (w.depth == 
 	        throw new NotImplementedException();
        }
    }

    class BBState : IComparable
    {

        private double[,] cost;

        public double bound;

        public double depth;

        public int numPoints;

        public BBState(int numPoints)
            : this(new double[numPoints, numPoints])
        {
        }

        public BBState(double[,] cost)
        {
            this.cost = cost;
            int numPoints = cost.GetLength(1);
        }

        /// Copy Constructor
        public BBState(BBState other)
        {
            this.cost = new double[numPoints, numPoints];
            this.depth = other.depth;
            this.bound = other.bound;
        }

        public double getPriority() {
            return bound - depth*100;
        }

        public int CompareTo(object obj)
        {
            return (int)(getPriority() - ((BBState)obj).getPriority());
        }
    
        public void chooseNextEdge(out int chosenX,out int chosenY)
        {
 	        for (int x = 0; x < numPoints; x++) {
                for (int y = 0; y < numPoints; y++) {
                    if (cost[x, y] != 0)
                    {
                        chosenX = x;
                        chosenY = y;
                        return;
                    }
                }
            }

            chosenX = chosenY = 0;
        }
    }

}
