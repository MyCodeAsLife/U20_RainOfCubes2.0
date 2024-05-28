using System.Collections;
using UnityEngine;

public class CubeSpawner : ObjectSpawner
{
    private const string ActiveCubesText = "Active cubes: ";
    private const string TotalCubesText = "Total spawned cubes: ";

    [SerializeField] private BombSpawner _bombSpawner;
    [SerializeField] private float _fallHeight;
    [SerializeField] private float _maxPositionOffset;

    private void OnEnable()
    {
        ObjectsPool.ChangedActiveObjectsCount += OnChangedActivCubesCount;
    }

    private void Start()
    {
        DisplayActiveObjectsCountText = ActiveCubesText + 0;
        DisplayTotalObjectsCountText = TotalCubesText + ObjectsPool.TotalSpawnedObjects;

        StartCoroutine(SpawnObjects());
    }

    private void OnDisable()
    {
        ObjectsPool.ChangedActiveObjectsCount -= OnChangedActivCubesCount;
    }

    public override void OnLifetimeEnded(BaseObject obj)
    {
        ObjectsPool.Return(obj);
    }

    private void OnChangedActivCubesCount(int count)
    {
        DisplayActiveObjectsCountText = ActiveCubesText + count;
    }

    private IEnumerator SpawnObjects()
    {
        const float Second = 0.5f;
        const bool IsWork = true;
        var spawnDelay = new WaitForSeconds(Second);

        while (IsWork)
        {
            float posX = Random.Range(-_maxPositionOffset, _maxPositionOffset);
            float posZ = Random.Range(-_maxPositionOffset, _maxPositionOffset);
            float lifetime = Random.Range(MinObjectLifetime, MaxObjectLifetime);
            Vector3 position = new Vector3(posX, _fallHeight, posZ);

            var obj = ObjectsPool.Get();
            obj.StartInitialization(position, lifetime);
            _bombSpawner.SubscribeToObject(obj);
            DisplayTotalObjectsCountText = TotalCubesText + ObjectsPool.TotalSpawnedObjects;

            yield return spawnDelay;
        }
    }
}
