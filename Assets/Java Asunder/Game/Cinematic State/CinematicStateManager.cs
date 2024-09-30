using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class CinematicStateManager : MonoBehaviour
{
    private const float TRANSITION_TIME = 0.5f;

    [SerializeField] private CinematicState[] _states;
    private CinematicState _currentState;
    private CinematicSoundscape _currentSoundscape;
    private float _volume;
    [SerializeField] private float _randomSoundTimer;
    [SerializeField] private float _randomSoundInterval;


    private float _transitionTimer;
    private bool _isTransitioning;
    
    private enum State
    {
        ToBattle,
        FromBattle,
        BetweenCinematics
    }
    private State _state;

    [Header("UI References:")]
    [SerializeField] private TMP_Dropdown _dropdown;

    [SerializeField] private CanvasGroup _cinematicsCanvasGroup;

    [SerializeField] private Image _imageForeground;
    [SerializeField] private CanvasGroup _canvasForeground;

    [SerializeField] private Image _imageBackground;
    [SerializeField] private CanvasGroup _canvasBackground;

    [SerializeField] private TextMeshProUGUI _locationText;

    [Header("Sound references:")]
    [SerializeField] private AudioSource _loopingAudioSource;
    [SerializeField] private AudioSource _randomAudioSource;

    private void Awake()
    {
        List<TMP_Dropdown.OptionData> optionDatas = new List<TMP_Dropdown.OptionData>();
        optionDatas.Add(new TMP_Dropdown.OptionData("Battle"));
        foreach (CinematicState state in _states)
        {
            optionDatas.Add(new TMP_Dropdown.OptionData(state.name));
        }

        _dropdown.options = optionDatas;

        _state = State.BetweenCinematics;
        TransitionToState(1);
    }

    public void TransitionToState(int value)
    {
        if (value == 0)
        {
            TransitionToBattle();
        }
        else
        {
            TransitionToState(_states[value-1]);
        }
    }

    public void TransitionToSoundscape(int value)
    {

    }

    public void SetMasterVolume(float newAmount)
    {
        _volume = newAmount;
    }

    private void Update()
    {
        VisualTransitionUpdate();

        if (_currentSoundscape.hasRandomSounds)
        {
            _randomSoundTimer += Time.deltaTime;

            if (_randomSoundTimer > _randomSoundInterval)
            {
                _randomSoundTimer = 0f;
                PlayRandomSound();
            }
        }
    }

    private void PlayRandomSound()
    {
        AudioClip toPlay = _currentSoundscape.GetRandomSound();

        _randomSoundInterval = toPlay.length + _currentSoundscape.GetRandomTimeInterval();

        _randomAudioSource.PlayOneShot(toPlay, _volume);
    }


    #region Visuals
    private void VisualTransitionUpdate()
    {
        if (_isTransitioning)
        {
            _transitionTimer += Time.deltaTime;

            float lerpAmount = _transitionTimer / TRANSITION_TIME;

            switch (_state)
            {
                case State.BetweenCinematics:
                    _canvasForeground.alpha = 1f - lerpAmount;
                    break;

                case State.FromBattle:
                    _cinematicsCanvasGroup.alpha = lerpAmount;
                    _canvasForeground.alpha = 1f - lerpAmount;

                    break;

                case State.ToBattle:
                    _cinematicsCanvasGroup.alpha = 1f - lerpAmount;
                    break;
            }

            if (_transitionTimer > TRANSITION_TIME)
            {
                FinishTransition();
            }
        }
    }
    private void TransitionToBattle()
    {
        _isTransitioning = true;
        _state = State.ToBattle;
    }

    private void TransitionToState(CinematicState newState)
    {
        _isTransitioning = true;

        _imageBackground.sprite = newState.GetRandomSprite();
        _canvasBackground.alpha = 1f;
    
        _locationText.text = newState.name;

        _currentState = newState;
    }

    private void FinishTransition()
    {
        if (_state == State.ToBattle)
        {
            _state = State.FromBattle;
        }
        else if (_state == State.FromBattle)
        {
            _state = State.BetweenCinematics;
        }

        _isTransitioning = false;
        _transitionTimer = 0f;

        _imageForeground.sprite = _imageBackground.sprite;
        _canvasForeground.alpha = 1f;       
        

     
    }
    #endregion
}
