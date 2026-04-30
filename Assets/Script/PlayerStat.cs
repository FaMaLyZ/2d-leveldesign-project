using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using Unity.Mathematics;

public class PlayerStat : MonoBehaviour
{   
    public int playerMaxHP = 10 ;
    public int playerHp = 10;

    public bool isIfram = false;

    private PlayerController playerController;
    
    private void Start() 
    {
        playerController = GetComponent<PlayerController>();
    }

    public void TakeDamage(int damagetaken)
    {
        if(!isIfram)
        {
            StartCoroutine(Ifram(damagetaken));
        }
    }
    public void Heal()
    {
        playerHp = playerMaxHP;
        Debug.Log(playerHp);
    }
    
    public IEnumerator Ifram(int damagetaken)
    {
        isIfram = true;
        playerHp -= damagetaken;
        playerController.DamageKnockback();
        Debug.Log(playerHp);
        yield return new WaitForSeconds(0.5f);
        isIfram = false;
    }
}
