using UnityEngine;

public class OptionModel
{
    private float _masterVolume;
    private float _bgmVolume;
    private float _seVolume;
    private float _voiceVolume;

    public OptionModel(AudioVolumeSettings defaultSettings)
    {
        _masterVolume = defaultSettings.MasterVolume;
        _bgmVolume = defaultSettings.BgmVolume;
        _seVolume = defaultSettings.SeVolume;
        _voiceVolume = defaultSettings.VoiceVolume;

        // 保存されたデータがあれば適用
        LoadSettings();
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetFloat("MasterVolume", _masterVolume);
        PlayerPrefs.SetFloat("BGMVolume", _bgmVolume);
        PlayerPrefs.SetFloat("SEVolume", _seVolume);
        PlayerPrefs.SetFloat("VoiceVolume", _voiceVolume);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// 保存済みの設定を読み込む
    /// </summary>
    public void LoadSettings()
    {
        // 保存されたデータがあれば適用
        _masterVolume = PlayerPrefs.HasKey("MasterVolume") ? PlayerPrefs.GetFloat("MasterVolume") : _masterVolume;
        _bgmVolume = PlayerPrefs.HasKey("BGMVolume") ? PlayerPrefs.GetFloat("BGMVolume") : _bgmVolume;
        _seVolume = PlayerPrefs.HasKey("SEVolume") ? PlayerPrefs.GetFloat("SEVolume") : _seVolume;
        _voiceVolume = PlayerPrefs.HasKey("VoiceVolume") ? PlayerPrefs.GetFloat("VoiceVolume") : _voiceVolume;
    }

    public float MasterVolume
    {
        get => _masterVolume;
        set => _masterVolume = Mathf.Clamp01(value);
    }

    public float BGMVolume
    {
        get => _bgmVolume;
        set => _bgmVolume = Mathf.Clamp01(value);
    }

    public float SEVolume
    {
        get => _seVolume;
        set => _seVolume = Mathf.Clamp01(value);
    }

    public float VoiceVolume
    {
        get => _voiceVolume;
        set => _voiceVolume = Mathf.Clamp01(value);
    }
}
