using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ボイスデータ
/// </summary>
[System.Serializable]
public struct VoiceData
{
    /// <summary>ボイスデータ（2種類）</summary>
    [Header("ボイスの名前")] public VoiceNameEnum VoiceName;
    [Header("ボイスデータ")] public List<AudioClip> Clips;
    [Header("音量")] public float Volume;
}
