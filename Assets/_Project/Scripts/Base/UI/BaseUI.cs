using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BaseUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Canvas _canvas;
    [SerializeField] private TextMeshProUGUI _lootCountText;
    [SerializeField] private Button _spawnHarvesterButton;
    [SerializeField] private Image _factionMarker;

    private BaseController _baseController;
    private Camera _camera;


    private void Awake()
    {
        _camera = Camera.main;
        _baseController = GetComponent<BaseController>();
    }

    private void Start()
    {
        _canvas.worldCamera = _camera;
        
        _baseController.OnResourcesChanged += UpdateResourceCount;
        _spawnHarvesterButton.onClick.AddListener(OnSpawnHarvesterClicked);
        
        UpdateFactionMarker();
        UpdateResourceCount(0);
    }

    private void OnDestroy()
    {
        _baseController.OnResourcesChanged -= UpdateResourceCount;
        _spawnHarvesterButton.onClick.RemoveListener(OnSpawnHarvesterClicked);
    }

    private void Update()
    {
        _canvas.transform.forward = _camera.transform.forward;
    }

    private void OnSpawnHarvesterClicked()
    {
        _baseController.SpawnDrone();
    }

    private void UpdateResourceCount(int count)
    {
        _lootCountText.text = $"Collected: {count}";
    }

    private void UpdateFactionMarker()
    {
        _factionMarker.color = _baseController.FactionColor;
    }
}
