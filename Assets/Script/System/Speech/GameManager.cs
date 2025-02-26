using System;
using System.Collections.Generic;
using R3;
using UnityEngine;
using UnityEngine.Serialization;
using Cysharp.Threading.Tasks;

public class GameManager : MonoBehaviour
{
    [SerializeField] private string _resourcesLoadPath = "GloomyBeat_speachText";
    [SerializeField] private VoiceInputHandler _voiceInputHandler;
    [SerializeField] private DropDownDevice _dropDownDevice;
    [SerializeField] private MissionsDisplay _missionsDisplay;

    private Stack<string> _wordStack = new Stack<string>();
    private Dictionary<string, string> _voiceData;
    [SerializeField] private float _lowThreshold = -30f; // 小さい声
    [SerializeField] private float _midThreshold = -20f; // 普通の声
    [SerializeField] private float _highThreshold = -10f; // 大きい声
    [SerializeField] private float _similarity = 0.8f; // 以上一致したらOK

    [SerializeField] private int _stackSize = 10;
    [SerializeField] private int _nextTurnMillisecDelay = 1000;

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
        _dropDownDevice.Construct(_speechToTextVolume);
        _textGenerator = new TextGenerator(_resourcesLoadPath);
        _voiceData = _textGenerator.GetVoiceData();

        if (_voiceData == null || _voiceData.Count == 0)
        {
            Debug.LogError("⚠ `csv` のロードに失敗しました！");
            return;
        }

        _speechToTextVolume = new SpeechToTextVolume();
        _voiceJudgement = new VoiceJudgement(this);

        _voiceInputHandler.Initialize(this);
        _dropDownDevice.Construct(_speechToTextVolume);


        _voiceInputHandler.MaxSpeechVolume.Subscribe(volume => { _missionsDisplay.SetMaxDbText(volume); });
        SpeechToTextVolume.OnSpeechResult.Subscribe(volume => { _missionsDisplay.SetPlayerText(volume); });
        SetNextMission();
    }


    /// <summary>
    /// 次のミッションのフレーズを設定
    /// </summary>
    private void SetNextMission()
    {
        if (_wordStack.Count == 1)
        {
            InitializeWordStack();
        }

        if (_wordStack.Count > 1)
        {
            _currentPhrase = _wordStack.Pop();
            _missionsDisplay.SetMissionText(_currentPhrase);
        }
    }

    /// <summary>
    /// ワードリストを初期化
    /// </summary>
    private void InitializeWordStack()
    {
        List<string> keys = new List<string>(_voiceData.Keys);
        Shuffle(keys);

        foreach (var key in keys)
        {
            _wordStack.Push(key);
        }
    }


    /// <summary>
    /// ワードリストをシャッフルする (Fisher-Yatesアルゴリズム)
    /// </summary>
    private void Shuffle<T>(List<T> list)
    {
        System.Random rng = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            (list[n], list[k]) = (list[k], list[n]);
        }
    }


    /// <summary>
    /// 音声認識の成功時の処理
    /// </summary>
    public async void OnMissionSuccess()
    {
        _missionsDisplay.MissionSuccess();
        await UniTask.Delay(TimeSpan.FromMilliseconds(_nextTurnMillisecDelay));
        SetNextMission();
    }

    /// <summary>
    /// 音声認識の失敗時の処理
    /// </summary>
    public void OnMissionFail()
    {
        _missionsDisplay.MissionFail();
    }

    /// <summary>
    /// `GameManager` の終了時に `SpeechToTextVolume.Dispose()` を実行
    /// </summary>
    private void OnDestroy()
    {
        _speechToTextVolume?.Dispose();
    }
}
