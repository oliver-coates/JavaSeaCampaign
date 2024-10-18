using System.Collections;
using System.Collections.Generic;
using Ships;
using UnityEngine;

public class ElectronicsGenericInstance : MonoBehaviour, IShipComponentInstance
{
    public void Setup(ShipInstance ship, ComponentSlot componentSlot)
    {
        if (componentSlot.component is not ElectronicsType)
        {
            Debug.LogError($"Provided component in electronics slot is not Electronics Type");
            return;
        }
 
        ElectronicsType electronics = (ElectronicsType) componentSlot.component;

        switch (electronics.type)
        {
            case ElectronicsType.Electronics.BallisticComputer:
                gameObject.AddComponent<FireControlInstance>().Setup(ship, componentSlot);
                break;

            case ElectronicsType.Electronics.Sonar:
                Debug.Log($"Implmenent sonar here");
                break;

            case ElectronicsType.Electronics.Radar:
                Debug.Log($"Implement radar here");
                break;
        }

        Destroy(this);
    }
}
