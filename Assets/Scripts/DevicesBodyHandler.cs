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

    [SerializeField] Collider triggerCollider;   // assign your trigger collider in Inspector

    [SerializeField] Renderer[] bodyMaterials;
    [SerializeField] GameObject rightTouchDevice;
    [SerializeField] GameObject leftTouchDevice;
    [SerializeField] GameObject platform;
    [SerializeField] Device device;
    [SerializeField] OVRHand handLeft;
    [SerializeField] OVRHand handRight;
    static bool isRightStylusUsed = false;
    static bool isLeftStylusUsed = false;
    static bool isStartedFadingIn = false;
    public int fadeValue = 0;

    //private void OnEnable()
    //{
    //    GameModeManager.Instance.onHapticEnabled += CheckTriggerOnce;
    //}

    private void OnDisable()
    {
        FadeOut();
        //GameModeManager.Instance.onHapticEnabled -= CheckTriggerOnce;
    }

    private void Start()
    {
        isStartedFadingIn = false;
    }

    private void OnEnable()
    {
        CheckTriggerOnce();
        Debug.Log("OnEnable - CheckTriggerOnce");
    }

    public void CheckTriggerOnce()
    {
        if (triggerCollider == null) { Debug.Log("triggerCollider == null"); return; }
        if (!GetComponent<Collider>().bounds.Intersects(triggerCollider.bounds))
        {
            HandleStylusOutside();
        }
        else if (GetComponent<Collider>().bounds.Intersects(triggerCollider.bounds))
        {
            HandleStylusInside();
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (device == Device.RightDevice && other.gameObject.tag == "HapticCollider_Right" && isRightStylusUsed)
        {
            HandleStylusInside();
        }
        else if (device == Device.LeftDevice && other.gameObject.tag == "HapticCollider_Left" && isLeftStylusUsed)
        {
            HandleStylusInside();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (device == Device.RightDevice && other.gameObject.tag == "HapticCollider_Right" && !isRightStylusUsed)
        {
            HandleStylusOutside();
        }
        else if (device == Device.LeftDevice && other.gameObject.tag == "HapticCollider_Left" && !isLeftStylusUsed)
        {
            HandleStylusOutside();
        }
    }

    public void HandleStylusInside()
    {
        if (device == Device.RightDevice)
        {
            isRightStylusUsed = false;
            ToggleDevices(true, true);

            if (!isLeftStylusUsed)
            {
                ToggleBody(false);
                if (platform != null)
                    platform.SetActive(true);
            }
        }
        else if (device == Device.LeftDevice)
        {
            isLeftStylusUsed = false;
            ToggleDevices(true, false);
            if (!isRightStylusUsed)
            {
                ToggleBody(false);
                if (platform != null)
                    platform.SetActive(true);
            }
        }

    }
    public void HandleStylusOutside()
    {
        if (device == Device.RightDevice)
        {

            isRightStylusUsed = true;
            ToggleDevices(false, true);
            if (!isLeftStylusUsed)
            {
                ToggleBody(true);
                if (platform != null)
                    platform.SetActive(false);
            }
        }
        else if (device == Device.LeftDevice)
        {
            isLeftStylusUsed = true;
            ToggleDevices(false, false);
            if (!isRightStylusUsed)
            {
                ToggleBody(true);
                if (platform != null)
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
        while (fadeValue <= 10)
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

