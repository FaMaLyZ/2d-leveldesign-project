using UnityEngine;

public class FinishGame : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            GameOverManager.TriggerGameOver();
            Debug.Log("Win");
        }
    }
}
