using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MicrophoneManager
{
    private List<string> _deviceList = new List<string>();
    private string _selectedDevice;
    private GameSettings _gameSettings;

    public IReadOnlyList<string> DeviceList => _deviceList;
    public string SelectedDevice => _selectedDevice;

    public MicrophoneManager(GameSettings gameSettings)
    {
        _gameSettings = gameSettings;
        RefreshDeviceList();
        _selectedDevice = ValidateMicDevice(_gameSettings.MicDeviceSettings.DeviceName);
    }

    /// <summary>
    /// マイクデバイスのリストを更新
    /// </summary>
    public void RefreshDeviceList()
    {
        _deviceList.Clear();
        _deviceList.AddRange(Microphone.devices);
    }

    /// <summary>
    /// マイクデバイスを設定
    /// </summary>
    public void SetDevice(string deviceName)
    {
        if (_deviceList.Contains(deviceName))
        {
            _selectedDevice = deviceName;
            _gameSettings.MicDeviceSettings.DeviceName = deviceName;
        }
        else
        {
            Debug.LogWarning($"⚠ 指定されたデバイス `{deviceName}` が見つかりません。デフォルト `{_deviceList.FirstOrDefault()}` を使用します。");
            _selectedDevice = _deviceList.FirstOrDefault();
        }
    }

    /// <summary>
    /// 設定されたマイクデバイスが有効か検証
    /// </summary>
    private string ValidateMicDevice(string deviceName)
    {
        RefreshDeviceList();

        if (_deviceList.Contains(deviceName))
        {
            return deviceName;
        }

        if (_deviceList.Count > 0)
        {
            Debug.LogWarning($"⚠ 指定されたデバイス `{deviceName}` が見つかりません。デフォルト `{_deviceList[0]}` を使用します。");
            _gameSettings.MicDeviceSettings.DeviceName = _deviceList[0]; // 設定を更新
            return _deviceList[0];
        }

        Debug.LogError("⚠ マイクデバイスが見つかりません！");
        return null;
    }
}
