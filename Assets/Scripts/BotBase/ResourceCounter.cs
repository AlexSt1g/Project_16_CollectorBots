using System;
using UnityEngine;

public class ResourceCounter : MonoBehaviour
{
    private int _count;

    public event Action<int> Changed;

    public void Add()
    {
        _count++;
        Changed?.Invoke(_count);
    }

    public void Reset()
    {
        _count = 0;
        Changed?.Invoke(_count);
    }
}
