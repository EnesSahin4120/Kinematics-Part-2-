using UnityEngine;

[ExecuteInEditMode]
public class Ball : MonoBehaviour
{
    [SerializeField] private Cannon cannon;
    [SerializeField] private Transform[] dotTransforms;

    private float speed;
    private float xAngle;
    private float yAngle;
    private float barrelLength;
    private float height_to_Base;
    private float currentTime;

    private void Update()
    {
        GetParameterInfo();
        SetDots();

        currentTime += Time.deltaTime;
        transform.position = PositionOnPath(currentTime);
    }

    private void GetParameterInfo()
    {
        speed = cannon.shootSpeed;
        xAngle = cannon.barrelAngle + 90;
        yAngle = cannon.cannonAngle + 90;
        barrelLength = (cannon.shootTransform.position - cannon.rootTransform.position).magnitude;
        height_to_Base = (cannon.shootTransform.position - Vector3.zero).magnitude;
    }

    private void SetDots()
    {
        for (int i = 0; i < dotTransforms.Length; i++)
            dotTransforms[i].position = PositionOnPath(i / 4f);
    }

    private Vector3 PositionOnPath(float t)
    {
        float cosX;
        float cosY;
        float cosZ;
        float barrelEnd_X, barrelEnd_Z;
        float barrelProjection, barrelLengthX, barrelLengthY, barrelLengthZ;


        barrelProjection = barrelLength * Mathf.Cos((90 - xAngle) * Mathf.Deg2Rad);
        barrelLengthX = barrelProjection * Mathf.Cos(yAngle * Mathf.Deg2Rad);
        barrelLengthY = barrelLength * Mathf.Cos(xAngle * Mathf.Deg2Rad);
        barrelLengthZ = barrelProjection * Mathf.Sin(yAngle * Mathf.Deg2Rad);

        cosX = barrelLengthX / barrelLength;
        cosY = barrelLengthY / barrelLength;
        cosZ = barrelLengthZ / barrelLength;

        barrelEnd_X = barrelLength * Mathf.Cos((90 - xAngle) * Mathf.Deg2Rad) * Mathf.Cos(yAngle * Mathf.Deg2Rad);
        barrelEnd_Z = barrelLength * Mathf.Cos((90 - xAngle) * Mathf.Deg2Rad) * Mathf.Sin(yAngle * Mathf.Deg2Rad);

        Vector3 result;
        result.x = speed * cosX * t + barrelEnd_X;
        result.y = (height_to_Base + barrelLength * Mathf.Cos(xAngle * Mathf.Deg2Rad)) + (speed * cosY * t) - (0.5f * - Physics.gravity.y * t * t);
        result.z = speed * cosZ * t + barrelEnd_Z;

        return result;
    }
}
