using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;
using UnityEngine.Audio;

public class ControlPanelModel
{
    private AudioMixer _audioMixer;
    private readonly CompositeDisposable _disposables = new();
    // SoundType と AudioMixer のパラメータ名を関連付ける
    private static readonly Dictionary<SoundType, string> MixerParameterMap = new()
    {
        { SoundType.MASTER, "Master" },
        { SoundType.BGM, "BGM" },
        { SoundType.SE, "SE" },
        { SoundType.VOICE, "Voice" }
    };

    // 音量のリアクティブプロパティ
    public Dictionary<SoundType, ReactiveProperty<float>> VolumeProperties { get; } = new();

    public ControlPanelModel(AudioMixer audioMixer, Dictionary<SoundType, float> initialVolumes)
    {
        _audioMixer = audioMixer;

        foreach (var soundType in MixerParameterMap.Keys)
        {
            var volume = new ReactiveProperty<float>(initialVolumes[soundType]);
            VolumeProperties[soundType] = volume;

            // 音量が変更されたときに AudioMixer に即時反映
            volume.Subscribe(value =>
            {
                if (MixerParameterMap.TryGetValue(soundType, out string parameterName))
                {
                    float dB = ConvertToDecibel(value);
                    _audioMixer.SetFloat(parameterName, dB);
                    Debug.Log($"{parameterName} 設定: {dB} dB");
                }
            }).AddTo(_disposables);
        }

        ApplyAllVolumes();
    }

    /// <summary>
    /// 音量を変更する（スライダーから）
    /// </summary>
    public void ChangeVolumeBySlider(SoundType soundType, float value)
    {
        if (VolumeProperties.ContainsKey(soundType))
        {
            VolumeProperties[soundType].Value = Mathf.Clamp01(value / 100f);
        }
    }

    /// <summary>
    /// 音量を変更する（入力フィールドから）
    /// </summary>
    public void ChangeVolumeByInput(SoundType soundType, string value)
    {
        if (float.TryParse(value, out float result))
        {
            if (VolumeProperties.ContainsKey(soundType))
            {
                VolumeProperties[soundType].Value = Mathf.Clamp01(result / 100f);
            }
        }
        else
        {
            Debug.LogWarning($"SoundType {soundType}: 入力が無効です - {value}");
        }
    }

    /// <summary>
    /// すべての音量を AudioMixer に適用する
    /// </summary>
    private void ApplyAllVolumes()
    {
        foreach (var soundType in MixerParameterMap.Keys)
        {
            if (VolumeProperties.ContainsKey(soundType))
            {
                float value = VolumeProperties[soundType].Value;
                float dB = ConvertToDecibel(value);
                _audioMixer.SetFloat(MixerParameterMap[soundType], dB);
            }
        }
    }

    /// <summary>
    /// 0-1 の値を -80dB 〜 20dB に変換
    /// </summary>
    private float ConvertToDecibel(float volume)
    {
        return Mathf.Clamp(20f * Mathf.Log10(Mathf.Clamp(volume, 0.0001f, 1f)), -80f, 20f);
    }
}
