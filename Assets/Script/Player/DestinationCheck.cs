using System;
using UnityEngine;

/// <summary>
/// レーンの移動先を決定する
/// </summary>
public class DestinationCheck : MonoBehaviour
{
    [SerializeField] private PendulumController _pendulumController;

    public bool CanMove { get; set; } // 音声入力があったらtrueにする

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

    }
}
