using System.Collections;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;

public class ParentConstraintHandler : MonoBehaviour
{
    ParentConstraint parentConstraint;
    [SerializeField] float loadingSpeed = 1;
    [SerializeField] HapticDevicesHandler hapticDevicesHandler;
    private void Awake()
    {
        parentConstraint = GetComponent<ParentConstraint>();
    }

    private void Start()
    {
        if (parentConstraint.GetSource(0).sourceTransform.gameObject.activeSelf)
        {
            //StartCoroutine(StartCallibration());
        } 
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Space))
        {
            StartCoroutine(StartCallibration());
        }
    }
    public void Calllibrate()
    {
        StartCoroutine(StartCallibration());
        Debug.Log("Callibrate manually");
    }
    IEnumerator StartCallibration()
    {
      //  hapticDevicesHandler.DeactivateDevices();
        parentConstraint.constraintActive = true;
        yield return new WaitForSeconds(1f);
        parentConstraint.constraintActive = false;
     //   hapticDevicesHandler.ActivateDevices();
    }

    public void ResetSimulation()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        StartCoroutine(StartCallibration());
        Debug.Log("Callibrate after restart");
    }
}
