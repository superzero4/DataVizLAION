using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Structures 
{
    public struct ImageInfo
    {
        //Name;Width;Height;Average Red;Average Green;Average Blue;Average Hue;Average Saturation;Average Value;
        public string Name;
        public int Width;
        public int Height;
        public Color averageColor;
        public HSV averageHSV;
    }

    public struct HSV
    {
        public float h, s, v;
    }
}
