using System;
using DG.Tweening;
using UnityEngine;

/// <summary>
/// タイトル画面の振り子用のクラス
/// </summary>
public class TitlePendulumController : MonoBehaviour
{
    [Header("初期設定")]
    [SerializeField, Tooltip("振り子が動く角度")]
    private float _swingAngle = 90f; //
    [SerializeField, Tooltip("往復する時間の半分")] private float _duration = 2f;
    [SerializeField, Tooltip("イージングの種類")] private Ease _easeType = Ease.InOutSine; // 次の振り子に飛び乗るevent

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
                .SetEase(_easeType))
            .Append(transform.DOLocalRotate(new Vector3(0, 0, -_swingAngle), _duration, RotateMode.LocalAxisAdd)
                .SetEase(_easeType));

        sequence.SetLoops(-1);
    }
}
