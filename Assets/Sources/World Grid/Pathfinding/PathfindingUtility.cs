using Priority_Queue;
using System;
using System.Collections.Generic;
using UnityEngine;

//TODO:! Try Clearance based Pathfinding if Performance is not enough for larger Agents
namespace VH.Pathfinding
{
    /// <summary>
    /// Utility Functiions for Pathfinding
    /// </summary>
    public static class PathfindingUtility
    {


        /// <summary>
        /// FindPath With Threading Support
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="request"></param>
        /// <param name="callback"></param>
        public static void FindPath(this Tilemap grid, PathRequest request, Action<PathResult> callback)
        {
            Vector3[] wayPoints = new Vector3[0];
            bool bPathFound = false;

            Tile startTile = grid.GetTileFromWorldPosition(request.startPosition);
            Tile targetTile = grid.GetTileFromWorldPosition(request.endPosition);

            //TODO : Add startTile is Walkable
            //if (targetTile.walkable)
            if (targetTile.walkable && GridUtility.GetDistanceBetweenTiles(startTile, targetTile) <= request.agent.range
                && grid.IsTileWalkableByAgent(targetTile, request.agent))
            {
                Heap<Tile> openSet = new Heap<Tile>(grid.Columns * grid.Rows);
                HashSet<Tile> closedSet = new HashSet<Tile>();
                int newMovementCost, tempDistance;

                //Add the Start Tile to the Open Set
                openSet.Add(startTile);

                while (openSet.Count > 0)
                {
                    Tile currentTile = openSet.RemoveFirst();
                    closedSet.Add(currentTile);

                    if (currentTile == targetTile)
                    {
                        bPathFound = true;
                        break;
                    }

                    List<Tile> neighbours = grid.GetNeighbouringTiles(currentTile);
                    foreach (var neighbourTile in neighbours)
                    {
                        //if (!neighbourTile.walkable || closedSet.Contains(neighbourTile))
                        if (!grid.IsTileWalkableByAgent(neighbourTile, request.agent) || closedSet.Contains(neighbourTile))
                        {
                            continue;
                        }

                        //TODO: Incase of Agents that Span multiple Tiles,
                        //Instead of adding neighbourTile.penalty, add the average of penalties of tiles that the Agent Spans
                        tempDistance = GridUtility.GetDistanceBetweenTiles(currentTile, neighbourTile) + neighbourTile.penalty;
                        newMovementCost = currentTile.gCost + tempDistance;
                        if (newMovementCost < neighbourTile.gCost || !openSet.Contains(neighbourTile))
                        {
                            neighbourTile.gCost = newMovementCost;
                            neighbourTile.hCost = GridUtility.GetDistanceBetweenTiles(neighbourTile, targetTile);

                            neighbourTile.parent = currentTile;
                            if (!openSet.Contains(neighbourTile))
                            {
                                openSet.Add(neighbourTile);
                            }
                            else
                            {
                                openSet.Update(neighbourTile);
                            }
                        }
                    }
                }

                int distanceCovered = 0;
                if (bPathFound)
                {
                    wayPoints = RetracePath(startTile, targetTile, out distanceCovered);
                    bPathFound = wayPoints.Length > 0 && distanceCovered <= request.agent.range;

                }
                callback(new PathResult(wayPoints, distanceCovered, bPathFound, request.callback));
            }

        }


        //TODO: Remove after Debugging
        public static List<Tile> Path;
        /// <summary>
        /// Retraces the path from the Destination back to the Starting tile 
        /// </summary>
        /// <param name="startTile">This is the current posirtion of the Agent</param>
        /// <param name="destinationTile">This is the Tile that the Agent should reach</param>
        /// <param name="distanceCovered">Manhattan Distance that should be Covered by the agent while taking this path</param>
        /// <returns></returns>
        public static Vector3[] RetracePath(Tile startTile, Tile destinationTile, out int distanceCovered)
        {
            List<Tile> path = new List<Tile>();
            Tile currentTile = destinationTile;
            distanceCovered = 0;

            while (currentTile != startTile)
            {
                path.Add(currentTile);
                distanceCovered += GridUtility.GetDistanceBetweenTiles(currentTile, currentTile.parent) + currentTile.parent.penalty;
                currentTile = currentTile.parent;
            }

            Path = path;
            path.Add(startTile);
            Vector3[] wayPoints = SimplifyPath(path);
            Array.Reverse(wayPoints);

            return wayPoints;
        }



        /// <summary>
        /// Simplifies by Path and reduces the number of Waypoints
        /// </summary>
        public static Vector3[] SimplifyPath(List<Tile> path)
        {
            List<Vector3> listwayPoints = new List<Vector3>();
            Vector2 vec2_OldDirection = Vector2.zero;
            Vector2 vec2_NewDirection = new Vector2();

            for (int i = 1; i < path.Count; i++)
            {
                vec2_NewDirection.Set(path[i - 1].x - path[i].x, path[i - 1].y - path[i].y);
                if (vec2_NewDirection != vec2_OldDirection)
                {
                    listwayPoints.Add(path[i - 1].position + (Vector3.forward * -1f));

                }
                vec2_OldDirection = vec2_NewDirection;
            }
            listwayPoints.Add(path[path.Count - 1].position + (Vector3.forward * -1f));
            return listwayPoints.ToArray();
        }

    }
}
