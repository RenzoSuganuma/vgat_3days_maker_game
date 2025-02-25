using NUnit.Framework;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class StageGenerator : MonoBehaviour
{
    [SerializeField] Transform _player;
    [SerializeField] GameObject _ringPrefab;

    // �����ƊԊu�̑Ή�
    [SerializeField, Tooltip("�Œቡ��")] float _baseWidth = 5;
    [SerializeField, Tooltip("�K�w���オ�邲�Ƃɑ����鉡��")] float _widthScale = 1;
    [SerializeField, Tooltip("�K�w���̍�����")] float _layerHeight = 5;
    

    [SerializeField, Tooltip("�����K�w")] int _baseHeightLayer = 10;
    [SerializeField, Tooltip("��x�ɐ������鋗��")] float _maxWidth = 100;
    [SerializeField, Tooltip("�������J�n����ړ�����")] float _generatePerMoveDistance = 50;

    private float _nextGeneratePosX;

    private void Start()
    {
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
        for (int height = 1; height < 20; height++)
        {
            for (int x = 0; x < 20; x++)
            {
                var spacing = _nextGeneratePosX;
                var ringX = spacing + _baseWidth + _widthScale * height * x;
                if (ringX > _maxWidth)
                {
                    break;
                }

                var ringY = (height - _baseHeightLayer) * _layerHeight; // height = _baseHeightLayer�̂Ƃ���0
                
                var relativePos = new Vector3(ringX, ringY);
                var absPos = _player.position + relativePos;

                Instantiate(_ringPrefab, absPos, Quaternion.identity);
            }
        }

        _nextGeneratePosX = _player.position.x + _generatePerMoveDistance;
    }
}

public class StageRowGenerator
{
    [SerializeField] GameObject _prefab;
    [SerializeField] int _index;
    [SerializeField] float _space;
    [SerializeField] float _height;

    public StageRowGenerator(GameObject prefab, float space, float height)
    {
        _prefab = prefab;
        _space = space;
        _height = height;
    }

    public void Generate(float maxX)
    {
        for (int i = 0; i < 1000; i++)
        {
            var x = _index * _space;
            if (x > maxX)
            {
                return;
            }

            Object.Instantiate(_prefab, new Vector3(x, _height), Quaternion.identity);
            _index++;
        }
    }
}
