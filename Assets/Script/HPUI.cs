using UnityEngine;
using UnityEngine.UI;

public class HPUI : MonoBehaviour
{
    [SerializeField] private PlayerStat playerStat;
    [SerializeField] private Image[] heartIcons;       
    [SerializeField] private Sprite heartFull;         
    [SerializeField] private Sprite heartEmpty;        

    void Update()
    {
        for (int i = 0; i < heartIcons.Length; i++)
        {
            heartIcons[i].sprite = i < playerStat.playerHp ? heartFull : heartEmpty;
        }
    }
}
