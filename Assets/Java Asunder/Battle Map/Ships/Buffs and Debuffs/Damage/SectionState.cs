using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.UI;

namespace Ships
{


[System.Serializable]
public class SectionState
{
    #region magic numbers

    public const float SEVERITY_RANDOM_MULTIPLIER = 0.5f;

    #endregion


    [SerializeField] private float _integrity;
    public float integrity
    {
        get
        {
            return _integrity;
        }	
    }


    public List<DamageInstance> damages;
    public float permanentIntegrityDamage;


    public SectionState()
    {
        damages = new List<DamageInstance>();
        permanentIntegrityDamage = 0f;
    }

    public void Tick(float deltaTime)
    {
        // Recalculate integrity & tick damage instances
        _integrity = 100f - permanentIntegrityDamage;

        foreach (DamageInstance damageInstance in damages)
        {
            damageInstance.Tick(deltaTime);
            _integrity -= damageInstance.integrityDamage; 
        }
    }

    public void RecieveHit(AmmunitionType ammunitionType)
    {
        DamageType[] possibleDamageTypes = GameMaster.damageTypes;

        float hitSeverity = ammunitionType.damage;

        // Apply a random boost/reduction to the severity:
        hitSeverity = hitSeverity * Random.Range(1f - SEVERITY_RANDOM_MULTIPLIER,
                                                    1f + SEVERITY_RANDOM_MULTIPLIER);


        // TODO: Implement armour reduction here


        // Roll for damage types from the severity of the hit
        List<DamageType> damageTypesDealt = RollForDamageTypes(possibleDamageTypes, hitSeverity);
        
        // Apply these damage types:
        foreach (DamageType damageType in damageTypesDealt)
        {
            ApplyDamageType(damageType, hitSeverity);
        }

    }

    private List<DamageType> RollForDamageTypes(DamageType[] possibleDamageTypes, float hitSeverity)
    {
        List<DamageType> damageTypesDealt = new List<DamageType>();

        foreach (DamageType damageType in possibleDamageTypes)
        {
            if (hitSeverity < damageType.minimumHitSeverity)
            {
                // Ignore if the hit severity is less than 
                continue;
            }

            // Find the chance for this hit to create this damage type (range 0-100)
            float chance = (hitSeverity - damageType.minimumHitSeverity) * damageType.chanceToOccurPerHitSeverity;

            // roll for this:
            float roll = Random.Range(0f, 100f);

            if (chance < roll)
            {
                // Damage type occurs!
                damageTypesDealt.Add(damageType);
            }
        }

        return damageTypesDealt;
    }

    private void ApplyDamageType(DamageType damageType, float hitSeverity)
    {
        // Go through each damage effect that this damage type should apply:
        foreach (DamageEffect damageEffect in damageType.effectsCaused)
        {
            ApplyDamageEffect(damageEffect, hitSeverity);
        }

        // Apply permenant damage to this hulls integrity:
        permanentIntegrityDamage += damageType.GetDamageToIntegrity();
    }

    private void ApplyDamageEffect(DamageEffect damageEffect, float hitSeverity)
    {
        // Check that a damage instance of this type does not exist:
        foreach (DamageInstance damageInstance in damages)
        {
            if (damageInstance.damageEffect == damageEffect)
            {
                // A damage instance already exists of this type,
                // Boost it:
                damageInstance.BoostDamage(hitSeverity);
                return;
            }
        }

        // No damage instance matches this effect, create a new one:
        DamageInstance newDamageInstance = new(damageEffect, hitSeverity);
        damages.Add(newDamageInstance);
    }

}

}