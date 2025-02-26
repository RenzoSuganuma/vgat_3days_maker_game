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
                // 炎の輪の右には障害物を生成しない
                if (randObj.isObstacle && lane[i].GetComponent<SkipObstacle>() != null)
                {
                    var l = lane[i].transform.position;
                    var r = lane[i + 1].transform.position;
                    Debug.DrawLine(l, r, Color.yellow, 1000);
                    continue;
                }

                // 振り子の中点を中心に生成
                var left = lane[i].transform.position;
                var right = lane[i + 1].transform.position;
                var pos = left + (right - left) / 2;

                Instantiate(obj, pos, Quaternion.identity, _parents[layer]);
            }
            else
            {
                bool existUp = layer != 5;

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

        Transform nextPendulum = null; // 移動先の振り子オブジェクト
        float currMinDistance = float.MaxValue; // 検索用

        foreach (var target in lane)
        {
            // 現在いる位置よりX座標でマイナス側にあるオブジェクトと現在掴まっているオブジェクトは検索に含めない
            if (target.transform.position.x <= current.position.x) continue;

            float distance = target.transform.position.x - current.position.x;
            if (distance < currMinDistance)
            {
                nextPendulum = target.transform;
                currMinDistance = target.transform.position.x - current.position.x;
            }
        }

        Debug.Log($"検索された移動先のオブジェクト:{nextPendulum.name}");
        return nextPendulum;
    }
}