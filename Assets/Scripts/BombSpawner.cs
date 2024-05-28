using UnityEngine;

public class BombSpawner : ObjectSpawner
{
    private const string ActiveBombsText = "Active bombs: ";
    private const string TotalBombsText = "Total spawned bombs: ";

    private void OnEnable()
    {
        ObjectsPool.ChangedActiveObjectsCount += OnChangedActivBombsCount;
    }

    private void Start()
    {
        DisplayActiveObjectsCountText = ActiveBombsText + 0;
        DisplayTotalObjectsCountText = TotalBombsText + ObjectsPool.TotalSpawnedObjects;
    }

    private void OnDisable()
    {
        ObjectsPool.ChangedActiveObjectsCount -= OnChangedActivBombsCount;
    }

    public override void OnLifetimeEnded(BaseObject obj)
    {
        ObjectsPool.Return(obj);
    }

    public void SubscribeToObject(BaseObject obj)
    {
        obj.TimeEnded += OnObjectLifeTimeEnded;
    }

    private void OnObjectLifeTimeEnded(BaseObject obj)
    {
        float lifetime = Random.Range(MinObjectLifetime, MaxObjectLifetime);
        obj.TimeEnded -= OnObjectLifeTimeEnded;
        var bomb = ObjectsPool.Get();
        bomb.StartInitialization(obj.transform.position, lifetime);
        DisplayTotalObjectsCountText = TotalBombsText + ObjectsPool.TotalSpawnedObjects;
    }

    private void OnChangedActivBombsCount(int count)
    {
        DisplayActiveObjectsCountText = ActiveBombsText + count;
    }
}
