using UnityEngine;

public class PaintWithMouse : MonoBehaviour
{

    [SerializeField] private Camera cam;
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


    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit))
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
}
