using System;
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

        Foundation.TaskOnChangedScene += ChangeBGM; // Foundationクラスのシーン遷移時に呼ばれるイベントを購読
    }

    private void OnDestroy()
    {
        Foundation.TaskOnChangedScene -= ChangeBGM; // 購読解除
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
        _bgmAudioSource.Play();
    }

    /// <summary>
    /// Foundationクラスでシーン遷移イベントが発火された時に呼び出されるメソッド
    /// </summary>
    private void ChangeBGM(string sceneName)
    {
        SceneNameEnum nextScene =  sceneName switch
        {
            "Title" => SceneNameEnum.Title,
            "InGame" => SceneNameEnum.InGame,
            "Result" => SceneNameEnum.Result,
            _ => throw new InvalidCastException()
        };

        PlayBGM(nextScene);
    }

    /// <summary>
    /// Enumで指定したSEを再生する
    /// </summary>
    public void PlaySE(SENameEnum seName)
    {
        ClipData data = _se.GetClipData((int)seName);
        _seAudioSource.volume = data.Volume;
        _seAudioSource.PlayOneShot(data.Clip);
    }

    /// <summary>
    /// Enumで指定したVoiceの中からランダムなClipを取得して再生する
    /// </summary>
    public void PlayRandomVoice(VoiceNameEnum voiceName)
    {
        VoiceData data = _voice.GetRandomVoiceClip(voiceName);
        _seAudioSource.volume = data.Volume;
        _seAudioSource.PlayOneShot(data.Clip);
    }
}
