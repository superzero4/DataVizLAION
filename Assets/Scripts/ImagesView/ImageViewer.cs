using System.Collections;
using System.Collections.Generic;
using Structures;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class ImageViewer : MonoBehaviour
{
    public Image imagePrefab;
    [FormerlySerializedAs("imagesInfo")] public ImagesInfo images;
    public void Awake()
    {
        Init();
    }
    [Button]
    public void Init()
    {
        if (images.images != null)
        {
            foreach (Structures.ImageInfo imageInfo in images.images)
            {
                GameObject image = Instantiate(imagePrefab, content);
                image.GetComponent<UnityEngine.UI.Image>().sprite = imageInfo.sprite;
            }
            images.images = null;
        }
    }
}
