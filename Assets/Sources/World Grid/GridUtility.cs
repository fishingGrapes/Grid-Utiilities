using System.Collections.Generic;
using UnityEngine;

namespace VH
{
    /// <summary>
    /// Utility Functions for Grid Opertaions
    /// </summary>
    public static class GridUtility
    {
        #region Tile Utils

        /// <summary>
        /// Find the Tile Instance from a Position in the World
        /// </summary>
        public static Tile GetTileFromWorldPosition(this Tilemap grid, Vector3 worldPosition)
        {

            float fPositionX = ((worldPosition.x) + grid.Rows * 0.5f);
            float fPositionY = ((worldPosition.y) + grid.Columns * 0.5f);

            fPositionX = Mathf.Clamp(fPositionX, 0, grid.Rows - 1);
            fPositionY = Mathf.Clamp(fPositionY, 0, grid.Columns - 1);

            int x = Mathf.FloorToInt(fPositionX);
            int y = Mathf.FloorToInt(fPositionY);

            return grid.Tiles[x, y];

        }

        /// <summary>
        /// Find the Tile Instance from a Position in the World
        /// </summary>
        public static Tile GetTileFromWorldPosition(this Tilemap grid, float worldX, float worldY)
        {

            float fPositionX = (worldX + grid.Rows * 0.5f);
            float fPositionY = (worldY + grid.Columns * 0.5f);

            fPositionX = Mathf.Clamp(fPositionX, 0, grid.Rows - 1);
            fPositionY = Mathf.Clamp(fPositionY, 0, grid.Columns - 1);

            int x = Mathf.FloorToInt(fPositionX);
            int y = Mathf.FloorToInt(fPositionY);

            return grid.Tiles[x, y];

        }

        /// <summary>
        /// Checks if a Given tile in the Grid is Walkable by an Agent
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="tile"></param>
        /// <param name="agentWidth"></param>
        /// <param name="agentHeight"></param>
        /// <returns></returns>
        public static bool IsTileWalkableByAgent(this Tilemap grid, Tile tile, NavigationAgent agent)
        {
            int xLimit = Mathf.Clamp(tile.x + agent.width, 0, grid.Columns);
            int yLimit = Mathf.Clamp(tile.y - agent.height, 0, grid.Rows);

            for (int y = tile.y; y > yLimit; y--)
            {
                for (int x = tile.x; x < xLimit; x++)
                {
                    if (!grid.Tiles[x, y].walkable)
                    {
                        return false;
                    }
                }
            }
            return true;
        }


        #endregion


        #region Intersection Utils
        /// <summary>
        /// Find the Intersection from a Position in the World, with a Favoured Direction
        /// </summary>
        public static Intersection GetNearestIntersection(this Tilemap grid, Vector3 worldPosition, Direction favouredDirection = Direction.NorthEast)
        {
            short modifierX, modifierY;
            switch (favouredDirection)
            {
                case Direction.South:
                    modifierX = 0;
                    modifierY = -1;
                    break;
                case Direction.West:
                    modifierX = -1;
                    modifierY = 0;
                    break;
                case Direction.NorthWest:
                    modifierX = -1;
                    modifierY = 0;
                    break;
                case Direction.SouthWest:
                    modifierX = -1;
                    modifierY = -1;
                    break;
                case Direction.SouthEast:
                    modifierX = 0;
                    modifierY = -1;
                    break;
                default:
                    modifierX = modifierY = 0;
                    break;
            }

            if (worldPosition.x % 0.5f == 0)
                worldPosition.x += modifierX;
            if (worldPosition.y % 0.5f == 0)
                worldPosition.y += modifierY;

            float fPositionX = ((worldPosition.x) + (grid.Rows + 1) * 0.5f);
            float fPositionY = ((worldPosition.y) + (grid.Columns + 1) * 0.5f);

            fPositionX = Mathf.Clamp(fPositionX, 0, grid.Rows + 1);
            fPositionY = Mathf.Clamp(fPositionY, 0, grid.Columns + 1);

            int x = Mathf.FloorToInt(fPositionX);
            int y = Mathf.FloorToInt(fPositionY);

            if (x <= grid.Columns && y <= grid.Rows)
                return grid.Intersections[x, y];
            else
                return default;

        }


        /// <summary>
        /// Get the Tile That is Encompassed by the Intersection with respect to another Intersection
        /// </summary>
        /// <param name="intersection"></param>
        /// <param name="grid"></param>
        /// <param name="reference"></param>
        /// <returns></returns>
        public static Tile GetEncompassedTile(this Tilemap grid, Intersection intersection, Intersection reference)
        {

            int x = intersection.x == reference.x ? 0 : (intersection.x < reference.x ? 1 : -1);
            int y = intersection.y == reference.y ? 0 : (intersection.y < reference.y ? 1 : -1);

            return grid.GetTileFromWorldPosition(intersection.position.x + x * 0.5f, intersection.position.y + y * 0.5f);
        }





        #endregion


        #region Distances

        /// <summary>
        /// returns the Distance between two tiles in a grid
        /// </summary>
        /// <param name="tileA"></param>
        /// <param name="tileB"></param>
        /// <returns></returns>
        public static int GetDistanceBetweenTiles(Tile tileA, Tile tileB)
        {
            return GridUtility.GetDistance(Mathf.Abs(tileA.x - tileB.x), Mathf.Abs(tileA.y - tileB.y));
        }

        /// <summary>
        /// returns the Distance between two Intersections in a grid
        /// </summary>
        /// <param name="interA"></param>
        /// <param name="interB"></param>
        /// <returns></returns>
        public static int GetDistanceBetweenIntersections(Intersection interA, Intersection interB)
        {
            return GridUtility.GetDistance(Mathf.Abs(interA.x - interB.x), Mathf.Abs(interA.y - interB.y));
        }


        private static readonly float InverseAdjacentDistance = 1f / (float)TemporaryConstants.AdjacentDistance;

        /// <summary>
        /// Returns the Pathfinder Distance between Two Nodes
        /// By Default the Primary Four Directions are 5 Foot away and  Diagonal Movement costs 7.5 Foot
        /// Reference : https://wiki.roll20.net/Ruler
        /// </summary>
        /// <param name="distanceX"></param>
        /// <param name="distanceY"></param>
        /// <returns></returns>
        private static int GetDistance(int distanceX, int distanceY)
        {
            if (distanceX > distanceY)
            {
                return (TemporaryConstants.AdjacentDistance
                    * Mathf.FloorToInt((TemporaryConstants.DiagonalDistance * distanceY) * InverseAdjacentDistance))
                    + (TemporaryConstants.AdjacentDistance * (distanceX - distanceY));
            }

            return (TemporaryConstants.AdjacentDistance
                * Mathf.FloorToInt((TemporaryConstants.DiagonalDistance * distanceX) * InverseAdjacentDistance))
                + (TemporaryConstants.AdjacentDistance * (distanceY - distanceX));
        }

        #endregion

        #region Neighbours

        /// <summary>
        /// Returns all the Walkable Neighboring Tiles for a Given Tile
        /// This also Checks for Cutting Corners, where a Diagonal move is perfecly legal
        /// But results in Awkward Visuals on Screen
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="tile"></param>
        /// <returns></returns>
        public static List<Tile> GetNeighbouringTiles(this Tilemap grid, Tile tile)
        {
            List<Tile> neighbours = new List<Tile>();
            int checkX, checkY;

            for (int y = -1; y <= 1; y++)
            {
                for (int x = -1; x <= 1; x++)
                {
                    if (x == 0 && y == 0)
                    {
                        continue;
                    }

                    checkX = tile.x + x;
                    checkY = tile.y + y;

                    if ((checkX >= 0 && checkX < grid.Rows) && (checkY >= 0 && checkY < grid.Columns))
                    {

                        //Checking for Cutting Corners
                        //if the Neighbour is a diagonal
                        if (Mathf.Abs(x) == 1 && Mathf.Abs(y) == 1)
                        {
                            //Bottom-Right and Top-Left Corners
                            if (x == y)
                            {
                                //If x = 1 and y = 1
                                if (x > 0)
                                {
                                    //x+1,y and x, y+1
                                    if (grid.Tiles[tile.x + 1, tile.y].walkable && grid.Tiles[tile.x, tile.y + 1].walkable)
                                    {
                                        neighbours.Add(grid.Tiles[checkX, checkY]);
                                    }
                                }
                                // else If x = -1 and y = -1
                                else
                                {
                                    //x,y-1 and x-1, y
                                    if (grid.Tiles[tile.x - 1, tile.y].walkable && grid.Tiles[tile.x, tile.y - 1].walkable)
                                    {
                                        neighbours.Add(grid.Tiles[checkX, checkY]);
                                    }
                                }
                            }
                            //Top-Right and Bottom-Left Corners
                            else
                            {
                                //If x == -1 and y == 1
                                if (x < 0 && y > 0)
                                {
                                    //x-1,y and x, y+1
                                    if (grid.Tiles[tile.x - 1, tile.y].walkable && grid.Tiles[tile.x, tile.y + 1].walkable)
                                    {
                                        neighbours.Add(grid.Tiles[checkX, checkY]);
                                    }
                                }
                                //else If x == 1 and y == -1
                                else
                                {
                                    //x,y-1 and x+1, y
                                    if (grid.Tiles[tile.x, tile.y - 1].walkable && grid.Tiles[tile.x + 1, tile.y].walkable)
                                    {
                                        neighbours.Add(grid.Tiles[checkX, checkY]);
                                    }
                                }

                            }
                        }
                        else
                        {
                            neighbours.Add(grid.Tiles[checkX, checkY]);
                        }
                    }
                }
            }

            return neighbours;
        }


        #endregion
    }
}
