using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeveloperCameraScript : MonoBehaviour
{
    public static event Action<bool> onCameraChangedStreamingState;
    [SerializeField] private bool _streamingCameraData;



    [SerializeField] private CameraScript _playerCamera;
    [SerializeField] private Camera _devCamera;

    [Header("Field of View settings:")]
    [SerializeField] private float _minSize = 100;
    [SerializeField] private float _maxSize = 500;
    [SerializeField] private float _scrollSensitivity = 10;
    private float _cameraSizeLerp;

    [Header("Movement Settings:")]
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _movementTightness;
    private Vector3 _targetPosition;


    private void Start()
    {
        _cameraSizeLerp = 0.5f;
    
        _targetPosition = transform.position;

        _playerCamera.SetupSizes(_minSize, _maxSize, _movementTightness);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            _streamingCameraData = !_streamingCameraData;

            onCameraChangedStreamingState?.Invoke(_streamingCameraData);
        }

        FOVUpdate();
        MovementUpdate();

        if (_streamingCameraData)
        {
            _playerCamera.UpdateValues(_cameraSizeLerp, _targetPosition);
        }
    }

    private void FOVUpdate()
    {
        _cameraSizeLerp -= Input.mouseScrollDelta.y * _scrollSensitivity * Time.deltaTime;
        _cameraSizeLerp = Mathf.Clamp(_cameraSizeLerp, 0, 1f);

        _devCamera.orthographicSize = Mathf.Lerp(_minSize, _maxSize, _cameraSizeLerp);
    }


    private void MovementUpdate()
    {
        float speedMultiplier = 1 + _cameraSizeLerp;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speedMultiplier *= 3f;
        }

        _targetPosition.x += Input.GetAxis("Horizontal") * Time.deltaTime * _movementSpeed * speedMultiplier;
        _targetPosition.z += Input.GetAxis("Vertical") * Time.deltaTime * _movementSpeed * speedMultiplier;

        transform.position = _targetPosition;
    }
}
