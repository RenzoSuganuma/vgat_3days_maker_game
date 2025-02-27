using System;
using System.Collections.Generic;
using R3;
using UnityEngine;
using UnityEngine.Serialization;
using Cysharp.Threading.Tasks;

public class GameManager : MonoBehaviour
{
    [SerializeField] private VoiceInputHandler _voiceInputHandler;
    [SerializeField] private DropDownDevice _dropDownDevice;
    [SerializeField] private MissionsDisplay _missionsDisplay;

    private GameSettings _gameSettings;
    private Stack<string> _wordStack = new();
    private Dictionary<string, string> _voiceData = new();

    private string _resourcesLoadPath ;
    private readonly Stack<string> _wordStack = new();
    private Dictionary<string, string> _voiceData = new();

    private readonly VoiceRecognitionSettings _voiceRecognitionSettings = new();
    private readonly GameFlowSettings _gameFlowSettings = new();

    private SpeechToTextVolume _speechToTextVolume;
    private VoiceJudgement _voiceJudgement;
    private TextGenerator _textGenerator;
    private string _currentPhrase;

    public MissionsDisplay MissionsDisplay => _missionsDisplay;
    public VoiceRecognitionSettings VoiceRecognitionSettings => _voiceRecognitionSettings;
    public GameFlowSettings GameFlowSettings => _gameFlowSettings;

    public SpeechToTextVolume SpeechToTextVolume => _speechToTextVolume;
    public VoiceJudgement VoiceJudgement => _voiceJudgement;
    public Dictionary<string, string> VoiceData => _voiceData;


    private void Start()
    {
        _gameSettings = Resources.Load<GameSettings>("GameSettings");
        if (_gameSettings != null)
        {
            _voiceRecognitionSettings.LowThreshold = _gameSettings.VoiceRecognitionSettings.LowThreshold;
            _voiceRecognitionSettings.MidThreshold = _gameSettings.VoiceRecognitionSettings.MidThreshold;
            _voiceRecognitionSettings.HighThreshold = _gameSettings.VoiceRecognitionSettings.HighThreshold;
            _voiceRecognitionSettings.Similarity = _gameSettings.VoiceRecognitionSettings.Similarity;

            _gameFlowSettings.StackSize = _gameSettings.GameFlowSettings.StackSize;
            _gameFlowSettings.NextTurnMilliSecDelay = _gameSettings.GameFlowSettings.NextTurnMilliSecDelay;

            _resourcesLoadPath = _gameSettings.GameLoadResourcesSettings.ResourcesLoadSpeechTextPath;
        }

        _textGenerator = new TextGenerator(_resourcesLoadPath);
        _voiceData = _textGenerator.GetVoiceData();

        if (_voiceData == null || _voiceData.Count == 0)
        {
            Debug.LogError("⚠ `csv` のロードに失敗しました！");
            return;
        }

        _speechToTextVolume = new SpeechToTextVolume(_gameSettings);

        _dropDownDevice.Construct(_gameSettings, _speechToTextVolume);

        _voiceJudgement = new VoiceJudgement(this);
        _voiceInputHandler.Initialize(this);
        _missionsDisplay = FindAnyObjectByType<MissionsDisplay>();

        SetNextMission();
    }


    /// <summary>
    /// 次のミッションのフレーズを設定
    /// </summary>
    private void SetNextMission()
    {
        if (_wordStack.Count < 2)
        {
            InitializeWordStack();
        }

        _currentPhrase = _wordStack.Pop();
        _missionsDisplay.SetMissionText(_currentPhrase);
        _missionsDisplay.SetNextText(_wordStack.Peek());
    }


    /// <summary>
    /// ワードリストを初期化
    /// </summary>
    private void InitializeWordStack()
    {
        List<string> keys = new List<string>(_voiceData.Keys);
        Shuffle(keys);

        int index = 0;

        while (_wordStack.Count < _gameFlowSettings.StackSize)
        {
            if (index >= keys.Count)
            {
                Shuffle(keys); // すべてのワードを使い切ったら再シャッフル
                index = 0;
            }

            _wordStack.Push(keys[index]);
            index++;
        }

        Debug.Log($"ワードリストを初期化しました: {_wordStack.Count} 件");
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
        await UniTask.Delay(TimeSpan.FromMilliseconds(_gameFlowSettings.NextTurnMilliSecDelay));
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
