using UnityEngine;

public class EnemyNormalFly : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public float moveSpeed = 2f;
    public Vector2 patrolDirection = new Vector2(1, 1); // ทิศเฉียง (x, y)
    public float patrolDistance = 4f; // ระยะรวมจากจุดเริ่มต้น

    private Vector2 startPosition;
    private int direction = 1;

    void Start()
    {
        
        startPosition = transform.position;

    }

    void Update()
    {
        // เดินตามทิศเฉียงที่กำหนด
        transform.position += (Vector3)(patrolDirection * moveSpeed * direction * Time.deltaTime);

        // วัดระยะจากจุดเริ่มต้น
        float distanceFromStart = Vector2.Distance(transform.position, startPosition);

        if (distanceFromStart >= patrolDistance)
        {
            direction *= -1;
            
        }
    }
}
