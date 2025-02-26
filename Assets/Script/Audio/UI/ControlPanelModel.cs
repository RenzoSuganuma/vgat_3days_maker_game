using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class ControlPanelModel
{
    private Dictionary<SoundType, float> _currentVolumes;
    private AudioMixer _audioMixer;

    // SoundType と AudioMixer のパラメータ名を関連付ける
    private static readonly Dictionary<SoundType, string> MixerParameterMap = new()
    {
        { SoundType.MASTER, "Master" },
        { SoundType.BGM, "BGM" },
        { SoundType.SE, "SE" },
        { SoundType.VOICE, "Voice" }
    };

    public ControlPanelModel(AudioMixer audioMixer, Dictionary<SoundType, float> initialVolumes)
    {
        _audioMixer = audioMixer;
        _currentVolumes = new Dictionary<SoundType, float>(initialVolumes);
        ApplyAllVolumes();
    }

    public Dictionary<SoundType, float> GetCurrentVolumes() => _currentVolumes;

    /// <summary>
    /// 音量変更処理（スライダー）
    /// </summary>
    public float ChangeVolumeBySlider(SoundType soundType, float value)
    {
        float normalizedValue = Mathf.Clamp01(value / 100f);
        ApplyVolumeChange(soundType, normalizedValue);
        return normalizedValue;
    }

    /// <summary>
    /// 音量変更処理（入力フィールド）
    /// </summary>
    public float ChangeVolumeByInput(SoundType soundType, string value)
    {
        if (float.TryParse(value, out float result))
        {
            float normalizedValue = Mathf.Clamp01(result / 100f);
            ApplyVolumeChange(soundType, normalizedValue);
            return normalizedValue;
        }
        else
        {
            Debug.LogWarning($"SoundType {soundType}: 入力が無効です - {value}");
            return _currentVolumes[soundType]; // 変更なし
        }
    }

    /// <summary>
    /// 音量変更を反映し、AudioMixer に適用
    /// </summary>
    private void ApplyVolumeChange(SoundType soundType, float value)
    {
        if (MixerParameterMap.TryGetValue(soundType, out string parameterName))
        {
            // -80dB ~ 20dB に変換
            float dB = Mathf.Clamp(Mathf.Log10(value) * 20f, -80f, 20f);
            _audioMixer.SetFloat(parameterName, dB);
            _currentVolumes[soundType] = value;

            Debug.Log($"{parameterName} 設定: {dB} dB");
        }
        else
        {
            Debug.LogWarning($"未対応の SoundType: {soundType}");
        }
    }

    /// <summary>
    /// すべての音量を AudioMixer に適用する
    /// </summary>
    private void ApplyAllVolumes()
    {
        foreach (var soundType in MixerParameterMap.Keys)
        {
            ApplyVolumeChange(soundType, _currentVolumes[soundType]);
        }
    }
}
