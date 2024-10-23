using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ships
{

[CreateAssetMenu(fileName = "New Electronics Type", menuName = "Java Asunder/Components/Electronics", order = 0)]
public class ElectronicsType : ShipComponent
{
    public enum Electronics
    {
        BallisticComputer,
        Sonar,
        Radar
    }

    public Electronics type;
    [Range(1, 10)]
    public int strength = 1;
    public float effectiveRange = 1000;

}
}