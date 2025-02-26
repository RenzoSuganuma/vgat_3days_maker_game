using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �e���[���ɃI�u�W�F�N�g�𐶐�����N���X�i�U��q�̊Ԃɐ����j<br></br>
/// ���[�������Ɠ����ɐ�������
/// </summary>
public class LaneObjectGenerator : MonoBehaviour
{
    [SerializeField] RandomObjectChooser _objChooser;
    [SerializeField] float _probability = 0.6f;
    [SerializeField] int[] _generatedIndex = new int[6];
    List<Transform> _parents = new();
    int _seedAdd;

    public void Generate()
    {
        for (int layer = 0; layer < 6; layer++)
        {
            _parents.Add(new GameObject("ObstacleRow").transform);
            Generate(layer);
        }
    }

    public void Generate(int layer)
    {
        var lane = Foundation.InGameLane[layer];
        for (int i = _generatedIndex[layer]; i < lane.Count - 1; i++)
        {
            // �m���Ő���
            Random.InitState(System.DateTime.Now.Millisecond + _seedAdd);
            _seedAdd++;
            _generatedIndex[layer]++;

            if (Random.value > _probability) return;

            // �����_���ȃI�u�W�F�N�g�𐶐�
            var randObj = _objChooser.Choose();
            var obj = randObj.obj;

            if (obj == null) return;

            if (randObj.mid)
            {
                // �U��q�̒��_�𒆐S�ɐ���
                var left = lane[i].transform.position;
                var right = lane[i + 1].transform.position;
                var pos = left + (right - left) / 2;

                Instantiate(obj, pos, Quaternion.identity, _parents[layer]);
            }
            else
            {
                // �U��q�𒆐S�ɐ���
                Instantiate(obj, lane[i].transform.position, Quaternion.identity, _parents[layer]);
            }
        }
    }
}