using Priority_Queue;
using System.Collections.Generic;
using UnityEngine;

namespace VH.RangeDetection
{
    /// <summary>
    /// Bunch of Utility Function for Range Detection
    /// </summary>
    public static class RangeDetectionUtility
    {
        //TODO:! Remove After Debugging
        public static List<Tile> MovementRange;
        /// <summary>
        /// Find the Range of an Agent
        /// Might also be used for Range of Spells and Weapons
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="request"></param>
        /// <param name="callback"></param>
        public static void FindMovementRange(this Tilemap grid, MovementRangeRequest request, System.Action<RangeResult> callback)
        {
            Tile referenceTile = grid.GetTileFromWorldPosition(request.position);

            //TODO: Change the Max Heap Size Accordingly
            FastPriorityQueue<Tile> frontier = new FastPriorityQueue<Tile>(50 * 12);
            Dictionary<Tile, int> tilesToCostSoFar = new Dictionary<Tile, int>();

            List<Tile> moveableTiles = new List<Tile>();
            List<ushort> distances = new List<ushort>();

            frontier.Enqueue(referenceTile, 0);
            tilesToCostSoFar.Add(referenceTile, 0);

            Tile currentTile;
            List<Tile> neighbours;
            int costSoFar, newMovementCost;
            bool bKeyExists;

            while (frontier.Count > 0)
            {
                currentTile = frontier.Dequeue();
                frontier.ResetNode(currentTile);

                if (tilesToCostSoFar[currentTile] <= request.agent.range)
                {
                    neighbours = grid.GetNeighbouringTiles(currentTile);
                    foreach (Tile neighbourTile in neighbours)
                    {
                        if (!grid.IsTileWalkableByAgent(neighbourTile, request.agent))
                        {
                            continue;
                        }

                        //TODO: Incase of Agents that Span multiple Tiles,
                        //Instead of adding neighbourTile.penalty, add the average of penalties of tiles that the Agent Spans
                        //newMovementCost = tilesToCostSoFar[currentTile] +
                        //   GridUtility.GetDistanceBetweenTiles(currentTile, neighbourTile) + neighbourTile.penalty;
                        newMovementCost = GridUtility.GetDistanceBetweenTiles(referenceTile, neighbourTile) + neighbourTile.penalty;

                        bKeyExists = tilesToCostSoFar.TryGetValue(neighbourTile, out costSoFar);

                        if (!bKeyExists && newMovementCost <= request.agent.range)
                        {
                            tilesToCostSoFar.Add(neighbourTile, newMovementCost);
                            frontier.Enqueue(neighbourTile, newMovementCost);
                            moveableTiles.Add(neighbourTile);
                            distances.Add((ushort)newMovementCost);
                        }
                        else
                        {
                            if (newMovementCost < costSoFar)
                            {
                                tilesToCostSoFar[neighbourTile] = newMovementCost;
                                frontier.UpdatePriority(neighbourTile, newMovementCost);
                            }
                        }
                    }
                }

            }

            MovementRange = moveableTiles;
            callback(new MovementRangeResult(moveableTiles.ToArray(), distances.ToArray(), request.callback));
        }




        //TODO:! Remove After Debugging
        public static List<Tile> LineRange;
        /// <summary>
        /// Retruns a Line based on the Range Specified using Bresenhams's Algorithm
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="request"></param>
        /// <param name="callback"></param>
        public static void FindLineRange(this Tilemap grid, LineRangeRequest request, System.Action<RangeResult> callback)
        {
            List<Tile> lineTiles = new List<Tile>();
            List<ushort> distances = new List<ushort>();

            Tile currentTile;
            Tile startTile = grid.GetTileFromWorldPosition(request.position);
            Tile endTile = grid.GetTileFromWorldPosition(request.destination);

            if (!startTile.Equals(endTile))
            {
                if (GridUtility.GetDistanceBetweenTiles(startTile, endTile) <= request.range)
                    endTile = grid.GetTileFromWorldPosition(MathUtility.Extrapolate(startTile.position, endTile.position, (ushort)(request.range / (float)TemporaryConstants.AdjacentDistance)));


                //TODO: Needs Testing to Find out every case Works. Might Perform Poorly.
                MathUtility.Line(startTile.x, startTile.y, endTile.x, endTile.y,
                    (int x, int y) =>
                            {
                                currentTile = grid.Tiles[x, y];
                                if (GridUtility.GetDistanceBetweenTiles(startTile, currentTile) <= request.range)
                                {
                                    lineTiles.Add(currentTile);
                                    distances.Add((ushort)GridUtility.GetDistanceBetweenTiles(startTile, currentTile));
                                }

                                return true;

                            }
                    );
            }


            LineRange = lineTiles;
            callback(new LineRangeResult(lineTiles.ToArray(), distances.ToArray(), request.callback));
        }



        //TODO:! Remove After Debugging
        public static List<Tile> CircleRange;
        /// <summary>
        /// Returns a Collection of Tiles inside a Circle based on the Range Specified
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="request"></param>
        /// <param name="callback"></param>
        public static void FindCircleRange(this Tilemap grid, CircleRangeRequest request, System.Action<RangeResult> callback)
        {
            List<Tile> circleTiles = new List<Tile>();
            List<ushort> distances = new List<ushort>();

            Intersection originIntersection = grid.GetNearestIntersection(request.point);
            int radius = request.range / 5;
            int originX = originIntersection.x - radius, limitX = originIntersection.x + radius;
            int originY = originIntersection.y - radius, limitY = originIntersection.y + radius;

            for (int y = originY; y <= limitY; y++)
            {
                for (int x = originX; x <= limitX; x++)
                {
                    if (y >= 0 && x >= 0 && y < (grid.Rows + 1) && x < (grid.Columns + 1))
                    {
                        if (GridUtility.GetDistanceBetweenIntersections(grid.Intersections[x, y], originIntersection) <= request.range)
                        {
                            circleTiles.Add(grid.GetEncompassedTile(grid.Intersections[x, y], originIntersection));
                            distances.Add((ushort)GridUtility.GetDistanceBetweenIntersections(grid.Intersections[x, y], originIntersection));
                        }
                    }
                }
            }

            CircleRange = circleTiles;
            callback(new CircleRangeResult(circleTiles.ToArray(), distances.ToArray(), request.callback));
        }


        //TODO:! Remove After Debugging
        public static List<Tile> ConeRange;
        /// <summary>
        /// Retruns a Collction of Tiles inside a Cone based on the Range Specified 
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="request"></param>
        /// <param name="callback"></param>
        public static void FindConeRange(this Tilemap grid, ConeRangeRequest request, System.Action<RangeResult> callback)
        {
            List<Tile> coneTiles = new List<Tile>();
            List<ushort> distances = new List<ushort>();

            Intersection originIntersection = grid.GetNearestIntersection(request.origin, GetConeOriginDirection(request.origin, request.position));
            short rangeInTiles = (short)(request.range / 5);

            int iCap = 0, jCap = 0;
            int jStartIndex = 0, jEndIndex = 0;
            int iOrigin = 0, jOrigin = 0;
            short iModifier = 0;

            //Get from a GridUtility called GetConeDirection
            Direction direction = GetConeDirection(request.origin, request.position);
            switch (direction)
            {
                case Direction.North:
                    iOrigin = originIntersection.y;
                    jOrigin = originIntersection.x;

                    iCap = grid.Rows + 1;
                    jCap = grid.Columns + 1;

                    jStartIndex = jOrigin - rangeInTiles;
                    jEndIndex = jOrigin + rangeInTiles;

                    iModifier = 1;
                    break;

                case Direction.East:
                    iOrigin = originIntersection.x;
                    jOrigin = originIntersection.y;

                    iCap = grid.Columns + 1;
                    jCap = grid.Rows + 1;

                    jStartIndex = jOrigin - rangeInTiles;
                    jEndIndex = jOrigin + rangeInTiles;

                    iModifier = 1;
                    break;

                case Direction.South:
                    iOrigin = originIntersection.y;
                    jOrigin = originIntersection.x;

                    iCap = grid.Rows + 1;
                    jCap = grid.Columns + 1;

                    jStartIndex = jOrigin - rangeInTiles;
                    jEndIndex = jOrigin + rangeInTiles;

                    iModifier = -1;
                    break;

                case Direction.West:
                    iOrigin = originIntersection.x;
                    jOrigin = originIntersection.y;

                    iCap = grid.Columns + 1;
                    jCap = grid.Rows + 1;

                    jStartIndex = jOrigin - rangeInTiles;
                    jEndIndex = jOrigin + rangeInTiles;

                    iModifier = -1;
                    break;

                case Direction.NorthWest:
                    iOrigin = originIntersection.y;
                    jOrigin = originIntersection.x;

                    iCap = grid.Rows + 1;
                    jCap = grid.Columns + 1;

                    jStartIndex = jOrigin - rangeInTiles;
                    jEndIndex = jOrigin;

                    iModifier = 1;
                    break;

                case Direction.NorthEast:
                    iOrigin = originIntersection.y;
                    jOrigin = originIntersection.x;

                    iCap = grid.Columns + 1;
                    jCap = grid.Rows + 1;

                    jStartIndex = jOrigin;
                    jEndIndex = jOrigin + rangeInTiles + 1;

                    iModifier = 1;
                    break;

                case Direction.SouthEast:
                    iOrigin = originIntersection.y;
                    jOrigin = originIntersection.x;

                    iCap = grid.Rows + 1;
                    jCap = grid.Columns + 1;

                    jStartIndex = jOrigin;
                    jEndIndex = jOrigin + rangeInTiles + 1;

                    iModifier = -1;
                    break;

                case Direction.SouthWest:
                    iOrigin = originIntersection.y;
                    jOrigin = originIntersection.x;

                    iCap = grid.Columns + 1;
                    jCap = grid.Rows + 1;

                    jStartIndex = jOrigin - rangeInTiles;
                    jEndIndex = jOrigin;

                    iModifier = -1;
                    break;
            }


            for (int i = iOrigin + iModifier, iterations = 0, distanceFromOrigin = 1;
                iterations < rangeInTiles + 1; iterations++, i += iModifier, distanceFromOrigin++)
            {
                for (int j = jStartIndex; j < jEndIndex; j++)
                {
                    if (j >= 0 && i >= 0 && j < jCap && i < iCap)
                    {
                        Intersection currentIntersection = grid.Intersections[j, i];
                        Intersection referenceIntersection = grid.Intersections[jOrigin, i];
                        bool insideCone = false;

                        switch (direction)
                        {
                            case Direction.North:
                                currentIntersection = grid.Intersections[j, i];
                                referenceIntersection = grid.Intersections[jOrigin, i];
                                insideCone = GridUtility.GetDistanceBetweenIntersections(currentIntersection, referenceIntersection)
                                    <= (distanceFromOrigin * TemporaryConstants.AdjacentDistance);
                                break;

                            case Direction.East:
                                currentIntersection = grid.Intersections[i, j];
                                referenceIntersection = grid.Intersections[i, jOrigin];
                                insideCone = GridUtility.GetDistanceBetweenIntersections(currentIntersection, referenceIntersection)
                                    <= (distanceFromOrigin * TemporaryConstants.AdjacentDistance);
                                break;

                            case Direction.South:
                                currentIntersection = grid.Intersections[j, i];
                                referenceIntersection = grid.Intersections[jOrigin, i];
                                insideCone = GridUtility.GetDistanceBetweenIntersections(currentIntersection, referenceIntersection)
                                    <= (distanceFromOrigin * TemporaryConstants.AdjacentDistance);
                                break;

                            case Direction.West:
                                currentIntersection = grid.Intersections[i, j];
                                referenceIntersection = grid.Intersections[i, jOrigin];
                                insideCone = GridUtility.GetDistanceBetweenIntersections(currentIntersection, referenceIntersection)
                                    <= (distanceFromOrigin * TemporaryConstants.AdjacentDistance);
                                break;

                            default:
                                currentIntersection = grid.Intersections[j, i];
                                referenceIntersection = grid.Intersections[jOrigin, i];
                                insideCone = true;
                                break;
                        }

                        if (GridUtility.GetDistanceBetweenIntersections(originIntersection, currentIntersection) <= request.range && insideCone)
                        {
                            coneTiles.Add(grid.GetEncompassedTile(currentIntersection, originIntersection));
                            distances.Add((ushort)GridUtility.GetDistanceBetweenIntersections(originIntersection, currentIntersection));
                        }

                    }
                }
            }

            ConeRange = coneTiles;
            callback(new ConeRangeResult(coneTiles.ToArray(), distances.ToArray(), request.callback));
        }


        /// <summary>
        /// Get the Origin Direction of a Cone
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        private static Direction GetConeOriginDirection(Vector3 origin, Vector3 position)
        {
            float angle = MathUtility.AngleInDegrees(origin, position);

            if (angle <= 90) return Direction.NorthEast;
            else if (angle > 90 && angle <= 180) return Direction.NorthWest;
            else if (angle > 180 && angle <= 270) return Direction.SouthWest;
            else return Direction.SouthEast;
        }


        /// <summary>
        /// Get the Direction of a Point with respect to another point
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="origin"></param>
        /// <param name="reference"></param>
        /// <returns></returns>
        private static Direction GetConeDirection(Vector3 origin, Vector3 position)
        {
            float angle = MathUtility.AngleInDegrees(origin, position);

            if (angle <= 30 || angle > 330) return Direction.East;
            else if (angle > 30 && angle <= 60) return Direction.NorthEast;
            else if (angle > 60 && angle <= 120) return Direction.North;
            else if (angle > 120 && angle <= 150) return Direction.NorthWest;
            else if (angle > 150 && angle <= 210) return Direction.West;
            else if (angle > 210 && angle <= 240) return Direction.SouthWest;
            else if (angle > 240 && angle <= 300) return Direction.South;
            else return Direction.SouthEast;
        }

    }
}
