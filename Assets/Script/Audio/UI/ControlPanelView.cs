using System.Collections.Generic;
using UnityEngine;

public class ControlPanelView : MonoBehaviour
{
    [SerializeField] private List<VolumeControl> _volumeControls = new();

    public void Initialize(Dictionary<SoundType, float> initialVolumes,
        System.Action<SoundType, float> onSliderChanged,
        System.Action<SoundType, string> onInputChanged)
    {
        foreach (var volumeControl in _volumeControls)
        {
            var soundType = volumeControl.SoundType;

            if (initialVolumes.TryGetValue(soundType, out float initialValue))
            {
                volumeControl.Initialize(
                    label: soundType.ToString(),
                    initialValue: initialValue,
                    onSliderChanged: value => onSliderChanged(soundType, value),
                    onInputChanged: value => onInputChanged(soundType, value)
                );
            }
            else
            {
                Debug.LogWarning($"SoundType {soundType} の初期値が設定されていません。");
            }
        }
    }
}
