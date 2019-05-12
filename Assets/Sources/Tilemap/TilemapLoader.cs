using System.Collections.Generic;
using System.Linq;
using TiledSharp;
using UnityEngine;

namespace VH
{
    //[ExecuteAfter(typeof(TilemapInitializer))]
    public class TilemapLoader : ComponentBehaviour
    {
        [SerializeField]
        private Tilemap m_Tilemap = null;

        #region Private Fields

        private Dictionary<int, short> m_GidToPenalty = null;
        private Dictionary<int, FrameData> m_GidToFrameData = null;
        private Dictionary<int, FrameAnimation> m_BaseGidToAnimation = null;
        private Dictionary<int, List<TileAnimation>> m_chunkIdToAnimations = null;

        private TmxMap m_TmxMap;

        #endregion

        //TODO:! Texture Test
        private Texture2D m_TextureTest;


        protected override void Awake()
        {
            base.Awake();

            m_GidToPenalty = new Dictionary<int, short>();
            m_GidToFrameData = new Dictionary<int, FrameData>();
            m_BaseGidToAnimation = new Dictionary<int, FrameAnimation>();
            m_chunkIdToAnimations = new Dictionary<int, List<TileAnimation>>();

            this.LoadTileSets($"{Application.streamingAssetsPath}/Maps/TMX/Map001.tmx");
        }

        private void Start()
        {
            this.LoadTiledMap();
        }

        private void LoadTileSets(string path)
        {
            m_TmxMap = new TmxMap(path);

            int mapWidth = m_TmxMap.Width;
            int mapHeight = m_TmxMap.Height;
            int tileWidth = m_TmxMap.TileWidth;
            int tileHeight = m_TmxMap.TileHeight;
            int tileSetCount = m_TmxMap.Tilesets.Count;

            m_Tilemap.Rows = (ushort)mapHeight;
            m_Tilemap.Columns = (ushort)mapWidth;


            for (int i = 0; i < tileSetCount; i++)
            {
                TmxTileset tileSet = m_TmxMap.Tilesets[i];

                int imageWidth = tileSet.Image.Width.Value;
                int imageHeight = tileSet.Image.Height.Value;
                int columns = tileSet.Columns.Value;
                int rows = tileSet.TileCount.Value / columns;

                string tileSetSource = tileSet.Image.Source;
                int firstGID = tileSet.FirstGid;

                float rowOffset = tileSet.TileHeight / (float)imageHeight;
                float columnOffset = tileSet.TileWidth / (float)imageWidth;

                //TODO: Texture Test
                m_TextureTest = TextureUtils.LoadTexture_PNG(tileSetSource, imageWidth, imageHeight, false, TextureFormat.ARGB32, true);


                //MAPPING UV COORDINATES and RECT TO GID
                //Starts from Top-Left Corner of the TileSet which is the First GId
                //and then proceeds to the Bottom-Right Corner of the tileSet

                int Gid = firstGID;
                int key = 0;
                Dictionary<int, TmxTilesetTile> m_TmxTiles = tileSet.Tiles;

                for (int y = rows - 1; y >= 0; y--)
                {
                    for (int x = 0; x < columns; x++)
                    {
                        Vector2 bottomLeft = new Vector2(x * columnOffset, y * rowOffset);
                        Vector2 topLeft = new Vector2(x * columnOffset, (y + 1) * rowOffset);
                        Vector2 bottomRight = new Vector2((x + 1) * columnOffset, y * rowOffset);
                        Vector2 topRight = new Vector2((x + 1) * columnOffset, (y + 1) * rowOffset);

                        //TODO:! Save FrameData According to Texture Atlas
                        FrameData m_FrameData = new FrameData(new Vector2[] { bottomLeft, topLeft, bottomRight, topRight },
                                                      new Rect(x * columnOffset, y * rowOffset, columnOffset, rowOffset));

                        m_GidToFrameData.Add(Gid, m_FrameData);
                        m_GidToPenalty.Add(Gid, (short)int.Parse(m_TmxTiles[key].Properties["Penalty"]));

                        Gid += 1;
                        key += 1;
                    }
                }

                m_TmxTiles = null;



                //MAPPING ANIMATION FRAMES TO GID
                //Starts from Zero => So both Tile ID and frame.Id is GID - 1
                //Only GIDs start from 1

                if (tileSet.Tiles.Keys != null)
                {
                    int[] keys = tileSet.Tiles.Keys.ToArray();
                    for (int j = 0; j < keys.Length; j++)
                    {
                        int frameCount = tileSet.Tiles[keys[j]].AnimationFrames.Count;
                        if (frameCount > 0)
                        {
                            int baseGID = tileSet.Tiles[keys[j]].Id + 1;
                            int currentFrame = 0;

                            KeyFrame[] m_KeyFrames = new KeyFrame[frameCount];
                            var animationFrames = tileSet.Tiles[keys[j]].AnimationFrames;
                            foreach (var frame in animationFrames)
                            {
                                m_KeyFrames[currentFrame] = new KeyFrame(m_GidToFrameData[frame.Id + 1], (ushort)frame.Duration);
                                currentFrame += 1;
                            }
                            m_BaseGidToAnimation.Add(baseGID, new FrameAnimation(m_KeyFrames));
                        }
                    }
                }

                tileSet = null;
            }
        }

        public void LoadTiledMap()
        {
            //ASSIGNING TILED DATA TO ACTUAL TILES
            //TODO:! Find a Way to Effectively Draw Textures. Like a Runtime Texture Atlas Packing with *Extrusions.
            //(2n - 1) * 4  Extra Padding
            int tileX, tileY;
            int currentGId, chunkID;
            int mainTex = Shader.PropertyToID("_MainTex");
            Tile tile;
            List<TileAnimation> m_TileAnimations = null;


            foreach (var layer in m_TmxMap.Layers)
            {
                if (layer.Visible)
                {
                    for (int i = 0; i < layer.Tiles.Count; i++)
                    {
                        //Tiles are listed from Top-Left(0,0)
                        //across Bottom-Right(mapWidth, mapHeight)
                        tileX = layer.Tiles[i].X;
                        tileY = m_TmxMap.Height - 1 - layer.Tiles[i].Y;
                        currentGId = layer.Tiles[i].Gid;
                        tile = m_Tilemap.Tiles[tileX, tileY];

                        //If this current GId points to an Animation in the Tileset
                        if (m_BaseGidToAnimation.ContainsKey(currentGId))
                        {
                            chunkID = tile.ParentChunk.id;

                            //If the Dictonary does not contain the current meshChunk
                            if (!m_chunkIdToAnimations.TryGetValue(chunkID, out m_TileAnimations))
                            {
                                m_chunkIdToAnimations.Add(chunkID, new List<TileAnimation>());
                                m_TileAnimations = m_chunkIdToAnimations[chunkID];
                            }

                            m_TileAnimations.Add(new TileAnimation(tile, m_BaseGidToAnimation[currentGId]));


                        }

                        tile.TextureData = m_GidToFrameData[currentGId].rect;
                        tile.penalty = m_GidToPenalty[currentGId];

                    }

                }
            }

            var keys = m_chunkIdToAnimations.Keys;
            for (int i = 0; i < m_Tilemap.FlattenedChunks.Length; i++)
            {
                m_Tilemap.FlattenedChunks[i].Material.SetTexture(mainTex, m_TextureTest);
                foreach (var id in keys)
                {
                    if (m_Tilemap.FlattenedChunks[i].id == id)
                    {
                        m_Tilemap.FlattenedChunks[i].TileAnimations = m_chunkIdToAnimations[id];
                    }
                }
            }



            m_BaseGidToAnimation = null;
            m_chunkIdToAnimations = null;
            m_GidToFrameData = null;
            m_GidToPenalty = null;
            m_TileAnimations = null;

            m_TmxMap = null;
        }
    }
}
