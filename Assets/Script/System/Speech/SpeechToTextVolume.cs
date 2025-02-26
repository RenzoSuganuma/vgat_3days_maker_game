using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;
using R3;
using Cysharp.Threading.Tasks;

public class SpeechToTextVolume : MonoBehaviour
{
    [SerializeField] private VoiceInputHandler _voiceInputHandler;
    private DictationRecognizer _dictationRecognizer;

    public Subject<string> OnSpeechResult = new Subject<string>(); // 認識した音声を通知
    public Subject<float> OnSpeechVolume = new Subject<float>(); // 最大音量を通知

    [SerializeField] private string _deviceName;
    private string _targetDevice = "";
    private AudioClip _audioClip;

    private int _lastAudioPos;
    private void Start()
    {
        _dictationRecognizer = new DictationRecognizer();

        _dictationRecognizer.DictationResult += DictationRecResult;
        _dictationRecognizer.DictationError += DictationRecError;

        // マイクの初期化
        InitMicrophone();

        Debug.Log("SpeechToText: 初期化完了");
    }

    /// <summary>
    /// マイクを初期化し、録音を開始
    /// </summary>
    private void InitMicrophone()
    {
        foreach (var device in Microphone.devices)
        {
            Debug.Log($"Device Name: {device}");
            if (device.Contains(_deviceName))
            {
                _targetDevice = device;
            }
        }

        Debug.Log($"<color=green> 録音デバイス: {_targetDevice}</color>");
        _audioClip = Microphone.Start(_targetDevice, true, 10, 48000);
    }

    /// <summary>
    /// デバイス名を設定
    /// </summary>
    public void SetDeviceName(string deviceName)
    {
        _deviceName = deviceName;
        InitMicrophone(); // 新しいデバイスでマイクを再初期化
    }

    /// <summary>
    /// 音声認識を開始
    /// </summary>
    public void StartSpeechRecognition()
    {
        if (_dictationRecognizer.Status != SpeechSystemStatus.Stopped) return;
        _dictationRecognizer.Start();
        Debug.Log("🎤 音声認識開始");
        _ = CaptureSpeechVolume();
    }

    /// <summary>
    /// 音声認識を停止
    /// </summary>
    public void StopSpeechRecognition()
    {
        if (_dictationRecognizer.Status != SpeechSystemStatus.Running) return;
        _dictationRecognizer.Stop();
        Debug.Log("🛑 音声認識停止");
    }

    /// <summary>
    /// 音声が認識されたときに発生するイベント
    /// </summary>
    private void DictationRecResult(string text, ConfidenceLevel confidence)
    {
        Debug.Log($" 認識した音声： {text}");
        OnSpeechResult.OnNext(text);
    }

    /// <summary>
    /// マイクの音量を取得し、最大値を送信
    /// </summary>
    private async UniTask CaptureSpeechVolume()
    {
        if (_voiceInputHandler == null)
        {
            Debug.LogWarning("⚠ `_voiceInputHandler` が設定されていません");
            return;
        }

        float maxVolume = -100f; // 初期値

        float startTime = Time.time;
        while (_dictationRecognizer.Status == SpeechSystemStatus.Running)
        {
            float volume = GetUpdatedAudio();
            maxVolume = Mathf.Max(maxVolume, volume);
            await UniTask.Delay(TimeSpan.FromMilliseconds(100)); // 負荷軽減のため
        }

        Debug.Log($"最大音量: {maxVolume} dB");
        OnSpeechVolume.OnNext(maxVolume); // 最大音量を通知
    }

    /// <summary>
    /// 最新のマイク音量を取得
    /// </summary>
    private float GetUpdatedAudio()
    {
        int nowAudioPos = Microphone.GetPosition(null); // デフォルトデバイス

        float[] waveData = Array.Empty<float>();

        if (_audioClip == null || nowAudioPos <= 0) return 0f;

        if (_lastAudioPos < nowAudioPos)
        {
            int audioCount = nowAudioPos - _lastAudioPos;
            waveData = new float[audioCount];
            _audioClip.GetData(waveData, _lastAudioPos);
        }
        else if (_lastAudioPos > nowAudioPos)
        {
            int audioBuffer = _audioClip.samples * _audioClip.channels;
            int audioCount = audioBuffer - _lastAudioPos;

            float[] wave1 = new float[audioCount];
            _audioClip.GetData(wave1, _lastAudioPos);

            float[] wave2 = new float[nowAudioPos];
            if (nowAudioPos != 0)
            {
                _audioClip.GetData(wave2, 0);
            }

            waveData = new float[audioCount + nowAudioPos];
            wave1.CopyTo(waveData, 0);
            wave2.CopyTo(waveData, audioCount);
        }

        _lastAudioPos = nowAudioPos;

        return waveData.Length > 0 ? waveData.Average(Mathf.Abs) : 0f;
    }

    /// <summary>
    /// 声認識でエラーが発生した場合
    /// </summary>
    private void DictationRecError(string error, int hresult)
    {
        Debug.Log($"🚨 <color=red>エラー：{error}, {hresult}");
    }

    private void OnDestroy()
    {
        _dictationRecognizer.Stop();
        _dictationRecognizer.Dispose();
    }


}
