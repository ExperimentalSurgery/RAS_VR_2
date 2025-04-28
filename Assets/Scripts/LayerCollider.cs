using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class LayerCollider : MonoBehaviour
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

   

    private void OnCollisionEnter(Collision other)
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
    private void OnCollisionStay(Collision other)
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
    private void OnCollisionExit(Collision other)
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
