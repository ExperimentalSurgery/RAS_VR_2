using Oculus.Interaction;
using UnityEngine;

public class PaintWithTouchDevice : MonoBehaviour
{
    private HapticPlugin newHapticPlugin;
    [SerializeField] private HapticPlugin currentHapticPlugin;
    [SerializeField] private Shader drawShader;
    private RenderTexture splatMap;
    private Material currentMaterial, drawMaterial;
    private RaycastHit hit;
    [SerializeField][Range(1, 500)] private float size;
    [SerializeField][Range(0, 1)] private float strength;
    [SerializeField] private Transform tipTransform;

    void Start()
    {
        drawMaterial = new Material(drawShader);
        drawMaterial.SetVector(name: "_Color", (Vector4)Color.red);
        currentMaterial = GetComponent<MeshRenderer>().material;
        splatMap = new RenderTexture(width: 4096, height: 4096, depth: 0, RenderTextureFormat.ARGBFloat); currentMaterial.SetTexture(name: "_SplatMap", splatMap);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.transform.parent.transform.GetChild(0).GetComponent<HapticPlugin>()) return;
        newHapticPlugin = other.gameObject.transform.parent.transform.GetChild(0).GetComponent<HapticPlugin>();
        if (newHapticPlugin == currentHapticPlugin) return;
        currentHapticPlugin = newHapticPlugin;
        tipTransform = other.transform.GetChild(0);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.transform.parent.transform.GetChild(0).GetComponent<HapticPlugin>() != currentHapticPlugin) return;
        CalculatePaintedPoint();
        Debug.Log("OnTriggerStay" + other.name);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.transform.parent.transform.GetChild(0).GetComponent<HapticPlugin>() != currentHapticPlugin) return;
        currentHapticPlugin = null;
        tipTransform = null;
    }

    void CalculatePaintedPoint()
    {
        if (currentHapticPlugin == null || tipTransform == null)
        {
            tipTransform = null;
            return;
        }

        RaycastHit rayInfoTip = new RaycastHit();
        Vector3 surfacePos = new Vector3();
        // Shoot a ray from the brush tip forward
        Ray ray = new Ray(tipTransform.position, tipTransform.forward *-1);
        Debug.DrawRay(ray.origin, ray.direction * 0.1f, Color.red, 1.0f);
        bool hitTip = Physics.Raycast(ray, out rayInfoTip);

        if (hitTip)
        {
            surfacePos += tipTransform.position;
            Debug.Log("Hit at texture coordinates: " + rayInfoTip.textureCoord);
            drawMaterial.SetVector(name: "_Coordinates", value: new Vector4(rayInfoTip.textureCoord.x, rayInfoTip.textureCoord.y, 0, 0));
            drawMaterial.SetFloat(name: "_Strength", strength);
            drawMaterial.SetFloat(name: "_Size", size);
            RenderTexture temp = RenderTexture.GetTemporary(splatMap.width, splatMap.height, 0, RenderTextureFormat.ARGBFloat);
            Graphics.Blit(splatMap, temp);
            Graphics.Blit(temp, splatMap, drawMaterial);
            RenderTexture.ReleaseTemporary(temp);
        }
        else
        {
            Debug.LogWarning($"No UVs available" + 
                             "Ensure a MeshCollider with UVs, or enable Read/Write for fallback.");
        }
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (!collision.gameObject.transform.parent.transform.GetChild(0).GetComponent<HapticPlugin>()) return;
    //    newHapticPlugin = collision.gameObject.transform.parent.transform.GetChild(0).GetComponent<HapticPlugin>();
    //    if (newHapticPlugin == currentHapticPlugin) return;
    //    currentHapticPlugin = newHapticPlugin;
    //    tipTransform = collision.transform.GetChild(0);
    //}
    //private void OnCollisionStay(Collision collision)
    //{
    //    CalculatePaintedPoint();
    //    if (currentHapticPlugin == null) return;

    //    RaycastHit hit = new RaycastHit();
    //    //Ray ray = new Ray((currentHapticPlugin.VisualizationMesh.transform.position), collision.contacts[0].normal);
    //    Ray ray = new Ray((collision.transform.GetChild(0).position), collision.GetContact(0).normal);

    //    if (Physics.Raycast(ray, out hit))
    //    {
    //        Debug.Log("Hit at texture coordinates: " + hit.textureCoord);
    //        drawMaterial.SetVector(name: "_Coordinates", value: new Vector4(hit.textureCoord.x, hit.textureCoord.y, 0, 0));
    //        drawMaterial.SetFloat(name: "_Strength", strength);
    //        drawMaterial.SetFloat(name: "_Size", size);
    //        RenderTexture temp = RenderTexture.GetTemporary(splatMap.width, splatMap.height, 0, RenderTextureFormat.ARGBFloat);
    //        Graphics.Blit(splatMap, temp);
    //        Graphics.Blit(temp, splatMap, drawMaterial);
    //        RenderTexture.ReleaseTemporary(temp);
    //    }
    //}
}
