using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
namespace Structures
{
    [CreateAssetMenu(fileName = "Images", menuName = "ImagesInfo")]
    public class ImagesInfo : ScriptableObject
    {
        public ImageInfo[] images;
        [Button]
        public async void LoadFromCSV(string path="./ImagesInfo.csv")
        {
            this.images = await CSVLoader.LoadImageInfo(path);
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
    }

    public struct HSV
    {
        public float h, s, v;
    }
}