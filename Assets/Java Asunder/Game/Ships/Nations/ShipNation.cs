using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ships
{

[CreateAssetMenu(fileName = "UnnamedNation", menuName = "Ships/Ship Nation", order = 1)]
public class Nation : ScriptableObject
{
    public string nationName;
    public string nationNameDesc;
    public string shipPrefix;
}

}