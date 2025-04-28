using HapticGUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HapticMaterialsProperties_", menuName = "ScriptableObjects/HapticMaterialsProperties", order = 1)]
public class HapticMaterialsProperties : ScriptableObject
{
    [Slider(0, 1)]
    public float hStiffness = 0.0f;
}
