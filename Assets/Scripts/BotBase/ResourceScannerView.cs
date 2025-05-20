using UnityEngine;

[RequireComponent(typeof(ResourceScanner))]
public class ResourceScannerView : MonoBehaviour
{
    [SerializeField] private ParticleSystem _scanVFXPrefab;

    private ResourceScanner _scanner;    

    private void Awake()
    {
        _scanner = GetComponent<ResourceScanner>();
    }

    private void OnEnable()
    {
        _scanner.Scanning += SpawnVFX;
    }

    private void OnDisable()
    {
        _scanner.Scanning -= SpawnVFX;
    }

    private void SpawnVFX()
    {
        ParticleSystem vfx = Instantiate(_scanVFXPrefab, transform.position, Quaternion.identity);
        Destroy(vfx.gameObject, vfx.main.duration);
    }
}
