using UnityEngine;
using UnityEngine.UI;

public class ImagePanel : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private Canvas _canvas;
    public void SetImage(Sprite sprite)
    {
        _image.sprite = sprite;
    }
    public void SetCamera(Camera camera)
    {
        _canvas.worldCamera = camera;
    }
}