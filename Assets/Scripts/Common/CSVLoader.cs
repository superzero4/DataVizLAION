using System.Threading.Tasks;
using Structures;
using UnityEngine;

public static class CSVLoader
{
    public static async Task<ImageInfo[]> LoadImageInfo(string path)
    {
        return await LoadImageInfo(await System.IO.File.ReadAllLinesAsync(path));
    }
    public static async Task<ImageInfo[]> LoadImageInfo(TextAsset text)
    {
        return await LoadImageInfo(text.text.Split('\n'));
    }

    public static async Task<ImageInfo[]> LoadImageInfo(string[] lines)
    {
        ImageInfo[] imageInfos = new ImageInfo[lines.Length - 1];
        for (int i = 1; i < lines.Length; i++)
        {
            string[] values = lines[i].Split(';');
            if (values.Length < 8)
                continue;
            ImageInfo imageInfo = new ImageInfo();
            imageInfo.Name = values[0];
            imageInfo.Width = int.Parse(values[1]);
            imageInfo.Height = int.Parse(values[2]);
            imageInfo.averageColor = new Color(float.Parse(values[3]), float.Parse(values[4]), float.Parse(values[5]));
            imageInfo.averageHSV = new HSV();
            imageInfo.averageHSV.h = float.Parse(values[6]);
            imageInfo.averageHSV.s = float.Parse(values[7]);
            imageInfo.averageHSV.v = float.Parse(values[8]);
            var name = imageInfo.Name;
            var withoutExt = string.Join(".",name.Split( '.')[0..^1]);
            imageInfo.sprite = Resources.Load<Sprite>("Images/" + withoutExt);
            Debug.Log(name+" => Trying to load image: " + withoutExt + ", result ? : " + (imageInfo.sprite != null));
            imageInfos[i - 1] = imageInfo;
            if (i % 50 == 0)
                await Task.Yield();
        }
        Debug.LogWarning("Done ! Loaded " + imageInfos.Length + " images.");
        return imageInfos;
    }
}