using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

/// <summary>
/// ビートに合わせて拡縮させたいオブジェクトを管理
/// </summary>
public class ObjectBeatController : MonoBehaviour
{
    [SerializeField] private ObjectBeatSync _beatController;
    [SerializeField, Tooltip("揺らしたいオブジェクト")] private List<Transform> _shakeObjs;
    [SerializeField, Tooltip("1回の揺れ時間")] private float _shakeDuration = 0.1f;
    [SerializeField, Tooltip("UIの拡大率")] private float _scaleMultiplier = 1.05f;

    private Dictionary<Transform, Vector3> _shakes = new Dictionary<Transform, Vector3>();

    private void Start()
    {
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
