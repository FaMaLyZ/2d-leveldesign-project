using UnityEngine;

public class RestArea : MonoBehaviour
{
    bool hasActivated = false;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && !hasActivated)
        {
            var playerStat = collision.gameObject.GetComponent<PlayerStat>();
            playerStat.Heal();
            hasActivated = true;
        }
    }
}
