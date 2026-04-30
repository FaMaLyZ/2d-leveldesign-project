using UnityEditor.Rendering;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public void Attacked()
    {
        Destroy(this.gameObject);
    }
}
