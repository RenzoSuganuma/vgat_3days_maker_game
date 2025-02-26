using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Result画面のManagerクラス
/// </summary>
public class ResultManager : MonoBehaviour
{
    [SerializeField, Tooltip("今回のスコア")] private TextMeshProUGUI _thisScoreText;
    [SerializeField, Tooltip("ランキングのスコア")] private TextMeshProUGUI[] _rankingTexts;
    private RankingManager _rankingManager; // ランキングのマネージャークラス

    private void Start()
    {
        _rankingManager = new RankingManager();

        _rankingManager.AddScore(1); // 今回のスコアを追加する
        UpdateResultText(1, _rankingManager.LoadScores()); // UI更新
    }

    /// <summary>
    /// UIの更新を行う
    /// </summary>
    private void UpdateResultText(int score, List<int> ranking)
    {
        _thisScoreText.text = score.ToString() + "m"; // 今回のスコア用

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
        }
    }
}
