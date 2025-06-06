using Oculus.Interaction.OVR;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;


public enum Device
{
    RightDevice,
    LeftDevice
}
public class DevicesBodyHandler : MonoBehaviour
{
    [SerializeField] Renderer[] bodyMaterials;
    [SerializeField] GameObject rightTouchDevice;
    [SerializeField] GameObject leftTouchDevice;
    [SerializeField] GameObject platform;
    [SerializeField] Device device;
    [SerializeField] OVRHand handLeft;
    [SerializeField] OVRHand handRight;
    //[SerializeField] OVRControllerInHandActiveState OVRControllerInHandActiveState_right;
    //[SerializeField] OVRControllerInHandActiveState OVRControllerInHandActiveState_left;
    static bool isRightStylusUsed = false;
    static bool isLeftStylusUsed = false;
    static bool isStartedFadingIn = false;
    bool isCalibrated = false;
    public int fadeValue = 0;

    private void OnEnable()
    {
        ParentConstraintHandler.onCalibrated += ToggleDeviceBodyHandler;
    }


    private void OnDisable()
    {
        FadeOut();
    }

    private void Start()
    {
        isStartedFadingIn = false;
    }

    void ToggleDeviceBodyHandler(bool isCalibrated)
    {
        this.isCalibrated = isCalibrated;
        Debug.Log("isCalibrated: " + isCalibrated);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!GameModeManager.Instance.isVirtualReality) //isCalibrated
        {
            return;
        }
        if (device == Device.RightDevice && other.gameObject.tag == "HapticCollider_Right" && isRightStylusUsed)
        {
            //OVRControllerInHandActiveState_right.ShowState = OVRInput.InputDeviceShowState.ControllerInHandOrNoHand;
            handRight.m_showState = OVRInput.InputDeviceShowState.ControllerNotInHand;
            isRightStylusUsed = false;
            ToggleDevices(true, true);

            if (!isLeftStylusUsed)
            {
                ToggleBody(false);
                platform.SetActive(true);
            }

        }
        else if (device == Device.LeftDevice && other.gameObject.tag == "HapticCollider_Left" && isLeftStylusUsed)
        {
            //OVRControllerInHandActiveState_left.ShowState = OVRInput.InputDeviceShowState.ControllerInHandOrNoHand;
            handLeft.m_showState = OVRInput.InputDeviceShowState.ControllerNotInHand;
            isLeftStylusUsed = false;
            ToggleDevices(true, false);
            if (!isRightStylusUsed)
            {
                ToggleBody(false);
                platform.SetActive(true);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (!GameModeManager.Instance.isVirtualReality) //isCalibrated
        {
            return;
        }
        if (device == Device.RightDevice && other.gameObject.tag == "HapticCollider_Right" && !isRightStylusUsed)
        {

            //OVRControllerInHandActiveState_right.ShowState = OVRInput.InputDeviceShowState.NoHand;
            //handRight.m_showState = OVRInput.InputDeviceShowState.NoHand;
            isRightStylusUsed = true;
            ToggleDevices(false, true);
            if (!isLeftStylusUsed)
            {
                ToggleBody(true);
                platform.SetActive(false);
            }
        }
        else if (device == Device.LeftDevice && other.gameObject.tag == "HapticCollider_Left" && !isLeftStylusUsed)
        {
            //OVRControllerInHandActiveState_left.ShowState = OVRInput.InputDeviceShowState.NoHand;
            //handLeft.m_showState = OVRInput.InputDeviceShowState.NoHand;
            isLeftStylusUsed = true;
            ToggleDevices(false, false);
            if (!isRightStylusUsed)
            {
                ToggleBody(true);
                platform.SetActive(false);
            }
        }
    }

    void ToggleDevices(bool toActivate, bool isRightDevice)
    {
        if (toActivate)
        {
            if (isRightDevice)
            {
                rightTouchDevice.gameObject.SetActive(true);
                Debug.Log("isRightStylusUsed: " + isRightStylusUsed);
            }
            else
            {
                leftTouchDevice.gameObject.SetActive(true);
                Debug.Log("isLeftStylusUsed: " + isLeftStylusUsed);
            }

        }
        else
        {
            if (isRightDevice)
            {
                rightTouchDevice.gameObject.SetActive(false);
                Debug.Log("isRightStylusUsed: " + isRightStylusUsed);
            }
            else
            {
                leftTouchDevice.gameObject.SetActive(false);
                Debug.Log("isLeftStylusUsed: " + isLeftStylusUsed);
            }
        }

    }


    void ToggleBody(bool toActivate)
    {
        fadeValue = 0;
        if (toActivate)
        {
            StartCoroutine(StartFadingIn());

        }
        else
        {
            FadeOut();
        }
    }

    IEnumerator StartFadingIn()
    {
        if (isStartedFadingIn) yield break;
        isStartedFadingIn = true;
        foreach (var bodyMat in bodyMaterials)
        {
            bodyMat.sharedMaterial.SetFloat("_FadeSize", 1);
        }
        while (fadeValue < 10)
        {
            if (!isRightStylusUsed && !isLeftStylusUsed)
            {
                foreach (var bodyMat in bodyMaterials)
                {
                    bodyMat.sharedMaterial.SetFloat("_FadeStart", 0);
                    bodyMat.sharedMaterial.SetFloat("_FadeSize", 0);
                }
                Debug.Log("!isRightStylusUsed && !isLeftStylusUsed");
                break;
            }
            yield return new WaitForSeconds(0.05f);
            foreach (var bodyMat in bodyMaterials)
            {
                bodyMat.sharedMaterial.SetFloat("_FadeStart", fadeValue);
            }
            fadeValue++;
        }
        isStartedFadingIn = false;
       
    }

    void FadeOut()
    {
        fadeValue = 0;
        foreach (var bodyMat in bodyMaterials)
        {    
            bodyMat.sharedMaterial.SetFloat("_FadeStart", 0);
            bodyMat.sharedMaterial.SetFloat("_FadeSize", 0);
        }
    }
}

