using System.Collections.Generic;
using UnityEngine;
using R3;

/// <summary>
/// レーンの移動先を決定する
/// </summary>
[RequireComponent(typeof(PendulumController))]
public class DestinatinCheck : MonoBehaviour
{
    private MissionsDisplay _missionsDisplay;
    private PendulumController _pendulumController;
    private Transform _playerTransform;
    private PlayerMove _move;
    private VoiceInputHandler _voiceInputHandler;
    private SpeechBubbleManager _speechBubbleManager;

    public bool CanMove { get; set; } // 音声入力があったらtrueにする
    public static int _currentLaneIndex { get; private set; } // 現在プレイヤーがいるレーンのindex（0~5）

    private void Start()
    {
        _pendulumController = GetComponent<PendulumController>(); // 親オブジェクトのPendulumControllerを取得する

        _missionsDisplay = FindAnyObjectByType<MissionsDisplay>();
        _speechBubbleManager = new SpeechBubbleManager(_missionsDisplay); // 音声入力に対して吹きだしを表示・ボイス再生をするクラスを生成

        _pendulumController.OnReachTheEdge += Move;
        _move = FindAnyObjectByType<PlayerMove>();
        _playerTransform = _move.transform;

        _voiceInputHandler = FindAnyObjectByType<VoiceInputHandler>();

        #region 音声認識との結合部分

        // 音声認識結果を監視
        _voiceInputHandler?.RecognizedText
            .Subscribe(text => { Debug.Log($"🎤 認識結果: {text}"); });

        // 音量を監視
        _voiceInputHandler?.MaxSpeechVolume.Subscribe(volume => { Debug.Log($"📊 最大音量: {volume} dB"); });

        // 音声入力成功時に移動を実行
        _voiceInputHandler?.IsVoiceInputSuccessful.Subscribe(isSuccessful =>
        {
            if (isSuccessful)
            {
                Debug.Log("音声入力成功: プレイヤーが移動可能");
                MovePlayer(_voiceInputHandler.LaneChange.Value);

                VoiceNameEnum voiceName = _voiceInputHandler.LaneChange.Value switch
                {
                    -1 => VoiceNameEnum.dB60, // 一段下がる＝ 60デシベルのとき
                    0 => VoiceNameEnum.dB70, // 維持＝ 70デシベルのとき
                    1 => VoiceNameEnum.dB80, // 一段上がる＝ 80デシベルのとき
                    _ => VoiceNameEnum.dB50,
                };

                _speechBubbleManager.ChangeDialogueAndPlayVoice(voiceName);
            }
        });

        #endregion
    }

    private void OnDestroy()
    {
        if (_pendulumController == null)
        {
            return;
        }

        _pendulumController.OnReachTheEdge -= Move;
    }

    private void Update()
    {
        #region 音声認識部分

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _voiceInputHandler?.StartSpeechRecognition();
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            _voiceInputHandler?.StopSpeechRecognition();
        }

        #endregion

        if (Input.GetKeyDown(KeyCode.A)) // キャラクターのジャンプテスト用
        {
            CanMove = true;
        }
    }

    /// <summary>
    /// 音声認識に合わせてプレイヤーの移動先のレーンのIndexを変更する
    /// </summary>
    private void MovePlayer(int laneChange)
    {
        _currentLaneIndex += laneChange;

        _currentLaneIndex = Mathf.Clamp(_currentLaneIndex, 0, 5);

        CanMove = true;
        Debug.Log($"現在のレーン: {_currentLaneIndex}");
    }

    /// <summary>
    /// プレイヤーを移動させる
    /// </summary>
    private void Move()
    {
        if (!CanMove) return; // 成功判定が出ていなかったら処理を行わない
        Debug.Log("音声入力成功→プレイヤーを移動させる");

        if (_currentLaneIndex < 0)
        {
            AudioManager.Instance.PlaySE(SENameEnum.Falling);
            // インデックスがマイナスになった時＝地面に落ちた時ゲームオーバー処理を呼ぶ
            Foundation.NotifyGameOver();
        }

        // インデックスが5を超える場合は5の状態を維持する
        if (_currentLaneIndex > 5)
        {
            _currentLaneIndex = 5;
            _move.ParticleGenerater.PlayConfettiParticle(); // 紙吹雪エフェクトを再生する
        }

        _move.JumpToNextPendulum(Search());
        CanMove = false;
    }

    /// <summary>
    /// 次のオブジェクトを検索する
    /// </summary>
    private Transform Search()
    {
        Transform currentPendulum = _playerTransform.parent; // 現在掴まっている振り子オブジェクトを取得
        var objects = Foundation.InGameLane[_currentLaneIndex]; // 配列を取得
        Transform nextPendulum = null; // 移動先の振り子オブジェクト
        float minDistance = float.MaxValue; // 検索用

        // 距離を計算
        foreach (var obj in objects)
        {
            // 現在いる位置よりX座標でマイナス側にあるオブジェクトと現在掴まっているオブジェクトは検索に含めない
            if (obj.transform.position.x <= currentPendulum.position.x) continue;

            float distance = obj.transform.position.x - currentPendulum.position.x;
            if (distance < minDistance)
            {
                nextPendulum = obj.GetComponent<PendulumController>().PlayerAnchor;
                minDistance = obj.transform.position.x - currentPendulum.position.x;
            }
        }

        // Event登録を変更する
        _pendulumController.OnReachTheEdge -= Move;
        _pendulumController = nextPendulum.GetComponent<PendulumController>();
        _pendulumController.OnReachTheEdge += Move;

        Debug.Log($"検索された移動先のオブジェクト:{nextPendulum.name}");
        return nextPendulum;
    }
}
