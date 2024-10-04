using System.Collections;
using System.Collections.Generic;
using Ships;
using UnityEngine;

namespace Battlemap
{

public class ShipInstanceUI : MonoBehaviour
{
    private ShipInstance _ship;
    private Camera _targetCamera;
    private bool _initialised;

    public void Initialise(ShipInstance ship, Camera camera)
    {
        _ship = ship;
        _targetCamera = camera;

        _initialised = true;
    }

    private void Update()
    {
        if (!_initialised)
        {
            return;
        }
    
        transform.position = _targetCamera.WorldToScreenPoint(_ship.transform.position + new Vector3(0, _ship.UIDisplayOffset, 0));
    }
}

}