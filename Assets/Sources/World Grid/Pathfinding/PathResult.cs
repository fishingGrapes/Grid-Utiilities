using System;
using UnityEngine;

namespace VH.Pathfinding
{
    public struct PathResult
    {
        public Vector3[] path;
        public bool succeess;
        public Action<Vector3[], int, bool> callback;
        public int distanceCovered;

        public PathResult(Vector3[] path, int distanceCovered,
                                bool succeess, Action<Vector3[], int, bool> callback)
        {
            this.path = path;
            this.distanceCovered = distanceCovered;
            this.succeess = succeess;
            this.callback = callback;
        }
    }
}
