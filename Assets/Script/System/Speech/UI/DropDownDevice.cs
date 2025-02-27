using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DropDownDevice : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown _dropdown;
    private readonly List<string> _devicelist = new();
    private SpeechToTextVolume _speechToTextVolume;
    private GameSettings _gameSettings;
    private string _currentDevice;

    public void Construct(GameSettings gameSettings, SpeechToTextVolume speechToTextVolume)
    {
        _gameSettings = gameSettings;
        _speechToTextVolume = speechToTextVolume;
    }

    private void Start()
    {
        if (_gameSettings == null || _speechToTextVolume == null)
        {
            Debug.LogError("⚠ GameSettings または SpeechToTextVolume が設定されていません！");
            return;
        }

        // すべての Options をクリア
        _dropdown.ClearOptions();

        // マイクデバイスリストを取得
        GetMicrophoneDevices();

        // `GameSettings` のデバイスが `Microphone.devices` にあるか確認
        _currentDevice = _gameSettings.MicDeviceSettings.DeviceName;
        if (!_devicelist.Contains(_currentDevice))
        {
            Debug.LogWarning($"⚠ `{_currentDevice}` は利用できません。デフォルト `{_devicelist[0]}` を使用します。");
            _currentDevice = _devicelist[0]; // デフォルトデバイスに設定
            _gameSettings.MicDeviceSettings.DeviceName = _currentDevice;
        }

        _dropdown.AddOptions(_devicelist);

        _dropdown.value = _devicelist.IndexOf(_currentDevice);

        SetMicrophoneDevice(_currentDevice);

        // `Dropdown` の変更イベントを追加
        _dropdown.onValueChanged.AddListener(delegate { OnDropdownValueChanged(); });
    }

    /// <summary>
    /// マイクデバイスリストを取得
    /// </summary>
    private void GetMicrophoneDevices()
    {
        _devicelist.Clear();
        foreach (var device in Microphone.devices)
        {
            _devicelist.Add(device);
        }
    }

    /// <summary>
    /// `Dropdown` から選択されたマイクデバイスを適用
    /// </summary>
    private void OnDropdownValueChanged()
    {
        string selectedDevice = _devicelist[_dropdown.value];
        SetMicrophoneDevice(selectedDevice);
    }

    /// <summary>
    /// 指定されたマイクデバイスを適用
    /// </summary>
    public void SetMicrophoneDevice(string device)
    {
        if (!_devicelist.Contains(device))
        {
            Debug.LogError($"⚠ `{device}` はリストに存在しません。");
            return;
        }

        _currentDevice = device;
        _gameSettings.MicDeviceSettings.DeviceName = _currentDevice;
        Debug.Log($"🎤 選択されたデバイス: {_currentDevice}");

        // SpeechToTextVolume のデバイスを更新
        _speechToTextVolume.SetDeviceName(_currentDevice);
    }
}
