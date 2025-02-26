using System;
using UnityEngine;
using DG.Tweening;
using Sequence = DG.Tweening.Sequence;

/// <summary>
/// 振り子挙動を制御する
/// </summary>
public class PendulumController : MonoBehaviour
{
    [Header("初期設定")] [SerializeField, Tooltip("振り子が動く角度")]
    private float _swingAngle = 90f; //

    [SerializeField, Tooltip("往復する時間の半分")] private float _duration = 2f;
    [SerializeField, Tooltip("イージングの種類")] private Ease _easeType = Ease.InOutSine; // 次の振り子に飛び乗るevent

    [SerializeField, Tooltip("プレイヤーの画像を変更するtweenの進行度")]
    private float _percent = 0.75f;
    public event Action OnReachTheEdge; // 端に到達した時のEvent
    public event Action OnEdgeLeft;
    public event Action OnEdgeRight;

    public bool IsChanged { get; set; }

    public void SetSwingAngle(float angle)
    {
        _swingAngle = angle;
    }

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

        Tween tween1 = null;
        tween1 = transform.DOLocalRotate(new Vector3(0, 0, _swingAngle), _duration, RotateMode.LocalAxisAdd)
            .SetEase(_easeType)
            .OnUpdate(() =>
            {
                float elapsedPercentage = tween1.ElapsedPercentage(); // 進行度(0.0 ~ 1.0)
                if (elapsedPercentage >= _percent && !IsChanged) // 90%以上進んでいたら変更
                {
                    OnEdgeInvokeLeft();
                    IsChanged = true;
                }
            })
            .OnComplete(OnReach);

        Tween tween2 = null;
        tween2 = transform.DOLocalRotate(new Vector3(0, 0, -_swingAngle), _duration, RotateMode.LocalAxisAdd)
            .SetEase(_easeType)
            .OnUpdate(() =>
            {
                float elapsedPercentage = tween2.ElapsedPercentage(); // 進行度(0.0 ~ 1.0)
                if (elapsedPercentage >= _percent && !IsChanged) // 90%以上進んでいたら変更
                {
                    OnEdgeInvokeRight();
                    IsChanged = true;
                }
            })
            .OnComplete(OnReach);

        sequence.Append(tween1).Append(tween2);

        sequence.SetLoops(-1);
    }

    /// <summary>
    /// 振り子が右端に到達した時にEventを発火する
    /// </summary>
    private void OnReach()
    {
        if ((_swingAngle < 0 && transform.localEulerAngles.z < 360 + _swingAngle) ||
            (_swingAngle > 0 && transform.localEulerAngles.z < _swingAngle / 2))
        {
            OnReachTheEdge?.Invoke();
        }

        IsChanged = false;
    }

    private void OnEdgeInvokeRight()
    {
        if (_swingAngle < 0) OnEdgeRight?.Invoke();
        else OnEdgeLeft?.Invoke();
    }

    private void OnEdgeInvokeLeft()
    {
        if (_swingAngle < 0) OnEdgeLeft?.Invoke();
        else OnEdgeRight?.Invoke();
    }
}
