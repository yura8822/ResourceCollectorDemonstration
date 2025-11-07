using System;
using UnityEngine;
using UnityEngine.UI;

public class FactionMarkerUI : MonoBehaviour
{
    [Header("UI Elements")] 
    [SerializeField] private Canvas _canvas;
    [SerializeField] private Image _factionMarker;

    private HarvesterController _harvesterController;
    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
        _harvesterController = GetComponent<HarvesterController>();
    }

    private void Start()
    {
        _camera = Camera.main;
        UpdateFactionMarker();
    }

    private void Update()
    {
        _canvas.transform.forward = _camera.transform.forward;
    }

    private void UpdateFactionMarker()
    {
        _factionMarker.color = _harvesterController.ParentBase.FactionColor;
    }
}