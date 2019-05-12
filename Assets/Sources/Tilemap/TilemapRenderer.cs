using UnityEngine;

namespace VH
{
    [ExecuteAfter(typeof(TilemapChunkCuller))]
    public class TilemapRenderer : ComponentBehaviour
    {
        [SerializeField]
        private Tilemap tileMap = null;
        private TilemapChunk chunk;

        /// <summary>
        /// It is Recommended to do all the Dependency Injection
        /// and Filling data from JSON/XML to the Scriptable Objects
        /// in the Awake() Phase and make sure all the data is
        /// available for use by the Start() Phase
        /// </summary>
        protected override void Awake()
        {
            //MUST BE CALLED FOR COMPONENT BEHAVIOUR TO WORK
            base.Awake();
        }

        private void Start()
        {
            //Logger.Log("Start Called in Renderer");

        }

        public override void OnUpdate()
        {
            for (int i = 0; i < tileMap.FlattenedChunks.Length; i++)
            {
                chunk = tileMap.FlattenedChunks[i];
                if (!chunk.Visible) continue;

                //TODO:! Dilemma on applying UVs and Colors to directly to the mesh or Apply it just befor drawing         
                chunk.Mesh.uv = chunk.TexCoords;
                chunk.Mesh.colors = chunk.Colors;

                Graphics.DrawMesh(chunk.Mesh, Vector3.zero, Quaternion.identity, chunk.Material, 0);
            }
        }
    }

}

