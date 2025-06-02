using UnityEngine;

public class Deformator : MonoBehaviour
{
    [Header("Pointer 1")]

    [SerializeField]
    Transform pointer1Tip;

    [SerializeField]
    Transform pointer1Tail;

    [SerializeField]
    Transform pointer1DirZ;

    [Header("Pointer 2")]

    [SerializeField]
    Transform pointer2Tip;

    [SerializeField]
    Transform pointer2Tail;

    [SerializeField]
    Transform pointer2DirZ;

    [Header("Deformed Object Settings")]

    [SerializeField]
    MeshRenderer deformMeshRenderer;

    [SerializeField]
    Collider deformMeshCollider;

    [SerializeField]
    LayerMask deformLayer;

    [SerializeField]
    float maxDepth = 0.02f;

    [SerializeField]
    float deformRadius;

    [Header("Debug Visualisation (optional)")]

    [SerializeField]
    bool enableDebugVisualisations;

    [SerializeField]
    GameObject depthVizPrefab;

    [SerializeField]
    GameObject surfaceVizPrefab;

    GameObject surfaceViz1;
    GameObject surfaceViz2;
    GameObject depthViz1;
    GameObject depthViz2;

    private void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        CalculateDeformation(pointer1Tip, pointer1Tail, pointer1DirZ, 1);
        CalculateDeformation(pointer2Tip, pointer2Tail, pointer2DirZ, 2);
    }

    void CalculateDeformation(Transform pointerTip, Transform pointerTail, Transform pointerDirZ, int pointerIndex )
    {
        RaycastHit rayInfoTip;
        RaycastHit rayInfoTail;
        int isInObject;

        Vector3 surfacePos;
        Vector3 depthPos;

        bool hitTip = Physics.Raycast(new Ray(pointerTip.position, pointerDirZ.forward), out rayInfoTip, 3f, deformLayer);
        bool hitTail = Physics.Raycast(new Ray(pointerTail.position, pointerDirZ.forward), out rayInfoTail, 3f, deformLayer);

        //Check if Tip is actually inside object
        if (!hitTip && hitTail)
        {
            isInObject = 1;
            surfacePos = rayInfoTail.point;
        }

        else
        {
            isInObject = 0;
            surfacePos= pointerTip.position;
        }

        //Limit the max deform depth
        if (Vector3.Distance(surfacePos, pointerTip.position) > maxDepth)
        {
            Vector3 dir = (pointerTip.position - surfacePos).normalized;
            depthPos = surfacePos + (dir * maxDepth);
        }

        else
        {
            depthPos = pointerTip.position;
        }

        //Get the deformation depth
        float deformDepth = Vector3.Distance(surfacePos, depthPos);


        //Set the Pointer-Dependent Deformation Shader values
        if(pointerIndex == 1)
        {
            deformMeshRenderer.sharedMaterial.SetVector("_SurfaceContactPosition1", surfacePos);
            deformMeshRenderer.sharedMaterial.SetVector("_DeformationPosition1", depthPos);
            deformMeshRenderer.sharedMaterial.SetFloat("_IsBeingDeformed1", isInObject);
            deformMeshRenderer.sharedMaterial.SetFloat("_DeformDepth1", deformDepth);

        }

        if (pointerIndex == 2)
        {
            deformMeshRenderer.sharedMaterial.SetVector("_SurfaceContactPosition2", surfacePos);
            deformMeshRenderer.sharedMaterial.SetVector("_DeformationPosition2", depthPos);
            deformMeshRenderer.sharedMaterial.SetFloat("_IsBeingDeformed2", isInObject);
            deformMeshRenderer.sharedMaterial.SetFloat("_DeformDepth2", deformDepth);

        }

        //Set the commen Deformation Shader values
        deformMeshRenderer.sharedMaterial.SetFloat("_MaxDepth", maxDepth);
        deformMeshRenderer.sharedMaterial.SetFloat("_DeformRadius", deformRadius);

        //Debug visualisations
        ShowDebugVisualisation(surfacePos, depthPos, pointerIndex);

    }

    void ShowDebugVisualisation(Vector3 surfaceVizPos, Vector3 depthVizPos, int pointerIndex)
    {
        if (enableDebugVisualisations)
        {
            if (surfaceViz1 == null)
                surfaceViz1 = Instantiate(surfaceVizPrefab);
            if (surfaceViz2 == null)
                surfaceViz2 = Instantiate(surfaceVizPrefab);
            if (depthViz1 == null)
                depthViz1 = Instantiate(depthVizPrefab);
            if (depthViz2 == null)
                depthViz2 = Instantiate(depthVizPrefab);

            surfaceViz1.transform.parent = pointer1Tip;
            surfaceViz2.transform.parent = pointer2Tip;
            depthViz1.transform.parent = pointer1Tip;
            depthViz2.transform.parent = pointer2Tip;
        }

        else
        {
            if(surfaceViz1 != null)
                Destroy(surfaceViz1);
            if (surfaceViz2 != null)
                Destroy(surfaceViz2);
            if (depthViz1 != null)
                Destroy(depthViz1);
            if (depthViz2 != null)
                Destroy(depthViz2);
        }

        if(pointerIndex == 1)
        {
            if (surfaceViz1 != null)
                surfaceViz1.transform.position = surfaceVizPos;
            if (depthViz1 != null)
                depthViz1.transform.position = depthVizPos;
        }

        if(pointerIndex == 2)
        {
            if (surfaceViz2 != null)
                surfaceViz2.transform.position = surfaceVizPos;
            if (depthViz2 != null)
                depthViz2.transform.position = depthVizPos;
        }

    }
}
