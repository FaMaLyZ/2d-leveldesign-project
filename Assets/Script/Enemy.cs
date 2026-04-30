using EnemyPatrolling;
using UnityEditor.Rendering;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int monsterDamge = 1 ;
    public DeathPatrolling dead;
    void Start()
    {
        dead = GetComponent<DeathPatrolling>();
    }
    public void Attacked()
    {
        dead.onDead();
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
