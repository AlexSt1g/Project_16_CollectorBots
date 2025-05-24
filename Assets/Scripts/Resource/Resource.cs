using System;
using UnityEngine;

public class Resource : MonoBehaviour
{    
    public event Action<Resource> DeliveredToCollectingZone;

    public void Init(Vector3 spawnPoint)
    {
        transform.position = spawnPoint;
    }

    public void PickUp(Transform parent, Vector3 grabPosition)
    {
        transform.SetParent(parent);        
        transform.position = grabPosition;
    }

    public void Drop()
    {
        transform.SetParent(null);
        DeliveredToCollectingZone?.Invoke(this);
    }
}
