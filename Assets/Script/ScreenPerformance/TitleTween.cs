using DG.Tweening;
using UnityEngine;

/// <summary>
/// タイトル画面でのTween
/// </summary>
public class TitleTween : MonoBehaviour
{
    [SerializeField] private Transform _logo;
    [SerializeField] private Transform _startButton;

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
        _logo.DOLocalMoveY(_logo.localPosition.y - 10f,2f).SetLoops(-1, LoopType.Yoyo);
    }

    /// <summary>
    /// 「スタートと叫べ！」のTween・ゆっくり拡大縮小する
    /// </summary>
    private void StartButtonTween()
    {
        _startButton.DOScale(transform.localScale * 1.05f, 1f).SetLoops(-1, LoopType.Yoyo);
    }
}
