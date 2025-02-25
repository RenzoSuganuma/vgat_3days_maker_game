using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ステージを生成するクラス
/// </summary>
public class StageGenerator : MonoBehaviour
{
    [SerializeField] Transform _player;
    [SerializeField] GameObject _ringPrefab;
    [SerializeField] IndependenceObstacleGenerator _obstacleGenerator;

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
        for (int layer = 0; layer < _generateLayers; layer++)
        {
            var space = _baseWidth + _widthPerLayer * layer;
            var height = (layer - _initialLayer) * _heightPerLayer;
            _generator.Add(new StageRowGenerator(_ringPrefab, space, height, layer));
            _obstacleGenerator.StartGenerate(_player, height, layer);
        }

        GenerateStage();
    }

    private void Update()
    {
        if (_player.position.x > _nextGeneratePosX)
        {
            GenerateStage();
        }
    }

    private void GenerateStage()
    {
        var stageParent = new GameObject("Stage").transform;

        foreach (var row in _generator)
        {
            row.Generate(_player.position.x + _generateDistance, stageParent);
        }

        _nextGeneratePosX = _player.position.x + _generatePerMoveDistance;
    }
}


/// <summary>
/// レーン毎にステージを生成するクラス
/// </summary>
[System.Serializable] // デバッグ用
public class StageRowGenerator
{
    [SerializeField] GameObject _prefab;
    [SerializeField] float _space;
    [SerializeField] float _height;
    [SerializeField] int _layer;

    [SerializeField] int _generateIndex; // 生成時に使用するindex
    [SerializeField] ObstacleGenerator _obstacleGenerator;

    public StageRowGenerator(GameObject prefab, float space, float height, int layer)
    {
        _prefab = prefab;
        _space = space;
        _height = height;
        _layer = layer;
    }

    public void Generate(float maxX, Transform parent = null)
    {
        for (int i = 0; i < 1000; i++)
        {
            var x = _generateIndex * _space;
            if (x > maxX)
            {
                return;
            }

            var obj = Object.Instantiate(_prefab, new Vector3(x, _height), Quaternion.identity);
            Foundation.InGameLane[_layer].Add(obj);

            if (parent) obj.transform.parent = parent;

            _generateIndex++;
        }
    }
}