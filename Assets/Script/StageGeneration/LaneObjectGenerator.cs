using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 各レーンにオブジェクトを生成するクラス（振り子の間に生成）<br></br>
/// レーン生成と同時に生成する
/// </summary>
public class LaneObjectGenerator : MonoBehaviour
{
    [SerializeField] RandomObjectChooser _chooser;
    [SerializeField] float _probability = 0.1f;
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
            // 確立で生成
            Random.InitState(System.DateTime.Now.Millisecond + _seedAdd);
            _seedAdd++;
            _generatedIndex[layer]++;
            if (Random.value > _probability) continue;

            // 振り子の中点を取得
            var prev = lane[i].transform.position;
            var below = lane[i + 1].transform.position;
            var pos = prev + (below - prev) / 2;

            // ランダムなオブジェクトを生成
            Instantiate(_chooser.Choose(), pos, Quaternion.identity, _parents[layer]);
        }
    }
}