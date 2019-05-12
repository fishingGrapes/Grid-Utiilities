using System;
using UnityEngine;

namespace VH.RangeDetection
{
    public interface RangeRequest { }

    public struct MovementRangeRequest : RangeRequest
    {
        public Vector3 position;
        public Action<Tile[]> callback;
        public NavigationAgent agent;

        public MovementRangeRequest(Vector3 position, NavigationAgent agent, Action<Tile[]> callback)
        {
            //TODO: Chnage AgentRange 
            this.position = position;
            this.callback = callback;
            this.agent = agent;
        }
    }

    public struct LineRangeRequest : RangeRequest
    {
        public Vector3 position;
        public Vector3 destination;
        public Action<Tile[]> callback;
        public ushort range;

        public LineRangeRequest(Vector3 position, Vector3 destination, ushort range, Action<Tile[]> callback)
        {
            //TODO: Chnage AgentRange 
            this.position = position;
            this.destination = destination;
            this.callback = callback;
            this.range = range;
        }
    }

    public struct ConeRangeRequest : RangeRequest
    {
        public Vector3 origin;
        public Vector3 position;
        public Action<Tile[]> callback;
        public ushort range;

        public ConeRangeRequest(Vector3 origin, Vector3 position, ushort range, Action<Tile[]> callback)
        {
            this.origin = origin;
            this.position = position;
            this.callback = callback;
            this.range = range;
        }
    }

    public struct CircleRangeRequest : RangeRequest
    {
        public Vector3 point;
        public Action<Tile[]> callback;
        public ushort range;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="point">The mouse Point</param>
        /// <param name="range">The Range od the Circle in Feet</param>
        /// <param name="callback"></param>
        public CircleRangeRequest(Vector3 point, ushort range, Action<Tile[]> callback)
        {
            this.point = point;
            this.callback = callback;
            this.range = range;
        }
    }
}