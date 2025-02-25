using UnityEngine;

/// <summary>
/// ���[�����ɏ�Q���𐶐�����N���X�B�ړ����Ȃ���Q���Ɏg�p
/// ���[�������Ɠ����ɐ�������
/// </summary>
public class ObstacleGenerator : MonoBehaviour
{
    [SerializeField] GameObject _obstaclePref;
    [SerializeField] float _probability = 0.1f;
    int[] _generatedIndex;
    int _seedAdd;

    public void Initialize()
    {
        _generatedIndex = new int[6];
    }

    public void Generate(int layer)
    {
        var lane = Foundation.InGameLane[layer];
        for (int i = 2; i < lane.Count; i++)
        {
            Random.InitState(System.DateTime.Now.Millisecond + _seedAdd);
            _seedAdd++;
            if (Random.value > _probability) continue;

            var index = _generatedIndex[layer] + i;

            var prev = lane[index - 1].transform.position;
            var below = lane[index].transform.position;

            var pos = prev + (below - prev) / 2;

            Instantiate(_obstaclePref, pos, Quaternion.identity);
        }
    }
}