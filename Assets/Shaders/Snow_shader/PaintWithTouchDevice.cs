using Oculus.Interaction;
using UnityEngine;

public class PaintWithTouchDevice : MonoBehaviour
{
    private HapticPlugin newHapticPlugin;
    private HapticPlugin currentHapticPlugin;
    [SerializeField] private Shader drawShader;
    private RenderTexture splatMap;
    private Material currentMaterial, drawMaterial;
    private RaycastHit hit;
    [SerializeField][Range(1, 500)] private float size;
    [SerializeField][Range(0, 1)] private float strength;

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
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (!collision.gameObject.transform.parent.transform.GetChild(0).GetComponent<HapticPlugin>()) return;
    //    newHapticPlugin = collision.gameObject.transform.parent.transform.GetChild(0).GetComponent<HapticPlugin>();
    //    if (newHapticPlugin == currentHapticPlugin) return;
    //    currentHapticPlugin = newHapticPlugin;
    //}
    private void OnCollisionStay(Collision collision)
    {
        if(currentHapticPlugin == null) return;
      
        RaycastHit hit = new RaycastHit();
        //Ray ray = new Ray((currentHapticPlugin.VisualizationMesh.transform.position), collision.contacts[0].normal);
        Ray ray = new Ray((collision.transform.GetChild(0).position), collision.contacts[0].normal);

        if (Physics.Raycast(ray, out hit))
        {
            drawMaterial.SetVector(name: "_Coordinates", value: new Vector4(hit.textureCoord.x, hit.textureCoord.y, 0, 0));
            drawMaterial.SetFloat(name: "_Strength", strength);
            drawMaterial.SetFloat(name: "_Size", size);
            RenderTexture temp = RenderTexture.GetTemporary(splatMap.width, splatMap.height, 0, RenderTextureFormat.ARGBFloat);
            Graphics.Blit(splatMap, temp);
            Graphics.Blit(temp, splatMap, drawMaterial);
            RenderTexture.ReleaseTemporary(temp);
        }
    }
}
