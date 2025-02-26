using UnityEngine;
using DG.Tweening;

public class ObjectBeatSync : MonoBehaviour
{
    [Header("設定")]
    [SerializeField, Tooltip("揺らしたいオブジェクト")] private Transform _shakeObj;
    [SerializeField, Tooltip("BGMのAudioSource")] private AudioSource _audioSource;
    [SerializeField, Tooltip("曲のBPM")] private float _bpm = 128f;
    [SerializeField, Tooltip("1回の揺れ時間")] private float _shakeDuration = 0.1f;
    [SerializeField, Tooltip("UIの拡大率")] private float _scaleMultiplier = 1.05f;

    private float _beatInterval;
    private float _nextBeatTime;
    private Vector3 _initialScale;

    private void Start()
    {
        if (_shakeObj == null)
        {
            Debug.LogError("CanvasのRectTransformを設定してください！");
            return;
        }

        _beatInterval = 60f / _bpm;
        _nextBeatTime = _audioSource.time + _beatInterval;
        _initialScale = _shakeObj.localScale;

        OnBeat();
    }

    private void Update()
    {
        if (_audioSource.isPlaying && _audioSource.time >= _nextBeatTime)
        {
            OnBeat();
            _nextBeatTime += _beatInterval;
        }
    }

    /// <summary>
    /// UIをリズムに合わせて拡大縮小
    /// </summary>
    private void OnBeat()
    {
        _shakeObj.DOScale(_initialScale * _scaleMultiplier, _shakeDuration)
            .SetLoops(2, LoopType.Yoyo);
    }
}
