using UnityEngine;

[ExecuteInEditMode]
public class BallMovement : MonoBehaviour
{
    [SerializeField] private float _mass;
    [SerializeField] private Cannon _cannon;
    [SerializeField] private Transform[] _dotTransforms;

    [Header("Drag Properties")]
    [SerializeField] private float _dragConstant;
    [SerializeField] private WindSettings _windSettings;

    private float _speed;
    private float _xAngle;
    private float _yAngle;
    private float _barrelLength;
    private float _height_to_Base;
    private float _currentTime;

    private const float _euler = 2.7182f;

    private void Update()
    {
        GetParameterInfo();
        SetDots();

        _currentTime += Time.deltaTime;
        transform.position = PositionOnPath(_currentTime);
    }

    private void GetParameterInfo()
    {
        _speed = _cannon.ShootSpeed;
        _xAngle = _cannon.BarrelAngle + 90;
        _yAngle = _cannon.CannonAngle + 90;
        _barrelLength = (_cannon.ShootTransform.position - _cannon.RootTransform.position).magnitude;
        _height_to_Base = (_cannon.ShootTransform.position - Vector3.zero).magnitude;
    }

    private void SetDots()
    {
        for (int i = 0; i < _dotTransforms.Length; i++)
            _dotTransforms[i].position = PositionOnPath(i / 4f);
    }

    private Vector3 PositionOnPath(float t)
    {
        float cosX;
        float cosY;
        float cosZ;
        float barrelEnd_X, barrelEnd_Z;
        float barrelProjection, barrelLengthX, barrelLengthY, barrelLengthZ;


        barrelProjection = _barrelLength * Mathf.Cos((90 - _xAngle) * Mathf.Deg2Rad);
        barrelLengthX = barrelProjection * Mathf.Cos(_yAngle * Mathf.Deg2Rad);
        barrelLengthY = _barrelLength * Mathf.Cos(_xAngle * Mathf.Deg2Rad);
        barrelLengthZ = barrelProjection * Mathf.Sin(_yAngle * Mathf.Deg2Rad);

        cosX = barrelLengthX / _barrelLength;
        cosY = barrelLengthY / _barrelLength;
        cosZ = barrelLengthZ / _barrelLength;

        barrelEnd_X = _barrelLength * Mathf.Cos((90 - _xAngle) * Mathf.Deg2Rad) * Mathf.Cos(_yAngle * Mathf.Deg2Rad);
        barrelEnd_Z = _barrelLength * Mathf.Cos((90 - _xAngle) * Mathf.Deg2Rad) * Mathf.Sin(_yAngle * Mathf.Deg2Rad);

        Vector3 result = Vector3.zero;

        result.x = ((_mass / _dragConstant) * Mathf.Pow(_euler, -(_dragConstant * t) / _mass)
                   * ((-_windSettings.DragConstant * _windSettings.Speed.x * Mathf.Cos(_windSettings.Angle * Mathf.Deg2Rad)) / _dragConstant - (_speed * cosX))
                   - (_windSettings.DragConstant * _windSettings.Speed.x * Mathf.Cos(_windSettings.Angle * Mathf.Deg2Rad) * t / _dragConstant)
                   - ((_mass / _dragConstant)
                   * ((-_windSettings.DragConstant * _windSettings.Speed.x * Mathf.Cos(_windSettings.Angle * Mathf.Deg2Rad))
                   / _dragConstant - (_speed * cosX)))
                   + barrelEnd_X);
        result.y = (_height_to_Base + _barrelLength * Mathf.Cos(_xAngle * Mathf.Deg2Rad))
                   + (-((_speed * cosY) + (_mass * -Physics.gravity.y) / _dragConstant)
                   * (_mass / _dragConstant) * Mathf.Pow(_euler, -(_dragConstant * t) / _mass)
                   - (_mass * -Physics.gravity.y * t) / _dragConstant)
                   + ((_mass / _dragConstant) * ((_speed * cosY) + (_mass * -Physics.gravity.y) / _dragConstant));
        result.z = ((_mass / _dragConstant) * Mathf.Pow(_euler, -(_dragConstant * t) / _mass)
                   * ((-_windSettings.DragConstant * _windSettings.Speed.z * Mathf.Sin(_windSettings.Angle * Mathf.Deg2Rad)) / _dragConstant - (_speed * cosZ))
                   - (_windSettings.DragConstant * _windSettings.Speed.z * Mathf.Sin(_windSettings.Angle * Mathf.Deg2Rad) * t / _dragConstant)
                   - ((_mass / _dragConstant)
                   * ((-_windSettings.DragConstant * _windSettings.Speed.z * Mathf.Sin(_windSettings.Angle * Mathf.Deg2Rad))
                   / _dragConstant - (_speed * cosZ)))
                   + barrelEnd_Z);

        return result;
    }
}
