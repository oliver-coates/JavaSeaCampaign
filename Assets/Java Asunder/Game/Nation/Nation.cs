using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ships
{

[CreateAssetMenu(fileName = "New Nation", menuName = "Java Asunder/Nations/New Nation", order = 1)]
public class Nation : ScriptableObject
{
    public string nationName;
    public string nationNameDesc;
    public string shipPrefix;
}

}