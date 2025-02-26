using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TextGenerator
{
    private readonly Dictionary<string, string> _voiceData = new Dictionary<string, string>();

    public TextGenerator(string resourcePath)
    {
        LoadVoiceData(resourcePath);
    }

    /// <summary>
    /// Resources から CSV をロードし、辞書に格納
    /// </summary>
    private void LoadVoiceData(string resourcePath)
    {
        TextAsset csvFile = Resources.Load<TextAsset>(resourcePath);
        if (csvFile == null)
        {
            Debug.LogError($"⚠ `{resourcePath}` のロードに失敗しました！");
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

        Debug.Log($"{resourcePath}: { _voiceData.Count } 件の音声データをロードしました");
    }

    /// <summary>
    /// 音声データを取得する（GameManagerで使用）
    /// </summary>
    public Dictionary<string, string> GetVoiceData()
    {
        return _voiceData;
    }
}
