using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

/// <summary>
/// Result画面のManagerクラス
/// </summary>
public class ResultManager : MonoBehaviour
{
    [SerializeField, Tooltip("今回のスコア")] private TextMeshProUGUI _thisScoreText;
    [SerializeField, Tooltip("ランキングのスコア")] private TextMeshProUGUI[] _rankingTexts;
    [SerializeField] private ResultTween _tween;
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

        _rankingManager.AddScore(score); // 今回のスコアを追加する
        UpdateResultText(score, _rankingManager.LoadScores()); // UI更新
    }

    /// <summary>
    /// UIの更新を行う
    /// </summary>
    private async UniTask UpdateResultText(int score, List<int> ranking)
    {
        await _tween.NumberCounter(score, _thisScoreText); // 今回のスコア用

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
    }
}
