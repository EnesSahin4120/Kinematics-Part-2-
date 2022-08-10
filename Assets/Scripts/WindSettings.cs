using UnityEngine;

[CreateAssetMenu(fileName = "Wind Settings")]
public class WindSettings : ScriptableObject
{
    public Vector3 Speed;
    public float Angle;
    public float DragConstant;
}
