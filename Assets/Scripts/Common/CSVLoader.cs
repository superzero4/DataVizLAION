using System.Threading.Tasks;
using Structures;
using UnityEngine;

public static class CSVLoader
{
    public static async Task<ImageInfo[]> LoadImageInfo(string path)
    {
        string[] lines = await System.IO.File.ReadAllLinesAsync(path);
        ImageInfo[] imageInfos = new ImageInfo[lines.Length - 1];
        for (int i = 1; i < lines.Length; i++)
        {
            string[] values = lines[i].Split(';');
            ImageInfo imageInfo = new ImageInfo();
            imageInfo.Name = values[0];
            imageInfo.Width = int.Parse(values[1]);
            imageInfo.Height = int.Parse(values[2]);
            imageInfo.averageColor = new Color(float.Parse(values[3]), float.Parse(values[4]), float.Parse(values[5]));
            imageInfo.averageHSV = new HSV();
            imageInfo.averageHSV.h = float.Parse(values[6]);
            imageInfo.averageHSV.s = float.Parse(values[7]);
            imageInfo.averageHSV.v = float.Parse(values[8]);
            imageInfo.sprite = Resources.Load<Sprite>("Images/" + imageInfo.Name);
            imageInfos[i - 1] = imageInfo;
            if (i % 50 == 0)
                await Task.Yield();
        }
        return imageInfos;
    }
}