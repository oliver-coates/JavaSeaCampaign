using UnityEngine;

[CreateAssetMenu(fileName = "ShipGunType", menuName = "JavaSeaCampaign/ShipGunType", order = 0)]
public class ShipGunType : ScriptableObject 
{
    public float turnSpeed;

    public float shotVelocity;

    public float accuracy;    
}