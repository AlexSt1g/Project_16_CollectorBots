using System;
using UnityEngine;

public class BotResourceCollector : MonoBehaviour
{
    [SerializeField] private Transform _resourceGrabPosition;

    private Resource _resource;
    private bool _hasResource;
    private Collider _resourceCollectingZone;

    public event Action ResourcePickedUp;
    public event Action ResourceDelivered;

    private void OnTriggerEnter(Collider other)
    {
        if (_hasResource == false)
            if (other.TryGetComponent(out Resource resource))
                if (resource == _resource)
                {
                    Grab(resource);
                    ResourcePickedUp?.Invoke();
                }

        if (_hasResource && _resource != null)
        {
            if (other == _resourceCollectingZone)
            {
                ResourceDelivered?.Invoke();
                Drop();
            }
        }
    }

    public void Init(Collider resourceCollectingZone)
    {
        _resourceCollectingZone = resourceCollectingZone;
    }

    public void SetResourceToCollect(Resource resource)
    {
        _resource = resource;
    }

    private void Grab(Resource resource)
    {
        resource.PickUp(transform, _resourceGrabPosition.position);
        _hasResource = true;
    }

    private void Drop()
    {
        _resource.Drop();
        _resource = null;
        _hasResource = false;
    }
}
