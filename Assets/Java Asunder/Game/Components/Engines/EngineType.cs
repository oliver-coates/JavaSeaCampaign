using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ships
{

[CreateAssetMenu(fileName = "New Engine Type", menuName = "Java Asunder/Components/Engine", order = 0)]
public class EngineType : ShipComponent
{
    public int strength = 1;
    public int agility = 1;

}
}