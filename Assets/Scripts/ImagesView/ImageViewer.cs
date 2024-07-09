using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Structures;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class ImageViewer : MonoBehaviour
{
    [SerializeField] private Transform _parent;
    [SerializeField] ImagesInfo _images;
    [SerializeField] ImagePanel _panelPrefab;
    [SerializeField] private List<ImagePanel> _panels;

    public void Awake()
    {
        FullRoutine();
    }
    [Button,InfoBox("Will make same operations as entering playmode basically doing everything needed",InfoMessageType.Info)]
    public void FullRoutine(int max =-1,float radius=25)
    {
        SetImages(max);
        PlaceImages(radius);
    }

    [Button]
    void ClearPanels() => InstantiatePanels(0);
    [Button]
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

            _panels.SetLength(count);
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
    [Button]
    public void PlaceImages(float radius = 25)
    {
        for(int im=0;im<Mathf.Min(_images.images.Length,_panels.Count);im++)
        {
            var tr = _panels[im].transform;
            Placement(radius,_images.images[im], out Vector3 position, out Vector3 scale, out Vector3 forward);
            tr.localPosition = position;
            tr.localScale = scale;
            tr.forward = forward;
        }
    }

    public void Placement(float radius, ImageInfo info, out Vector3 position, out Vector3 scale, out Vector3 forward)
    {
        var color = info.averageHSV;
        position = new Vector3(color.h, color.v, 0);
        position = FromSpherical(radius, color.h, color.v);
        scale = Vector3.one * (info.averageHSV.s+.5f);
        //We are centered on 0 so facing direction is position vector actually
        forward = position.normalized;
        Debug.Log($"{info} => Position: {position}, Scale: {scale}, Forward: {forward}");
        
    }
    public Vector3 FromSpherical(float r, float relativeInclinaison, float relativeAzimuth)
    {
        var theta=relativeInclinaison*Mathf.PI;
        var phi=relativeAzimuth*Mathf.PI*2;
        return new Vector3(
            r * Mathf.Sin(theta) * Mathf.Cos(phi),
            r * Mathf.Sin(theta) * Mathf.Sin(phi),
            r * Mathf.Cos(theta)
        );
    }
    [Button]
    public void SetImages(int max=-1)
    {
        InstantiatePanels(max < 0 ? _images.images.Length : Mathf.Min(max, _images.images.Length));
        if (_images.images != null)
        {
            for(int im=0;im<Mathf.Min(_images.images.Length,_panels.Count);im++)
            {
                _panels[im].SetImage(_images.images[im].sprite);
            }
        }
    }
}