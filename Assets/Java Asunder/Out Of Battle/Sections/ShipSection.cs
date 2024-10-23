using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ships
{

public class ShipSection : MonoBehaviour
{
    public string sectionName = "Unnamed section";

    [HideInInspector] public ShipInstance ship;

    [Header("Slots: (Auto-populated at runtime)")]
    public ComponentSlot[] slots;



    public void Initialise(ShipInstance ship)
    {
        this.ship = ship;
        slots = GetComponentsInChildren<ComponentSlot>();

        foreach (ComponentSlot slot in slots)
        {
            slot.Initialise(ship);
        }
    }
}

}