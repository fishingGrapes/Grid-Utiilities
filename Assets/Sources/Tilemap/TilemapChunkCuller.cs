using UnityEngine;

namespace VH
{
    /// <summary>
    /// Can recive callbacks based on the Visbility of a Bounds
    /// </summary>
    [ExecuteAfter(typeof(TilemapGenerator))]
    public class TilemapChunkCuller : ComponentBehaviour
    {
        [SerializeField]
        private Tilemap tileMap = null;
        private CullingGroup cullingGroup;

        protected override void Awake()
        {
            base.Awake();
            this.InitializeCullingGroup();
        }

        private void InitializeCullingGroup()
        {

            cullingGroup = new CullingGroup();
            BoundingSphere[] cullingPoints = new BoundingSphere[tileMap.FlattenedChunks.Length];

            for (int i = 0; i < tileMap.FlattenedChunks.Length; i++)
            {
                cullingPoints[i].position = tileMap.FlattenedChunks[i].TRS.GetColumn(3);
                cullingPoints[i].radius = tileMap.ChunkSize * 0.5f;

            }

            cullingGroup.onStateChanged = CullingEvent;
            cullingGroup.SetBoundingSpheres(cullingPoints);
            cullingGroup.targetCamera = Camera.main;
            cullingGroup.SetBoundingSphereCount(cullingPoints.Length);
        }


        private void OnDestroy()
        {
            cullingGroup.Dispose();
            cullingGroup = null;
        }

        private void CullingEvent(CullingGroupEvent cullingGroupEvent)
        {
            tileMap.FlattenedChunks[cullingGroupEvent.index].Visible = cullingGroupEvent.isVisible;
        }
    }
}
