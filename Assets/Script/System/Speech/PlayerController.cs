using System;
using R3;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private VoiceInputHandler _voiceInputHandler;
    private int _currentLane = 0; // 現在のレーン

    private void Start()
    {
        // 音声認識結果を監視
        _voiceInputHandler.RecognizedText.Subscribe(text => { Debug.Log($"🎤 認識結果: {text}"); });

        // 音量を監視
        _voiceInputHandler.MaxSpeechVolume.Subscribe(volume => { Debug.Log($"📊 最大音量: {volume} dB"); });

        // 音声入力成功時に移動を実行
        _voiceInputHandler.IsVoiceInputSuccessful.Subscribe(isSuccessful =>
        {
            if (isSuccessful)
            {
                Debug.Log("音声入力成功: プレイヤーが移動可能");
                MovePlayer(_voiceInputHandler.LaneChange.Value);
            }
        });
    }

    /// <summary>
    /// プレイヤーをレーン移動させる
    /// </summary>
    private void MovePlayer(int laneChange)
    {
        if (laneChange == -1)
        {
            // レーンを下に移動
        }
        else if (laneChange == 1)
        {
            // レーンを上に移動
        }
        else
        {
            Debug.Log("➡ プレイヤーのレーン維持");
        }
    }

    /// <summary>
    /// Test Button
    /// </summary>
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _voiceInputHandler.StartSpeechRecognition();
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            _voiceInputHandler.StopSpeechRecognition();
        }
    }
}
