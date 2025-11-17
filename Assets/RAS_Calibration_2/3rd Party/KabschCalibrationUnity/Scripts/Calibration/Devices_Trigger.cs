using UnityEngine;

public class Devices_Trigger : MonoBehaviour
{
    [SerializeField]
    InstructionsHandler instructionsHandler;
    [SerializeField]
    Device device;

    private void OnTriggerEnter(Collider other)
    {
        if(device == Device.RightDevice && other.gameObject.tag == "HapticCollider_Right")
        {
            instructionsHandler.WasRightStylusActivated = false;
            Debug.Log("Right Stylus Deactivated");
        }
        else if(device == Device.LeftDevice && other.gameObject.tag == "HapticCollider_Left")
        {
            instructionsHandler.WasLeftStylusActivated = false;
            Debug.Log("Left Stylus Deactivated");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(device == Device.RightDevice && other.gameObject.tag == "HapticCollider_Right")
        {
            instructionsHandler.WasRightStylusActivated = true;
            instructionsHandler.DisplayFirstInstrictionForStylus(true);
            Debug.Log("Right Stylus Activated");
        }
        else if(device == Device.LeftDevice && other.gameObject.tag == "HapticCollider_Left")
        {
            instructionsHandler.WasLeftStylusActivated = true;
            instructionsHandler.DisplayFirstInstrictionForStylus(false);
            Debug.Log("Left Stylus Activated");
        }
    }

}
