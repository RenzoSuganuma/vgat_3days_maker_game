using System.Collections;
using UnityEngine;

/// <summary>
/// ���[����������Ɨ����ď�Q���𐶐�����N���X<br></br>
/// �����ꏊ�ɉ��x��������Ă����Ȃ��I�u�W�F�N�g�Ɏg�p���Ă�������
/// </summary>
public class IndependenceObstacleGenerator : MonoBehaviour
{
    [SerializeField] GameObject _obstaclePref;
    [SerializeField, Tooltip("�v���C���[����ǂꂾ�������Đ������邩")] float _distance = 20;
    [SerializeField, Tooltip("���C���[���̐����Ԋu")] AnimationCurve[] _duration;
    [SerializeField] float _yOffset;

    public void StartGenerate(Transform playerPos, float height, int layer)
    {
        if (layer < _duration.Length)
        {
            StartCoroutine(Generate(playerPos, height, layer, _duration[layer].Evaluate(Random.value)));
        }
    }

    private IEnumerator Generate(Transform playerPos, float height, int layer, float t)
    {
        yield return new WaitForSeconds(t);
        var generatePos = new Vector3(playerPos.position.x + _distance, height + _yOffset, 0);
        Instantiate(_obstaclePref, generatePos, Quaternion.identity);

        StartCoroutine(Generate(playerPos, height, layer, _duration[layer].Evaluate(Random.value)));
    } 
}
