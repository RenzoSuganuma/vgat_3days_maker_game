using UnityEngine;

/// <summary>
/// ���[�����ɏ�Q���𐶐�����N���X�B�ړ����Ȃ���Q���Ɏg�p
/// </summary>
public class ObstacleGenerator : MonoBehaviour
{
    [SerializeField] Transform _player;
    [SerializeField] GameObject _obstaclePref;
    [SerializeField, Tooltip("�v���C���[����ǂꂾ�������Đ������邩")] float _distance = 20;
    [SerializeField, Tooltip("������")] AnimationCurve[] _duration;
    [SerializeField] float _yOffset;

    public void Generate(int layer)
    {

    }
}
