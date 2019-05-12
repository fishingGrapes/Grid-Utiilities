using UnityEngine;

namespace VH
{

    public struct Intersection
    {
        public int x;
        public int y;

        public Vector3 position;

        public Intersection(int x, int y)
        {
            this.x = x;
            this.y = y;

            position = new Vector3();
        }
    }
}
