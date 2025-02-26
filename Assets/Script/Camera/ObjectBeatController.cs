using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

/// <summary>
/// ビートに合わせて拡縮させたいオブジェクトを管理
/// </summary>
public class ObjectBeatController : MonoBehaviour
{
    [SerializeField] private ObjectBeatSync _beatController;

    [SerializeField, Tooltip("揺らしたいオブジェクト")]
    private List<Transform> _shakeObjs;

    private float _shakeDuration;
    private float _scaleMultiplier;

    private Dictionary<Transform, Vector3> _shakes = new Dictionary<Transform, Vector3>();

    private void Start()
    {
        var settings = Resources.Load<GameSettings>("GameSettings");
        if (settings != null)
        {
            _shakeDuration = settings.ObjectBeatSettings.ShakeDuration;
            _scaleMultiplier = settings.ObjectBeatSettings.ScaleMultiplier;
        }

        _beatController.OnBeat += OnBeat;

        foreach (var shakeObj in _shakeObjs)
        {
            _shakes[shakeObj] = shakeObj.localScale;
        }
    }

    private void OnDestroy()
    {
        _beatController.OnBeat -= OnBeat;
    }

    /// <summary>
    /// UIをリズムに合わせて拡大縮小
    /// </summary>
    private void OnBeat()
    {
        foreach (var shakeObj in _shakeObjs)
        {
            shakeObj.DOScale(_shakes[shakeObj] * _scaleMultiplier, _shakeDuration)
                .SetLoops(2, LoopType.Yoyo);
        }
    }
}
