using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CameraDevUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _cameraStatusText;

    private void Awake()
    {
        DeveloperCameraScript.onCameraChangedStreamingState += CameraStreamingStateChanged;
        CameraStreamingStateChanged(false);
    }

    private void CameraStreamingStateChanged(bool state)
    {
        if (state == true)
        {
            _cameraStatusText.text = "Camera is currently streaming";
            _cameraStatusText.color = Color.green;
        }
        else
        {
            _cameraStatusText.text = "Camera is NOT streaming";
            _cameraStatusText.color = Color.red;
        }
    }
}
