// This code contains 3D SYSTEMS Confidential Information and is disclosed to you
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
using TMPro;

public class SceneControl_2 : MonoBehaviour
{
    // Start is called before the first frame update
    public HapticPlugin ActiveHPlugin;
   // public HapticPlugin[] HPlugins = null;
    private int ActiveStage = 0;
    private float dbAS;
    //public GameObject DeviceInfo;
   // public GameObject Device1;
   // public GameObject Device2;
    //public GameObject[] StageBorders;
    [SerializeField] Vector3 startPos;
    [SerializeField] Transform[] sceneObjects;
    [SerializeField] Vector3[] sceneObjectsStartPos;
    [SerializeField] Quaternion[] sceneObjectsStartRot;



    private void UpdateKeys()
    {
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {

            CenterStage();

        }

    }

    public void ActivateMovement()
    {
        if (!ActiveHPlugin.isNavTranslation())
        {
            ActiveHPlugin.EnableNavTranslation();
            ActiveHPlugin.DisableNavRotation();

        }
        else
        {
            ActiveHPlugin.DisableNavTranslation();
        }
    }

    public void ActivateRotation()
    {
        if (!ActiveHPlugin.isNavRotation())
        {
            ActiveHPlugin.EnableNavRotation();
            ActiveHPlugin.DisableNavTranslation();
        }
        else
        {
            ActiveHPlugin.DisableNavRotation();
        }
    }
    public void CenterStage()
    {
        // HPlugin.transform.SetPositionAndRotation(new Vector3(-0.7f * GetActiveStage(), Camera.main.transform.position.y, Camera.main.transform.position.z), HPlugin.transform.rotation); //startPos
        ActiveHPlugin.gameObject.transform.parent.gameObject.SetActive(false);
        ActiveHPlugin.transform.SetPositionAndRotation(startPos, Quaternion.identity); //startPos
        ResetObjPos();
        StartCoroutine(StartReset());

    }

    IEnumerator StartReset()
    {
        yield return new WaitForSeconds(1);
        ActiveHPlugin.gameObject.transform.parent.gameObject.SetActive(true);
    }
    void ResetObjPos()
    {
        if (sceneObjects == null) return;
        for (int i = 0; i < sceneObjects.Length; i++)
        {
            sceneObjects[i].position = sceneObjectsStartPos[i];
            sceneObjects[i].rotation = sceneObjectsStartRot[i];

        }
    }

    void SaveStartObjPos()
    {
        if (sceneObjects == null) return;
        sceneObjectsStartPos = new Vector3[sceneObjects.Length];
        sceneObjectsStartRot = new Quaternion[sceneObjects.Length];
        for (int i = 0; i < sceneObjects.Length; i++)
        {
            sceneObjectsStartPos[i] = sceneObjects[i].position;
            sceneObjectsStartRot[i] = sceneObjects[i].rotation;
        }
    }
   

    private void Awake()
    {

       // SelectActiveHPlugin();
    }
    private void Start()
    {
        ActivateMovement();
        SaveStartObjPos();
    }



    // Update is called once per frame
    private void Update()
    {

       // UpdateKeys();
        //UpdateStageBorder();










    }

    private void LateUpdate()
    {


    }
}
