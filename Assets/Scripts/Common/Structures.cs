using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
namespace Structures
{
    [CreateAssetMenu(fileName = "Images", menuName = "ImagesInfo")]
    public class ImagesInfo : ScriptableObject
    {
        [SerializeField] private TextAsset _csv;
        public ImageInfo[] images;
        [Button("Using TextAsset field csv")]
        public async void LoadFromCSV()
        {
            this.images = await CSVLoader.LoadImageInfo(_csv);
        }
    }
    [System.Serializable]
    public struct ImageInfo
    {
        //Name;Width;Height;Average Red;Average Green;Average Blue;Average Hue;Average Saturation;Average Value;
        public string Name;
        public int Width;
        public int Height;
        public Color averageColor;
        public HSV averageHSV;
        public Sprite sprite;
        override public string ToString()
        {
            return Name + " " + Width + "x" + Height + " " + averageColor + " " + averageHSV;
        }
    }
    [Serializable]
    public struct HSV
    {
        public float h, s, v;
        public override  string ToString()
        {
            return "HSV(" + h + "," + s + "," + v + ")";
        }
    }
}