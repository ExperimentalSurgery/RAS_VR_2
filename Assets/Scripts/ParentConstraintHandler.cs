using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;

public class ParentConstraintHandler : MonoBehaviour
{
    ParentConstraint parentConstraint;
    [SerializeField] float loadingSpeed = 1;
    [SerializeField] HapticDevicesHandler hapticDevicesHandler;
    public static Action<bool> onCalibrated;
    private void Awake()
    {
        parentConstraint = GetComponent<ParentConstraint>();
    }

    private void Start()
    {
        StartCoroutine(StartCalibration(0.5f));
    }
    public void Calibrate()
    {
        StartCoroutine(StartCalibration(0.5f));
        Debug.Log("Calibrate manually");
    }
    IEnumerator StartCalibration(float waitTime)
    {
        //  hapticDevicesHandler.DeactivateDevices();
        onCalibrated?.Invoke(false);
        parentConstraint.constraintActive = true;
        yield return new WaitForSeconds(waitTime);
        parentConstraint.constraintActive = false;
        onCalibrated?.Invoke(true);
        Debug.Log("onCalibrated" + onCalibrated);
        //   hapticDevicesHandler.ActivateDevices();
    }

    //IEnumerator InitiateCalibration()
    //{
    //    parentConstraint.constraintActive = false;
    //    yield return new WaitForSeconds(1f);
    //    onCalibrated?.Invoke(true);
    //    parentConstraint.constraintActive = true;
    //}
    public void ResetSimulation()
    {
        StopAllCoroutines();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Debug.Log("Calibrate after restart");
    }
}
