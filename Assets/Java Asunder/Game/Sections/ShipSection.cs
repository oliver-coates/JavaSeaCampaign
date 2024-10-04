using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ships
{

public class ShipSection : MonoBehaviour
{
    public string sectionName = "Unnamed section";

    [Header("Slots: (Auto-populated at runtime)")]
    public ComponentSlot[] slots;

    public void Initialise()
    {
        slots = GetComponentsInChildren<ComponentSlot>();
    }
}

}