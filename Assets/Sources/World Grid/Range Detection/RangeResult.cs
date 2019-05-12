using System;

namespace VH.RangeDetection
{

    public interface RangeResult
    {
        Action<Tile[]> callback { get; set; }
        Tile[] tiles { get; set; }
        ushort[] distances { get; set; }
    }

    public struct MovementRangeResult : RangeResult
    {
        public Tile[] tiles { get; set; }
        public Action<Tile[]> callback { get; set; }
        public ushort[] distances { get; set; }

        public MovementRangeResult(Tile[] tiles, ushort[] distances, Action<Tile[]> callback)
        {
            this.tiles = tiles;
            this.callback = callback;
            this.distances = distances;
        }
    }

    public struct LineRangeResult : RangeResult
    {
        public Tile[] tiles { get; set; }
        public Action<Tile[]> callback { get; set; }
        public ushort[] distances { get; set; }

        public LineRangeResult(Tile[] tiles, ushort[] distances, Action<Tile[]> callback)
        {
            this.tiles = tiles;
            this.callback = callback;
            this.distances = distances;
        }
    }


    public struct ConeRangeResult : RangeResult
    {
        public Tile[] tiles { get; set; }
        public Action<Tile[]> callback { get; set; }
        public ushort[] distances { get; set; }

        public ConeRangeResult(Tile[] tiles, ushort[] distances, Action<Tile[]> callback)
        {
            this.tiles = tiles;
            this.callback = callback;
            this.distances = distances;
        }
    }

    public struct CircleRangeResult : RangeResult
    {
        public Tile[] tiles { get; set; }
        public Action<Tile[]> callback { get; set; }
        public ushort[] distances { get; set; }

        public CircleRangeResult(Tile[] tiles, ushort[] distances, Action<Tile[]> callback)
        {
            this.tiles = tiles;
            this.callback = callback;
            this.distances = distances;
        }
    }
}