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


    [Header("The speed, in degrees/second that this turret can turn")]
    public float turnSpeed;

    [Header("The time, in seconds, between shots from this gun")]
    public float reloadTime;

    [Header("The inaccuracy of this gun (in degrees)")]
    public float inaccuracy;    

    [Header("The type of ammunuition fired by this gun")]
    public AmmunitionType ammo;
}

}