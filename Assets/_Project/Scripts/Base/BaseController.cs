using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BaseUI))]
public class BaseController : MonoBehaviour
{
   [Header("Base Settings")]
   [SerializeField] private Color _factionColor;
   [SerializeField] private HarvesterController harvesterPrefab;
   [SerializeField] private Transform _spawnTransform;
   [SerializeField] private Transform _unloadTransform;

   private List<HarvesterController> _harvesters = new();
   private int _collectedLoot;
   
   public Transform UnloadTransform => _unloadTransform;
   public Color FactionColor => _factionColor;
   
   public UnityAction<int> OnResourcesChanged;
   
    
   public void SpawnDrone()
   {
      HarvesterController harvester = Instantiate(
         harvesterPrefab, 
         _spawnTransform.position, 
         _spawnTransform.rotation
      );
      
      harvester.transform.SetParent(transform);
      harvester.onDroneDestroyed += OnDroneDestroyed;
      harvester.ParentBase = this;
     
      _harvesters.Add(harvester);
   }

   public void AddLoot()
   {
      _collectedLoot++;
      OnResourcesChanged?.Invoke(_collectedLoot);
   }
    
   private void OnDroneDestroyed(HarvesterController harvester)
   {
      harvester.onDroneDestroyed -= OnDroneDestroyed;
      _harvesters.Remove(harvester);
   }

}
