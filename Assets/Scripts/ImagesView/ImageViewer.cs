using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using Sirenix.Utilities;
#endif
using Structures;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class ImageViewer : MonoBehaviour
{
    public enum Mode
    {
        HSV,
        RGB,
        Size,
        RBG,
        BGR
    }

    public enum AltMode
    {
        None,
        Size,
        Saturation,
        Color
    }

    [SerializeField] public Mode _mode;
    [SerializeField] private bool _useEventCanvas = false;

    public void SetMode(Mode mode)
    {
        _mode = mode;
        PlaceImages();
    }

    public void SetMode(int mode)
    {
        SetMode((Mode)mode);
    }

    [SerializeField] public AltMode _depthMode;

    public void SetDepthMode(int mode)
    {
        SetDepthMode((AltMode)mode);
    }

    public void SetDepthMode(AltMode mode)
    {
        _depthMode = mode;
        PlaceImages();
    }

    [SerializeField] public AltMode _scaleMode;

    public void SetScaleMode(AltMode mode)
    {
        _scaleMode = mode;
        PlaceImages();
    }

    public void SetScaleMode(int mode)
    {
        SetScaleMode((AltMode)mode);
    }

    [SerializeField] private Transform _parent;
    [SerializeField] ImagesInfo _images;
    [SerializeField] ImagePanel _panelPrefab;
    [SerializeField] private List<ImagePanel> _panels;
    [SerializeField] private Camera _mainCamera;
#if UNITY_EDITOR
    [Button,
     InfoBox("Will make same operations as entering playmode basically doing everything needed", InfoMessageType.Info)]
#endif
    public void FullRoutine(int max = -1, float radius = 25)
    {
        SetImages(max);
        PlaceImages(radius);
    }
#if UNITY_EDITOR
    [Button]
#endif
    void ClearPanels() => InstantiatePanels(0);
#if UNITY_EDITOR
    [Button]
#endif
    public void InstantiatePanels(int count = 90)
    {
        if (_panels.Count > count)
        {
            int begin = count;
            int end = _panels.Count;
            for (int i = begin; i < end; i++)
            {
                DestroyImmediate(_panels[i].gameObject);
            }

            _panels.RemoveRange(begin, end - begin);
            //_panels.SetLength(count);
        }
        else
        {
            for (int i = _panels.Count; i < count; i++)
            {
                ImagePanel panel = Instantiate(_panelPrefab, _parent);
                _panels.Add(panel);
            }
        }
    }
#if UNITY_EDITOR
    [Button]
#endif
    public void PlaceImages(float radius = 25)
    {
        for (int im = 0; im < Mathf.Min(_images.images.Length, _panels.Count); im++)
        {
            var tr = _panels[im].transform;
            Placement(radius, _images.images[im], out Vector3 position, out Vector3 scale, out Vector3 forward);
            tr.localPosition = position;
            tr.localScale = scale;
            tr.forward = forward;
            _panels[im].SetCamera(_useEventCanvas ? _mainCamera : null);
        }
    }

    public void Placement(float radius, ImageInfo info, out Vector3 position, out Vector3 scale, out Vector3 forward)
    {
        var color = info.averageHSV;
        position = new Vector3(color.h, color.v, 0);
        CalculatePosition(info, out float inclination, out float azimuth);
        position = FromSpherical(CalculateRadius(info, radius), inclination, azimuth);
        scale = CalculateScale(info);
        //We are centered on 0 so facing direction is position vector actually
        forward = position.normalized;
        //Debug.Log($"{info} => Position: {position}, Scale: {scale}, Forward: {forward}");
    }

    private Vector3 CalculateScale(ImageInfo info)
    {
        //We normalize all images by default before applygin a mult (which can actually take zie)
        Vector3 scale = new Vector3((maxW * 1f) / (info.Width), (maxH * 1f / info.Height), 1);
        if (info.Width == 0)
            scale.x = 1;
        if (info.Height == 0)
            scale.y = 1;
        float mult = 1f;
        switch (_scaleMode)
        {
            case AltMode.None:
                break;
            case AltMode.Color:
                mult = GetThirdComponent(info);
                break;
            case AltMode.Saturation:
                mult = info.averageHSV.s;
                break;
            case AltMode.Size:
                mult = (info.Width * info.Height) * 1f / (maxW * maxH);
                break;
        }

        return mult * scale;
    }

    private void CalculatePosition(ImageInfo image, out float inclination, out float azimuth)
    {
        inclination = image.averageHSV.h;
        azimuth = image.averageHSV.v;
        switch (_mode)
        {
            case Mode.HSV:
                inclination = image.averageHSV.h;
                azimuth = image.averageHSV.v;
                break;
            case Mode.RGB:
                inclination = image.averageColor.r;
                azimuth = image.averageColor.g;
                break;
            case Mode.RBG:
                inclination = image.averageColor.r;
                azimuth = image.averageColor.b;
                break;
            case Mode.BGR:
                inclination = image.averageColor.b;
                azimuth = image.averageColor.g;
                break;
            case Mode.Size:
                inclination = image.Width * 1f / maxW;
                azimuth = image.Height * 1f / maxH;
                break;
            default:
                Debug.LogError($"Mode {_mode} not handled yet.");
                break;
        }
    }

    public const int maxH = 1080;
    public const int maxW = 1920;

    private float CalculateRadius(ImageInfo info, float baseRadius)
    {
        float rad = 1;
        switch (_depthMode)
        {
            case AltMode.None:
                break;
            case AltMode.Size:
                rad = (info.Width * info.Height) * 1f / (maxW * maxH);
                break;
            case AltMode.Saturation:
                rad = info.averageHSV.s + .5f;
                break;
            case AltMode.Color:
                rad = (GetThirdComponent(info) / 2f) + .5f;
                break;
        }

        return rad * baseRadius;
    }

    private float GetThirdComponent(ImageInfo info)
    {
        float m;
        switch (_mode)
        {
            case Mode.RGB:
                m = info.averageColor.b;
                break;
            case Mode.RBG:
                m = info.averageColor.g;
                break;
            case Mode.BGR:
                m = info.averageColor.r;
                break;
            default:
                m = (info.averageColor.r + info.averageColor.g + info.averageColor.b) / 3;
                break;
        }

        return m;
    }

    public Vector3 FromSpherical(float r, float relativeInclinaison, float relativeAzimuth)
    {
        var theta = relativeInclinaison * Mathf.PI;
        var phi = relativeAzimuth * Mathf.PI * 2;
        return new Vector3(
            r * Mathf.Sin(theta) * Mathf.Cos(phi),
            r * Mathf.Sin(theta) * Mathf.Sin(phi),
            r * Mathf.Cos(theta)
        );
    }
#if UNITY_EDITOR
    [Button]
#endif
    public void SetImages(int max = -1)
    {
        InstantiatePanels(max < 0 ? _images.images.Length : Mathf.Min(max, _images.images.Length));
        if (_images.images != null)
        {
            for (int im = 0; im < Mathf.Min(_images.images.Length, _panels.Count); im++)
            {
                _panels[im].SetImage(_images.images[im].sprite, im);
            }
        }
    }

    public string InfoText()
    {
        return $"Mapping of {_mode} with "+(_depthMode==AltMode.None ? "constant distance from center" : (_depthMode.ToString() + " as value for distance from center")) +  (_scaleMode==AltMode.None ? " " : (" and image scale based on their "+_scaleMode.ToString()));
    }
}