using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;

/// <summary>
/// Result画面で使用するtweenをまとめたクラス
/// </summary>
public class ResultTween : MonoBehaviour
{
    [Header("キャラクター移動Tweenの設定")]
    [SerializeField] private Transform _character;
    [SerializeField, Tooltip("移動量")] private float _movementDistance = 125;
    [SerializeField, Tooltip("移動にかける時間")] private float _moveDuration = 0.8f;

    [Header("テキストのカウントアップTweenの設定")]
    [SerializeField] private float _duration = 1.5f; // カウントアップにかける時間
    private int _startValue = 0;

    [SerializeField] private TextMeshProUGUI _text;

    private void Start()
    {
        MoveCharacter();
    }

    /// <summary>
    /// キャラクターのTween。画面右から左へスライドする
    /// </summary>
    private void MoveCharacter()
    {
        _character.DOLocalMoveX(_character.transform.localPosition.x - _movementDistance, _moveDuration);
    }

    /// <summary>
    /// テキストをカウントアップするTween
    /// </summary>
    public async UniTask NumberCounter(int endValue, TextMeshProUGUI text)
    {
        int currentValue = _startValue;

        DOTween.To(() => currentValue, x => {
            currentValue = x;
            text.text = $"{currentValue.ToString()}m";
        }, endValue, _duration).SetEase(Ease.OutQuad);

        await UniTask.Delay((int)_duration * 1000);
    }
}
