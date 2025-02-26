using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class DropDownDevice : MonoBehaviour
{
    [SerializeField] TMP_Dropdown _dropdown;
    readonly List<string> _devicelist = new List<string>();
    [SerializeField] private SpeechToTextVolume _speechToTextVolume;

    private void Start()
    {
        //一度すべてのOptionsをクリア
        _dropdown.ClearOptions();

        GetMicrophoneDevices();
        //リストを追加
        _dropdown.AddOptions(_devicelist);
        SetMicrophoneDevice();
    }

    void GetMicrophoneDevices()
    {
        foreach (var device in Microphone.devices)
        {
            _devicelist.Add(device);
        }
    }

    public void SetMicrophoneDevice()
    {
        // 選択されたデバイスを取得
        string device = _devicelist[_dropdown.value];
        Debug.Log($"選択されたデバイス: {device}");

        // SpeechToTextVolumeの_deviceNameに設定
        if (_speechToTextVolume != null)
        {
            _speechToTextVolume.SetDeviceName(device);
        }
    }
}
