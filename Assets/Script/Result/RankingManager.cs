using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// ランキングを管理するクラス
/// </summary>
public class RankingManager
{
    private const int _maxRank = 5; // 保存しておくランクの数
    private const string _rankKeyPrefix = "Rank_";

    /// <summary>
    /// スコアを渡して順位付けを行う処理
    /// </summary>
    public void AddScore(int newScore)
    {
        List<int> scores = LoadScores();
        scores.Add(newScore);
        scores.Sort((a, b) => b.CompareTo(a)); // 降順ソート
        if (scores.Count > _maxRank)
        {
            scores.RemoveAt(_maxRank); // 6位以下は削除する
        }
        SaveScores(scores);
    }

    /// <summary>
    /// 保存されているスコアをロードしてくる
    /// </summary>
    public List<int> LoadScores()
    {
        List<int> scores = new List<int>();
        for (int i = 0; i < _maxRank; i++)
        {
            scores.Add(PlayerPrefs.GetInt(_rankKeyPrefix + i, 0));
        }
        return scores;
    }

    /// <summary>
    /// スコアをPLayerPrefsに保存する
    /// </summary>
    private void SaveScores(List<int> scores)
    {
        for (int i = 0; i < _maxRank; i++)
        {
            PlayerPrefs.SetInt(_rankKeyPrefix + i, scores[i]);
        }
        PlayerPrefs.Save();
    }

    /// <summary>
    /// PlayerPrefsをクリアする
    /// </summary>
    public void ClearScores()
    {
        for (int i = 0; i < _maxRank; i++)
        {
            PlayerPrefs.DeleteKey(_rankKeyPrefix + i);
        }
        PlayerPrefs.Save();
    }
}

