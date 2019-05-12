using System.Collections.Generic;
using UnityEngine;

namespace VH
{
    public class TilemapChunk
    {
        /// <summary>
        /// The Unique Identifier for this MeshChunk
        /// </summary>
        public int id;

        /// <summary>
        /// The Mesh Object that makes this Chunk.
        /// </summary>
        public Mesh Mesh;

        /// <summary>
        /// The Material applied to the Mesh of this Chunk.
        /// </summary>
        public Material Material;

        /// <summary>
        /// The TexCoords Corresponding the Mesh vertices.
        /// </summary>
        public Vector2[] TexCoords;

        /// <summary>
        /// The Color for each Tile Vertex.
        /// </summary>
        public Color[] Colors;

        /// <summary>
        /// The Translate, rotate and Scale Factor resulting in MVP matrix.
        /// </summary>
        public Matrix4x4 TRS;

        /// <summary>
        /// Is the Mesh Chunk Vsisible?
        /// </summary>
        public bool Visible;

        /// <summary>
        /// The Tiles that are covered by this TilemapChunk
        /// </summary>
        //public Tile[] Tiles;

        /// <summary>
        /// The Animations inside this Chunk.
        /// Looping through this and Updating the Animation is 
        /// performant than looping through all Tiles in the Chunk and 
        /// Checking if it has an Animation
        /// </summary>
        public List<TileAnimation> TileAnimations;

    }

}
