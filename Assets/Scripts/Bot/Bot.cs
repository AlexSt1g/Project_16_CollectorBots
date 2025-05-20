using System;
using UnityEngine;

[RequireComponent(typeof(BotMover))]
public class Bot : MonoBehaviour
{
    [SerializeField] private Transform _resourceGrabPosition;

    private BotMover _mover;
    private Collider _resourceCollectingZone;      
    private bool _hasResource;
    private Resource _resource;

    public event Action ResourceDelivered;

    public bool HasTask { get; private set; }

    private void Awake()
    {
        _mover = GetComponent<BotMover>();
    }    

    private void OnTriggerEnter(Collider other)
    {
        if (_hasResource == false)
            if (other.TryGetComponent(out Resource resource))
                if (resource == _resource)
                {
                    GrabResource(resource);
                    _mover.MoveTo(_resourceCollectingZone.bounds.ClosestPoint(transform.position));
                }

        if (_hasResource && _resource != null)
        {
            if (other == _resourceCollectingZone)
            {
                ResourceDelivered?.Invoke();
                DropResource();
                HasTask = false;
            }
        }
    }

    public void Init(BotBase botBase)
    {        
        _resourceCollectingZone = botBase.ResourceCollectingZone;
    }

    public void CollectResource(Resource resource)
    {
        _resource = resource;
        _mover.MoveTo(resource.transform.position);
        HasTask = true;
    }

    private void GrabResource(Resource resource)
    {
        resource.PickUp(transform, _resourceGrabPosition.position);
        _hasResource = true;
    }

    private void DropResource()
    {
        _resource.Drop();
        _resource = null;
        _hasResource = false;
    }
}
