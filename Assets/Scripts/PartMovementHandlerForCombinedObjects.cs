using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Rendering;

public class PartMovementHandlerForCombinedObjects : MonoBehaviour
{
    [SerializeField] Transform triggerObj;
    [SerializeField] bool check;
    [SerializeField] GameObject[] parts;
    [SerializeField] bool isGrabbed = false;
    [SerializeField] Transform externalObj;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log(" gameObject.layer: " + LayerMask.NameToLayer("ExternalObject"));
        }

     
    }

    public bool GetIsGrabbed()
    {
        return isGrabbed;
    }

    public void SetIsGrabbed(bool isGrabbed)
    {
        this.isGrabbed = isGrabbed;

        SetCollisions();
    }


    public void SetCollisions()
    {
        if (isGrabbed)
        {
            //Hapticlabs.StartTrack("Drill Simulation");
            DisableCollisionsForAllParts();
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            
        }
        if (!isGrabbed)
        {
           
            if (triggerObj != null && !triggerObj.GetComponent<InternalObjectTriggerHandler>().isInside)
            {
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                EnableCollisionsForAllParts();
            }


            else
            {
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                EnableCollisionsForAllParts();
            }
            
           

        } 
    }
    void DisableCollisionsForAllParts() // code for part doesn't work
    {
        GetComponent<Rigidbody>().excludeLayers = ~0;
        if (parts.Length > 0)
        {
            foreach (var part in parts)
            {
                if (part.GetComponent<Rigidbody>() != null)
                {

                    part.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

                }

            }
        }
    }

    void EnableCollisionsForAllParts() // code for part doesn't work
    {
        GetComponent<Rigidbody>().excludeLayers = 0;

        if (parts.Length > 0)
        {
            foreach (var part in parts)
            {
                if (part.GetComponent<Rigidbody>() != null)
                {

                    part.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;


                }

            }
        }
    }
  
}
