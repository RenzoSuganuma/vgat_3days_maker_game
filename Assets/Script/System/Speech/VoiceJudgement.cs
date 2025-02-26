using System.Collections.Generic;
using System.IO;
using UnityEngine;

// 音声の音量とフレーズの判定を行う
public class VoiceJudgement : MonoBehaviour
{
    [SerializeField] private string _resourcesLoadPath = "voice_data";
    private readonly Dictionary<string, string> _voiceData = new Dictionary<string, string>();
    VoiceInputHandler _voiceInputHandler;

    public void Start()
    {
        LoadVoiceData();
    }

    /// <summary>
    /// CSV から比較データをロード
    /// </summary>
    private void LoadVoiceData()
    {
        TextAsset csvFile = Resources.Load<TextAsset>(_resourcesLoadPath);
        if (csvFile == null)
        {
            Debug.LogError($"⚠ {_resourcesLoadPath} が見つかりません！");
            return;
        }

        StringReader reader = new StringReader(csvFile.text);
        while (reader.Peek() != -1)
        {
            string line = reader.ReadLine();
            string[] values = line.Split(',');

            if (values.Length == 2)
            {
                _voiceData[values[0]] = values[1]; // 画面表示用 → 比較用テキスト
            }
        }

        Debug.Log($"✅ {_voiceData.Count} 件の音声データをロードしました");
    }

    /// <summary>
    /// 音量に応じたレーン移動を決定
    /// </summary>
    public int DetermineLaneChange(float maxVolume)
    {
        float lowThreshold = -30f; // 小さい声
        float midThreshold = -20f; // 普通の声
        float highThreshold = -10f; // 大きい声

        if (maxVolume < lowThreshold)
        {
            return _voiceInputHandler.LaneChange.Value = -1; // 下がる
        }
        else if (maxVolume < midThreshold)
        {
            return _voiceInputHandler.LaneChange.Value = 0; // 維持
        }
        else
        {
            return _voiceInputHandler.LaneChange.Value = 1; // 上がる
        }
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
            if (similarity >= 0.8f) // 80%以上一致したらOK
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

        for (int i = 0; i <= lenInput; i++)
            dp[i, 0] = i;
        for (int j = 0; j <= lenTarget; j++)
            dp[0, j] = j;

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
