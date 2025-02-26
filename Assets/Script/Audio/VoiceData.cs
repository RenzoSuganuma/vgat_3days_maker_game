using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ボイス一つのクリップ
/// </summary>
[System.Serializable]
public struct VoiceData
{
    public AudioClip Clip;
    public float Volume;
}

/// <summary>
/// ボイスデータ
/// </summary>
[System.Serializable]
public struct VoiceDatas
{
    [Header("ボイスの名前")] public VoiceNameEnum VoiceName;
    [Header("ボイスデータ")] public List<VoiceData> VoiceDatasList;
}
