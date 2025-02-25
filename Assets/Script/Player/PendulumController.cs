using System;
using UnityEngine;
using DG.Tweening;
using Sequence = DG.Tweening.Sequence;

/// <summary>
/// 振り子挙動を制御する
/// </summary>
public class PendulumController : MonoBehaviour
{
    [Header("初期設定")]
    [SerializeField, Tooltip("振り子が動く角度")] private float _swingAngle = 90f; //
    [SerializeField, Tooltip("往復する時間の半分")] private float _duration = 2f;
    [SerializeField, Tooltip("イージングの種類")] private Ease _easeType = Ease.InOutSine; // 次の振り子に飛び乗るevent
    public event Action OnReachTheEdge; // 端に到達した時のEvent

    private void Start()
    {
        transform.rotation = Quaternion.Euler(0f, 0f, -_swingAngle / 2); // 振り子の初期の傾きを設定する
        StartPendulumMotion();
    }

    /// <summary>
    /// 振り子挙動
    /// </summary>
    private void StartPendulumMotion()
    {
        Sequence sequence = DOTween.Sequence();

        sequence
            .Append(transform.DOLocalRotate(new Vector3(0, 0, _swingAngle), _duration, RotateMode.LocalAxisAdd)
                .SetEase(_easeType).OnComplete(OnReach))
            .Append(transform.DOLocalRotate(new Vector3(0, 0, -_swingAngle), _duration, RotateMode.LocalAxisAdd)
                .SetEase(_easeType).OnComplete(OnReach));

        sequence.SetLoops(-1);
    }

    /// <summary>
    /// 振り子が右端に到達した時にEventを発火する
    /// </summary>
    private void OnReach()
    {
        if ((_swingAngle < 0 && transform.localEulerAngles.z < 360 + _swingAngle) ||// TODO: アングルにマイナスを設定している場合がうまくいかない
            (_swingAngle > 0 && transform.localEulerAngles.z < _swingAngle / 2))
        {
            OnReachTheEdge?.Invoke();
        }
    }
}
