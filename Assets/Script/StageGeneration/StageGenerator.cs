using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �X�e�[�W�𐶐�����N���X
/// </summary>
public class StageGenerator : MonoBehaviour
{
    [SerializeField] GameObject _player;
    [SerializeField] StageRowGenerator _stageRowGenerator;
    [SerializeField] LaneObjectGenerator _obstacleGenerator;

    [SerializeField, Tooltip("��{����")] float _baseWidth = 5;

    [SerializeField, Tooltip("�K�w���オ�邲�Ƃɑ����鉡��")]
    float _widthPerLayer = 1;

    [SerializeField, Tooltip("�K�w���̍����̕�")]
    float _heightPerLayer = 5;

    [SerializeField, Tooltip("��������K�w�̐�")]
    int _generateLayers = 6;

    [SerializeField, Tooltip("Y=0�ɐ��������K�w�i0-origin�j")]
    int _initialLayer = 2;

    [SerializeField, Tooltip("��x�ɐ������鋗��")]
    float _generateDistance = 100;

    [SerializeField, Tooltip("�������J�n����ړ�����")]
    float _generatePerMoveDistance = 50;

    List<StageRowGenerator> _generator = new();

    private float _nextGeneratePosX;

    private void Start()
    {
        // �e���[���̃W�F�l���[�^�[�𐶐��A�v���p�e�B���Z�b�g
        for (int layer = 0; layer < _generateLayers; layer++)
        {
            var space = _baseWidth + _widthPerLayer * layer;
            var height = (layer - _initialLayer) * _heightPerLayer;
            var row = Instantiate(_stageRowGenerator);
            row.Initialize(space, height, layer);
            _generator.Add(row);
        }

        // �X�e�[�W����
        GenerateStage();

        _player.GetComponent<PlayerMove>().Initialize();
    }

    /// <summary>
    /// �e���[���ŃX�e�[�W��������B
    /// </summary>
    private void GenerateStage()
    {
        if (Foundation.InGameLane == null) Debug.Log("Foundation.InGameLane is null");

        // var stageParent = new GameObject("Stage").transform;

        for (int i = 0; i < _generator.Count; i++)
        {
            var row = _generator[i];
            if (i == 0 && FindAnyObjectByType<PlayerMove>() != null)
            {
                row.Generate(_player.transform.position.x + _generateDistance, row.gameObject.transform);
            }
            else
            {
                row.Generate(_player.transform.position.x + _generateDistance, row.gameObject.transform);
            }
        }

        _obstacleGenerator.Generate();

        _nextGeneratePosX = _player.transform.position.x + _generatePerMoveDistance;
    }
}
