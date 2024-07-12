using UnityEngine;
using UnityEngine.UI;

public class ImagePanel : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private Canvas _canvas;
    public void SetImage(Sprite sprite, int sortOrder)
    {
        if(_image != null)
            _image.sprite = sprite;
        if (_sprite != null)
            _sprite.sprite = sprite;
        _sprite.sortingOrder = 0;
    }
    public void SetCamera(Camera camera)
    {
        if(_canvas != null)
            _canvas.worldCamera = camera;
    }
}