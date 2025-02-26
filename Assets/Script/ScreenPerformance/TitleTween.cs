using DG.Tweening;
using UnityEngine;

/// <summary>
/// タイトル画面でのTween
/// </summary>
public class TitleTween : MonoBehaviour
{
    [Header("ロゴの設定")]
    [SerializeField] private Transform _logo;
    [SerializeField, Tooltip("移動量(下に下がる)")] private float _amountOfMovement = 10f;
    [SerializeField] private float _logoAnimDuration = 2f;

    [Header("スタートボタンの設定")]
    [SerializeField] private Transform _startButton;
    [SerializeField, Tooltip("拡大率")] private float _magnificationRate = 1.05f;
    [SerializeField] private float _startButtonAnimDuration = 1f;

    private void Start()
    {
        TweenStart();
    }

    /// <summary>
    /// Tweenを開始する
    /// </summary>
    public void TweenStart()
    {
        LogoTween();
        StartButtonTween();
    }

    /// <summary>
    /// ロゴのTween・上下にふわふわ動く
    /// </summary>
    private void LogoTween()
    {
        _logo.DOLocalMoveY(_logo.localPosition.y - _amountOfMovement,_logoAnimDuration).SetLoops(-1, LoopType.Yoyo);
    }

    /// <summary>
    /// 「スタートと叫べ！」のTween・ゆっくり拡大縮小する
    /// </summary>
    private void StartButtonTween()
    {
        _startButton.DOScale(transform.localScale * _magnificationRate, _startButtonAnimDuration).SetLoops(-1, LoopType.Yoyo);
    }
}
