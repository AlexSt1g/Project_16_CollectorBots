using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(BotSpawner))]
[RequireComponent(typeof(ResourceScanner))]
public class BotBase : MonoBehaviour
{
    [SerializeField] private Transform _botsSpawnPoint;
    [SerializeField] private int _startNumberOfBots = 3;
    [SerializeField] private float _resourceScannerDelay = 5f;
    [SerializeField] private ResourceDataBase _resourceDataBase;
    [SerializeField] private ResourceCounter _resourceCounter;

    private BotSpawner _botSpawner;
    private List<Bot> _bots = new();
    private List<Bot> _freeBots = new();
    private ResourceScanner _resourceScanner;

    [field: SerializeField] public Collider ResourceCollectingZone { get; private set; }

    private void Awake()
    {
        _botSpawner = GetComponent<BotSpawner>();
        _resourceScanner = GetComponent<ResourceScanner>();
        _resourceCounter.Reset();
    }

    private void OnEnable()
    {
        _resourceScanner.Found += OnResourcesFound;
    }

    private void OnDisable()
    {
        _resourceScanner.Found -= OnResourcesFound;

        foreach (var bot in _bots)
            bot.ResourceDelivered -= OnResourceDelivered;
    }

    private void Start()
    {
        for (int i = 0; i < _startNumberOfBots; i++)
            _botSpawner.Spawn(_botsSpawnPoint.position, this);

        StartCoroutine(ScanResourcesWithDelay());
    }

    public void AddBot(Bot bot)
    {
        _bots.Add(bot);
        bot.ResourceDelivered += OnResourceDelivered;
    }

    private IEnumerator ScanResourcesWithDelay()
    {
        WaitForSeconds wait = new(_resourceScannerDelay);

        while (enabled)
        {
            yield return wait;
            _resourceScanner.Scan();
        }
    }

    private void OnResourcesFound(List<Resource> resources)
    {
        _resourceDataBase.AddResources(resources);

        if (TryGetFreeBots())
        {
            List<Resource> freeResources = _resourceDataBase.GetFreeResources();

            SortResourcesByDistanceToBase(ref freeResources);
            OrderBotsToCollectResources(freeResources);
        }
    }

    private void SortResourcesByDistanceToBase(ref List<Resource> resources)
    {
        resources = resources.OrderBy(resource => GetDistanceToResource(resource.transform.position)).ToList();
    }

    private float GetDistanceToResource(Vector3 resourcePosition)
    {
        return (transform.position - resourcePosition).sqrMagnitude;
    }

    private bool TryGetFreeBots()
    {
        _freeBots = _bots.Where(bot => bot.HasTask == false).ToList();

        return _freeBots.Count > 0;
    }

    private void OrderBotsToCollectResources(List<Resource> resources)
    {
        for (int i = 0; i < _freeBots.Count; i++)
        {
            if (i >= resources.Count)
                return;

            _freeBots[i].CollectResource(resources[i]);            
            _resourceDataBase.EngageResource(resources[i]);
        }
    }

    private void OnResourceDelivered()
    {
        _resourceCounter.Add();
    }
}
