using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// レーンの移動先を決定する
/// </summary>
public class DestinationCheck : MonoBehaviour
{
    [SerializeField] private PendulumController _pendulumController;
    [SerializeField] private Transform _playerTransform;
    private PlayerMove _move;

    [Header("Debug用")]
    [SerializeField] private List<GameObject> objects;

    public bool CanMove { get; set; } // 音声入力があったらtrueにする
    private int _currentLaneIndex; // 現在プレイヤーがいるレーンのindex（0~5）

    private void Start()
    {
        _pendulumController.OnReachTheEdge += Move;
        _move = _playerTransform.GetComponent<PlayerMove>();
    }

    private void OnDestroy()
    {
        _pendulumController.OnReachTheEdge -= Move;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // テスト用
        {
            CanMove = true;
        }
    }

    private void Move()
    {
        if(!CanMove) return; // 成功判定が出ていなかったら処理を行わない

        if (_currentLaneIndex < 0)
        {
            // インデックスがマイナスになった時＝地面に落ちた時ゲームオーバー処理を呼ぶ
            Foundation.NotifyGameOver();
        }

        // Indexを変更する処理
        _currentLaneIndex++;
        _move.JumpToNextPendulum(Search());
        CanMove = false;
    }

    /// <summary>
    /// 次のオブジェクトを検索する
    /// </summary>
    private Transform Search()
    {
        Transform currentPendulum = _playerTransform.parent; // 現在掴まっている振り子オブジェクトを取得
        //var objects = Foundation.InGameLane[_currentLaneIndex]; // 配列を取得
        Transform nextPendulum = null; // 移動先の振り子オブジェクト
        float minDistance = float.MaxValue; // 検索用

        // 距離を計算
        foreach (var obj in objects)
        {
            // 現在いる位置よりX座標でマイナス側にあるオブジェクトと現在掴まっているオブジェクトは検索に含めない
            if(obj.transform.position.x <= currentPendulum.position.x) continue;

            float distance = obj.transform.position.x - currentPendulum.position.x;
            if (distance < minDistance)
            {
                nextPendulum = obj.transform;
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
