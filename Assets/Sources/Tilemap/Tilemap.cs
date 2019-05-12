using UnityEngine;
using NaughtyAttributes;
using System.Collections.Generic;

namespace VH
{
    //TODO:! This is a Scriptable Object For Only Debugging Purposes

    /// <summary>
    /// Turn this into a plain old Struct/Class when running not Debugging/Developing
    /// Inject using Zenject to requires Classes
    /// </summary>
    [CreateAssetMenu(menuName = "Singletons/TileMap", fileName = "TileMap")]
    public class Tilemap : ScriptableObject
    {
        /// <summary>
        /// The Number of Rows and Columns in the Grid
        /// Most Probably Limited to a set of multiples of 5 like :
        /// 100 x 100, 150 x 125, 200 x 100, 175 x 225. etc.,
        //TODO: If Users give any other Dimension throw an Exception
        /// </summary>
        [BoxGroup("Map Data"), Slider(50, 300)]
        public ushort Rows = 100;

        [BoxGroup("Map Data"), Slider(50, 300)]
        public ushort Columns = 100;


        /// <summary>
        /// Denotes the resolution fo the TileMap itself.
        /// For Instance, a 100 x 100 Tilemap with a resolution of 25 is split into 4 x 4 Chunks,
        /// each chunk with 25 Tiles. This makes the Tilemaps more easier to manage, 
        /// This also helps with rendering the Tilemap with acceptable Framerates.
        /// and in Optimizing Pathfinding Algorithms.
        /// THIS MUST BE A DIVISOR OF BOTH NUMBER OF ROWS AND NUMBER OF COLUMNS
        /// </summary>
        //[BoxGroup("Map Data"), SerializeField, Slider(10, 50)]
        //private byte redsolution = 25;

        /// <summary>
        /// This represnsts the Size of each chunk in World units
        /// If the "TileSize" Variable is set to 1.0f , this variable is the same as "resolution"
        /// and thus can be safely ignored.
        /// </summary>
        [BoxGroup("Map Data"), SerializeField, Slider(10, 50)]
        private byte chunkSize = 25;


        /// <summary>
        /// The Tilemap Itself as a 2D Array of Tiles
        /// </summary>
        [HideInInspector]
        public Tile[,] Tiles;


        /// <summary>
        /// The Intersections of Tiles in the Tilemap as a 2D Array of Vector3
        /// </summary>
        [HideInInspector]
        public Intersection[,] Intersections;


        /// <summary>
        /// The Array Of "Tilemap Chunks", as discussed earlier in "resolution" variable
        /// </summary>
        [HideInInspector]
        public TilemapChunk[,] Chunks;

        /// <summary>
        /// Tilemap Chunks Flattened into an 1-D array for Easier Access
        /// </summary>
        public TilemapChunk[] FlattenedChunks;


        #region Accessors

        [HideInInspector]
        public byte ChunkSize
        {
            get { return chunkSize; }
            set { chunkSize = value; }
        }

        #endregion
    }


}
