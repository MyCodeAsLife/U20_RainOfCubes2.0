using TMPro;
using UnityEngine;

public abstract class ObjectSpawner : MonoBehaviour
{
    public const float MinObjectLifetime = 2f;
    public const float MaxObjectLifetime = 5f;

    [SerializeField] private BaseObject _spawnObject;
    [SerializeField] private TMP_Text _activeObjectsCountText;
    [SerializeField] private TMP_Text _totalObjectsCountText;

    public ObjectPool<BaseObject> ObjectsPool { get; private set; }

    public string DisplayTotalObjectsCountText
    {
        get
        {
            return _totalObjectsCountText.text;
        }
        set
        {
            _totalObjectsCountText.text = value;
        }
    }

    public string DisplayActiveObjectsCountText
    {
        get
        {
            return _activeObjectsCountText.text;
        }
        set
        {
            _activeObjectsCountText.text = value;
        }
    }

    private void Awake()
    {
        ObjectsPool = new ObjectPool<BaseObject>(_spawnObject, Create, Enable, Disable);
    }

    private void OnDisable()
    {
        ObjectsPool.ReturnAll();
    }

    public abstract void OnLifetimeEnded(BaseObject obj);

    private BaseObject Create(BaseObject prefab)
    {
        var obj = Instantiate<BaseObject>(prefab);
        obj.transform.SetParent(transform);

        return obj;
    }

    private void Enable(BaseObject obj)
    {
        obj.gameObject.SetActive(true);
        obj.TimeEnded += OnLifetimeEnded;
    }

    private void Disable(BaseObject obj)
    {
        obj.TimeEnded -= OnLifetimeEnded;
        obj.gameObject.SetActive(false);
    }
}
