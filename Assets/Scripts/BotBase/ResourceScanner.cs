using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourceScanner : MonoBehaviour
{
    [SerializeField] private Transform _scanningArea;
    [SerializeField] private LayerMask _layerMask;

    private List<Resource> _foundResources = new();

    public event Action<List<Resource>> Found;
    public event Action Scanning;

    [field: SerializeField] public float Delay { get; private set; } = 5f;

    public void Scan()
    {
        Scanning?.Invoke();

        _foundResources.Clear();

        Collider[] others = Physics.OverlapBox(transform.position, GetScanningAreaSize(), Quaternion.identity, _layerMask);
        
        foreach (Collider collider in others)
            if (collider.TryGetComponent(out Resource resource))
                if (resource.IsFreeForCollecting)
                    _foundResources.Add(resource);

        if (_foundResources.Count > 0)
            Found?.Invoke(_foundResources);
    }

    private Vector3 GetScanningAreaSize()
    {
        int planeScaleMultiplier = 10;
        int halfExtentsDivider = 2;

        return _scanningArea.localScale * planeScaleMultiplier / halfExtentsDivider;
    }
}
