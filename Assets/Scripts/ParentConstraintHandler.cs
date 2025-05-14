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

       // StartCoroutine(InitiateCalibration());
        //if (parentConstraint.GetSource(0).sourceTransform.gameObject.activeSelf)
        //{
        //    StartCoroutine(StartCalibration());
        //} 
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Space))
        {
           // StartCoroutine(StartCalibration());
        }
    }
    public void Calibrate()
    {
        StartCoroutine(StartCalibration());
        Debug.Log("Calibrate manually");
    }
    IEnumerator StartCalibration()
    {
        //  hapticDevicesHandler.DeactivateDevices();
        onCalibrated?.Invoke(false);
        parentConstraint.constraintActive = true;
        yield return new WaitForSeconds(0.5f);
        parentConstraint.constraintActive = false;
        onCalibrated?.Invoke(true);
        //   hapticDevicesHandler.ActivateDevices();
    }

    IEnumerator InitiateCalibration()
    {
        parentConstraint.constraintActive = false;
        yield return new WaitForSeconds(1f);
        onCalibrated?.Invoke(true);
        parentConstraint.constraintActive = true;
    }
    public void ResetSimulation()
    {
        StopAllCoroutines();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        StartCoroutine(StartCalibration());
        Debug.Log("Calibrate after restart");
    }
}
