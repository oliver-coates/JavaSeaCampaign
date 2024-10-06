using System.Collections;
using System.Collections.Generic;
using Ships;
using UnityEngine;

namespace Battlemap
{


public class ShipInfoTagUIManager : MonoBehaviour
{

    [SerializeField] private GameObject _UIPrefab;

    private Dictionary<ShipInstance, ShipInfoTagUI> shipUIDict;

    private void Awake()
    {
        shipUIDict = new Dictionary<ShipInstance, ShipInfoTagUI>();

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
        ShipInfoTagUI newUI = Instantiate(_UIPrefab, transform).GetComponent<ShipInfoTagUI>();
        newUI.Initialise(instance);

        shipUIDict.Add(instance, newUI);
    }

    private void DestroyUIInstance(ShipInstance instance)
    {
        Destroy(shipUIDict[instance].gameObject);
    }
}


}