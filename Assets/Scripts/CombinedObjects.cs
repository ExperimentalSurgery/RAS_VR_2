using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombinedObjects : MonoBehaviour
{
    [SerializeField] Transform[] combinedObjects;
    public bool isLeftTouching = false;
    public bool isLRightTouching = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("HapticCollider_Right"))
        {
            isLRightTouching = true;
            foreach (Transform t in combinedObjects)
            { 
             if(this.transform != t)
                {

                }
            }
        }

        if (other.gameObject.CompareTag("HapticCollider_Left"))
        {

            isLeftTouching = true;
            

        }
    }
}
