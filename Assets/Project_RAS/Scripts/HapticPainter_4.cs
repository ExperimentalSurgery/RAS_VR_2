using UnityEngine;

public class PaintManager : MonoBehaviour
{
    public RenderTexture paintRT;
    public Material paintMaterial;
    public Texture2D brushTexture;
    public Color brushColor = Color.red;
    public float brushSize = 0.05f;

    void Start()
    {
        if (paintRT == null)
        {
            paintRT = new RenderTexture(1024, 1024, 0, RenderTextureFormat.ARGB32);
            paintRT.wrapMode = TextureWrapMode.Clamp;
            paintRT.filterMode = FilterMode.Bilinear;
            paintRT.Create();
            GetComponent<MeshRenderer>().material.SetTexture("_PaintTex", paintRT);

        }

        GetComponent<MeshRenderer>().material.SetTexture("_BrushTex", brushTexture);
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            // Raycast to find UV hit
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Vector2 uv = hit.textureCoord;
                Debug.Log("Painting at UV: " + uv);
                // Pass brush data to the shader
                paintMaterial.SetColor("_BrushColor", brushColor);
                paintMaterial.SetVector("_BrushUV", new Vector4(uv.x, uv.y, brushSize, 0));

                // Temporary RT for blitting
                RenderTexture temp = RenderTexture.GetTemporary(paintRT.width, paintRT.height, 0, paintRT.format);

                // Copy old content
                Graphics.Blit(paintRT, temp);

                // Paint new stroke
                Graphics.Blit(temp, paintRT, paintMaterial);

                RenderTexture.ReleaseTemporary(temp);
            }
        }
    }
}