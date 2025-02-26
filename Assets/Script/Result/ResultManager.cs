using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Result画面のManagerクラス
/// </summary>
public class ResultManager : MonoBehaviour
{
    [SerializeField, Tooltip("今回のスコア")] private TextMeshProUGUI _thisScoreText;
    [SerializeField, Tooltip("ランキングのスコア")] private TextMeshProUGUI[] _rankingTexts;
    [SerializeField] private ResultTween _tween;
    [SerializeField] private Button _backTitleButton;
    private RankingManager _rankingManager; // ランキングのマネージャークラス

    private void Start()
    {
        _rankingManager = new RankingManager();

        // ランキングのテキストは全て非表示にしておく
        foreach (TextMeshProUGUI text in _rankingTexts)
        {
            text.gameObject.SetActive(false);
        }

        int score = 10;

        // ランキング関連
        _rankingManager.AddScore(score); // 今回のスコアを追加する
        UpdateResultText(score, _rankingManager.LoadScores()); // UI更新

        // ボタン関係
        _backTitleButton.onClick.AddListener(Foundation.ResetGame);
    }

    private void OnDestroy()
    {
        _backTitleButton.onClick.RemoveAllListeners();
    }

    /// <summary>
    /// UIの更新を行う
    /// </summary>
    private async UniTask UpdateResultText(int score, List<int> ranking)
    {
        AudioManager.Instance.PlaySE(SENameEnum.RollUp);
        await _tween.NumberCounter(score, _thisScoreText); // 今回のスコア用

        AudioManager.Instance.PlaySE(SENameEnum.Ranking);
        for (int i = 0; i < _rankingTexts.Length; i++) //ランキング用
        {
            if (i < ranking.Count)
            {
                _rankingTexts[i].text = $"{i+1}位 {ranking[i]}m";

            }
            else
            {
                _rankingTexts[i].text = "";
            }

            _rankingTexts[i].gameObject.SetActive(true); // アクティブにする
            
            await UniTask.Delay(500);
        }

        await UniTask.Delay(1000);

        _tween.BackTitle().Forget();
    }
}
