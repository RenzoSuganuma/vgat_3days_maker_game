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
    [SerializeField] private float _countUpDuration = 1.5f; // カウントアップにかける時間
    private int _startValue = 0;

    [Header("ランキング表示後のTweenの設定")]
    [SerializeField] private float _tweenDuration = 1f;
    [SerializeField] private Transform _backTitleButton;
    [SerializeField] private Transform _rankingBoard;

    private void Start()
    {
        _backTitleButton.localScale = new Vector3(1.1f, 1.1f, 1.1f);
        _backTitleButton.gameObject.SetActive(false); // ボタンは最初は非表示に
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
        }, endValue, _countUpDuration).SetEase(Ease.OutQuad);

        await UniTask.Delay((int)_countUpDuration * 1000);
    }

    /// <summary>
    /// ランキングを表示し終わった後のTween
    /// </summary>
    public async UniTask BackTitle()
    {
        // ボタンのスケールを1に戻す
        _backTitleButton.gameObject.SetActive(true);
        _backTitleButton.DOScale(Vector3.one, _tweenDuration); // ボタンのscaleを元に戻す

        await UniTask.Delay((int)_tweenDuration + 1 * 1000);

        _rankingBoard.DOScale(new Vector3(1.01f, 1.01f, 1.01f), _tweenDuration).SetLoops(-1, LoopType.Yoyo);
    }
}
