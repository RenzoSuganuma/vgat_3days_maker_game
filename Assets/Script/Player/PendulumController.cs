using UnityEngine;
using DG.Tweening;

/// <summary>
/// 振り子挙動を制御する
/// </summary>
public class PendulumController : MonoBehaviour
{
    [Header("初期設定")]
    [SerializeField] private float _swingAngle = 90f; //
    [SerializeField] private float _duration = 2f;
    [SerializeField] private Ease _easeType = Ease.InOutSine; // 次の振り子に飛び乗るevent

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
            .Append(transform.DOLocalRotate(new Vector3(0, 0, _swingAngle), _duration, RotateMode.LocalAxisAdd).SetEase(_easeType))
            .Append(transform.DOLocalRotate(new Vector3(0, 0, -_swingAngle), _duration, RotateMode.LocalAxisAdd).SetEase(_easeType));

        sequence.SetLoops(-1, LoopType.Yoyo);
    }
}
