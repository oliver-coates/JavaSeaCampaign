using System.Collections;
using System.Collections.Generic;
using Ships;
using UnityEngine;

public class ElectronicsGenericInstance : MonoBehaviour, IShipComponentInstance
{
    public ComponentEffectiveness[] GetComponentEffectivenesses()
    {
        return new ComponentEffectiveness[0];
    }

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
                FireControlInstance fireControlInstance = gameObject.AddComponent<FireControlInstance>();
                fireControlInstance.Setup(ship, componentSlot);
                componentSlot.componentInstance = fireControlInstance;
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
