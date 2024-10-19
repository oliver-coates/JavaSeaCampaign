using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[Serializable]
public class ComponentEffectiveness
{
    private const float EFFECTIVENESS_AMOUNT = 0.5f;

    public float value
    {
        get
        {
            return GetEffectiveness();
        }	
    }

    [SerializeField] private List<EffectivenessModifier> _modifiers;


    public ComponentEffectiveness()
    {
        _modifiers = new List<EffectivenessModifier>();
    }


    public float GetEffectiveness()
    {
        float effectiveness = 1f;

        foreach (EffectivenessModifier modifier in _modifiers)
        {
            effectiveness += modifier.effect;
        }

        effectiveness = Mathf.Clamp(effectiveness, 0, 2f);

        return effectiveness;
    }

    public void Tick()
    {
        List<EffectivenessModifier> toRemove = new List<EffectivenessModifier>();

        foreach (EffectivenessModifier modifer in _modifiers)
        {
            modifer.Tick(Time.deltaTime);

            if (modifer.timeRemaining < 0)
            {
                toRemove.Add(modifer);
            }   
        }


        // Rmeove modifers who's time remaining has fallen below 0
        foreach (EffectivenessModifier modifierToRemove in toRemove)
        {
            _modifiers.Remove(modifierToRemove);
        }
    }

    public void ApplyPlayerBuff(int roll)
    {
        // Get the roll as a value ranging from
        // 0 at a roll of 3 to 1 at a roll of 18
        // High rolls will exceed this (if players roll exceptionally well)
        float rollAsPercent = (roll - 3) / 15;

        float effectMultipier = Mathf.Lerp(-EFFECTIVENESS_AMOUNT, EFFECTIVENESS_AMOUNT, rollAsPercent);

        _modifiers.Add(new EffectivenessModifier(effectMultipier));
    }


}
