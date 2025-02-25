using UnityEngine;

/// <summary>
/// レーン内に障害物を生成するクラス。移動しない障害物に使用
/// </summary>
public class ObstacleGenerator : MonoBehaviour
{
    [SerializeField] Transform _player;
    [SerializeField] GameObject _obstaclePref;
    [SerializeField, Tooltip("プレイヤーからどれだけ離して生成するか")] float _distance = 20;
    [SerializeField, Tooltip("生成個数")] AnimationCurve[] _duration;
    [SerializeField] float _yOffset;

    public void Generate(int layer)
    {

    }
}
