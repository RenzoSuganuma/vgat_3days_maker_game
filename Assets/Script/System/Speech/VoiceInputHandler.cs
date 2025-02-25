using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using R3;

public class VoiceInputHandler : MonoBehaviour
{
    [SerializeField] private SpeechToTextVolume _speechToText;

    // 認識したフレーズ
    public ReactiveProperty<string> RecognizedText = new ReactiveProperty<string>();

    // 最大音量
    public ReactiveProperty<float> MaxSpeechVolume = new ReactiveProperty<float>();

    // 音声入力成功の監視
    public ReactiveProperty<bool> IsVoiceInputSuccessful = new ReactiveProperty<bool>(false);

    // レーン移動情報 (-1: 下がる, 0: 維持, 1: 上がる)
    public ReactiveProperty<int> LaneChange = new ReactiveProperty<int>(0);

    private void Start()
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
        });

        // 音量データを受け取る
        _speechToText.OnSpeechVolume.Subscribe(volume =>
        {
            MaxSpeechVolume.Value = volume;
            DetermineLaneChange(volume);
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
    /// 音量に応じたレーン移動を決定
    /// </summary>
    private void DetermineLaneChange(float maxVolume)
    {
        float lowThreshold = -30f; // 小さい声
        float midThreshold = -20f; // 普通の声
        float highThreshold = -10f; // 大きい声

        if (maxVolume < lowThreshold)
        {
            LaneChange.Value = -1; // 下がる
        }
        else if (maxVolume < midThreshold)
        {
            LaneChange.Value = 0; // 維持
        }
        else
        {
            LaneChange.Value = 1; // 上がる
        }
    }

    /// <summary>
    /// 音声認識を開始（Presenter経由で呼び出し）
    /// </summary>
    public void StartSpeechRecognition()
    {
        _speechToText?.StartSpeechRecognition();
    }

    /// <summary>
    /// 音声認識を停止（Presenter経由で呼び出し）
    /// </summary>
    public void StopSpeechRecognition()
    {
        _speechToText?.StopSpeechRecognition();
    }
}
