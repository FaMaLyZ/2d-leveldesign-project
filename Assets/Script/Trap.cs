using UnityEngine;

public class Trap : MonoBehaviour
{
    public int trapDamage = 1 ;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            var player = collision.gameObject.GetComponent<PlayerStat>();
            player.TakeDamage(trapDamage);
        }
    }
    
}
