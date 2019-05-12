using UnityEngine;

namespace VH
{
    public struct FrameData
    {
        public Rect rect;
        public Vector2[] UV;

        public FrameData(Vector2[] UV = null, Rect rect = default)
        {
            this.rect = rect;
            this.UV = UV;
        }
    }
}
