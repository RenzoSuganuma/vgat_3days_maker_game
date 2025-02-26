using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "GameSettings", menuName = "GameSettings")]
public class GameSettings : ScriptableObject
{
    [Header("音声認識の設定"), SerializeField]
    private VoiceRecognitionSettings _voiceRecognitionSettings = new VoiceRecognitionSettings();

    [Header("ゲームフローの設定"), SerializeField] private GameFlowSettings _gameFlowSettings = new GameFlowSettings();

    [Header("ゲームのロードするリソースの設定"), SerializeField]
    private GameLoadResourcesSettings _gameLoadResourcesSettings = new GameLoadResourcesSettings();

    [Header("マイクデバイスの設定"), SerializeField] private MicDeviceSettings _micDeviceSettings = new MicDeviceSettings();

    [Header("オーディオボリュームの設定"), SerializeField]
    private AudioVolumeSettings _audioVolumeSettings = new AudioVolumeSettings();

    [Header("プレイヤーの設定"), SerializeField] private PlayerSettings _playerSettings = new PlayerSettings();


    public VoiceRecognitionSettings VoiceRecognitionSettings => _voiceRecognitionSettings;
    public GameFlowSettings GameFlowSettings => _gameFlowSettings;
    public GameLoadResourcesSettings GameLoadResourcesSettings => _gameLoadResourcesSettings;
    public MicDeviceSettings MicDeviceSettings => _micDeviceSettings;
    public AudioVolumeSettings AudioVolumeSettings => _audioVolumeSettings;
    public PlayerSettings PlayerSettings => _playerSettings;
}


[Serializable]
public class VoiceRecognitionSettings
{
    [Tooltip("小さい声の閾値"), SerializeField] private float _lowThreshold = -30f;
    [Tooltip("普通の声の閾値"), SerializeField] private float _midThreshold = -20f;
    [Tooltip("大きい声の閾値"), SerializeField] private float _highThreshold = -10f;
    [Tooltip("音声の類似度"), SerializeField] private float _similarity = 0.8f;

    public float LowThreshold
    {
        get => _lowThreshold;
        set => _lowThreshold = value;
    }

    public float MidThreshold
    {
        get => _midThreshold;
        set => _midThreshold = value;
    }

    public float HighThreshold
    {
        get => _highThreshold;
        set => _highThreshold = value;
    }

    public float Similarity
    {
        get => _similarity;
        set => _similarity = value;
    }
}

[Serializable]
public class GameFlowSettings
{
    [Tooltip("スタックする単語の数"), SerializeField]
    private int _stackSize = 10;

    [Tooltip("次のターンに移るまでの遅延(ms)"), SerializeField]
    private int _nextTurnMilliSecDelay = 1000;

    public int StackSize
    {
        get => _stackSize;
        set => _stackSize = value;
    }

    public int NextTurnMilliSecDelay
    {
        get => _nextTurnMilliSecDelay;
        set => _nextTurnMilliSecDelay = value;
    }
}

[Serializable]
public class GameLoadResourcesSettings
{
    [Tooltip("お題のワードをロードするCSV"), SerializeField]
    private string _resourcesLoadSpeechTextPath = "GloomyBeat_speechText";

    [Tooltip("セーブデータのパスJson"), SerializeField]
    private string _saveDataPath = "saveData";
}


[Serializable]
public class MicDeviceSettings
{
    [Tooltip("マイクデバイスの名前"), SerializeField]
    private string _deviceName = "Microphone";

    [Tooltip("マイク入力のサンプリングレート"), SerializeField]
    private int _sampleRate = 48000;

    public string DeviceName
    {
        get => _deviceName;
        set => _deviceName = value;
    }

    public int SampleRate
    {
        get => _sampleRate;
        set => _sampleRate = value;
    }
}

[Serializable]
public class AudioVolumeSettings
{
    [Tooltip("全体の音量の設定"), SerializeField] private float _masterVolume = 0.8f;
    [Tooltip("BGMの音量の設定"), SerializeField] private float _bgmVolume = 0.8f;
    [Tooltip("SEの音量の設定"), SerializeField] private float _seVolume = 0.8f;

    [Tooltip("Voiceの音量の設定"), SerializeField]
    private float _voiceVolume = 0.8f;

    public float MasterVolume
    {
        get => _masterVolume;
        set => _masterVolume = value;
    }

    public float BgmVolume
    {
        get => _bgmVolume;
        set => _bgmVolume = value;
    }

    public float SeVolume
    {
        get => _seVolume;
        set => _seVolume = value;
    }

    public float VoiceVolume
    {
        get => _voiceVolume;
        set => _voiceVolume = value;
    }
}

[Serializable]
public class PlayerSettings
{
    [Tooltip("プレイヤーの移動速度"), SerializeField]
    private float _moveSpeed = 5f;

    [Tooltip("プレイヤーのジャンプ力"), SerializeField]
    private float _jumpPower = 10f;

    [Tooltip("プレイヤーの重力"), SerializeField] private float _gravity = 9.8f;
    [Tooltip("振り子が動く角度"), SerializeField] private float _swingAngle = 90f;
    [Tooltip("往復する時間の半分"), SerializeField] private float _duration = 2f;
    [Tooltip("イージングの種類"), SerializeField] private Ease _easeType = Ease.InOutSine;

    [Tooltip("プレイヤーの画像を変更するtweenの進行度"), SerializeField]
    private float _percent = 0.75f;

    public float MoveSpeed
    {
        get => _moveSpeed;
        set => _moveSpeed = value;
    }

    public float JumpPower
    {
        get => _jumpPower;
        set => _jumpPower = value;
    }

    public float Gravity
    {
        get => _gravity;
        set => _gravity = value;
    }
}
