using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{

    [SerializeField] private Camera _playerCamera;
    private float _camSize;

    private float _minSize;
    private float _maxSize;

    private float _cameraSizeLerp;
    private Vector3 _targetPosition;

    private float _movementTightness;

    public void SetupSizes(float min, float max, float tightness)
    {
        _minSize = min;
        _maxSize = max;

        _movementTightness = tightness;

    }

    public void UpdateValues(float cameraSizeLerp, Vector3 targetPos)
    {
        _targetPosition = targetPos;
        _cameraSizeLerp = cameraSizeLerp;
    }

    private void Update()
    {
        FOVUpdate();
        MovementUpdate();
    }


    public void FOVUpdate()
    {
        float camSizeTarget = Mathf.Lerp(_minSize, _maxSize, _cameraSizeLerp);

        _camSize = Mathf.Lerp(_camSize, camSizeTarget, Time.deltaTime * 2f);

        _playerCamera.orthographicSize = _camSize;

    }


    private void MovementUpdate()
    {
        float speedMultiplier = 1 + _cameraSizeLerp;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speedMultiplier *= 3f;
        }

        transform.position = Vector3.Lerp(transform.position, _targetPosition, Time.deltaTime * _movementTightness);
    }
}
