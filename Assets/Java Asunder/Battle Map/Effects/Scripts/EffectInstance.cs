using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Effects
{


public class EffectInstance : MonoBehaviour
{

    private EffectPool _pool;
    
    [SerializeField] private ParticleSystem _particleSystem;

    /// <summary>
    /// First time intiialisation, only called once
    /// </summary>
    public void Initialise(EffectPool pool)
    {
        _pool = pool;
    }

    public void TakenFromPool()
    {
        _particleSystem.Play();
    }

    private void Update()
    {
        if (GameMaster.BattlePaused)
        {
            if (_particleSystem.isPaused == false)
            {
                _particleSystem.Pause();
            }
        }
        else
        {
            if (_particleSystem.isPaused == true)
            {
                _particleSystem.Play();
            }

            if (!_particleSystem.isPlaying)
            {
                ReturnToPool();
            }
        }
    }

    private void ReturnToPool()
    {
        _pool.ReturnToPool(this);
    }

}

}