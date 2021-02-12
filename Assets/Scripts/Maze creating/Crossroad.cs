using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MazeCreating
{
    public class Crossroad
    {
        public Vector3 coords { get; }
        public List<Crossroad> outgoingConnections { get; private set; }
        public List<Crossroad> incomingConnections;

        public Crossroad(Vector3 coords)
        {
            outgoingConnections = new List<Crossroad>();
            incomingConnections = new List<Crossroad>();
            this.coords = coords;
        }

        public void Connect(Crossroad crossroadToJoin)
        {
            outgoingConnections.Add(crossroadToJoin);
            crossroadToJoin.incomingConnections.Add(this);
        }
    }
}
