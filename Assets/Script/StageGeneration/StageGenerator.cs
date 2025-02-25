using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

/// <summary>
/// ステージを生成するクラス
/// </summary>
public class StageGenerator : MonoBehaviour
{
    [SerializeField] Transform _player;
    [SerializeField] StageRowGenerator _stageRowGenerator;
    [SerializeField] IndependenceObstacleGenerator _indObstacleGenerator;
    [SerializeField] LaneObjectGenerator _obstacleGenerator;

    [SerializeField, Tooltip("基本横幅")] float _baseWidth = 5;
    [SerializeField, Tooltip("階層が上がるごとに増える横幅")] float _widthPerLayer = 1;
    [SerializeField, Tooltip("階層毎の高さの幅")] float _heightPerLayer = 5;

    [SerializeField, Tooltip("生成する階層の数")] int _generateLayers = 6;
    [SerializeField, Tooltip("Y=0に生成される階層（0-origin）")] int _initialLayer = 2;
    [SerializeField, Tooltip("一度に生成する距離")] float _generateDistance = 100;
    [SerializeField, Tooltip("生成を開始する移動距離")] float _generatePerMoveDistance = 50;

    List<StageRowGenerator> _generator = new();

    private float _nextGeneratePosX;

    private void Start()
    {
        // 各レーンのジェネレーターを生成、プロパティをセット
        for (int layer = 0; layer < _generateLayers; layer++)
        {
            var space = _baseWidth + _widthPerLayer * layer;
            var height = (layer - _initialLayer) * _heightPerLayer;
            var row = Instantiate(_stageRowGenerator);
            row.Initialize(space, height, layer);
            _generator.Add(row);
        }

        // ステージ生成
        GenerateStage();

        for ( int layer = 0; layer < _generateLayers; layer++)
        {
            var space = _baseWidth + _widthPerLayer * layer;
            var height = (layer - _initialLayer) * _heightPerLayer;

            if (_indObstacleGenerator != null)
            {
                _indObstacleGenerator.StartGenerate(_player, height, layer);
            }

            if (_obstacleGenerator != null)
            {
                _obstacleGenerator.Generate(layer);
            }
        }
    }

    private void Update()
    {
        // 一定距離移動するごとに生成
        if (_player.position.x > _nextGeneratePosX)
        {
            GenerateStage();
        }
    }

    /// <summary>
    /// 各レーンでステージ生成する。
    /// </summary>
    private void GenerateStage()
    {
        if (Foundation.InGameLane == null) Debug.Log("Foundation.InGameLane is null");

        // var stageParent = new GameObject("Stage").transform;

        foreach (var row in _generator)
        {
            row.Generate(_player.position.x + _generateDistance, row.gameObject.transform);
        }

        _obstacleGenerator.Generate();

        _nextGeneratePosX = _player.position.x + _generatePerMoveDistance;
    }
}