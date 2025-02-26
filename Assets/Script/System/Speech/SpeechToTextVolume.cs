using System;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.Windows.Speech;
using R3;
using Cysharp.Threading.Tasks;

public class SpeechToTextVolume : IDisposable
{
    private DictationRecognizer _dictationRecognizer;
    public Subject<string> OnSpeechResult = new Subject<string>();
    public Subject<float> OnSpeechVolume = new Subject<float>();

    private string _deviceName;
    private string _targetDevice = "";
    private AudioClip _audioClip;
    private int _lastAudioPos;
    private CancellationTokenSource _cancellationTokenSource;

    public SpeechToTextVolume()
    {
        _dictationRecognizer = new DictationRecognizer();
        _dictationRecognizer.DictationResult += DictationRecResult;
        _dictationRecognizer.DictationError += DictationRecError;

        _deviceName = Microphone.devices[0];
        InitMicrophone();
        Debug.Log("SpeechToTextVolume: 初期化完了");
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

        if (string.IsNullOrEmpty(_targetDevice))
        {
            Debug.LogError("⚠ マイクデバイスが見つかりません！");
            return;
        }

        Debug.Log($"🎤 録音デバイス: {_targetDevice}");
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

        _cancellationTokenSource = new CancellationTokenSource();
        _ = CaptureSpeechVolume(_cancellationTokenSource.Token);
    }

    /// <summary>
    /// 音声認識を停止
    /// </summary>
    public void StopSpeechRecognition()
    {
        if (_dictationRecognizer.Status != SpeechSystemStatus.Running) return;

        _dictationRecognizer.Stop();
        Debug.Log("🛑 音声認識停止");

        _cancellationTokenSource?.Cancel();
    }

    /// <summary>
    /// 音声が認識されたときに発生するイベント
    /// </summary>
    private void DictationRecResult(string text, ConfidenceLevel confidence)
    {
        Debug.Log($"🎤 認識した音声： {text}");
        OnSpeechResult.OnNext(text);
    }

    /// <summary>
    /// マイクの音量を取得し、最大値を送信
    /// </summary>
    private async UniTask CaptureSpeechVolume(CancellationToken cancellationToken)
    {
        float maxVolume = -100f;

        try
        {
            while (_dictationRecognizer.Status == SpeechSystemStatus.Running &&
                   !cancellationToken.IsCancellationRequested)
            {
                float volume = GetUpdatedAudio();
                maxVolume = Mathf.Max(maxVolume, volume);
                await UniTask.Delay(TimeSpan.FromMilliseconds(100), cancellationToken: cancellationToken);
            }
        }
        catch (OperationCanceledException)
        {
            Debug.Log("🛑 音量測定がキャンセルされました");
        }

        Debug.Log($"最大音量: {maxVolume} dB");
        OnSpeechVolume.OnNext(maxVolume);
    }

    /// <summary>
    /// ✅ マイクから音声データを取得
    /// </summary>
    private float GetUpdatedAudio()
    {
        int nowAudioPos = Microphone.GetPosition(null);
        float[] waveData = Array.Empty<float>();

        if (_audioClip == null || nowAudioPos <= 0) return 0f;

        if (_lastAudioPos < nowAudioPos)
        {
            int audioCount = nowAudioPos - _lastAudioPos;
            waveData = new float[audioCount];
            _audioClip.GetData(waveData, _lastAudioPos);
        }

        _lastAudioPos = nowAudioPos;

        return waveData.Length > 0 ? waveData.Average(Mathf.Abs) : 0f;
    }

    /// <summary>
    /// 音声認識でエラーが発生した場合
    /// </summary>
    private void DictationRecError(string error, int hresult)
    {
        Debug.LogError($"音声認識エラー: {error}, {hresult}");
    }

    /// <summary>
    /// リソース解放
    /// </summary>
    public void Dispose()
    {
        _dictationRecognizer?.Stop();
        _dictationRecognizer?.Dispose();
        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource?.Dispose();
    }
}
