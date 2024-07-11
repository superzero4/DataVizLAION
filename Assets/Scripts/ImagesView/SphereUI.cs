using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Dropdown = TMPro.TMP_Dropdown;

public class SphereUI : MonoBehaviour
{
    [SerializeField] private ImageViewer _imageViewer;
    [SerializeField] private Dropdown _modeDropdown, _depthDropdown, _scaleDropdown;

    private void Awake()
    {
        SetDropdowns();
    }

    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.Q)) && Input.GetKey(KeyCode.LeftControl))
        {
            _modeDropdown.gameObject.SetActive(false);
            _scaleDropdown.gameObject.SetActive(false);
            _depthDropdown.gameObject.SetActive(false);
        }
    }

    [Button]
    private void SetDropdowns()
    {
        _modeDropdown.options = Enum.GetValues(typeof(ImageViewer.Mode))
            .Cast<ImageViewer.Mode>()
            .Select(x => new Dropdown.OptionData(x.ToString()))
            .ToList();
        _modeDropdown.value = (int)_imageViewer._mode;
        _modeDropdown.onValueChanged.AddListener((x) => _imageViewer.SetMode(x));
        _depthDropdown.options = Enum.GetValues(typeof(ImageViewer.AltMode))
            .Cast<ImageViewer.AltMode>()
            .Select(x => new Dropdown.OptionData(x.ToString()))
            .ToList();
        _depthDropdown.value = (int)_imageViewer._depthMode;
        _depthDropdown.onValueChanged.AddListener((x) => _imageViewer.SetDepthMode(x));

        _scaleDropdown.options = Enum.GetValues(typeof(ImageViewer.AltMode))
            .Cast<ImageViewer.AltMode>()
            .Select(x => new Dropdown.OptionData(x.ToString()))
            .ToList();
        _scaleDropdown.value = (int)_imageViewer._scaleMode;
        _scaleDropdown.onValueChanged.AddListener((x) => _imageViewer.SetScaleMode(x));
    }
}