using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TextuePainter : MonoBehaviour
{
    [Range(2f, 512f)]

    [SerializeField] int textureSize = 128;
    [SerializeField] TextureWrapMode textureWrapMode;
    [SerializeField] FilterMode filterMode;
    [SerializeField]  private Texture2D texture;
    [SerializeField] Material material;

    [SerializeField] Camera camera;
    [SerializeField] private Collider colliderObj;
    [SerializeField] Color color = Color.black;
    [SerializeField] int brushSize = 8;

    private void Start()
    {
        //if(texture == null)
        //{
        //    texture = new Texture2D(textureSize, textureSize);
        //}
        //if(texture.width != textureSize)
        //{
        //    texture.Reinitialize(textureSize, textureSize);
        //}

        texture.wrapMode = textureWrapMode;
        texture.filterMode = filterMode;
        material.mainTexture = texture;
        texture.Apply();
    }

    private void Update()
    {

        if(Input.GetMouseButton(0))
        {

            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(colliderObj.Raycast(ray, out hit, 100))
            {
                int rayX = (int)(hit.textureCoord.x * textureSize);
                int rayY = (int)(hit.textureCoord.y * textureSize);
                //texture.SetPixel(rayX, rayY, color);
                DrawCircle(rayX, rayY);
                texture.Apply();
            }
            else
            {
                Debug.Log(" No raycast");
            }
            
        }
    }

    void DrawCircle(int rayX, int rayY)
    {
       for(int y = 0; y < brushSize; y++) {

            for (int x = 0; x < brushSize; x++)
            {
                float x2 = Mathf.Pow(x - brushSize / 2, 2);
                float y2 = Mathf.Pow(y - brushSize / 2, 2);
                float r2 = Mathf.Pow(brushSize / 2 - 0.5f, 2);

                if(x2 + y2 < r2)
                {
                    texture.SetPixel(rayX + x - brushSize / 2, rayY + y - brushSize / 2, color);
                }
            }
        }
    }
}
