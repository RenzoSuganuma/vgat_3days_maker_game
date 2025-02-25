using System;
using UnityEngine;

/// <summary>
/// レーンの移動先を決定する
/// </summary>
public class DestinationCheck : MonoBehaviour
{
    [SerializeField] private PendulumController _pendulumController;
    [SerializeField] private Transform _playerTransform;

    public bool CanMove { get; set; } // 音声入力があったらtrueにする
    private int _currentLaneIndex; // 現在プレイヤーがいるレーンのindex（0~5）

    private void Start()
    {
        _pendulumController.OnReachTheEdge += Move;
    }

    private void OnDestroy()
    {
        _pendulumController.OnReachTheEdge -= Move;
    }

    private void Move()
    {
        if (_currentLaneIndex < 0)
        {
            // インデックスがマイナスになった時＝地面に落ちた時ゲームオーバー処理を呼ぶ
            Foundation.NotifyGameOver();
        }

        // Indexを変更する処理
        _currentLaneIndex++;
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
            // 現在いる位置よりX座標でマイナス側にあるオブジェクトは検索に含めない
            if(obj.transform.position.x < currentPendulum.position.x) continue;

            float distance = obj.transform.position.x - currentPendulum.position.x;
            if (distance > minDistance)
            {
                nextPendulum = obj.transform;
                minDistance = obj.transform.position.x - currentPendulum.position.x;
            }
        }

        return nextPendulum;
    }
}
