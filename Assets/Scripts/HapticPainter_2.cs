using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class HapticPainter_2 : MonoBehaviour
{
    //public HapticPlugin hapticPlugin;
    //public int Size = 5;


    //[SerializeField] Texture2D normalMap;
    //public Color DrawColor;
    //public float forceImpact = 0.2f;


    //private void Awake()
    //{
    //    ClearTexture();
    //    normalMap = null;
    //}
    //public void ClearTexture()
    //{
    //    Texture2D tex = (Texture2D)gameObject.GetComponent<MeshRenderer>().sharedMaterial.mainTexture;
    //    //Texture2D normalTex = (Texture2D)gameObject.GetComponent<MeshRenderer>().sharedMaterial.;
    //    //Texture2D bumpMap = (Texture2D)rend.sharedMaterial.mainTexture;
    //    if (tex != null)
    //    {

    //        for (int u = 0; u < tex.width; u++)
    //        {
    //            for (int v = 0; v < tex.height; v++)
    //            {
    //                tex.SetPixel(u, v, new Color(1f, 1f, 1f, 1));
    //               // normalMap.SetPixel(u, v, new Color(0.95f, 0.95f, 1));

    //            }
    //        }

    //        tex.Apply();

    //        // normal map
    //        if (normalMap != null)
    //        {

    //            for (int u = 0; u < normalMap.width; u++)
    //            {
    //                for (int v = 0; v < normalMap.height; v++)
    //                {
    //                    normalMap.SetPixel(u, v, new Color(0.5f, 0.5f, 1));

    //                }
    //            }

    //            normalMap.Apply();
    //        }
    //    }
    //    else
    //    {
    //        Debug.Log("tex == null");
    //    }
    //}

    //private void OnCollisionEnter(Collision collision)
    //{
    //    hapticPlugin = collision.gameObject.transform.parent.transform.GetChild(0).GetComponent<HapticPlugin>();
    //}
    //private void OnCollisionStay(Collision collision)
    //{

    //    RaycastHit hit = new RaycastHit();
    //    Ray ray = new Ray((hapticPlugin.VisualizationMesh.transform.position), collision.contacts[0].normal);
    //    //Debug.DrawRay(hapticPlugin.VisualizationMesh.transform.position - collision.contacts[0].normal, collision.contacts[0].normal / 10, Color.yellow, 2, false);
    //    if (Physics.Raycast(ray, out hit))
    //    {
    //        //Debug.Log(hit.textureCoord);
    //        Renderer rend = hit.transform.GetComponent<Renderer>();
    //        MeshCollider meshCollider = hit.collider as MeshCollider;


    //       // Texture2D texture = new Texture2D(512, 512);
    //       // gameObject.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = texture;

    //        Texture2D tex = (Texture2D)gameObject.GetComponent<MeshRenderer>().sharedMaterial.mainTexture;

    //        //Get normal map
    //        //Fetch the Renderer from the GameObject
    //        //renderer = GetComponent<Renderer>();

    //        //Make sure to enable the Keywords
    //        rend.material.EnableKeyword("_NORMALMAP");

    //        //Set the Normal map using the Texture
    //        //renderer.material.SetTexture("_BumpMap", normalTexture);

    //        // Get the normal map
    //        normalMap = (Texture2D)gameObject.GetComponent<MeshRenderer>().sharedMaterial.GetTexture("_BumpMap");

    //        if (normalMap != null)
    //        {
    //            string[] propertyNames = gameObject.GetComponent<MeshRenderer>().sharedMaterial.GetTexturePropertyNames();
    //            foreach (var name in propertyNames)
    //            {
    //                //Debug.Log("propertyNames: " + name);
    //            }

    //            Vector2 pixelNormalUV = hit.textureCoord;
    //            pixelNormalUV.x *= normalMap.width;
    //            pixelNormalUV.y *= normalMap.height;

    //            int strokeSize = Size * (int)(hapticPlugin.CurrentForce.magnitude * 10 * forceImpact);
    //            normalMap.CircleNormalTex((int)pixelNormalUV.x, (int)pixelNormalUV.y, strokeSize);
    //            normalMap.Apply();
    //        }

    //        if (tex != null)
    //        {
    //            Vector2 pixelUV = hit.textureCoord;
    //            pixelUV.x *= tex.width;
    //            pixelUV.y *= tex.height;


    //            int strokeSize = Size * (int)(hapticPlugin.CurrentForce.magnitude * 10 * forceImpact);
    //            tex.Circle((int)pixelUV.x, (int)pixelUV.y, strokeSize, DrawColor);
    //            tex.Apply();
    //        }
    //        else
    //        {
    //            Debug.Log("bumpMap is null");
    //        }
    //    }

    //}


    //private void OnDisable()
    //{
    //    ClearTexture();
    //}

    //private void OnApplicationQuit()
    //{
    //    ClearTexture();
    //}
}