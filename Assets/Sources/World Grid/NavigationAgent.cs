using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VH
{
    public struct NavigationAgent
    {
        public byte width;
        public byte height;
        public ushort range;

        public byte penaltyModifier;
    }
}
