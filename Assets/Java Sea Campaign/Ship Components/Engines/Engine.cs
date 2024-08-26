using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ships
{

[CreateAssetMenu(fileName = "Engine Type", menuName = "JavaSeaCampaign/Component/Engine", order = 0)]
public class Engine : Component
{
    public int strength = 1;
    public int agility = 1;

}
}