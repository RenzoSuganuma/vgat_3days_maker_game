using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private string _resourcesLoadPath = "GloomyBeat_speachText";
    [SerializeField] private VoiceInputHandler _voiceInputHandler;
    [SerializeField] private MissionsText _missionsText;

    private Dictionary<string, string> _voiceData;
    [SerializeField] private float _lowThreshold = -30f; // 小さい声
    [SerializeField] private float _midThreshold = -20f; // 普通の声
    [SerializeField] private float _highThreshold = -10f; // 大きい声
    [SerializeField] private float _similarity = 0.8f; // 以上一致したらOK

    private SpeechToTextVolume _speechToTextVolume;
    private VoiceJudgement _voiceJudgement;
    private TextGenerator _textGenerator;
    private string _currentPhrase;

    public SpeechToTextVolume SpeechToTextVolume => _speechToTextVolume;
    public VoiceJudgement VoiceJudgement => _voiceJudgement;
    public Dictionary<string, string> VoiceData => _voiceData;
    public float LowThreshold => _lowThreshold;
    public float MidThreshold => _midThreshold;
    public float HighThreshold => _highThreshold;
    public float Similarity => _similarity;

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
        _voiceJudgement = new VoiceJudgement(this);

        _voiceInputHandler.Initialize(this);
        SetNextMission();
    }

    /// <summary>
    /// 次のミッションのフレーズを設定
    /// </summary>
    private void SetNextMission()
    {
        _currentPhrase = GetRandomPhrase();
        _missionsText.SetMissionText(_currentPhrase);
    }

    /// <summary>
    /// ランダムなフレーズを取得
    /// </summary>
    private string GetRandomPhrase()
    {
        List<string> keys = new List<string>(_voiceData.Keys);
        return keys[UnityEngine.Random.Range(0, keys.Count)];
    }

    /// <summary>
    /// 音声認識の成功時の処理
    /// </summary>
    public void OnMissionSuccess()
    {
        _missionsText.MissionSuccess();
        Invoke(nameof(SetNextMission), 2.0f); // 2秒後に次のミッションへ
    }

    /// <summary>
    /// 音声認識の失敗時の処理
    /// </summary>
    public void OnMissionFail()
    {
        _missionsText.MissionFail();
    }

    /// <summary>
    /// `GameManager` の終了時に `SpeechToTextVolume.Dispose()` を実行
    /// </summary>
    private void OnDestroy()
    {
        _speechToTextVolume?.Dispose();
    }
}
