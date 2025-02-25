using UnityEngine;

/// <summary>
/// レーン毎にステージを生成するクラス
/// </summary>
public class StageRowGenerator : MonoBehaviour
{
    [SerializeField] GameObject _ringPrefab;
    [SerializeField] GameObject _fireRingPrefab;
    [SerializeField] float _fireRingProbability = 0.1f;
    float _space;
    float _height;
    int _layer;

    int _generateIndex; // 生成時に使用するindex

    public void Initialize(float space, float height, int layer)
    {
        _space = space;
        _height = height;
        _layer = layer;
    }

    public void Generate(float maxX, Transform parent)
    {
        for (int i = 0; i < 1000; i++)
        {
            var x = _generateIndex * _space;
            if (x > maxX)
            {
                return;
            }

            var pref = Random.value < _fireRingProbability ? _fireRingPrefab : _ringPrefab;
            var obj = Instantiate(pref, new Vector3(x, _height), Quaternion.identity);

            Foundation.InGameLane?[_layer].Add(obj);
            if (parent) obj.transform.parent = parent;

            _generateIndex++;
        }
    }
}
