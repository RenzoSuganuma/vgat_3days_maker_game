using UnityEngine;

/// <summary>
/// レーン内に障害物を生成するクラス。移動しない障害物に使用
/// レーン生成と同時に生成する
/// </summary>
public class ObstacleGenerator : MonoBehaviour
{
    [SerializeField] GameObject _obstaclePref;
    [SerializeField] float _probability = 0.1f;
    [SerializeField] int[] _generatedIndex = new int[6];
    int _seedAdd;

    public void Generate()
    {
        for (int i = 0; i < 6; i++)
        {
            Generate(i);
        }
    }

    public void Generate(int layer)
    {
        var lane = Foundation.InGameLane[layer];
        for (int i = _generatedIndex[layer]; i < lane.Count - 1; i++)
        {
            Random.InitState(System.DateTime.Now.Millisecond + _seedAdd);
            _seedAdd++;
            _generatedIndex[layer]++;
            if (Random.value > _probability) continue;

            var prev = lane[i].transform.position;
            var below = lane[i + 1].transform.position;

            var pos = prev + (below - prev) / 2;

            Instantiate(_obstaclePref, pos, Quaternion.identity);
        }
    }
}