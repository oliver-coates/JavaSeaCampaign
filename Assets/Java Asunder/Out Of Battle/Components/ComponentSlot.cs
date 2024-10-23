using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ships
{



public class ComponentSlot : MonoBehaviour
{
    [Header("Decorative")]
    public string slotName;

    [Header("Type of component that this is: (i.e. Medium Gun)")]
    public ShipComponentType componentType;
    
    [Header("The actual component that this is (i.e. 40mm gun)")]
    public ShipComponent component;

    [Header("The instaniated script related to this component:")]
    public IShipComponentInstance componentInstance;
    

    public void Initialise(ShipInstance ship)
    {
        if (componentType.genericPrefab != null && component != null)
        {
            componentInstance = Instantiate(componentType.genericPrefab, transform.position, transform.rotation, transform).GetComponent<IShipComponentInstance>();    

            if (componentInstance == null)
            {
                Debug.LogError($"Ship component instance not found on gameobject {name}");
                return;
            }
        
            componentInstance.Setup(ship, this);
        }   
    }

}

}