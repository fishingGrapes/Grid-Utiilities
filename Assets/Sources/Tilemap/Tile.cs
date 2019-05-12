using Priority_Queue;
using UnityEngine;

namespace VH
{
    /// <summary>
    /// This is basic entity which compose the Tilemap
    /// Used for Rendering, pathfinding
    /// </summary>
    public class Tile : FastPriorityQueueNode, IHeapNode<Tile>
    {
        /// <summary>
        /// The Unique Identifier of this Tile in the Tilemap
        /// which is also it's index in the Falttened Tilemap Array
        /// </summary>
        public int id;


        public int x;
        public int y;

        /// <summary>
        /// Position in World Space
        /// </summary>
        public Vector3 position;


        public Tile(int id, int x, int y)
        {
            this.x = x;
            this.y = y;

            position = new Vector3();
        }


        #region Rendering, Lighting and Animation

        //Metadata about Tile for Tile-Level Opertaions on the Chunk
        //Like Tile Animations, Lighting or Fog of War

        /// <summary>
        /// The Vertices this Tile is made up of in the TilemapChunk Mesh
        /// </summary>
        public Vector3[] vertices;

        /// <summary>
        /// The Indices of the above vertices in the TilemapChunk Triangles Array
        /// </summary>
        public int[] indices;

        /// <summary>
        /// The TilemapChunk that this Tile belogs to
        /// </summary>
        public TilemapChunk ParentChunk;

        /// <summary>
        /// The Color Overlay of this particular Tile
        /// Crucial for Fake Lighting and Fog of War
        /// </summary>
        public Color Gradient
        {
            set
            {
                ParentChunk.Colors[indices[0]] = value;
                ParentChunk.Colors[indices[1]] = value;
                ParentChunk.Colors[indices[2]] = value;
                ParentChunk.Colors[indices[3]] = value;
            }
        }

        //The Rect Respresenting a Part of the Tileset that this Tile 
        public Rect TextureData
        {
            set
            {
                ParentChunk.TexCoords[indices[0]].Set(value.x, value.y);
                ParentChunk.TexCoords[indices[1]].Set(value.x, value.y + value.height);
                ParentChunk.TexCoords[indices[2]].Set(value.x + value.width, value.y);
                ParentChunk.TexCoords[indices[3]].Set(value.x + value.width, value.y + value.height);
            }
        }

        #endregion

        #region Pathfinding

        /// <summary>
        /// The Parent of this Tile in Pathfinding
        /// If the path is given by , a -> b -> c,
        /// Then b is the parent of c and a is the parent of b
        /// </summary>
        public Tile parent;

        /// <summary>
        /// Used for Pathfining Calculations
        /// </summary>
        public int gCost;
        public int hCost;

        public int fCost
        {
            get { return (gCost + hCost); }
        }

        /// <summary>
        /// Penalty of this Tile
        /// A penalty of -1 is UnWalkable
        /// </summary>
        public short penalty;

        /// <summary>
        /// Easier Access to Penalty Data 
        /// </summary>
        public bool walkable
        {
            get { return penalty > -1; }
        }

        /// <summary>
        /// For Pathfinding Purposes
        /// </summary>
        private int nHeapIndex;
        public int HeapIndex
        {
            get { return nHeapIndex; }
            set { nHeapIndex = value; }
        }

        private int nCompareValue;
        public int CompareTo(Tile other)
        {
            nCompareValue = fCost.CompareTo(other.fCost);
            if (nCompareValue == 0)
            {
                nCompareValue = hCost.CompareTo(other.hCost);
            }

            return -nCompareValue;
        }

        #endregion

    }

}
