using System.Collections.Generic;
using R3;
using UnityEngine;
using UnityEngine.Audio;

public class ControlPresenter : MonoBehaviour
{
    [SerializeField] private ControlPanelView _controlPanelView;
    [SerializeField] private AudioMixer _audioMixer;

    private ControlPanelModel _model;
    private OptionModel _optionModel;
    private readonly CompositeDisposable _disposables = new CompositeDisposable();

    public void Initialize(OptionModel optionModel)
    {
        _optionModel = optionModel;

        var initialVolumes = new Dictionary<SoundType, float>
        {
            { SoundType.MASTER, _optionModel.MasterVolume },
            { SoundType.BGM, _optionModel.BGMVolume },
            { SoundType.SE, _optionModel.SEVolume },
            { SoundType.VOICE, _optionModel.VoiceVolume }
        };

        _model = new ControlPanelModel(_audioMixer, initialVolumes);

        foreach (var soundType in _model.VolumeProperties.Keys)
        {
            _model.VolumeProperties[soundType]
                .Subscribe(value => SaveToOptionModel(soundType, value))
                .AddTo(_disposables);
        }

        InitializeUI();
    }

    private void InitializeUI()
    {
        var initialValues = new Dictionary<SoundType, float>();
        foreach (var kvp in _model.VolumeProperties)
        {
            initialValues[kvp.Key] = Mathf.FloorToInt(kvp.Value.Value * 100f); // 小数点以下を切り捨て
        }

        _controlPanelView.Initialize(
            initialValues,
            onSliderChanged: ChangeVolume,
            onInputChanged: ChangeVolumeFromInput
        );
    }

    public void ChangeVolume(SoundType soundType, float value)
    {
        float normalizedValue = Mathf.FloorToInt(value) / 100f; // 100倍されていたら元の値に戻す
        Debug.Log($"[スライダー変更] {soundType}: {value} -> {normalizedValue}"); // デバッグログ追加
        _model.ChangeVolumeBySlider(soundType, normalizedValue);
    }

    public void ChangeVolumeFromInput(SoundType soundType, string value)
    {
        if (float.TryParse(value, out float result))
        {
            float normalizedValue = Mathf.FloorToInt(result) / 100f;
            Debug.Log($"[入力変更] {soundType}: {value} -> {normalizedValue}"); // デバッグログ追加
            _model.ChangeVolumeByInput(soundType, normalizedValue.ToString("F0")); // 小数点なしで出力
        }
    }

    private void SaveToOptionModel(SoundType soundType, float value)
    {
        float normalizedValue = Mathf.FloorToInt(value * 100f) / 100f; // 100倍誤差修正
        Debug.Log($"[保存] {soundType}: {value} -> {normalizedValue}"); // デバッグログ追加

        switch (soundType)
        {
            case SoundType.MASTER: _optionModel.MasterVolume = normalizedValue; break;
            case SoundType.BGM: _optionModel.BGMVolume = normalizedValue; break;
            case SoundType.SE: _optionModel.SEVolume = normalizedValue; break;
            case SoundType.VOICE: _optionModel.VoiceVolume = normalizedValue; break;
        }
        _optionModel.SaveSettings();
    }

    private void OnDestroy()
    {
        _disposables.Dispose();
    }
}
