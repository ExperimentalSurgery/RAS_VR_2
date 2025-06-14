﻿// This code contains 3D SYSTEMS Confidential Information and is disclosed to you
// under a form of 3D SYSTEMS software license agreement provided separately to you.
//
// Notice
// 3D SYSTEMS and its licensors retain all intellectual property and
// proprietary rights in and to this software and related documentation and
// any modifications thereto. Any use, reproduction, disclosure, or
// distribution of this software and related documentation without an express
// license agreement from 3D SYSTEMS is strictly prohibited.
//
// ALL 3D SYSTEMS DESIGN SPECIFICATIONS, CODE ARE PROVIDED "AS IS.". 3D SYSTEMS MAKES
// NO WARRANTIES, EXPRESSED, IMPLIED, STATUTORY, OR OTHERWISE WITH RESPECT TO
// THE MATERIALS, AND EXPRESSLY DISCLAIMS ALL IMPLIED WARRANTIES OF NONINFRINGEMENT,
// MERCHANTABILITY, AND FITNESS FOR A PARTICULAR PURPOSE.
//
// Information and code furnished is believed to be accurate and reliable.
// However, 3D SYSTEMS assumes no responsibility for the consequences of use of such
// information or for any infringement of patents or other rights of third parties that may
// result from its use. No license is granted by implication or otherwise under any patent
// or patent rights of 3D SYSTEMS. Details are subject to change without notice.
// This code supersedes and replaces all information previously supplied.
// 3D SYSTEMS products are not authorized for use as critical
// components in life support devices or systems without express written approval of
// 3D SYSTEMS.
//
// Copyright (c) 2021 3D SYSTEMS. All rights reserved.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HapticPainter_3 : MonoBehaviour
{
    
    public HapticPlugin hapticPlugin;
    public int Size = 5;
    
    
    public Color DrawColor;
    public float forceImpact = 0.2f;
    [SerializeField] private Texture2D origTexture;
    [SerializeField] private Texture2D tempTexture;
    [SerializeField] Material material;

    private void Awake()
    {
       
        ClearTexture();
    }
    void Start()
    {
        tempTexture = new Texture2D(origTexture.width, origTexture.height);
        tempTexture.SetPixels(origTexture.GetPixels());
        material.mainTexture = tempTexture;
        tempTexture.Apply();
    }

    public void ClearTexture()
    {
       
        tempTexture.wrapMode = TextureWrapMode.Clamp;
        tempTexture.filterMode = FilterMode.Bilinear;
        material.mainTexture= origTexture;
        origTexture.Apply();
        //Texture2D tex = (Texture2D)gameObject.GetComponent<MeshRenderer>().sharedMaterial.mainTexture;
        //if (tex != null)
        //{

        //    for (int u = 0; u < tex.width; u++)
        //    {
        //        for (int v = 0; v < tex.height; v++)
        //        {
        //            tex.SetPixel(u, v, Color.yellow);
        //        }
        //    }
        //}
    }

    private void OnCollisionStay(Collision collision)
    {
        
            RaycastHit hit = new RaycastHit();
            Ray ray = new Ray((hapticPlugin.VisualizationMesh.transform.position), collision.contacts[0].normal);
            //Debug.DrawRay(hapticPlugin.VisualizationMesh.transform.position - collision.contacts[0].normal, collision.contacts[0].normal / 10, Color.yellow, 2, false);
            if (Physics.Raycast(ray, out hit))
            {
                //Debug.Log(hit.textureCoord);
                Renderer rend = hit.transform.GetComponent<Renderer>();
                MeshCollider meshCollider = hit.collider as MeshCollider;



            Texture2D bumpMap = (Texture2D)gameObject.GetComponent<MeshRenderer>().sharedMaterial.mainTexture;
     

            if (bumpMap != null)
            {
                Vector2 pixelUV = hit.textureCoord;
                pixelUV.x *= bumpMap.width;
                pixelUV.y *= bumpMap.height;


                int strokeSize = Size * (int)(hapticPlugin.CurrentForce.magnitude * 10 * forceImpact);
                bumpMap.Circle((int)pixelUV.x, (int)pixelUV.y, strokeSize, DrawColor);
                bumpMap.Apply();

            }
            else
            {
                Debug.Log("bumpMap " +
                    "is null");
            }
        }
        
    }





    void Update()
    {

        // Update is called once per frame
    }

    private void OnDisable()
    {
        ClearTexture(); 
    }

    private void OnApplicationQuit()
    {
        ClearTexture();
    }
}
