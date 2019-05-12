using System;
using UnityEngine;


namespace VH.Pathfinding
{

    public struct PathRequest
    {
        public Vector3 startPosition;
        public Vector3 endPosition;
        public Action<Vector3[], int, bool> callback;
        public NavigationAgent agent;



        public PathRequest(NavigationAgent agent, Vector3 startPosition,
                                    Vector3 endPosition, Action<Vector3[], int, bool> callback)
        {
            this.agent = agent;
            this.startPosition = startPosition;
            this.endPosition = endPosition;
            this.callback = callback;
        }


    }
}
