using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(FactionMarkerUI))]
[RequireComponent(typeof(PathVisualizer))]
public class HarvesterController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _movementSpeed = 5f;
    [SerializeField] private float _rotationSpeed = 120f;
    [SerializeField] private float _acceleration = 8f;
    
    [Header("Visuals")]
    [SerializeField] private GameObject _cargoVisual;
    
    [Header("Collection Settings")]
    [SerializeField] private float _collectionDuration  = 2f;
    [SerializeField] private float _unloadDuration = 0.5f;
    
    public UnityAction<HarvesterController> onDroneDestroyed;

    private StateMachine _stateMachine;
    private IPathfindingProvider _pathFindingProvider;
    private PathVisualizer _pathVisualizer;
    private bool _hasResource;


    public BaseController ParentBase { get; set; }
    public Loot LootTarget { get; set; }
    
    public float MovementSpeed => _movementSpeed;
    public float RotationSpeed => _rotationSpeed;
    public float Acceleration => _acceleration;
    
    public float CollectionDuration => _collectionDuration;
    public float UnloadDuration => _unloadDuration;

    public IPathfindingProvider PathFindingProvider => _pathFindingProvider;
    public PathVisualizer PathVisualizer => _pathVisualizer;
    public Vector3 Position => transform.position;
    
    public bool HasResource
    {
        get => _hasResource;
        set
        {
            _hasResource = value;
            UpdateCargoVisual();
        }
    }

    private void Start()
    {
        InitializePathFindingProvider();
        InitializePathFindingVisualiser(_pathFindingProvider);
        InitializeStateMachine();
        UpdateCargoVisual();
    }

    public void Destroy()
    {
        onDroneDestroyed?.Invoke(this);
        Destroy(gameObject);
    }

    private void InitializeStateMachine()
    {
        if (_stateMachine == null)
        {
            GameObject sm = new GameObject("HarvesterStateMachine");
            sm.transform.SetParent(transform);
            _stateMachine = sm.AddComponent<StateMachine>();
        }

        _stateMachine.RegisterState(new SearchResourceState(_stateMachine, this));
        _stateMachine.RegisterState(new MoveToResourceState(_stateMachine, this));
        _stateMachine.RegisterState(new CollectResourceState(_stateMachine, this));
        _stateMachine.RegisterState(new ReturnToBaseState(_stateMachine, this));
        _stateMachine.RegisterState(new UnloadResourceState(_stateMachine, this));

        _stateMachine.StartStateMachine<SearchResourceState>();
    }

    private void InitializePathFindingProvider()
    {
        _pathFindingProvider = GetComponent<IPathfindingProvider>();
        if (_pathFindingProvider == null)
        {
            Debug.LogError("HarvesterController: Path Finding Provider not found");
            return;
        }
        _pathFindingProvider.Initialize(_movementSpeed, _rotationSpeed, _acceleration);
    }

    private void InitializePathFindingVisualiser(IPathfindingProvider pathfindingProvider)
    {
        _pathVisualizer = GetComponent<PathVisualizer>();
        _pathVisualizer.Initialize(_pathFindingProvider);
    }
    
    private void UpdateCargoVisual()
    {
        if (_cargoVisual != null)
        {
            _cargoVisual.SetActive(_hasResource);
        }
    }
}