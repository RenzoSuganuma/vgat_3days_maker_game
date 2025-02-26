using UnityEngine;

/// <summary>
/// プレイヤーと障害物オブジェクトの当たり判定を監視するクラス
/// </summary>
public class HitJudgment : MonoBehaviour
{
    [Header("障害物のレイヤー")]
    [SerializeField] private LayerMask _gimmickLayer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & _gimmickLayer) != 0)
        {
            Foundation.NotifyGameOver(); // ゲームオーバー処理を呼ぶ
        }
    }
}
