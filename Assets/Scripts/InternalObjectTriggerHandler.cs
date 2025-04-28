using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InternalObjectTriggerHandler : MonoBehaviour
{
    public bool isInside = false;
    private void OnTriggerStay(Collider other)
    {
        Debug.Log("OnTriggerStay");
        if (other == null) return;
        if (other.gameObject.layer == LayerMask.NameToLayer("ExternalObject"))
        {
            isInside = true;
            Debug.Log("Inside");

            GetComponentInParent<PartMovementHandlerForCombinedObjects>().SetCollisions();


        }

    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("ExternalObject"))
        {
            isInside = false;
            Debug.Log("Outside");
        }
    }
}
