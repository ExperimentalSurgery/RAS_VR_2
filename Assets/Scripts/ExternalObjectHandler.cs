using HapticGUI;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public class ExternalObjectHandler : MonoBehaviour
{
//[SerializeField] Transform internalObject;
     public enum InteractionType
    {
        layerTrigger,
        layerCollider,
        None
    }
    public InteractionType type;
    [SerializeField] LayerTrigger layerTrigger;
    [SerializeField] LayerCollider layerCollider;



    private void OnEnable()
    {
        //layerTrigger = GetComponentInChildren<LayerTrigger>();
    }

    private void Start()
    {
        GetComponent<Rigidbody>().excludeLayers = UnityEngine.LayerMask.GetMask("InternalObject");
    }


    public void OnPopThroughRight()
    {
        if (type ==  InteractionType.layerTrigger && layerTrigger != null)
        {
            if (!layerTrigger.IsRightInside) { return; }
            Debug.Log("PopThroughRight");
            if (layerTrigger.IsLeftInside)
            {
                GetComponent<Rigidbody>().excludeLayers = UnityEngine.LayerMask.GetMask("HapticCollider_Left", "HapticCollider_Right", "InternalObject");
            }
            else
            {
                GetComponent<Rigidbody>().excludeLayers = UnityEngine.LayerMask.GetMask("HapticCollider_Right", "InternalObject");
            }
        }

        else if (type == InteractionType.layerCollider && layerCollider != null)
        {
            if (!layerCollider.IsRightInside) { return; }
            Debug.Log("PopThroughRight");
            if (layerCollider.IsLeftInside)
            {
                GetComponent<Rigidbody>().excludeLayers = UnityEngine.LayerMask.GetMask("HapticCollider_Left", "HapticCollider_Right", "InternalObject");
                layerCollider.GetComponent<Rigidbody>().excludeLayers = UnityEngine.LayerMask.GetMask("HapticCollider_Left", "HapticCollider_Right", "InternalObject", "ExternalObjectCollision");
            }
            else
            {
                GetComponent<Rigidbody>().excludeLayers = UnityEngine.LayerMask.GetMask("HapticCollider_Right", "InternalObject");
                layerCollider.GetComponent<Rigidbody>().excludeLayers = UnityEngine.LayerMask.GetMask("HapticCollider_Right","InternalObject");
            }
        }
       
        
    }
    public void OnPopThroughLeft()
    {
        if (type == InteractionType.layerTrigger && layerTrigger != null)
        {
            if (!layerTrigger.IsLeftInside) { return; }
            Debug.Log("PopThroughLeft");
            if (layerTrigger.IsRightInside)
            {
                GetComponent<Rigidbody>().excludeLayers = UnityEngine.LayerMask.GetMask("HapticCollider_Left", "HapticCollider_Right", "InternalObject");
            }
            else
            {
                GetComponent<Rigidbody>().excludeLayers = UnityEngine.LayerMask.GetMask("HapticCollider_Left", "InternalObject");
            }
        }

        if (type == InteractionType.layerCollider && layerCollider != null)
        {
            if (!layerCollider.IsLeftInside) { return; }
            Debug.Log("PopThroughLeft");
            if (layerCollider.IsRightInside)
            {
                GetComponent<Rigidbody>().excludeLayers = UnityEngine.LayerMask.GetMask("HapticCollider_Left", "HapticCollider_Right", "InternalObject");
                layerCollider.GetComponent<Rigidbody>().excludeLayers = UnityEngine.LayerMask.GetMask("HapticCollider_Left", "HapticCollider_Right", "InternalObject", "ExternalObjectCollision");
            }
            else
            {
                GetComponent<Rigidbody>().excludeLayers = UnityEngine.LayerMask.GetMask("HapticCollider_Left", "InternalObject");
                layerCollider.GetComponent<Rigidbody>().excludeLayers = UnityEngine.LayerMask.GetMask("HapticCollider_Left", "InternalObject");
            }
        }
    }


    public void OnPopThroughDoneRight()
    {
        if (type == InteractionType.layerTrigger && layerTrigger != null)
        {
            Debug.Log("PopThroughDoneRight");
            if (layerTrigger.IsLeftInside)
            {
                GetComponent<Rigidbody>().excludeLayers = UnityEngine.LayerMask.GetMask("HapticCollider_Left", "InternalObject");
            }
            else
            {
                GetComponent<Rigidbody>().excludeLayers = UnityEngine.LayerMask.GetMask("InternalObject");
            }
        }

        if (type == InteractionType.layerCollider && layerCollider != null)
        {
            Debug.Log("PopThroughDoneRight");
            if (layerCollider.IsLeftInside)
            {
                GetComponent<Rigidbody>().excludeLayers = UnityEngine.LayerMask.GetMask("HapticCollider_Left", "InternalObject");
                layerCollider.GetComponent<Rigidbody>().excludeLayers = UnityEngine.LayerMask.GetMask("HapticCollider_Left", "InternalObject", "ExternalObjectCollision");
            }
            else
            {
                GetComponent<Rigidbody>().excludeLayers = UnityEngine.LayerMask.GetMask("InternalObject");
                layerCollider.GetComponent<Rigidbody>().excludeLayers = UnityEngine.LayerMask.GetMask("InternalObject");
            }
        }
    }
       

    public void OnPopThroughDoneLeft()
    {
        if (type == InteractionType.layerTrigger && layerTrigger != null)
        {
            Debug.Log("PopThroughDoneLeft");
            if (layerTrigger.IsRightInside)
            {
                GetComponent<Rigidbody>().excludeLayers = UnityEngine.LayerMask.GetMask("HapticCollider_Right", "InternalObject");
            }
            else
            {
                GetComponent<Rigidbody>().excludeLayers = UnityEngine.LayerMask.GetMask("InternalObject");
            }
        }

        if (type == InteractionType.layerCollider && layerCollider != null)
        {
            Debug.Log("PopThroughDoneLeft");
            if (layerCollider.IsRightInside)
            {
                GetComponent<Rigidbody>().excludeLayers = UnityEngine.LayerMask.GetMask("HapticCollider_Right", "InternalObject");
                layerCollider.GetComponent<Rigidbody>().excludeLayers = UnityEngine.LayerMask.GetMask("HapticCollider_Right", "InternalObject", "ExternalObjectCollision");
            }
            else
            {
                GetComponent<Rigidbody>().excludeLayers = UnityEngine.LayerMask.GetMask("InternalObject");
                layerCollider.GetComponent<Rigidbody>().excludeLayers = UnityEngine.LayerMask.GetMask("InternalObject");
            }
        }


    }


}
