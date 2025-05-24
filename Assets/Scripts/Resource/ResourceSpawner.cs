using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class ResourceSpawner : MonoBehaviour
{
    [SerializeField] private Resource _prefab;
    [SerializeField] private float _delay = 3f;
    [SerializeField] private Transform _spawningArea;
    [SerializeField] private LayerMask _freeSpaceLayerMaskFilter;
    
    private ObjectPool<Resource> _pool;
    private int _poolCapacity = 50;
    private int _poolMaxSize = 100;

    private void Awake()
    {
        _pool = new ObjectPool<Resource>(
            createFunc: () => Instantiate(_prefab),
            actionOnGet: (resource) => HandleResourcePlacement(resource),
            actionOnRelease: (resource) => DisableResource(resource),
            actionOnDestroy: (resource) => Destroy(resource),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize);
    }

    private void Start()
    {
        StartCoroutine(GetResourceWithDelay());
    }

    private IEnumerator GetResourceWithDelay()
    {
        WaitForSeconds wait = new(_delay);

        while (enabled)
        {
            yield return wait;
            _pool.Get();
        }
    }

    private void HandleResourcePlacement(Resource resource)
    {
        if (TryGetSpawnPoint(out Vector3 spawnPoint))
        {
            resource.Init(spawnPoint);
            resource.gameObject.SetActive(true);

            resource.DeliveredToCollectingZone += ReleaseResource;
        }
        else
        {
            ReleaseResource(resource);

            Debug.Log("Failed to spawn resource. There are too many resources in the spawing area.");
        }
    }

    private void DisableResource(Resource resource)
    {
        resource.gameObject.SetActive(false);

        resource.DeliveredToCollectingZone -= ReleaseResource;
    }

    private void ReleaseResource(Resource resource)
    {
        _pool.Release(resource);
    }

    private bool TryGetSpawnPoint(out Vector3 spawnPoint, int numberOfAttempts = 50)
    {        
        int halfOfScaleCoefficient = 2;
        int planeScaleMultiplier = 10;        

        float sidestepX = _spawningArea.transform.localScale.x / halfOfScaleCoefficient * planeScaleMultiplier;
        float sidestepZ = _spawningArea.transform.localScale.z / halfOfScaleCoefficient * planeScaleMultiplier;
        float heigthOffset = _prefab.transform.localScale.y / halfOfScaleCoefficient;

        spawnPoint = GetNewPosition(sidestepX, sidestepZ, heigthOffset);

        while (DetermineIfPositionIsEngaged(spawnPoint) && numberOfAttempts != 0)
        {
            spawnPoint = GetNewPosition(sidestepX, sidestepZ, heigthOffset);
            numberOfAttempts--;
        }

        return numberOfAttempts != 0;
    }

    private bool DetermineIfPositionIsEngaged(Vector3 spawnPoint)
    {
        int radiusDivider = 2;

        return Physics.CheckSphere(spawnPoint, _prefab.transform.localScale.x / radiusDivider, _freeSpaceLayerMaskFilter);
    }

    private Vector3 GetNewPosition(float maxXValue, float maxZValue, float heightOffset)
    {
        return new Vector3(Random.Range(-maxXValue, maxXValue), heightOffset, Random.Range(-maxZValue, maxZValue));
    }
}
