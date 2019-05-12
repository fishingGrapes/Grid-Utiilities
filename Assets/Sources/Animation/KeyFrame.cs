
namespace VH
{
    /// <summary>
    /// A KeyFrame is the FrameID which is mostly a Key in a Map Containing UVs
    /// or Other Information about that fame of Animation and duartion for which that
    /// information is Extended.
    /// </summary>
    public struct KeyFrame
    {
        public FrameData Data;
        public ushort Duration;

        public KeyFrame(FrameData data, ushort duration)
        {
            this.Data = data;
            this.Duration = duration;
        }
    }
}
