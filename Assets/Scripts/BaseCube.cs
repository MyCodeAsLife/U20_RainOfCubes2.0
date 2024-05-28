using UnityEngine;

public class BaseCube : BaseObject
{
    private int _groundLayer;
    private bool _isColorChanged;

    private void Awake()
    {
        _groundLayer = LayerMask.NameToLayer("Ground");
    }

    private void Start()
    {
        _isColorChanged = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == _groundLayer && _isColorChanged == false)
        {
            _isColorChanged = true;
            SwitchColor();
        }
    }

    public override void StartInitialization(Vector3 position, float lifetime)
    {
        base.StartInitialization(position, lifetime);
        _isColorChanged = false;
        SetTransform();
    }

    private void SetTransform()
    {
        const int AxisCount = 3;
        const float MinTorque = 100f;
        const float MaxTorque = 1000f;

        int randomAxis = Random.Range(0, AxisCount);
        Vector3 startTorque = new Vector3();
        startTorque[randomAxis] = Random.Range(MinTorque, MaxTorque);

        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.AddTorque(startTorque);
    }

    private void SwitchColor()
    {
        float red = Random.Range(0f, 1f);
        float green = Random.Range(0f, 1f);
        float blue = Random.Range(0f, 1f);
        float alpha = 1;
        Color newColor = new Color(red, green, blue, alpha);
        Material.color = newColor;
    }
}