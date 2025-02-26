using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "GameSettings")]
public class GameSettings : ScriptableObject
{
    [Header("音声認識の設定")]
    [Tooltip("小さい声の閾値")] public float lowThreshold = -30f;
    [Tooltip("普通の声の閾値")] public float midThreshold = -20f;
    [Tooltip("大きい声の閾値")] public float highThreshold = -10f;
    [Tooltip("発音の一致率")] public float similarity = 0.8f;

    [Header("ゲームフローの設定")]
    [Tooltip("スタックする単語の数")]
    public int stackSize = 10;

    [Tooltip("次のターンに移るまでの遅延(ms)")] public int nextTurnMillisecDelay = 1000; // 次のターンに移るまでの遅延(ms)

    [Header("初期設定")] [Tooltip("リソースのロードパス")]
    public string resourcesLoadPath = "GloomyBeat_speachText";

    [Header("マイクデバイスの設定")] public string deviceName = "Microphone"; // マイクデバイスの名前

    [Header("AudioVolumeの設定")] public int sampleRate = 48000; // サンプリングレート
    public float bgmVolume = 0.5f; // BGMの音量
    public float seVolume = 0.5f; // SEの音量
}
