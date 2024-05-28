using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBomb : BaseObject
{
    private Explosion _prefabExplosion;
    private float _explosionRadius;
    private float _explosionForce;

    private void Awake()
    {
        _explosionRadius = 30;
        _explosionForce = 900;
        _prefabExplosion = Resources.Load<Explosion>("Prefabs/Explosion");
        TimeEnded += Explode;
    }

    public override void StartInitialization(Vector3 position, float lifetime)
    {
        base.StartInitialization(position, lifetime);
        Material.color = Color.black;

        StartCoroutine(Disappearance());
    }

    private void Explode(BaseObject obj)
    {
        Vector3 position = obj.transform.position;
        var interactiveObjects = GetExplodableObjects(position);
        float newExplosionForce = _explosionForce / transform.localScale.x;
        float newExplosionRadius = _explosionRadius / transform.localScale.x;

        foreach (var interactiveObject in interactiveObjects)
            interactiveObject.AddExplosionForce(newExplosionForce, position, newExplosionRadius);

        var effect = Instantiate(_prefabExplosion);
        effect.transform.position = position;
    }

    private List<Rigidbody> GetExplodableObjects(Vector3 position)
    {
        Collider[] hits = Physics.OverlapSphere(position, _explosionRadius);
        List<Rigidbody> interactiveObjects = new List<Rigidbody>();

        foreach (Collider hit in hits)
            if (hit.attachedRigidbody != null)
                interactiveObjects.Add(hit.attachedRigidbody);

        return interactiveObjects;
    }

    private IEnumerator Disappearance()
    {
        bool isWork = true;
        float step = Material.color.a / Lifetime;
        Color newColor = Material.color;

        while (isWork)
        {
            newColor.a -= step * Time.deltaTime;
            Material.color = newColor;

            if (newColor.a <= 0)
                isWork = false;

            yield return null;
        }
    }
}
