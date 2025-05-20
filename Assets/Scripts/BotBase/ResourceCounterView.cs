using TMPro;
using UnityEngine;

[RequireComponent(typeof(ResourceCounter))]
[RequireComponent(typeof(Canvas))]
public class ResourceCounterView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    private ResourceCounter _counter;
    private Camera _pointOfViewCamera;

    private void Awake()
    {
        _counter = GetComponent<ResourceCounter>();
        _pointOfViewCamera = GetComponent<Canvas>().worldCamera;
    }

    private void Update()
    {        
        TurnToCamera();
    }

    private void OnEnable()
    {
        _counter.Changed += OnCountChange;
    }

    private void OnDisable()
    {
        _counter.Changed -= OnCountChange;
    }

    private void OnCountChange(int value)
    {
        _text.text = value.ToString();
    }

    private void TurnToCamera()
    {
        if (transform.rotation != _pointOfViewCamera.transform.rotation)
            transform.rotation = _pointOfViewCamera.transform.rotation;
    }
}
