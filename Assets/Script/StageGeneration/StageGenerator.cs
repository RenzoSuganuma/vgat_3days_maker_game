using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

/// <summary>
/// �X�e�[�W�𐶐�����N���X
/// </summary>
public class StageGenerator : MonoBehaviour
{
    [SerializeField] Transform _player;
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
        }

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
        if (_player.position.x > _nextGeneratePosX)
        {
            GenerateStage();
        }
    }

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