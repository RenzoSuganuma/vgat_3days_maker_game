using System;
using UnityEngine;
using DG.Tweening;

public class ObjectBeatSync : MonoBehaviour
{
    [Header("設定")] [SerializeField, Tooltip("BGMのAudioSource")]
    private AudioSource _audioSource;

    private float _bpm;

    private float _beatInterval;
    private float _nextBeatTime;

    public event Action OnBeat;

    private void Start()
    {
        var settings = Resources.Load<GameSettings>("GameSettings");
        if (settings != null)
        {
            _bpm = settings.ObjectBeatSettings.Bpm;
        }

        _beatInterval = 60f / _bpm;
        _nextBeatTime = _audioSource.time + _beatInterval;

        OnBeat?.Invoke();
    }

    private void Update()
    {
        if (_audioSource.isPlaying && _audioSource.time >= _nextBeatTime)
        {
            OnBeat?.Invoke();
            _nextBeatTime += _beatInterval;
        }
    }
}
