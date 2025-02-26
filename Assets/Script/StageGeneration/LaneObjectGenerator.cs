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
    private int[] _generatedIndex = new int[6];
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
        Debug.Log($"[log] {Foundation.InGameLane.Length}");

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
                // ���̗ւ̉E�ɂ͏�Q���𐶐����Ȃ�
                if (randObj.isObstacle && lane[i].GetComponent<SkipObstacle>() != null)
                {
                    var l = lane[i].transform.position;
                    var r = lane[i + 1].transform.position;
                    Debug.DrawLine(l, r, Color.yellow, 1000);
                    continue;
                }

                // �U��q�̒��_�𒆐S�ɐ���
                var left = lane[i].transform.position;
                var right = lane[i + 1].transform.position;
                var pos = left + (right - left) / 2;

                Instantiate(obj, pos, Quaternion.identity, _parents[layer]);
            }
            else
            {
                bool existUp = layer != 5;

                Debug.Log($"[log] {lane[i]}");

                var right = Search(lane[i].transform, i, layer + (existUp ? 1 : -1)).position;
                var left = lane[i].transform.position;

                var pos = left + (right - left) / 2;
                Debug.DrawLine(left, right, Color.red, 1000);

                Instantiate(obj, pos, Quaternion.identity, _parents[layer]);
            }
        }
    }

    private Transform Search(Transform current, int index, int layer)
    {
        var lane = Foundation.InGameLane[layer];

        Transform nextPendulum = null; // �ړ���̐U��q�I�u�W�F�N�g
        float currMinDistance = float.MaxValue; // �����p

        foreach (var target in lane)
        {
            // ���݂���ʒu���X���W�Ń}�C�i�X���ɂ���I�u�W�F�N�g�ƌ��ݒ͂܂��Ă���I�u�W�F�N�g�͌����Ɋ܂߂Ȃ�
            if (target.transform.position.x <= current.position.x) continue;

            float distance = target.transform.position.x - current.position.x;
            if (distance < currMinDistance)
            {
                nextPendulum = target.transform;
                currMinDistance = target.transform.position.x - current.position.x;
            }
        }

        Debug.Log($"�������ꂽ�ړ���̃I�u�W�F�N�g:{nextPendulum.name}");
        return nextPendulum;
    }
}
