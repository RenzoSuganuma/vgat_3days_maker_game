using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private string _resourcesLoadPath;
    [SerializeField] private VoiceInputHandler _voiceInputHandler;

    private Dictionary<string, string> _voiceData;
    [SerializeField] private float _lowThreshold = -30f; // 小さい声
    [SerializeField] private float _midThreshold = -20f; // 普通の声
    [SerializeField] private float _highThreshold = -10f; // 大きい声
    [SerializeField] private float _similarity = 0.8f; // 以上一致したらOK

    private SpeechToTextVolume _speechToTextVolume;
    private VoiceJudgement _voiceJudgement;
    private TextGenerator _textGenerator;

    private void Awake()
    {
        _textGenerator = new TextGenerator(_resourcesLoadPath);
        _voiceData = _textGenerator.GetVoiceData();

        if (_voiceData == null || _voiceData.Count == 0)
        {
            Debug.LogError("⚠ `voice_data.csv` のロードに失敗しました！");
            return;
        }

        _speechToTextVolume = new SpeechToTextVolume();

        _voiceJudgement = new VoiceJudgement(_voiceData, _lowThreshold, _midThreshold, _highThreshold, _similarity);

        _voiceInputHandler.Initialize(_speechToTextVolume, _voiceJudgement);
    }

    /// <summary>
    /// `GameManager` の終了時に `SpeechToTextVolume.Dispose()` を実行
    /// </summary>
    private void OnDestroy()
    {
        _speechToTextVolume?.Dispose();
    }
}
