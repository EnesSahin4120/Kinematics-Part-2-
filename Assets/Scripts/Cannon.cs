using UnityEngine;

[ExecuteInEditMode]
public class Cannon : MonoBehaviour
{
    [SerializeField] private Transform barrel;
    public Transform shootTransform;
    public Transform rootTransform;

    [Header("Cannon Parameters")]
    [Range(-90, 90)]
    public float cannonAngle;
    [Range(-60, 0)]
    public float barrelAngle;
    [Range(0, 100)]
    public float shootSpeed;

    private void Update() 
    {
        Set_CannonRotation();
        Set_BarrelRotation();
    }

    private void Set_CannonRotation() 
    {
        float xValue = transform.up.x * Mathf.Cos(cannonAngle * Mathf.Deg2Rad) - transform.up.y * Mathf.Sin(cannonAngle * Mathf.Deg2Rad);
        float zValue = transform.up.x * Mathf.Sin(cannonAngle * Mathf.Deg2Rad) + transform.up.y * Mathf.Cos(cannonAngle * Mathf.Deg2Rad);

        transform.forward = new Vector3(xValue, 0, zValue);
    }

    private void Set_BarrelRotation()
    {
        Quaternion barrelRot = Quaternion.Euler(barrelAngle, 0, 0);
        barrel.localRotation = barrelRot;
    }
}
