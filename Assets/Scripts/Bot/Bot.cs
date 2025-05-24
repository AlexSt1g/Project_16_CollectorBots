using System;
using UnityEngine;

[RequireComponent(typeof(BotMover))]
[RequireComponent(typeof(BotResourceCollector))]
public class Bot : MonoBehaviour
{
    private BotMover _mover;
    private BotResourceCollector _collector;
    private Collider _resourceCollectingZone;

    public event Action ResourceDelivered;

    public bool HasTask { get; private set; }

    private void Awake()
    {
        _mover = GetComponent<BotMover>();
        _collector = GetComponent<BotResourceCollector>();
    }

    private void OnEnable()
    {
        _collector.ResourcePickedUp += OnResourcePickUp;
        _collector.ResourceDelivered += OnResourceDelivered;
    }

    private void OnDisable()
    {
        _collector.ResourcePickedUp -= OnResourcePickUp;
        _collector.ResourceDelivered -= OnResourceDelivered;
    }    

    public void Init(Collider resourceCollectingZone)
    {
        _resourceCollectingZone = resourceCollectingZone;
        _collector.Init(resourceCollectingZone);
    }

    public void CollectResource(Resource resource)
    {        
        _collector.SetResourceToCollect(resource);
        _mover.MoveTo(resource.transform.position);
        HasTask = true;
    }

    private void OnResourcePickUp()
    {
        _mover.MoveTo(_resourceCollectingZone.bounds.ClosestPoint(transform.position));
    }

    private void OnResourceDelivered()
    {
        ResourceDelivered?.Invoke();
        HasTask = false;
    }
}
