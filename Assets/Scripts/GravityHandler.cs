using UnityEngine;

public class GravityHandler : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if(other.gameObject.tag == "part")
    //    {
    //        other.GetComponent<PartMovementHandler>().isInZone = true;
    //       // other.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.tag == "Part")
    //    {
    //        Debug.Log("gravity");
    //        other.GetComponent<PartMovementHandler>().isInZone = false;

    //        other.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
    //        other.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX;
    //        other.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationZ;
    //        other.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX;
    //        other.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationY;
    //        other.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationZ;
    //    }
    //}
}
