using System.Collections;
using UnityEditor.XR.LegacyInputHelpers;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

public class CameraVRPos : BaseTeleportationInteractable
{
    public TeleportationProvider teleporter;
    public GameObject targObj;
    public GameObject cameraOffset;

    private void Start()
    {
       // StartCoroutine(ResetPos());
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            ForceTeleport();
        }
    }
    public void ForceTeleport()
    {
        CameraOffsetRepositon();
        //TeleportRequest teleportRequest = new TeleportRequest();
        ////teleportRequest.destinationPosition = new Vector3(targObj.transform.position.x, targObj.transform.position.y, targObj.transform.position.z);
        //teleportRequest.destinationPosition.x = targObj.transform.position.x;
        //teleportRequest.destinationPosition.z = targObj.transform.position.z;
        //teleportRequest.destinationRotation = targObj.transform.rotation;
        //teleportRequest.matchOrientation = MatchOrientation.TargetUpAndForward;
        //teleporter.QueueTeleportRequest(teleportRequest);
        //Debug.Log("ForceTeleport");
    }
    public void CameraOffsetRepositon()
    {
        cameraOffset.transform.localPosition = new Vector3(targObj.transform.localPosition.x, cameraOffset.transform.localPosition.y, targObj.transform.localPosition.z);
        cameraOffset.transform.localRotation = targObj.transform.localRotation;
        Debug.Log("CameraOffsetRepositon");
    }
    IEnumerator ResetPos()
    {
        yield return new WaitForSeconds(3);
        ForceTeleport();
    }

}
