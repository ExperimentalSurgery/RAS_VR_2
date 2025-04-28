using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VibrationHandler : MonoBehaviour
{
    [Header("Material Settings")]
    [SerializeField] Material glowMaterial;
    [SerializeField] Material defaultMat;
    //Material defaultMat;
    [SerializeField] GameObject trail;

    [Header("Vibration Settings")]
    [SerializeField] bool isVibrationEnabled = false;
    [SerializeField] bool isShortVibrationFeedback = false;
    [SerializeField] float magnitude = 0.7f, frequency = 70f;
    [SerializeField] bool enableHapticlabsFeedback = false;

    [Header("Dynamic Feedback")]
    [SerializeField] Transform startPos;
    [SerializeField] Transform endPos;
    [SerializeField] Transform currentStilusPos;

    HapticPlugin hapticPluginR;
    HapticPlugin hapticPluginL;

    bool isRightVibrationToggled = false;
    bool isLeftVibrationToggled = false;

    private void Start()
    {
        isShortVibrationFeedback = false;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision == null) return;
        if (collision.gameObject.tag == "HapticCollider_Left" || collision.gameObject.tag == "HapticCollider_Right")
        {
            if (collision.gameObject.tag == "HapticCollider_Right")
            {
                hapticPluginR = collision.gameObject.transform.parent.transform.GetChild(0).GetComponent<HapticPlugin>();
                if(isRightVibrationToggled) { hapticPluginR.EnableVibration(); }

                if (isShortVibrationFeedback)
                {
                    StartCoroutine(DisableVibration(hapticPluginR));
                }
                else
                {
                    CalculateDistance(hapticPluginR);
                }

                Renderer renderer = collision.gameObject.GetComponent<Renderer>();
                //defaultMat = renderer.material;
                SwapMaterial(true, renderer);
            }

            else if (collision.gameObject.tag == "HapticCollider_Left")
            {
                hapticPluginL = collision.gameObject.transform.parent.transform.GetChild(0).GetComponent<HapticPlugin>();
                if (isLeftVibrationToggled) { hapticPluginL.EnableVibration(); }

                if (isShortVibrationFeedback)
                {
                    StartCoroutine(DisableVibration(hapticPluginL));
                }
                else
                {
                    CalculateDistance(hapticPluginL);
                }

                Renderer renderer = collision.gameObject.GetComponent<Renderer>();
                //defaultMat = renderer.material;
                SwapMaterial(true, renderer);
            }

            currentStilusPos = collision.transform;
            if (hapticPluginR == null) { Debug.Log("hapticPlugin == null"); }
            ToggleTrail();
            
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "HapticCollider_Left" || collision.gameObject.tag == "HapticCollider_Right")
        {
            HapticPlugin hapticPlugin = collision.gameObject.transform.parent.transform.GetChild(0).GetComponent<HapticPlugin>();
            CalculateDistance(hapticPlugin);
            StartTrack();
            Renderer renderer = collision.gameObject.GetComponent<Renderer>();
            SwapMaterial(true, renderer);
        }
    }


    private void OnCollisionExit(Collision collision)
    {
        if (collision == null) return;
        if ((collision.gameObject.tag == "HapticCollider_Left" || collision.gameObject.tag == "HapticCollider_Right") && !isShortVibrationFeedback)
        {
            //HapticPlugin hapticPlugin = collision.gameObject.transform.parent.transform.GetChild(0).GetComponent<HapticPlugin>();
            if (collision.gameObject.tag == "HapticCollider_Right")
            {
                if (hapticPluginR == null) { Debug.Log("hapticPlugin == null"); }
                hapticPluginR.DisableVibration();
                hapticPluginR = null;
                Renderer renderer = collision.gameObject.GetComponent<Renderer>();
                //Material defaultMat = GetComponent<Renderer>().material;
                SwapMaterial(false, renderer);
                ToggleTrail();
            }

            else if (collision.gameObject.tag == "HapticCollider_Left")
            {
                if (hapticPluginL == null) { Debug.Log("hapticPlugin == null"); }
                hapticPluginL.DisableVibration();
                hapticPluginL = null;
                Renderer renderer = collision.gameObject.GetComponent<Renderer>();
                //Material defaultMat = GetComponent<Renderer>().material;
                SwapMaterial(false, renderer);
                ToggleTrail();
            }
        }

    }

    IEnumerator DisableVibration(HapticPlugin hapticPlugin)
    {
        yield return new WaitForSeconds(.5f);

        hapticPlugin.DisableVibration();
    }

    public void SwapMaterial(bool isOn, Renderer renderer)
    {
        if (isOn)
        {
            renderer.material = glowMaterial;
        }
        else
        {
            renderer.material = defaultMat;
        }
    }

    void ToggleTrail()
    {
        if (trail == null) return;
        trail.SetActive(!trail.activeSelf);
    }
    private void StartTrack()
    {
        //Hapticlabs.StartTrack("Pulse vibration", true);
        //Hapticlabs.StartTrack("QuickTrigger Test", false);
        //Debug.Log("StartTrack");
    }

    void CalculateDistance(HapticPlugin hapticPlugin)
    {
        if (endPos == null || startPos == null) { return; }
        Debug.DrawLine(endPos.position, startPos.position, Color.blue);
        Vector3 projectedPoint = ClosestPointConstrained(endPos.position, startPos.position, currentStilusPos.position);
        Debug.DrawLine(endPos.position, projectedPoint, Color.red);
        float num = Remap(currentStilusPos.position.x, startPos.position.x, endPos.position.x, 0, 1);
        num = Mathf.Clamp(num, 0, 1);

        //Debug.Log("num:" + num);
        hapticPlugin.VibrationGMag = num;
    }

    private void OnDrawGizmos()
    {
        if (endPos == null || startPos == null) { return; }
        Debug.DrawLine(endPos.position, startPos.position, Color.blue);
        Vector3 projectedPoint = ClosestPointConstrained(endPos.position, startPos.position, currentStilusPos.position);
        Debug.DrawLine(endPos.position, projectedPoint, Color.red);
        float num = Remap(currentStilusPos.position.x, startPos.position.x, endPos.position.x, 0, 1);
        num = Mathf.Clamp(num, 0, 1);

        //Debug.Log("num:" + num);

    }

    Vector3 ClosestPointConstrained(Vector3 a, Vector3 b, Vector3 p)
    {
        Vector3 ab = b - a;
        Vector3 ap = p - a;
        Vector3 ar = Vector3.Project(ap, ab);

        if (Vector3.Dot(ab, ar) < 0)
        {
            return a;
        }
        if (ar.sqrMagnitude > ab.sqrMagnitude)
        {
            return b;
        }
        return a + ar;
    }


    public float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    public void TogglleVibrationRight()
    {
        isRightVibrationToggled = !isRightVibrationToggled;
        if (isRightVibrationToggled)
        {
            isVibrationEnabled = true;
            if (hapticPluginR != null)
            {
                hapticPluginR.EnableVibration();
            }
        }
        else
        {
            isVibrationEnabled = false;
            if (hapticPluginR != null)
            {
                hapticPluginR.DisableVibration();
            }
        }   

    }

    public void TogglleVibrationLeft()
    {
        isLeftVibrationToggled = !isLeftVibrationToggled;
        if (isLeftVibrationToggled)
        {
            isVibrationEnabled = true;
            if (hapticPluginL != null)
            {
                hapticPluginL.EnableVibration();
            }
        }
        else
        {
            isVibrationEnabled = false;
            if (hapticPluginL != null)
            {
                hapticPluginL.DisableVibration();
            }
        }
    }

}
