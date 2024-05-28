using System;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(Rigidbody))]
public abstract class BaseObject : MonoBehaviour
{
    private float _livedTime;

    protected Material Material;

    public event Action<BaseObject> TimeEnded;

    public float Lifetime { get; private set; }         // на какой позиции? форматирование кода

    private void LateUpdate()
    {
        _livedTime += Time.deltaTime;

        if (_livedTime >= Lifetime)
        {
            TimeEnded?.Invoke(this);
            Material.color = Color.white;
        }
    }

    public virtual void StartInitialization(Vector3 position, float lifetime)
    {
        Material = GetComponent<Renderer>().material;
        transform.position = position;
        Lifetime = lifetime;
        _livedTime = 0;
    }
}
