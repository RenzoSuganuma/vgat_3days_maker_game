using System;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.Windows.Speech;
using R3;
using Cysharp.Threading.Tasks;

public class SpeechToTextVolume : IDisposable
{
    private GameSettings _gameSettings;

    private DictationRecognizer _dictationRecognizer;
    public Subject<string> OnSpeechResult = new Subject<string>(); // 音声認識結果
    public Subject<float> OnSpeechVolume = new Subject<float>(); // 音量データ

    private string _deviceName;
    private string _targetDevice = "";
    private AudioClip _audioClip;
    private int _lastAudioPos;
    private CancellationTokenSource _cancellationTokenSource;
    private MissionsDisplay _missionsDisplay;


    public SpeechToTextVolume(GameSettings gameSettings)
    {
        _gameSettings = gameSettings;

        _dictationRecognizer = new DictationRecognizer();
        _dictationRecognizer.DictationResult += DictationRecResult;
        _dictationRecognizer.DictationError += DictationRecError;

        _deviceName = _gameSettings.MicDeviceSettings.DeviceName;
        InitMicrophone(_gameSettings.MicDeviceSettings.DeviceName);
        Debug.Log("SpeechToTextVolume: 初期化完了");
    }

    /// <summary>
    /// マイクを初期化し、録音を開始
    /// </summary>
    private void InitMicrophone(string targetDevice)
    {
        if (string.IsNullOrEmpty(targetDevice))
        {
            Debug.LogError("⚠ マイクデバイスが見つかりません！");
            return;
        }

        Debug.Log($"🎤 録音デバイス: {targetDevice}");
        _audioClip = Microphone.Start(targetDevice, true, 10, _gameSettings.MicDeviceSettings.SampleRate);
    }


    /// <summary>
    /// デバイス名を設定
    /// </summary>
    public void SetDeviceName(string targetDevice)
    {
        _deviceName = targetDevice;
        if (_gameSettings != null)
        {
            _gameSettings.MicDeviceSettings.DeviceName = _deviceName;
        }

        InitMicrophone(_deviceName); // 新しいデバイスでマイクを再初期化
    }

    /// <summary>
    /// 音声認識を開始
    /// </summary>
    public async void StartSpeechRecognition()
    {
        if (_dictationRecognizer.Status != SpeechSystemStatus.Stopped) return;

        _dictationRecognizer.Start();
        Debug.Log("🎤 音声認識開始");

        _cancellationTokenSource = new CancellationTokenSource();
        await UniTask.WaitUntil(() => _dictationRecognizer.Status == SpeechSystemStatus.Running);
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
    }

    /// <summary>
    /// 音声認識をキャンセル
    /// </summary>
    public void CancelSpeechRecognition()
    {
        if (_dictationRecognizer.Status != SpeechSystemStatus.Running) return;
        _cancellationTokenSource?.Cancel();
        Debug.Log("🛑 音量測定がキャンセルされました");
    }


    /// <summary>
    /// 指定されたマイクデバイスが存在するか確認
    /// </summary>
    private string ValidateMicDevice(string deviceName)
    {
        if (Microphone.devices.Length == 0)
        {
            Debug.LogError("⚠ マイクデバイスが見つかりません！");
            return null;
        }

        // 指定されたデバイスが `Microphone.devices` に含まれているかチェック
        if (Microphone.devices.Contains(deviceName))
        {
            return deviceName;
        }

        // 存在しない場合はデフォルトデバイスを使用
        Debug.LogWarning($"⚠ 指定されたデバイス `{deviceName}` が見つかりません。デフォルト `{Microphone.devices[0]}` を使用します。");
        _gameSettings.MicDeviceSettings.DeviceName = Microphone.devices[0]; // 設定を更新
        return Microphone.devices[0];
    }

    /// <summary>
    /// 音声が認識されたときに発生するイベント
    /// </summary>
    private void DictationRecResult(string text, ConfidenceLevel confidence)
    {
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
                float volume = GetUpdatedAudioRelative();
                maxVolume = Mathf.Max(maxVolume, volume);
                await UniTask.Delay(TimeSpan.FromMilliseconds(1), cancellationToken: cancellationToken);
            }
        }
        catch (OperationCanceledException) // タイムアップ
        {
            _dictationRecognizer.Stop();
        }

        OnSpeechVolume.OnNext(maxVolume);
    }

    /// <summary>
    /// マイクから音声データを取得 RMS (Root Mean Square) のリニアスケールを計算
    /// </summary>
    private float GetUpdatedAudioRMS()
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
    /// マイクから音声データを取得し、相対スケールに変換`-80dB ~ 20dB` の範囲に収める
    /// 無音は -80dB にする
    /// </summary>
    private float GetUpdatedAudioRelative()
    {
        int nowAudioPos = Microphone.GetPosition(null);
        float[] waveData = Array.Empty<float>();

        if (_audioClip == null || nowAudioPos <= 0) return -80f; // 無音は -80dB にする

        if (_lastAudioPos < nowAudioPos)
        {
            int audioCount = nowAudioPos - _lastAudioPos;
            waveData = new float[audioCount];
            _audioClip.GetData(waveData, _lastAudioPos);
        }

        _lastAudioPos = nowAudioPos;

        float rms = Mathf.Sqrt(waveData.Sum(sample => sample * sample) / waveData.Length);

        // `AudioMixer` に近いスケールにする
        float db = 20f * Mathf.Log10(Mathf.Max(rms, 0.0001f)); // 0.0001 を下限にする

        return Mathf.Clamp(db, -80f, 20f); // `-80dB ~ 20dB` の範囲に収める
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
