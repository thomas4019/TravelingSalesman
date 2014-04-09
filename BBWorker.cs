using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TSP
{
    class BBWorker
    {
        C5.IntervalHeap<BBState> agenda;

        public BBWorker()
        {
            agenda = new C5.IntervalHeap<BBState>();
            
        }

    }

    class BBState : IComparable
    {

        private double[,] cost;

        private double bound;

        public BBState(int numPoints)
            : this(new double[numPoints, numPoints])
        {
        }

        public BBState(double[,] cost)
        {
            this.cost = cost;
        }

        public int CompareTo(object obj)
        {
            throw new NotImplementedException();
        }
    }

}
