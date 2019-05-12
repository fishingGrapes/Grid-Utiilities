using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VH
{
    [ExecuteAfter(typeof(TilemapChunkCuller))]
    public class TilemapAnimator : ComponentBehaviour
    {
        [SerializeField]
        private Tilemap tileMap = null;

        private TilemapChunk chunk;
        private List<TileAnimation> m_TileAnimations;
        private int animationCount;

        protected override void Awake()
        {
            base.Awake();
        }

        private void Start()
        {

        }

        public override void FixedTick()
        {
            for (int i = 0; i < tileMap.FlattenedChunks.Length; i++)
            {
                chunk = tileMap.FlattenedChunks[i];
                if (!chunk.Visible) continue;

                m_TileAnimations = chunk.TileAnimations;
                if (m_TileAnimations == null)
                    continue;

                animationCount = m_TileAnimations.Count;
                for (int j = 0; j < animationCount; j++)
                {
                    m_TileAnimations[j].Update();
                }
            }
        }
    }
}
