namespace VH
{
    public class FrameAnimation
    {
        #region public Accessors

        /// <summary>
        /// The Actual KeyFrames containing the FrameData.
        /// </summary>
        public KeyFrame[] Frames;

        /// <summary>
        /// The Total Duration of this Animation in MilliSeconds.
        /// </summary>
        public ushort Duration;

        /// <summary>
        /// The Total Number in Frames in this Animation.
        /// </summary>
        public ushort FrameCount;

        /// <summary>
        /// Whether the Animation is Updating or Not.
        /// </summary>
        public bool Animating;

        /// <summary>
        /// The Frame in the Current Update Cycle of the Animation.
        /// </summary>
        public KeyFrame CurrentFrame { get { return Frames[n_CurrentFrameIndex]; } }

        /// <summary>
        /// The Number of times this Animation is Set to repeat.
        /// If set to -1, then the Animation is repeated indefinitely.
        /// </summary>
        public short Repeat { set { Looping = value >= 0 ? false : true; } }

        /// <summary>
        /// Whether the Animation is Looping or Not.
        /// </summary>
        public bool Looping;

        #endregion

        #region Private Fields

        private int n_CurrentFrameIndex;
        private float f_TimeElapsed;

        #endregion



        /// <summary>
        /// 
        /// </summary>
        /// <param name="frames">The Keyframes thatmek up the Animation.</param>
        /// <param name="repeat">If -1, then the Animation keeps Looping.</param>
        public FrameAnimation(KeyFrame[] frames, short repeat = -1)
        {
            this.Frames = frames;
            this.Animating = true;
            this.FrameCount = (ushort)frames.Length;
            this.Repeat = repeat;
            this.Looping = repeat >= 0 ? false : true;

            for (int i = 0; i < FrameCount; i++)
            {
                this.Duration += Frames[i].Duration;
            }

            n_CurrentFrameIndex = 0;
            f_TimeElapsed = 0;
        }


        public void Update()
        {

            if (!Animating)
                return;

            //TODO:! Find the Correct way of doing this.
            f_TimeElapsed += (UnityEngine.Time.fixedDeltaTime * 5f);

            if (f_TimeElapsed >= Frames[n_CurrentFrameIndex].Duration)
            {

                n_CurrentFrameIndex += 1;
                f_TimeElapsed = 0;
            }

            n_CurrentFrameIndex = (n_CurrentFrameIndex >= FrameCount) ? 0 : n_CurrentFrameIndex;

        }
    }
}
