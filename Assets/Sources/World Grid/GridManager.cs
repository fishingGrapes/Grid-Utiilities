using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using VH.Pathfinding;
using VH.RangeDetection;

namespace VH
{
    /// <summary>
    /// Agents Requests are processed through this Manager
    /// </summary>
    public class GridManager : ComponentBehaviour
    {

        private static Queue<RangeResult> rangeResults;
        private static Queue<PathResult> pathResults;

        private int nItemsInPathRequestQueue;
        private int nItemsInQueue;

        protected override void Awake()
        {
            base.Awake();

            pathResults = new Queue<PathResult>();
            rangeResults = new Queue<RangeResult>();
        }



        #region Path Finding

        public static void RequestPath(Tilemap grid, PathRequest request)
        {
            ThreadStart threadStart = delegate
            {
                grid.FindPath(request, OnFinishedProcessingPath);
            };

            threadStart.Invoke();
        }


        private static void OnFinishedProcessingPath(PathResult result)
        {
            lock (pathResults)
            {
                pathResults.Enqueue(result);
            }
        }

        #endregion


        #region Range Finding

        /// <summary>
        /// Starts a Range Detection Job to find Moveable Tiles
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="request"></param>
        public static void RequestMovementRange(Tilemap grid, MovementRangeRequest request)
        {
            ThreadStart threadStart = delegate
            {
                grid.FindMovementRange(request, OnFinishedProcessingRange);
            };

            threadStart.Invoke();
        }

        /// <summary>
        /// Starts a Range Detection Job for Line Pattern
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="request"></param>
        public static void RequestLineRange(Tilemap grid, LineRangeRequest request)
        {

            ThreadStart threadStart = delegate
            {
                grid.FindLineRange(request, OnFinishedProcessingRange);
            };

            threadStart.Invoke();

        }

        /// <summary>
        /// Starts a Range Detection Job for Cone Pattern
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="request"></param>
        public static void RequestConeRange(Tilemap grid, ConeRangeRequest request)
        {

            ThreadStart threadStart = delegate
            {
                grid.FindConeRange(request, OnFinishedProcessingRange);
            };

            threadStart.Invoke();

        }

        /// <summary>
        /// Starts a Range Detection Job for Circular Pattern
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="request"></param>
        public static void RequestCircleRange(Tilemap grid, CircleRangeRequest request)
        {
            ThreadStart threadStart = delegate
            {
                grid.FindCircleRange(request, OnFinishedProcessingRange);
            };

            threadStart.Invoke();
        }


        private static void OnFinishedProcessingRange(RangeResult result)
        {
            lock (rangeResults)
            {
                rangeResults.Enqueue(result);
            }
        }

        #endregion



        public override void FixedTick()
        {
            base.FixedTick();

            if (rangeResults.Count > 0)
            {
                nItemsInQueue = rangeResults.Count;
                lock (rangeResults)
                {
                    for (int i = 0; i < nItemsInQueue; i++)
                    {
                        RangeResult result = rangeResults.Dequeue();
                        result.callback?.Invoke(result.tiles);

                    }
                }
            }

            if (pathResults.Count > 0)
            {
                nItemsInPathRequestQueue = pathResults.Count;
                lock (pathResults)
                {
                    for (int i = 0; i < nItemsInPathRequestQueue; i++)
                    {
                        PathResult result = pathResults.Dequeue();
                        result.callback?.Invoke(result.path, result.distanceCovered, result.succeess);
                    }
                }
            }
        }

    }
}
