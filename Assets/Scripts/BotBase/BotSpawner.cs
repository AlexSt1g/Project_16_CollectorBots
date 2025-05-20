using UnityEngine;

public class BotSpawner : MonoBehaviour
{
    [SerializeField] private Bot _botPrefab;
    [SerializeField] private LayerMask _freeSpaceLayerMaskFilter;
    
    public void Spawn(Vector3 spawnPoint, BotBase botBase)
    {
        while (TryCheckSpawnPoint(spawnPoint))
            SetNewPosition(ref spawnPoint);

        Bot bot = Instantiate(_botPrefab, spawnPoint, Quaternion.identity);
        bot.Init(botBase);
        botBase.AddBot(bot);
    }

    private bool TryCheckSpawnPoint(Vector3 spawnPoint)
    {
        int halfExtentsDivider = 2;

        return Physics.CheckBox(spawnPoint, _botPrefab.transform.localScale / halfExtentsDivider, Quaternion.identity, _freeSpaceLayerMaskFilter);
    }

    private void SetNewPosition(ref Vector3 spawnPoint)
    {
        float offset = _botPrefab.transform.localScale.z;

        spawnPoint.z += offset;
    }
}
