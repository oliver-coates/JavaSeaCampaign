using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ships
{

[CreateAssetMenu(fileName = "UnnamedType", menuName = "Java Asunder/Ships/Ship Type", order = 1)]
public class ShipType : ScriptableObject
{
    public string typeName;
}

}