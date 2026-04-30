using UnityEditor.Rendering;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int monsterDamge = 1 ;
    public void Attacked()
    {
        Destroy(this.gameObject);
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            var player = collision.gameObject.GetComponent<PlayerStat>();
            player.TakeDamage(monsterDamge);
        }
    }
}
