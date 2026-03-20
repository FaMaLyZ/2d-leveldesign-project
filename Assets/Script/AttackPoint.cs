using UnityEngine;

public class AttackPoint : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public  GameObject target;

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {   
        Destroy(target.gameObject);
        Destroy(this.gameObject);
    }
}
