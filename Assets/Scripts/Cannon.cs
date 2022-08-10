using UnityEngine;

[ExecuteInEditMode]
public class Cannon : MonoBehaviour
{
    [SerializeField] private Transform _barrel;
    public Transform ShootTransform;
    public Transform RootTransform;

    [Header("Cannon Parameters")]
    [Range(-90, 90)]
    public float CannonAngle;
    [Range(-60, 0)]
    public float BarrelAngle;
    [Range(0, 100)]
    public float ShootSpeed;

    private void Update()
    {
        Set_CannonRotation();
        Set_BarrelRotation();
    }

    private void Set_CannonRotation()
    {
        float xValue = transform.up.x * Mathf.Cos(CannonAngle * Mathf.Deg2Rad) - transform.up.y * Mathf.Sin(CannonAngle * Mathf.Deg2Rad);
        float zValue = transform.up.x * Mathf.Sin(CannonAngle * Mathf.Deg2Rad) + transform.up.y * Mathf.Cos(CannonAngle * Mathf.Deg2Rad);

        transform.forward = new Vector3(xValue, 0, zValue);
    }

    private void Set_BarrelRotation()
    {
        Quaternion barrelRot = Quaternion.Euler(BarrelAngle, 0, 0);
        _barrel.localRotation = barrelRot;
    }
}
