using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField]private Transform playerPos;
    
    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(playerPos.position.x,playerPos.position.y +1 ,-10);
    }
}
