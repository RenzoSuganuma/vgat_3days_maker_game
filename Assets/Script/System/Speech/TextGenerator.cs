using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class TextGenerator
{
    private readonly string _resourcesLoadPath;
    private readonly Dictionary<string, string> _voiceData = new Dictionary<string, string>();


    /// <summary>
    /// コンストラクタ
    /// </summary>
    private TextGenerator(string resourcesLoadPath)
    {
        _resourcesLoadPath = resourcesLoadPath;
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

    // CSVの中からランダムで1つのフレーズを取得して返す
    public string GetRandomVoiceData()
    {
        int index = Random.Range(0, _voiceData.Count);
        return _voiceData.Values.ElementAt(index);
    }
}
