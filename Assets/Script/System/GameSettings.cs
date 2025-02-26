using System;
using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "GameSettings")]
public class GameSettings : ScriptableObject
{
    [SerializeField, Header("音声認識の設定")]
    private VoiceRecognitionSettings _voiceRecognitionSettings = new VoiceRecognitionSettings();

    [SerializeField, Header("ゲームフローの設定")] private GameFlowSettings _gameFlowSettings = new GameFlowSettings();

    [SerializeField, Header("ゲームのロードするリソースの設定")]
    private GameLoadResourcesSettings _gameLoadResourcesSettings = new GameLoadResourcesSettings();

    [SerializeField, Header("マイクデバイスの設定")] private MicDeviceSettings _micDeviceSettings = new MicDeviceSettings();

    [SerializeField, Header("オーディオボリュームの設定")]
    private AudioVolumeSettings _audioVolumeSettings = new AudioVolumeSettings();

    [SerializeField, Header("プレイヤーの設定")]
    private PlayerSettings _playerSettings = new PlayerSettings();

    [SerializeField, Header("ステージの設定")]
    private StageSettings _stageSettings = new StageSettings();

    [SerializeField, Header("オブジェクトのビートの設定")]
    private ObjectBeatSettings _objectBeatSettings = new ObjectBeatSettings();

    [SerializeField, Header("フェードパネルの設定")]
    private FadePanelSettings _fadePanelSettings = new FadePanelSettings();

    public VoiceRecognitionSettings VoiceRecognitionSettings => _voiceRecognitionSettings;
    public GameFlowSettings GameFlowSettings => _gameFlowSettings;
    public GameLoadResourcesSettings GameLoadResourcesSettings => _gameLoadResourcesSettings;
    public MicDeviceSettings MicDeviceSettings => _micDeviceSettings;
    public AudioVolumeSettings AudioVolumeSettings => _audioVolumeSettings;
    public PlayerSettings PlayerSettings => _playerSettings;
    public StageSettings StageSettings => _stageSettings;
    public ObjectBeatSettings ObjectBeatSettings => _objectBeatSettings;
    public FadePanelSettings FadePanelSettings => _fadePanelSettings;
}


[Serializable]
public class VoiceRecognitionSettings
{
    [SerializeField, Tooltip("小さい声の閾値")] private float _lowThreshold = -30f;
    [SerializeField, Tooltip("普通の声の閾値")] private float _midThreshold = -20f;
    [SerializeField, Tooltip("大きい声の閾値")] private float _highThreshold = -10f;
    [SerializeField, Tooltip("音声の類似度")] private float _similarity = 0.8f;

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
    [SerializeField, Tooltip("スタックする単語の数")]
    private int _stackSize = 10;

    [SerializeField, Tooltip("次のターンに移るまでの遅延(ms)")]
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
    [SerializeField, Tooltip("お題のワードをロードするCSV")]
    private string _resourcesLoadSpeechTextPath = "GloomyBeat_speechText";

    [SerializeField, Tooltip("セーブデータのパスJson")]
    private string _saveDataPath = "saveData";

    public string ResourcesLoadSpeechTextPath
    {
        get => _resourcesLoadSpeechTextPath;
        set => _resourcesLoadSpeechTextPath = value;
    }

    public string SaveDataPath
    {
        get => _saveDataPath;
        set => _saveDataPath = value;
    }
}


[Serializable]
public class MicDeviceSettings
{
    [SerializeField, Tooltip("マイクデバイスの名前")]
    private string _deviceName = "Microphone";

    [SerializeField, Tooltip("マイク入力のサンプリングレート")]
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
    [SerializeField, Tooltip("全体の音量の設定")] private float _masterVolume = 0.8f;
    [SerializeField, Tooltip("BGMの音量の設定")] private float _bgmVolume = 0.8f;
    [SerializeField, Tooltip("SEの音量の設定")] private float _seVolume = 0.8f;

    [SerializeField, Tooltip("Voiceの音量の設定")]
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
    [Header("ジャンプの設定")] [SerializeField, Tooltip("ジャンプ時間")]
    private float _jumpDuration = 1.0f;

    [SerializeField, Tooltip("最大高さ")] private float height = 3.0f;
    [SerializeField, Tooltip("重力加速度")] private float _gravity = 9.8f;

    [SerializeField, Tooltip("Spriteの更新スピード")]
    private float _animSpeed = 0.06f;

    [Header("プレイヤーの振り子に掴まっているときの設定")] [SerializeField, Tooltip("振り子が動く角度")]
    private float _swingAngle = 90f;

    [SerializeField, Tooltip("往復する時間の半分")] private float _duration = 2f;
    [SerializeField, Tooltip("イージングの種類")] private Ease _easeType = Ease.InOutSine;

    [SerializeField, Tooltip("プレイヤーの画像を変更するtweenの進行度")]
    private float _percent = 0.75f;

    [Header("アニメーションの設定")] [SerializeField, Tooltip("キャラクターの画像を変更しておく時間")]
    private float _animationDuration = 1f;

    [SerializeField, Tooltip("振り子の終端に到達したときのアニメーションの角度")]
    private int _angle = 40;

    public float JumpDuration
    {
        get => _jumpDuration;
        set => _jumpDuration = value;
    }

    public float Height
    {
        get => height;
        set => height = value;
    }

    public float Gravity
    {
        get => _gravity;
        set => _gravity = value;
    }

    public float AnimSpeed
    {
        get => _animSpeed;
        set => _animSpeed = value;
    }

    public float SwingAngle
    {
        get => _swingAngle;
        set => _swingAngle = value;
    }

    public float Duration
    {
        get => _duration;
        set => _duration = value;
    }

    public Ease EaseType
    {
        get => _easeType;
        set => _easeType = value;
    }

    public float Percent
    {
        get => _percent;
        set => _percent = value;
    }

    public float AnimationDuration
    {
        get => _animationDuration;
        set => _animationDuration = value;
    }

    public int Angle
    {
        get => _angle;
        set => _angle = value;
    }
}

[Serializable]
public class StageSettings
{
    [Header("ステージ全体の設定")] [SerializeField, Tooltip("レーンの幅の基準値")]
    private float _baseWidth = 5f;

    [SerializeField, Tooltip("レーンの幅の増加量")] private float _widthPerLayer = 1f;

    [SerializeField, Tooltip("レーンの高さの増加量")]
    private float _heightPerLayer = 5f;

    [SerializeField, Tooltip("生成するレイヤー数")] private int _generateLayers = 6;

    [SerializeField, Tooltip("初期レイヤー")] private int _initialLayer = 2;

    [SerializeField, Tooltip("ステージの生成距離")] private float _generateDistance = 100f;

    [SerializeField, Tooltip("プレイヤーが進んだときの追加生成距離")]
    private float _generatePerMoveDistance = 50f;

    public float BaseWidth => _baseWidth;
    public float WidthPerLayer => _widthPerLayer;
    public float HeightPerLayer => _heightPerLayer;
    public int GenerateLayers => _generateLayers;
    public int InitialLayer => _initialLayer;
    public float GenerateDistance => _generateDistance;
    public float GeneratePerMoveDistance => _generatePerMoveDistance;
}

[Serializable]
public class FadePanelSettings
{
    [SerializeField] private float _duration;

    public float Duration
    {
        get => _duration;
        set => _duration = value;
    }
}

[Serializable]
public class ObjectBeatSettings
{
    [SerializeField, Tooltip("1回の揺れ時間")] private float _shakeDuration = 0.1f;
    [SerializeField, Tooltip("UIの拡大率")] private float _scaleMultiplier = 1.05f;
    [SerializeField, Tooltip("曲のBPM")] private float _bpm = 128f;

    public float ShakeDuration
    {
        get => _shakeDuration;
        set => _shakeDuration = value;
    }

    public float ScaleMultiplier
    {
        get => _scaleMultiplier;
        set => _scaleMultiplier = value;
    }

    public float Bpm
    {
        get => _bpm;
        set => _bpm = value;
    }
}
