using NUnit.Framework;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class StageGenerator : MonoBehaviour
{
    [SerializeField] Transform _player;
    [SerializeField] GameObject _ringPrefab;

    // 高さと間隔の対応
    [SerializeField, Tooltip("基本横幅")] float _baseWidth = 5;
    [SerializeField, Tooltip("階層が上がるごとに増える横幅")] float _widthPerLayer = 1;
    [SerializeField, Tooltip("階層毎の高さの幅")] float _heightPerLayer = 5;

    [SerializeField, Tooltip("生成する階層の数")] int _generateLayers = 30;
    [SerializeField, Tooltip("Y=0に生成される階層")] int _initialLayer = 10;
    [SerializeField, Tooltip("一度に生成する距離")] float _generateDistance = 100;
    [SerializeField, Tooltip("生成を開始する移動距離")] float _generatePerMoveDistance = 50;

    List<StageRowGenerator> _generator = new();

    private float _nextGeneratePosX;

    private void Start()
    {
        for (int layer = 1; layer <= _generateLayers; layer++)
        {
            var space = _baseWidth + _widthPerLayer * layer;
            var height = (layer - _initialLayer) * _heightPerLayer;
            _generator.Add(new StageRowGenerator(_ringPrefab, space, height));
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

public class StageRowGenerator
{
    [SerializeField] GameObject _prefab;
    [SerializeField] Transform _parent;
    [SerializeField] int _index;
    [SerializeField] float _space;
    [SerializeField] float _height;

    public StageRowGenerator(GameObject prefab, float space, float height)
    {
        _prefab = prefab;
        _space = space;
        _height = height;
    }

    public void Generate(float maxX, Transform parent = null)
    {
        for (int i = 0; i < 1000; i++)
        {
            var x = _index * _space;
            if (x > maxX)
            {
                return ;
            }

            var obj = Object.Instantiate(_prefab, new Vector3(x, _height), Quaternion.identity);

            if (parent) obj.transform.parent = parent;

            _index++;
        }
    }
}
