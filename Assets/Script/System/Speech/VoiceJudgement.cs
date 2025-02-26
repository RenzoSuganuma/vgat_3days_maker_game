using System.Collections.Generic;
using UnityEngine;

// 音声の音量とフレーズの判定を行う
public class VoiceJudgement
{
    private Dictionary<string, string> _voiceData;
    private float _lowThreshold; // 小さい声
    private float _midThreshold; // 普通の声
    private float _highThreshold; // 大きい声

    private float _similarity = 0.8f; //以上一致したらOK;

    public VoiceJudgement(Dictionary<string, string> voiceData,
        float lowThreshold, float midThreshold, float highThreshold,
        float similarity)
    {
        _voiceData = voiceData;
        _lowThreshold = lowThreshold;
        _midThreshold = midThreshold;
        _highThreshold = highThreshold;
        _similarity = similarity;
    }

    /// <summary>
    /// 音量に応じたレーン移動を決定
    /// </summary>
    public int DetermineLaneChange(float maxVolume)
    {
        if (maxVolume < _lowThreshold) return -1;
        if (maxVolume < _midThreshold) return 0;
        return maxVolume < _highThreshold ? 1 : 0;
    }

    /// <summary>
    /// 音声が正しく発音されたかを判定
    /// </summary>
    public bool CheckVoice(string recognizedText)
    {
        foreach (var pair in _voiceData)
        {
            string correctText = pair.Value;

            float similarity = CalculateSimilarity(recognizedText, correctText);
            if (similarity >= _similarity) // 00%以上一致したらOK
            {
                Debug.Log($"正しく発音されました！ {recognizedText} ≈ {correctText}");
                return true;
            }
        }

        Debug.Log($"発音が間違っています: {recognizedText}");
        return false;
    }

    /// <summary>
    /// 文字列の類似度（Levenshtein距離）を計算
    /// </summary>
    private float CalculateSimilarity(string input, string target)
    {
        int lenInput = input.Length;
        int lenTarget = target.Length;
        int[,] dp = new int[lenInput + 1, lenTarget + 1];

        for (int i = 0; i <= lenInput; i++) dp[i, 0] = i;
        for (int j = 0; j <= lenTarget; j++) dp[0, j] = j;

        for (int i = 1; i <= lenInput; i++)
        {
            for (int j = 1; j <= lenTarget; j++)
            {
                int cost = (input[i - 1] == target[j - 1]) ? 0 : 1;
                dp[i, j] = Mathf.Min(dp[i - 1, j] + 1, Mathf.Min(dp[i, j - 1] + 1, dp[i - 1, j - 1] + cost));
            }
        }

        int distance = dp[lenInput, lenTarget];
        int maxLength = Mathf.Max(lenInput, lenTarget);
        return 1.0f - (float)distance / maxLength; // 類似度 (1.0 = 完全一致)
    }
}
