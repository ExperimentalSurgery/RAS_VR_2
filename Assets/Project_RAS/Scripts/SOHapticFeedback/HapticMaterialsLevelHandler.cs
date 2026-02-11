using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
struct HapticMaterialLevel
{
    public HapticMaterialsProperties hapticMaterialsLevelProperties;
    public Transform transformLevel;
}
public class HapticMaterialsLevelHandler : MonoBehaviour
{
    [Header("Haptic Levels")]
    [SerializeField] HapticMaterialLevel[] hapticMaterialsLevels;
    [SerializeField] HapticMaterialLevel currentStartLevel;
    [SerializeField] HapticMaterialLevel currentEndLevel;

    [Header("Dynamic Feedback")]
    [SerializeField] bool arePropertiesInterpolated;
    [SerializeField] Transform currentStartPos;
    [SerializeField] Transform currentEndPos;
    [SerializeField] Transform currentStilusPos;

    [SerializeField] float currentStiffness = 0;
    HapticMaterial hapticMaterial;
    private void Awake()
    {
        hapticMaterial = GetComponent<HapticMaterial>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision == null) return;
        if (collision.gameObject.tag == "HapticCollider_Left" || collision.gameObject.tag == "HapticCollider_Right")
        {
            currentStilusPos = collision.transform;
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision == null) return;
        if (collision.gameObject.tag == "HapticCollider_Left" || collision.gameObject.tag == "HapticCollider_Right")
        {
            if(arePropertiesInterpolated) { CalculateInterpolatedHapticProperties(); }
            else { CalculateHapticProperties(); }
           
        }
    }
    private void OnDrawGizmosSelected()
    {
        if (currentEndPos == null || currentStartPos == null) { return; }

        if (Vector3.Distance(currentStartPos.position, currentStilusPos.position) > Vector3.Distance(currentStartPos.position, currentEndPos.position))
        {
            currentEndPos = hapticMaterialsLevels[2].transformLevel;
            currentStartPos = hapticMaterialsLevels[1].transformLevel;
            currentStartLevel = hapticMaterialsLevels[1];
            currentEndLevel = hapticMaterialsLevels[2];
        }
        else
        {
            currentStartPos = hapticMaterialsLevels[0].transformLevel;
            currentEndPos = hapticMaterialsLevels[1].transformLevel;
            currentStartLevel = hapticMaterialsLevels[0];
            currentEndLevel = hapticMaterialsLevels[1];
        }


        Debug.DrawLine(currentEndPos.position, currentStartPos.position, Color.blue);
        Vector3 projectedPoint = ClosestPointConstrained(currentEndPos.position, currentStartPos.position, currentStilusPos.position);
        Debug.DrawLine(currentEndPos.position, projectedPoint, Color.red);
        float currentPos = Remap(currentStilusPos.position.x, currentStartLevel.transformLevel.position.x, currentEndLevel.transformLevel.position.x, 0, 1);
        currentPos = Mathf.Clamp(currentPos, 0, 1);
        currentStiffness = Mathf.Lerp(currentStartLevel.hapticMaterialsLevelProperties.hStiffness, currentEndLevel.hapticMaterialsLevelProperties.hStiffness, currentPos);
        //Debug.Log(testStif);
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

    void CalculateInterpolatedHapticProperties()
    {
        if (currentEndPos == null || currentStartPos == null) { return; }

        if (Vector3.Distance(currentStartPos.position, currentStilusPos.position) > Vector3.Distance(currentStartPos.position, currentEndPos.position))
        {
            currentEndPos = hapticMaterialsLevels[2].transformLevel;
            currentStartPos = hapticMaterialsLevels[1].transformLevel;
            currentStartLevel = hapticMaterialsLevels[1];
            currentEndLevel = hapticMaterialsLevels[2];
        }
        else
        {
            currentStartPos = hapticMaterialsLevels[0].transformLevel;
            currentEndPos = hapticMaterialsLevels[1].transformLevel;
            currentStartLevel = hapticMaterialsLevels[0];
            currentEndLevel = hapticMaterialsLevels[1];
        }


        Debug.DrawLine(currentEndPos.position, currentStartPos.position, Color.blue);
        Vector3 projectedPoint = ClosestPointConstrained(currentEndPos.position, currentStartPos.position, currentStilusPos.position);
        Debug.DrawLine(currentEndPos.position, projectedPoint, Color.red);
        float currentPos = Remap(currentStilusPos.position.x, currentStartLevel.transformLevel.position.x, currentEndLevel.transformLevel.position.x, 0, 1);
        currentPos = Mathf.Clamp(currentPos, 0, 1);
        currentStiffness = Mathf.Lerp(currentStartLevel.hapticMaterialsLevelProperties.hStiffness, currentEndLevel.hapticMaterialsLevelProperties.hStiffness, currentPos);

        hapticMaterial.hStiffness = currentStiffness;
    }

    void CalculateHapticProperties()
    {
        if (currentEndPos == null || currentStartPos == null) { return; }

        if (Vector3.Distance(currentStartPos.position, currentStilusPos.position) > Vector3.Distance(currentStartPos.position, currentEndPos.position))
        {
            currentEndPos = hapticMaterialsLevels[2].transformLevel;
            currentStartPos = hapticMaterialsLevels[1].transformLevel;
            currentStartLevel = hapticMaterialsLevels[1];
            currentEndLevel = hapticMaterialsLevels[2];
        }
        else
        {
            currentStartPos = hapticMaterialsLevels[0].transformLevel;
            currentEndPos = hapticMaterialsLevels[1].transformLevel;
            currentStartLevel = hapticMaterialsLevels[0];
            currentEndLevel = hapticMaterialsLevels[1];
        }


        Debug.DrawLine(currentEndPos.position, currentStartPos.position, Color.blue);
        Vector3 projectedPoint = ClosestPointConstrained(currentEndPos.position, currentStartPos.position, currentStilusPos.position);
        Debug.DrawLine(currentEndPos.position, projectedPoint, Color.red);
        float currentPos = Remap(currentStilusPos.position.x, currentStartLevel.transformLevel.position.x, currentEndLevel.transformLevel.position.x, 0, 1);
        currentPos = Mathf.Clamp(currentPos, 0, 1);
        currentStiffness = currentPos < 0.5? currentStartLevel.hapticMaterialsLevelProperties.hStiffness : currentEndLevel.hapticMaterialsLevelProperties.hStiffness;

        hapticMaterial.hStiffness = currentStiffness;
    }
}
