using UnityEngine;
using System;

public class ColliderPainter : MonoBehaviour
{
    [SerializeField] bool isPainted;
    public static Action onPainted;

    public void SetIsPainted(bool isPainted)
    {
        this.isPainted = isPainted;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "HapticCollider_Right") { 
            if(!isPainted)
            {
                SetIsPainted(true);
                onPainted?.Invoke();
            } 
        }
    }
}
