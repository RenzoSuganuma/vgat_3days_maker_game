using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �X�e�[�W�𐶐�����N���X
/// </summary>
public class StageGenerator : MonoBehaviour
{
    [SerializeField] Transform _player;
    [SerializeField] GameObject _ringPrefab;
    [SerializeField] GameObject _fireRingPrefab;
    [SerializeField] StageRowGenerator _stageRowGenerator;
    [SerializeField] IndependenceObstacleGenerator _indObstacleGenerator;
    [SerializeField] ObstacleGenerator _obstacleGenerator;

    [SerializeField, Tooltip("��{����")] float _baseWidth = 5;
    [SerializeField, Tooltip("�K�w���オ�邲�Ƃɑ����鉡��")] float _widthPerLayer = 1;
    [SerializeField, Tooltip("�K�w���̍����̕�")] float _heightPerLayer = 5;

    [SerializeField, Tooltip("��������K�w�̐�")] int _generateLayers = 6;
    [SerializeField, Tooltip("Y=0�ɐ��������K�w�i0-origin�j")] int _initialLayer = 2;
    [SerializeField, Tooltip("��x�ɐ������鋗��")] float _generateDistance = 100;
    [SerializeField, Tooltip("�������J�n����ړ�����")] float _generatePerMoveDistance = 50;

    List<StageRowGenerator> _generator = new();

    private float _nextGeneratePosX;

    private void Start()
    {
        for (int layer = 0; layer < _generateLayers; layer++)
        {
            var space = _baseWidth + _widthPerLayer * layer;
            var height = (layer - _initialLayer) * _heightPerLayer;

            var row = Instantiate(_stageRowGenerator);
            row.Initialize(space, height, layer);
            _generator.Add(row);

            if (_indObstacleGenerator)
            {
                _indObstacleGenerator.StartGenerate(_player, height, layer);
            }

            if (_obstacleGenerator)
            {
                
            }
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
        if (Foundation.InGameLane == null) Debug.Log("Foundation.InGameLane is null");

        var stageParent = new GameObject("Stage").transform;

        foreach (var row in _generator)
        {
            row.Generate(_player.position.x + _generateDistance, stageParent);
        }

        _nextGeneratePosX = _player.position.x + _generatePerMoveDistance;
    }
}

[System.Serializable]
public class ObstacleRowGenerator
{
    [SerializeField] GameObject _prefab;
    [SerializeField] float _space;
    [SerializeField] float _height;
    [SerializeField] int _layer;
    [SerializeField] float _offsetX;
    [SerializeField] float _probability;

    [SerializeField] int _generateIndex; // �������Ɏg�p����index

    public ObstacleRowGenerator(GameObject prefab, float space, float height, int layer, float offsetX, float probability)
    {
        _prefab = prefab;
        _space = space;
        _height = height;
        _layer = layer;
        _offsetX = offsetX;
        _probability = probability;
    }

    public void Generate(float maxX, Transform parent = null)
    {
        for (int i = 0; i < 1000; i++)
        {
            var x = _generateIndex * _space + _offsetX;
            if (x > maxX)
            {
                return;
            }

            if (_probability < Random.value)
            {
                var obj = Object.Instantiate(_prefab, new Vector3(x, _height), Quaternion.identity);
                if (parent) obj.transform.parent = parent;
            }

            _generateIndex++;
        }
    }
}