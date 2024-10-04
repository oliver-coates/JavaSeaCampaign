using System.Collections;
using System.Collections.Generic;
using Ships;
using UnityEngine;

namespace Battlemap
{


public class ShipInstanceUIManager : MonoBehaviour
{

    [SerializeField] private GameObject _UIPrefab;
    [SerializeField] private Camera _targetCamera;

    private Dictionary<ShipInstance, ShipInstanceUI> shipUIDict;

    private void Awake()
    {
        shipUIDict = new Dictionary<ShipInstance, ShipInstanceUI>();

        ShipInstance.OnShipInstanceCreated += CreateUIInstance;
        ShipInstance.OnShipInstanceDestroyed += DestroyUIInstance;
    }

    private void OnDestroy()
    {
        ShipInstance.OnShipInstanceCreated -= CreateUIInstance;
        ShipInstance.OnShipInstanceDestroyed -= DestroyUIInstance;
    }

    private void CreateUIInstance(ShipInstance instance)
    {
        ShipInstanceUI newUI = Instantiate(_UIPrefab, transform).GetComponent<ShipInstanceUI>();
        newUI.Initialise(instance, _targetCamera);

        shipUIDict.Add(instance, newUI);
    }

    private void DestroyUIInstance(ShipInstance instance)
    {
        Destroy(shipUIDict[instance].gameObject);
    }
}


}