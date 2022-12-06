using System.Linq;
using UnityEngine;

public enum TimeOfDay 
{ 

    MORNING,
    DAY,
    EVENING,
    NIGHT,

}

public enum SoundState { 

    FADE_OUT,
    FADE_IN,

}

public class AmbienceManager : MonoBehaviour
{

    private float _nightStart = 20;
    private float _morningStart = 6;
    private float _dayStart = 11;
    private float _eveningStart = 17;

    private float _soundFadeSpeed = 1;

    private GameManager _gameManager;

    private AudioSource _daySound;
    private SoundState _daySoundState;
    
    private AudioSource _eveningSound;
    private SoundState _eveningSoundState;
    
    private AudioSource _nightSound;
    private SoundState _nightSoundState;
    
    private AudioSource _morningSound;
    private SoundState _morningSoundState;

    private TimeOfDay _lastObservedTimeOfDay = TimeOfDay.NIGHT;

    // Start is called before the first frame update
    void Start()
    {

        var audioSources = GetComponentsInChildren<AudioSource>();
        _daySound = audioSources.Where(audioSource => audioSource.name == "DaySound").FirstOrDefault();
        _eveningSound = audioSources.Where(audioSource => audioSource.name == "EveningSound").FirstOrDefault();
        _nightSound = audioSources.Where(audioSource => audioSource.name == "NightSound").FirstOrDefault();
        _morningSound = audioSources.Where(audioSource => audioSource.name == "MorningSound").FirstOrDefault();
        
        _gameManager = FindObjectOfType<GameManager>();

        if (_gameManager != null) { AdvanceLastObservedTimeOfDay(); }

    }

    void FixedUpdate()
    {

        float dayVolumeTarget     = _daySoundState     == SoundState.FADE_IN ? 1 : 0;
        float eveningVolumeTarget = _eveningSoundState == SoundState.FADE_IN ? 1 : 0;
        float nightVolumeTarget   = _nightSoundState   == SoundState.FADE_IN ? 1 : 0;
        float morningVolumeTarget = _morningSoundState == SoundState.FADE_IN ? 1 : 0;

        _daySound.volume     = Mathf.Lerp(_daySound.volume, dayVolumeTarget, _soundFadeSpeed * Time.fixedDeltaTime);
        _eveningSound.volume = Mathf.Lerp(_eveningSound.volume, eveningVolumeTarget, _soundFadeSpeed * Time.fixedDeltaTime);
        _nightSound.volume   = Mathf.Lerp(_nightSound.volume, nightVolumeTarget, _soundFadeSpeed * Time.fixedDeltaTime);
        _morningSound.volume = Mathf.Lerp(_morningSound.volume, morningVolumeTarget, _soundFadeSpeed * Time.fixedDeltaTime);

        if (_gameManager != null && GetRecognizedTimeOfDay() != _lastObservedTimeOfDay) { AdvanceLastObservedTimeOfDay(); }
        
    }

    private void AdvanceLastObservedTimeOfDay()
    {

        _lastObservedTimeOfDay = GetRecognizedTimeOfDay();
        AdjustSoundStates(_lastObservedTimeOfDay);

    }

    public TimeOfDay GetRecognizedTimeOfDay()
    {

        float currentTime = _gameManager.TimeOfDay;
        
        if (currentTime >= _morningStart && currentTime < _dayStart) { return TimeOfDay.MORNING; }

        else if (currentTime >= _dayStart && currentTime < _eveningStart) { return TimeOfDay.DAY; }

        else if (currentTime >= _eveningStart && currentTime < _nightStart) { return TimeOfDay.EVENING; }

        return TimeOfDay.NIGHT;

    }

    private void AdjustSoundStates(TimeOfDay forTimeOfDay)
    {

        _daySoundState = SoundState.FADE_OUT;
        _eveningSoundState = SoundState.FADE_OUT;
        _nightSoundState = SoundState.FADE_OUT;
        _morningSoundState = SoundState.FADE_OUT;

        switch (forTimeOfDay) 
        {

            case TimeOfDay.DAY:
                _daySoundState = SoundState.FADE_IN;
                break;

            case TimeOfDay.EVENING:
                _eveningSoundState = SoundState.FADE_IN;
                break;

            case TimeOfDay.NIGHT:
                _nightSoundState = SoundState.FADE_IN;
                break;

            case TimeOfDay.MORNING:
                _morningSoundState = SoundState.FADE_IN;
                break;

        }

    }

}
