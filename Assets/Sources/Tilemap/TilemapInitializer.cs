using NaughtyAttributes;
using UnityEngine;

namespace VH
{
    /// <summary>
    /// Generates the World Grid which is used by the Tilemap and Pathfinding System
    /// </summary>
    [ExecuteAfter(typeof(TilemapLoader))]
    public class TilemapInitializer : ComponentBehaviour
    {
        [SerializeField]
        private Tilemap tileMap = null;

        protected override void Awake()
        {
            base.Awake();

            this.ConstructWorld();
            //Logger.Log("World Initialized");
        }

        [Button("Reconstruct")]
        private void ConstructWorld()
        {
            float horizontalOffset = -(tileMap.Columns * 0.5f) + 0.5f;
            float verticalOffset = -(tileMap.Rows * 0.5f) + 0.5f;
            float tileX, tileY;

            tileMap.Tiles = new Tile[tileMap.Columns, tileMap.Rows];

            for (int y = 0; y < tileMap.Rows; y++)
            {
                for (int x = 0; x < tileMap.Columns; x++)
                {
                    tileMap.Tiles[x, y] = new Tile(x + (y * tileMap.Rows), x, y);

                    tileX = horizontalOffset + x;
                    tileY = verticalOffset + y;
                    tileMap.Tiles[x, y].position.Set(tileX, tileY, 0);

                    tileMap.Tiles[x, y].penalty = 0;
                }
            }


            tileMap.Intersections = new Intersection[tileMap.Columns + 1, tileMap.Rows + 1];
            for (int y = 0; y < tileMap.Columns + 1; y++)
            {
                for (int x = 0; x < tileMap.Rows + 1; x++)
                {
                    tileMap.Intersections[x, y] = new Intersection(x, y);
                }
            }
        }


    }
}
