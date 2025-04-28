using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class LayerTrigger : MonoBehaviour
{
    [SerializeField] bool isLeftInside = false;
    [SerializeField] bool isRightInside = false;
    [SerializeField] ExternalObjectHandler externalObjectHandler;
    public bool IsLeftInside
    {
        get
        {
            return isLeftInside;
        }
    }

    public bool IsRightInside
    {
        get
        {
            return isRightInside;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("HapticCollider_Right"))
        {
            isRightInside = true;
            externalObjectHandler.OnPopThroughRight();
            //GetComponentInParent<ExternalObjectHandler>().OnPopThroughRight();
        }

        if (other.gameObject.CompareTag("HapticCollider_Left"))
        {

            isLeftInside = true;
            externalObjectHandler.OnPopThroughLeft();
            //GetComponentInParent<ExternalObjectHandler>().OnPopThroughLeft();
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("HapticCollider_Right"))
        {
            isRightInside = true;
            externalObjectHandler.OnPopThroughRight();
            //GetComponentInParent<ExternalObjectHandler>().OnPopThroughRight();
        }

        if (other.gameObject.CompareTag("HapticCollider_Left"))
        {

            isLeftInside = true;
            externalObjectHandler.OnPopThroughLeft();
            //GetComponentInParent<ExternalObjectHandler>().OnPopThroughLeft();
        }
    }
    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.CompareTag("HapticCollider_Right"))
        {

            isRightInside = false;
            externalObjectHandler.OnPopThroughDoneRight();
            //GetComponentInParent<ExternalObjectHandler>().OnPopThroughDoneRight();
        }

        if (other.gameObject.CompareTag("HapticCollider_Left"))
        {

            isLeftInside = false;
            externalObjectHandler.OnPopThroughDoneLeft();
            //GetComponentInParent<ExternalObjectHandler>().OnPopThroughDoneLeft();
        }

    }

}
