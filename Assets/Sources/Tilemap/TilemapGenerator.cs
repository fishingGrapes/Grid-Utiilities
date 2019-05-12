using UnityEngine;
using NaughtyAttributes;

namespace VH
{
    [ExecuteAfter(typeof(TilemapInitializer))]
    public class TilemapGenerator : ComponentBehaviour
    {
        [SerializeField]
        private Tilemap tileMap = null;

        protected override void Awake()
        {
            base.Awake();
            this.GenerateMeshes();
        }


        [Button("Regenerate")]
        private void GenerateMeshes()
        {
            byte chunkSize = (byte)tileMap.ChunkSize;

            //Offsets For Positioning the Grid
            float vertexOffset = 0.5f;
            Vector3 tileOffset = new Vector3();
            Vector3 chunkOffset = new Vector3();

            //Row and Column Count of the Chunks
            int chunkRows = tileMap.Rows / chunkSize;
            int chunkColumns = tileMap.Columns / chunkSize;

            float chunkOffsetX = -(chunkColumns * chunkSize * 0.5f) + (chunkSize * 0.5f);
            float chunkOffsetY = -(chunkRows * chunkSize * 0.5f) + (chunkSize * 0.5f);

            //TODO: Load Materials from a Different Place and maybe assign it to Tilemap Data
            Material mat_Tilemap = this.CreateMaterial();

            int x, y, meshID = 0;
            tileMap.Chunks = new TilemapChunk[chunkColumns, chunkRows];
            tileMap.FlattenedChunks = new TilemapChunk[chunkColumns * chunkRows];

            for (int j = 0; j < chunkRows; j++)
            {
                for (int i = 0; i < chunkColumns; i++)
                {

                    //Arrays for Filling the Chunk Data
                    Vector3[] vertices = new Vector3[chunkSize * chunkSize * 4];
                    int[] triangles = new int[chunkSize * chunkSize * 6];
                    Vector2[] texCoords = new Vector2[vertices.Length];
                    Color[] colors = new Color[vertices.Length];

                    //Tracker Variables
                    int v = 0;
                    int t = 0;

                    //Create a New Tilemap Chunk and a corresponding Mesh
                    TilemapChunk tileMapChunk = new TilemapChunk();
                    Mesh chunkMesh = new Mesh();
                    Tile[] chunkTiles = new Tile[chunkSize * chunkSize];

                    chunkOffset.Set(chunkOffsetX + (i * chunkSize), chunkOffsetY + (j * chunkSize), 0);

                    for (int cY = 0; cY < chunkSize; cY++)
                    {
                        for (int cX = 0; cX < chunkSize; cX++)
                        {
                            tileOffset.Set(cX - chunkSize * 0.5f + 0.5f, cY - chunkSize * 0.5f + 0.5f, 0);

                            vertices[v + 0] = new Vector3(-vertexOffset, -vertexOffset, 0) + tileOffset + chunkOffset;
                            vertices[v + 1] = new Vector3(-vertexOffset, vertexOffset, 0) + tileOffset + chunkOffset;
                            vertices[v + 2] = new Vector3(vertexOffset, -vertexOffset, 0) + tileOffset + chunkOffset;
                            vertices[v + 3] = new Vector3(vertexOffset, vertexOffset, 0) + tileOffset + chunkOffset;

                            texCoords[v + 0] = new Vector2(0, 0);
                            texCoords[v + 1] = new Vector2(0, 1);
                            texCoords[v + 2] = new Vector2(1, 0);
                            texCoords[v + 3] = new Vector2(1, 1);

                            colors[v + 0] = Color.white;
                            colors[v + 1] = Color.white;
                            colors[v + 2] = Color.white;
                            colors[v + 3] = Color.white;

                            triangles[t + 0] = v + 0;
                            triangles[t + 1] = triangles[t + 4] = v + 1;
                            triangles[t + 2] = triangles[t + 3] = v + 2;
                            triangles[t + 5] = v + 3;


                            x = cX + (i * chunkSize);
                            y = cY + (j * chunkSize);
                            tileMap.Tiles[x, y].indices = new int[] { (v + 0), (v + 1), (v + 2), (v + 3) };
                            tileMap.Tiles[x, y].ParentChunk = tileMapChunk;
                            tileMap.Tiles[x, y].vertices = new Vector3[] { vertices[v + 0], vertices[v + 1], vertices[v + 2], vertices[v + 3] };

                            //TODO:! Fill Tiles of Chunk
                            //Debug.Log($"{i},{j}  {(chunkSize * i) + cX + (j * chunkSize) + cY}");
                            //chunkTiles[((chunkSize * i) + cX) + ((j * chunkRows) + cY)] = tileMap.Tiles[x, y];

                            //Calulate the Intersection Points of The Grid Tiles
                            tileMap.Intersections[x, y].position = vertices[v + 0];
                            if (x == tileMap.Columns - 1) tileMap.Intersections[x + 1, y].position = vertices[v + 2];
                            if (y == tileMap.Rows - 1) tileMap.Intersections[x, y + 1].position = vertices[v + 1];
                            if (x == tileMap.Columns - 1 && y == tileMap.Rows - 1) tileMap.Intersections[x + 1, y + 1].position = vertices[v + 3];


                            v += 4;
                            t += 6;

                        }

                    }

                    //Generate the Mesh from the Calculated vertices, indices, UVs and colors
                    chunkMesh.Clear();
                    chunkMesh.vertices = vertices;
                    chunkMesh.triangles = triangles;
                    chunkMesh.uv = texCoords;
                    chunkMesh.colors = colors;
                    chunkMesh.RecalculateNormals();

                    //Apply the Chunk data
                    tileMapChunk.id = meshID;
                    tileMapChunk.Mesh = chunkMesh;
                    tileMapChunk.TexCoords = texCoords;
                    tileMapChunk.Material = mat_Tilemap;
                    tileMapChunk.Colors = colors;
                    tileMapChunk.TRS = Matrix4x4.TRS(chunkOffset, Quaternion.identity, Vector3.one);
                    tileMapChunk.Visible = false;
                    //tileMapChunk.Tiles = chunkTiles;

                    //One can always use a 1-D array with "meshID" as index
                    tileMap.Chunks[i, j] = tileMapChunk;
                    tileMap.FlattenedChunks[meshID] = tileMapChunk;


                    meshID += 1;

                }

            }

        }

        //TODO:! Move this OUT of Here TEST      
        private Material CreateMaterial()
        {
            Material mat = Resources.Load<Material>("Materials/mat_Tilemap");

            Texture2D texture = TextureUtils.LoadTexture_DDS("Smiley_DDS.DDS", TextureFormat.DXT5, true);
            mat.SetTexture("_MainTex", texture);

            return mat;
        }

    }
}
