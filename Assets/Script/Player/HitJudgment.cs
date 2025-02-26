using UnityEngine;

/// <summary>
/// プレイヤーと障害物オブジェクトの当たり判定を監視するクラス
/// </summary>
public class HitJudgment : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Gimmick"))
        {
            Foundation.NotifyGameOver();
        }
    }
}
