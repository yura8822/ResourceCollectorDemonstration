using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class SimulationSpeedUI : MonoBehaviour
{
    [Header("UI Elements")] 
    [SerializeField] private Slider _speedSlider;

    [Header("Speed Settings")] 
    [SerializeField] private float _minSpeed = 1f;
    [SerializeField] private float _maxSpeed = 2f;

    private void Start()
    {
        InitializeSlider();
    }

    private void OnDestroy()
    {
        _speedSlider.onValueChanged.RemoveListener(OnSpeedChanged);
        // Восстанавливаем нормальную скорость при выходе
        Time.timeScale = 1f;
    }

    private void InitializeSlider()
    {
        _speedSlider.minValue = _minSpeed;
        _speedSlider.maxValue = _maxSpeed;
        _speedSlider.value = 1f; // Начальная скорость

        _speedSlider.onValueChanged.AddListener(OnSpeedChanged);
    }

    private void OnSpeedChanged(float value)
    {
        Time.timeScale = value;
    }
}