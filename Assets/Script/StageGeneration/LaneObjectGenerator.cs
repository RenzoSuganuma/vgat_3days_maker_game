using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 各レーンにオブジェクトを生成するクラス（振り子の間に生成）<br></br>
/// レーン生成と同時に生成する
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
            // 確率で生成
            Random.InitState(System.DateTime.Now.Millisecond + _seedAdd);
            _seedAdd++;
            _generatedIndex[layer]++;

            if (Random.value > _probability) return;

            // ランダムなオブジェクトを生成
            var randObj = _objChooser.Choose();
            var obj = randObj.obj;

            if (obj == null) return;

            if (randObj.mid)
            {
                // 振り子の中点を中心に生成
                var left = lane[i].transform.position;
                var right = lane[i + 1].transform.position;
                var pos = left + (right - left) / 2;

                Instantiate(obj, pos, Quaternion.identity, _parents[layer]);
            }
            else
            {
                // 振り子を中心に生成
                Instantiate(obj, lane[i].transform.position, Quaternion.identity, _parents[layer]);
            }
        }
    }
}