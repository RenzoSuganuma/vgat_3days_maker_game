using System;
using UnityEngine;
using DG.Tweening;

public class ObjectBeatSync : MonoBehaviour
{
    [Header("設定")]

    [SerializeField, Tooltip("BGMのAudioSource")] private AudioSource _audioSource;
    [SerializeField, Tooltip("曲のBPM")] private float _bpm = 128f;

    private float _beatInterval;
    private float _nextBeatTime;

    public event Action OnBeat;

    private void Start()
    {
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
