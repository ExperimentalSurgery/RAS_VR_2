using UnityEngine;

public class Deformator : MonoBehaviour
{
    [SerializeField]
    Transform pointerTip;

    [SerializeField]
    Transform pointerTail;

    [SerializeField]
    Transform pointerDirZ;

    [SerializeField]
    Transform surfacePosition;

    [SerializeField]
    Transform depthPosition;

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

    int isInObject;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit rayInfoTip;
        RaycastHit rayInfoTail;

        bool hitTip = Physics.Raycast(new Ray(pointerTip.position, pointerDirZ.forward), out rayInfoTip, 3f, deformLayer);
        bool hitTail = Physics.Raycast(new Ray(pointerTail.position, pointerDirZ.forward), out rayInfoTail, 3f, deformLayer);

        //Check if Tip is actually inside object
        if (!hitTip && hitTail)
        {
            isInObject = 1;
            surfacePosition.position = rayInfoTail.point;
        }

        else 
        {
            isInObject = 0;
            surfacePosition.position = pointerTip.position;
        }

        //Limit the max deform depth
        if (Vector3.Distance(surfacePosition.position, pointerTip.position) > maxDepth)
        {
            Vector3 dir = (pointerTip.position - surfacePosition.position).normalized;
            depthPosition.position = surfacePosition.position + (dir * maxDepth);
        }

        else
        {
            depthPosition.position = pointerTip.position;
        }

        //Get the deformation depth
        float deformDepth = Vector3.Distance(surfacePosition.position, depthPosition.position);


        //Set Deformation Shader values
        deformMeshRenderer.sharedMaterial.SetVector("_SurfaceContactPosition", surfacePosition.position);
        deformMeshRenderer.sharedMaterial.SetVector("_DeformationPosition", depthPosition.position);
        deformMeshRenderer.sharedMaterial.SetFloat("_DeformDepth", deformDepth);
        deformMeshRenderer.sharedMaterial.SetFloat("_MaxDepth", maxDepth);
        deformMeshRenderer.sharedMaterial.SetFloat("_DeformRadius", deformRadius);
        deformMeshRenderer.sharedMaterial.SetFloat("_IsBeingDeformed", isInObject);
    }
}
