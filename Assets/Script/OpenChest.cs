using UnityEngine;

public class OpenChest : MonoBehaviour
{
    public Animator animator;
    private bool isOpened;
    [SerializeField] private AudioClip openSound;
    [SerializeField] private AudioSource audioSource;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!isOpened)
            {
                audioSource.PlayOneShot(openSound);
                isOpened = true;
                animator.SetBool("IsOpened", isOpened);
            }
        }
    }
}
