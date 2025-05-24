using UnityEngine;

[RequireComponent(typeof(ResourceScanner))]
public class ResourceScannerView : MonoBehaviour
{
    [SerializeField] private ParticleSystem _scanVFX;

    private ResourceScanner _scanner;    

    private void Awake()
    {
        _scanner = GetComponent<ResourceScanner>();        
    }

    private void OnEnable()
    {
        _scanner.Scanning += PlayVFX;
    }

    private void OnDisable()
    {
        _scanner.Scanning -= PlayVFX;
    }

    private void PlayVFX()
    {
        _scanVFX.Play();
    }
}
