using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class ControlPresenter : MonoBehaviour
{
    [SerializeField] private ControlPanelView _controlPanelView;
    [SerializeField] private AudioMixer _audioMixer;

    private ControlPanelModel _model;
    private OptionModel _optionModel;

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

        // UI の初期化
        _controlPanelView.Initialize(
            _model.GetCurrentVolumes(),
            onSliderChanged: (soundType, value) => ChangeVolume(soundType, value),
            onInputChanged: (soundType, value) => ChangeVolumeFromInput(soundType, value)
        );
    }

    /// <summary>
    /// スライダーから音量を変更し、OptionModel にデータを保存
    /// </summary>
    public void ChangeVolume(SoundType soundType, float value)
    {
        float normalizedValue = _model.ChangeVolumeBySlider(soundType, value);
        UpdateOptionModel(soundType, normalizedValue);
    }

    /// <summary>
    /// 入力フィールドから音量を変更し、OptionModel にデータを保存
    /// </summary>
    public void ChangeVolumeFromInput(SoundType soundType, string value)
    {
        float normalizedValue = _model.ChangeVolumeByInput(soundType, value);
        UpdateOptionModel(soundType, normalizedValue);
    }

    /// <summary>
    /// OptionModel に音量データを保存
    /// </summary>
    private void UpdateOptionModel(SoundType soundType, float value)
    {
        switch (soundType)
        {
            case SoundType.MASTER:
                _optionModel.MasterVolume = value;
                break;
            case SoundType.BGM:
                _optionModel.BGMVolume = value;
                break;
            case SoundType.SE:
                _optionModel.SEVolume = value;
                break;
            case SoundType.VOICE:
                _optionModel.VoiceVolume = value;
                break;
        }
        _optionModel.SaveSettings();
    }
}
