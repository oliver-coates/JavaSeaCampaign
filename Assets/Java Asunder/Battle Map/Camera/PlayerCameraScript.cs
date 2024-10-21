using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraScript : MonoBehaviour
{

    [SerializeField] private Camera _playerCamera;
    [SerializeField] private AudioListener _audioListener;
    private float _camSize;

    private float _minSize;
    private float _maxSize;

    private float _cameraSizeLerpAmount;
    private Vector3 _targetPosition;

    private float _movementTightness;

    private void Awake()
    {
        GameMaster.OnBattleEnd += CenterCamera;
        GameMaster.OnBattleEnd += EnableListener;
        GameMaster.OnBattleStart += DisableListener;
    }

    private void OnDestroy()
    {
        GameMaster.OnBattleEnd -= CenterCamera;
        GameMaster.OnBattleEnd -= EnableListener;
        GameMaster.OnBattleStart -= DisableListener;
    }

    public void SetupSizes(float min, float max, float tightness)
    {
        _minSize = min;
        _maxSize = max;

        _movementTightness = tightness;

        CenterCamera();
    }

    public void SetTargetPositionAndSize(float cameraSizeAmount, Vector3 targetPos)
    {
        _targetPosition = targetPos;
        _cameraSizeLerpAmount = cameraSizeAmount;
    }



    public void CenterCamera()
    {
        SetTargetPositionAndSize(Mathf.Lerp(_minSize, _maxSize, 0.5f), Vector3.zero);
    }


    private void Update()
    {
        FOVUpdate();
        MovementUpdate();
    }


    public void FOVUpdate()
    {
        float camSizeTarget = Mathf.Lerp(_minSize, _maxSize, _cameraSizeLerpAmount);
        _camSize = Mathf.Lerp(_camSize, camSizeTarget, Time.deltaTime * 2f);
        _playerCamera.orthographicSize = _camSize;

    }

    private void MovementUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, _targetPosition, Time.deltaTime * _movementTightness);
    }

    private void DisableListener()
    {
        _audioListener.enabled = false;
    }

    private void EnableListener()
    {
        _audioListener.enabled = true;
    }
}
