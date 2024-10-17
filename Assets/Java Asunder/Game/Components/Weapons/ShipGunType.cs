using UnityEngine;

namespace Ships
{


[CreateAssetMenu(fileName = "New Gun Type", menuName = "Java Asunder/Components/Gun", order = 0)]
public class ShipGunType : ShipComponent
{
    public enum Size
    {
        Small,
        Medium,
        Large
    }

    public float turnSpeed;

    public float reloadTime;

    public float shotVelocity;

    public float accuracy;    

    public AmmunitionType ammo;
}

}