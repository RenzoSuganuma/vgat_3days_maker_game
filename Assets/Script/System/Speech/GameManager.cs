using System;
using System.Collections.Generic;
using R3;
using UnityEngine;
using UnityEngine.Serialization;
using Cysharp.Threading.Tasks;

public class GameManager : MonoBehaviour
{
    private string _resourcesLoadPath = "GloomyBeat_speechText";
    [SerializeField] private VoiceInputHandler _voiceInputHandler;
    [SerializeField] private DropDownDevice _dropDownDevice;
    [SerializeField] private MissionsDisplay _missionsDisplay;

    private Stack<string> _wordStack = new();
    private Dictionary<string, string> _voiceData;

    private VoiceRecognitionSettings _voiceRecognitionSettings;
    private GameFlowSettings _gameFlowSettings;


    private SpeechToTextVolume _speechToTextVolume;
    private VoiceJudgement _voiceJudgement;
    private TextGenerator _textGenerator;
    private string _currentPhrase;

    public VoiceRecognitionSettings VoiceRecognitionSettings => _voiceRecognitionSettings;
    public GameFlowSettings GameFlowSettings => _gameFlowSettings;

    public SpeechToTextVolume SpeechToTextVolume => _speechToTextVolume;
    public VoiceJudgement VoiceJudgement => _voiceJudgement;
    public Dictionary<string, string> VoiceData => _voiceData;


    private void Awake()
    {
        var settings = Resources.Load<GameSettings>("GameSettings");
        if (settings != null)
        {
            _voiceRecognitionSettings.LowThreshold = settings.VoiceRecognitionSettings.LowThreshold;
            _voiceRecognitionSettings.MidThreshold = settings.VoiceRecognitionSettings.MidThreshold;
            _voiceRecognitionSettings.HighThreshold = settings.VoiceRecognitionSettings.HighThreshold;
            _voiceRecognitionSettings.Similarity = settings.VoiceRecognitionSettings.Similarity;

            _gameFlowSettings.StackSize = settings.GameFlowSettings.StackSize;
            _gameFlowSettings.NextTurnMilliSecDelay = settings.GameFlowSettings.NextTurnMilliSecDelay;

            _resourcesLoadPath = settings.GameLoadResourcesSettings.ResourcesLoadSpeechTextPath;
        }


        _dropDownDevice.Construct(_speechToTextVolume);
        _textGenerator = new TextGenerator(_resourcesLoadPath);
        _voiceData = _textGenerator.GetVoiceData();

        if (_voiceData == null || _voiceData.Count == 0)
        {
            Debug.LogError("⚠ `csv` のロードに失敗しました！");
            return;
        }

        _speechToTextVolume = new SpeechToTextVolume(settings);
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
        if (_wordStack.Count == 0)
        {
            InitializeWordStack();
        }

        if (_wordStack.Count > 0)
        {
            _currentPhrase = _wordStack.Pop();
            _missionsDisplay.SetMissionText(_currentPhrase);

            _missionsDisplay.SetNextText(_wordStack.Peek());
        }
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
