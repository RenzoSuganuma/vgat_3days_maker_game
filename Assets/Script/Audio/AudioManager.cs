using UnityEngine;

/// <summary>
/// AudioManager
/// </summary>
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioSource _bgmAudioSource;
    [SerializeField] private AudioSource _seAudioSource;

    [Header("音声データ")]
    [SerializeField] private ClipDataSO _bgm;
    [SerializeField] private ClipDataSO _se;
    [SerializeField] private VoiceDataSO _voice;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    /// <summary>
    /// Enumで指定したBGMを再生する
    /// </summary>
    public void PlayBGM(SceneNameEnum sceneName)
    {
        // SceneNameEnumをint型にキャストして、BGMを再生
        ClipData data = _bgm.GetClipData((int)sceneName);
        _bgmAudioSource.clip = data.Clip;
        _bgmAudioSource.volume = data.Volume;
    }

    /// <summary>
    /// Enumで指定したSEを再生する
    /// </summary>
    public void PlaySE(SENameEnum seName)
    {
        ClipData data = _se.GetClipData((int)seName);
        _seAudioSource.clip = data.Clip;
        _seAudioSource.volume = data.Volume;
    }

    /// <summary>
    /// Enumで指定したVoiceの中からランダムなClipを取得して再生する
    /// </summary>
    public void PlayRandomVoice(VoiceNameEnum voiceName)
    {
        (AudioClip Clip, float Volume) data = _voice.GetRandomVoiceClip(voiceName);
        _seAudioSource.clip = data.Clip;
        _seAudioSource.volume = data.Volume;
    }
}
