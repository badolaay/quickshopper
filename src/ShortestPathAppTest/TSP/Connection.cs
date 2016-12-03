using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickShopper.TSP
{
    public class Connection
    {
        public Point From { get; set; }
        public Point To { get; set; }
        public float Cost { get; set; }

        public Connection(Point from, Point to, float cost)
        {
            From = from;
            To = to;
            Cost = cost;
        }
    }
}
