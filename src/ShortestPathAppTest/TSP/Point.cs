using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickShopper.TSP
{
    public class Point
    {
        public int Id { get; set; }
        public IList<Connection> PossibleConnections { get; set; }
        public Connection IncommingConnection { get; set; }
        public Connection OutgoingConnection { get; set; }

        public Point(int id) : this(id, new List<Connection>()) { }

        public Point()
        {
        }

        public Point(int id, IList<Connection> possibleConnections)
        {
            Id = id;
            PossibleConnections = possibleConnections;
        }

        public Point AddPossibleConnection(Connection connection)
        {
            PossibleConnections.Add(connection);
            return this;
        }

        /// Inserts a new point between an existing connection
        /// this => B becomes this => point > B
        public void ConnectTo(Point point)
        {
            //Is this not part of a connection?
            if (this.IncommingConnection == null)
            {
                CreateNewRouteTo(point);
            }
            else
            {
                InsertIntoExistingRoute(point);
            }
        }

        private void CreateNewRouteTo(Point point)
        {
            this.OutgoingConnection = this.GetConnectionTo(point);

            this.IncommingConnection = point.GetConnectionTo(this);


            point.OutgoingConnection = point.GetConnectionTo(this);

            point.IncommingConnection = this.GetConnectionTo(point);

        }

        private void InsertIntoExistingRoute(Point point)
        {
            //The new point has to be connected to THIS
            //This is possible as an incoming or outgoing connection
            //Is has to be checked, whether it is better to use the incoming or outgoing connection of THIS
            //v-- The incoming connection is better
            if (this.IncommingConnection.From.GetConnectionTo(point).Cost < point.GetConnectionTo(this.OutgoingConnection.To).Cost)
            {

                //Use the incoming connection
                point.IncommingConnection = this.IncommingConnection.From.GetConnectionTo(point);
                point.IncommingConnection.From.OutgoingConnection = point.IncommingConnection;

                //set the outgoing connection to THIS
                point.OutgoingConnection = point.GetConnectionTo(this);
                point.OutgoingConnection.To.IncommingConnection = point.OutgoingConnection;
            }
            //v-- the outgoing connection is better
            else
            {

                //use the outgoing connection
                point.OutgoingConnection = point.GetConnectionTo(this.OutgoingConnection.To);
                point.OutgoingConnection.To.IncommingConnection = point.OutgoingConnection;

                //set the incoming connection to THIS
                point.IncommingConnection = this.GetConnectionTo(point);
                point.IncommingConnection.From.OutgoingConnection = point.IncommingConnection;

            }
        }

        public Connection GetConnectionTo(Point point)
        {
            return PossibleConnections.Single(x => x.To == point);
        }
    }
}

