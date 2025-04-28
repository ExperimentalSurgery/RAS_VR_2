using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PartMovementHandler : MonoBehaviour
{
    [Header("Grab Settings")]
    [SerializeField] GameObject[] parts;
    [SerializeField] bool isGrabbed = false;

    [Header("Material Settings")]
    [SerializeField] Material glowMaterial;
    [SerializeField] Material defaultMat;
    [SerializeField] Renderer renderer;

    [Header("Vibration Settings")]
    [SerializeField] bool isShortVibrationFeedback = false;
    [SerializeField] float magnitude = 0.7f, frequency = 70f;
    [SerializeField] bool enableHapticlabsFeedback = false;
    public bool isInZone = true;
    private void Start()
    {
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
    }
    void Update()
    {

    }

    public void SetIsGrabbed(bool isGrabbed, GameObject collisionMesh)
    {
        this.isGrabbed = isGrabbed;
        renderer =  collisionMesh.GetComponent<Renderer>();
        SetCollisions();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "HapticCollider_Left" || collision.gameObject.tag == "HapticCollider_Right")
        {
            collision.gameObject.GetComponent<Renderer>().material = glowMaterial;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "HapticCollider_Left" || collision.gameObject.tag == "HapticCollider_Right")
        {
            if (!isGrabbed)
            {
                collision.gameObject.GetComponent<Renderer>().material = defaultMat;
            }
        }
    }

    void SetCollisions()
    {  if(!isInZone) { return; }
        if (isGrabbed)
        {
            DisableCollisionsForAllParts();
            SwapMaterial(true);

            // GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        }
        if (!isGrabbed)
        {
            EnableCollisionsForAllParts();
            SwapMaterial(false);
            //  GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        } 
    }
    void DisableCollisionsForAllParts()
    {
        GetComponent<Rigidbody>().excludeLayers = ~0;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        // GetComponent<Rigidbody>().excludeLayers = ~UnityEngine.LayerMask.GetMask("Placeable Surface");
        foreach (var part in parts)
        {
            if (part.GetComponent<Rigidbody>() != null)
            {

                part.GetComponent<Rigidbody>().excludeLayers = ~0;
               // part.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

            }

        }
    }

    void EnableCollisionsForAllParts()
    {
        GetComponent<Rigidbody>().excludeLayers = 0;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        foreach (var part in parts)
        {
            if (part.GetComponent<Rigidbody>() != null)
            {

                part.GetComponent<Rigidbody>().excludeLayers = 0;
              //  part.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;


            }

        }
    }



    public void SwapMaterial(bool isOn)
    {
        if (isOn)
        {
           renderer.material = glowMaterial;
        }
        else
        {
            renderer.material = defaultMat;
        }
    }

}
