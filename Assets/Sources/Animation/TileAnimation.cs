namespace VH
{
    /// <summary>
    /// A Warpper around the Frame Animation clasas for ANimating Tiles
    /// </summary>
    public struct TileAnimation
    {
        /// <summary>
        /// The Tile which is being Animated
        /// </summary>
        public Tile tile;

        /// <summary>
        /// The Frame Animation on the Tile 
        /// </summary>
        public FrameAnimation frameAnimation;

        public TileAnimation(Tile tile, FrameAnimation frameAnimation)
        {
            this.tile = tile;
            this.frameAnimation = frameAnimation;
        }

        public void Update()
        {
            frameAnimation.Update();
            tile.TextureData = frameAnimation.CurrentFrame.Data.rect;
        }
    }
}