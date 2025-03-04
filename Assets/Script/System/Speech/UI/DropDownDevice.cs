using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DropDownDevice : MonoBehaviour
{
    [SerializeField] TMP_Dropdown _dropdown;
    private MicrophoneManager _microphoneManager;
    private string _currentDevice;

    public void Construct(MicrophoneManager microphoneManager)
    {
        _microphoneManager = microphoneManager;
    }

    private void Start()
    {
        _dropdown.ClearOptions();
        _currentDevice = _microphoneManager.SelectedDevice;
        RefreshDropdownOptions();
        SetMicrophoneDevice();
    }

    /// <summary>
    /// マイクデバイスリストを取得し、Dropdownを更新
    /// </summary>
    private void RefreshDropdownOptions()
    {
        _dropdown.ClearOptions();
        List<string> deviceList = new List<string>(_microphoneManager.DeviceList);
        _dropdown.AddOptions(deviceList);

        if (deviceList.Contains(_currentDevice))
        {
            _dropdown.value = deviceList.IndexOf(_currentDevice);
        }
        else
        {
            _dropdown.value = 0;
        }
    }

    /// <summary>
    /// 選択されたデバイスを設定
    /// </summary>
    public void SetMicrophoneDevice()
    {
        string selectedDevice = _dropdown.options[_dropdown.value].text;
        Debug.Log($"🎤 選択されたデバイス: {selectedDevice}");
        _microphoneManager.SetDevice(selectedDevice);
    }
}
