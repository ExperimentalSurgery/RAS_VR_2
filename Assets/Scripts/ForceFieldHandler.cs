using UnityEngine;

public class ForceFieldHandler : MonoBehaviour
{
    MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.enabled = false; 
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "HapticCollider_Right" || other.gameObject.tag == "HapticCollider_Left")
        {
            if (!meshRenderer.enabled)
            {
                meshRenderer.enabled = true; // Enable the renderer when a collision occurs
            }
            Debug.Log("Collision with haptic collider detected: " + other.gameObject.tag);
        }
    }
    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "HapticCollider_Right" || other.gameObject.tag == "HapticCollider_Left")
        {
            if (meshRenderer.enabled)
            {
                meshRenderer.enabled = false; // Disable the renderer when the collision ends
            }
            Debug.Log("Collision exit with haptic collider detected: " + other.gameObject.tag);
        }
       
    }

  
}
