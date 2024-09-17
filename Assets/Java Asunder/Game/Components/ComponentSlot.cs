using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ships
{


public enum ComponentType
{
    Bridge,
    Hold,
    Magazine,
    LightGun,
    MediumGun,
    HeavyGun,
    DeckMount,
    Electronics,
    Engine,
    LightArmour,
    MediumArmour,
    HeavyArmour,
    Additional

}

public class ComponentSlot : MonoBehaviour
{
    public string slotName;
    public ComponentType type;

    
}

}