using UnityEngine;

public class RestArea : MonoBehaviour
{
    bool hasActivated = false;
    public AudioClip healSound;
    public AudioSource audioSource;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && !hasActivated)
        {
            var playerStat = collision.gameObject.GetComponent<PlayerStat>();
            audioSource.PlayOneShot(healSound);
            playerStat.Heal();
            hasActivated = true;
        }
    }
}
