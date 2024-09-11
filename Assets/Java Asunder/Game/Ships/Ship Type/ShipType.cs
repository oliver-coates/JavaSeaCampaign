using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ships
{

[CreateAssetMenu(fileName = "UnnamedType", menuName = "Ships/Ship Type", order = 1)]
public class ShipType : ScriptableObject
{
    public string typeName;

    [Header("Base Stats:")]
    public int base_manuverability;
    public int base_armour;
    public int base_repairs;
    public int base_maxCondition;
    public int base_cargo;

    [Header("UI:")]
    public Sprite shipImage;
}

}