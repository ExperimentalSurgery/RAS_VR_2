using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeManager : MonoBehaviour
{
    [Header("Material Settings")]
    [SerializeField] Material glowMaterial;
    [SerializeField] Material defaultMat;
    [SerializeField] Renderer rightStylus;
    [SerializeField] Renderer leftStylus;

    [Header("Styluses Settings")]
    [SerializeField] Collider rightStylusCollider;
    [SerializeField] Collider leftStylusCollider;
    [SerializeField] bool isVirtualReality = false;

    [Header("Vibration Settings")]
    [SerializeField] HapticPlugin hapticPluginR;
    [SerializeField] HapticPlugin hapticPluginL;

    private void Start()
    {
        isVirtualReality = false;
        rightStylusCollider.enabled = false;
        leftStylusCollider.enabled = false;
    }
    public void ToggleGameMode(bool isRightStylus)
    {
        isVirtualReality = !isVirtualReality;
        if (isVirtualReality)
        {
            rightStylusCollider.enabled = true;
            leftStylusCollider.enabled = true;
            SwapMaterial(true, isRightStylus);
            StartCoroutine(EnableQuickVibration());

        }
        else
        {
            rightStylusCollider.enabled = false;
            leftStylusCollider.enabled = false;
            SwapMaterial(false, isRightStylus);
        }
    }

    public void SwapMaterial(bool isOn, bool isRightStylus)
    {
        if (isOn)
        {
            rightStylus.material = glowMaterial;
            leftStylus.material = glowMaterial;
        }
        else
        {
            rightStylus.material = defaultMat;
            leftStylus.material = defaultMat;
        }
    }

    IEnumerator EnableQuickVibration()
    {
        hapticPluginR.EnableVibration();
        hapticPluginL.EnableVibration();
        yield return new WaitForSeconds(.5f);
        hapticPluginR.DisableVibration();
        hapticPluginL.DisableVibration();
    }
}
