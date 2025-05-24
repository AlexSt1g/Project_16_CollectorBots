using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ResourceDataBase : MonoBehaviour
{
    private List<Resource> _foundResources = new();
    private List<Resource> _engagedResources = new();

    private void OnDisable()
    {
        foreach (var resource in _engagedResources)
            resource.DeliveredToCollectingZone -= OnResourceDelivered;
    }

    public void AddResources(List<Resource> resources)
    {
        _foundResources.Clear();
        _foundResources.AddRange(resources);
    }

    public List<Resource> GetFreeResources()
    {
        return _foundResources.Except(_engagedResources).ToList();
    }

    public void EngageResource(Resource resource)
    {
        _engagedResources.Add(resource);

        resource.DeliveredToCollectingZone += OnResourceDelivered;
    }

    private void OnResourceDelivered(Resource resource)
    {
        _engagedResources.Remove(resource);
    }
}
