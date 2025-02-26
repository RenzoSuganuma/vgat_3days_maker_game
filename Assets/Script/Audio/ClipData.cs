using UnityEngine;

/// <summary>
/// AudioClipのデータを管理するコンストラクタ
/// </summary>
[System.Serializable]
public struct ClipData
{
    [Header("データの名前")] public SENameEnum ClipName;
    [Header("クリップ")] public AudioClip Clip;
    [Header("音量")] public float Volume;
}
