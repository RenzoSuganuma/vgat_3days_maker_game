using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using R3;

// 他の人が簡単に参照出来るようにするために敢えてMonoBehaviourを継承している
public class VoiceInputHandler : MonoBehaviour
{
    private GameManager _gameManager;
    private SpeechToTextVolume _speechToText;
    private VoiceJudgement _voiceJudgement;

    // 認識したフレーズ
    public ReactiveProperty<string> RecognizedText = new ReactiveProperty<string>();

    // 正しいフレーズかどうか
    public ReactiveProperty<bool> IsCorrectVoice = new ReactiveProperty<bool>(false);

    // 最大音量
    public ReactiveProperty<float> MaxSpeechVolume = new ReactiveProperty<float>(-100f);

    // 音声入力成功の監視
    public ReactiveProperty<bool> IsVoiceInputSuccessful = new ReactiveProperty<bool>(false);

    // レーン移動情報 (-1: 下がる, 0: 維持, 1: 上がる)
    public ReactiveProperty<int> LaneChange = new ReactiveProperty<int>(0);

    /// <summary>
    /// GameManager から `SpeechToTextVolume` と `VoiceJudgement` を設定
    /// </summary>
    public void Initialize(GameManager gameManager)
    {
        _gameManager = gameManager;
        _speechToText = gameManager.SpeechToTextVolume;
        _voiceJudgement = gameManager.VoiceJudgement;

        InitializeSubscriptions();
    }

    private void InitializeSubscriptions()
    {
        if (_speechToText == null)
        {
            Debug.LogError("⚠ SpeechToTextVolume がアタッチされていません！");
            return;
        }

        // 音声認識結果を受け取る
        _speechToText.OnSpeechResult.Subscribe(text =>
        {
            RecognizedText.Value = text;
            IsVoiceInputSuccessful.Value = true;
            IsCorrectVoice.Value = _voiceJudgement.CheckVoice(text);
        });

        // 音量データを受け取る
        _speechToText.OnSpeechVolume.Subscribe(volume =>
        {
            MaxSpeechVolume.Value = volume;
            LaneChange.Value = _voiceJudgement.DetermineLaneChange(volume);
        });

        // 音声入力成功フラグを1秒後にリセット
        IsVoiceInputSuccessful.Subscribe(success =>
        {
            if (success)
            {
                UniTask.Delay(TimeSpan.FromSeconds(1)).ContinueWith(() => { IsVoiceInputSuccessful.Value = false; });
            }
        });
    }


    /// <summary>
    /// 音声認識を開始（Presenter経由で呼び出し）
    /// </summary>
    public void StartSpeechRecognition() => _speechToText?.StartSpeechRecognition();

    /// <summary>
    /// 音声認識を停止（Presenter経由で呼び出し）
    /// </summary>
    public void StopSpeechRecognition() => _speechToText?.StopSpeechRecognition();
}
