using System.Collections;
using System.Collections.Generic;
using UnityEditor.TerrainTools;
using UnityEngine;

public class PaintingQuestForStiffness : MonoBehaviour
{
    [SerializeField] Collider[] paintingColliders;
    bool isDrawnCompletely = false;
    [SerializeField] int amountOfPaintedColliders = 0;

    private void OnEnable()
    {
        ColliderPainter.onPainted += CheckIfDrawingIsComplete;
    }
    void AssignColliders()
    {

    }
    void CheckIfDrawingIsComplete()
    {
        if(paintingColliders.Length > amountOfPaintedColliders+1) {
        amountOfPaintedColliders += 1;
        }
        else
        {
            Debug.Log("DrawingIsComplete");
        }
    }
}
