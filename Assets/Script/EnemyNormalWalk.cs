using UnityEngine;

public class EnemyNormalWalk : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Vector2 startPoint;
    public float enemyWalkRange;
    public float enemySpeed;
    private int direction = 1;
    // Update is called once per frame
    private void Awake()
    {
        startPoint = transform.position;
    }
    void Update()
    {

        transform.position += new Vector3(enemySpeed * direction * Time.deltaTime, 0, 0);
        if (transform.position.x >= startPoint.x + enemyWalkRange)
        {
            direction = -1;
            
        }
        // ถึงขอบซ้าย → กลับขวา
        else if (transform.position.x <= startPoint.x - enemyWalkRange)
        {
            direction = 1;
            
        }

    }
}
