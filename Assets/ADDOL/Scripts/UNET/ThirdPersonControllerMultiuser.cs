﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.Cameras;
using UnityStandardAssets.Characters.ThirdPerson;

public class ThirdPersonControllerMultiuser : NetworkBehaviour
{
    /// <summary>
    /// The FreeLookCameraRig GameObject to configure for the UserMe
    /// </summary>
    GameObject goFreeLookCameraRig = null;



    // Use this for initialization
    void Start()
    {
        Debug.Log("isLocalPlayer:" + isLocalPlayer);
        updateGoFreeLookCameraRig();
        followLocalPlayer();
        activateLocalPlayer();
    }

    /// <summary>
    /// Get the GameObject of the CameraRig
    /// </summary>
    protected void updateGoFreeLookCameraRig()
    {
        try
        {
            // Get the Camera to set as the followed camera
            goFreeLookCameraRig = transform.Find("/FreeLookCameraRig").gameObject;
        }
        catch (System.Exception ex)
        {
            Debug.LogWarning("Warning, no goFreeLookCameraRig found\n" + ex);
        }
    }

    /// <summary>
    /// Make the CameraRig following the LocalPlayer only.
    /// </summary>
    protected void followLocalPlayer()
    {
        if (isLocalPlayer)
        {
            if (goFreeLookCameraRig != null)
            {
                // find Avatar EthanHips
                Transform transformFollow = transform.Find("EthanSkeleton/EthanHips") != null ? transform.Find("EthanSkeleton/EthanHips") : transform;
                // call the SetTarget on the FreeLookCam attached to the FreeLookCameraRig
                goFreeLookCameraRig.GetComponent<FreeLookCam>().SetTarget(transformFollow);
                Debug.Log("ThirdPersonControllerMultiuser follow:" + transformFollow);
            }
        }
    }

    protected void activateLocalPlayer()
    {
        // enable the ThirdPersonUserControl if it is a Loacl player = UserMe
        // disable the ThirdPersonUserControl if it is not a Loacl player = UserOther
        GetComponent<ThirdPersonUserControl>().enabled = isLocalPlayer;
        if (isLocalPlayer)
        {
            try
            {
                // Change the material of the Ethan Glasses
                //GameObjectLocalPlayerColor.GetComponent<Renderer>().material = PlayerLocalMat;
            }
            catch (System.Exception)
            {

            }
        }
    }




    // Update is called once per frame
    void Update()
    {
        // Don't do anything if we are not the UserMe isLocalPlayer
        if (!isLocalPlayer) return;

        if (Input.GetButtonUp("Fire3"))
        {
            CmdStartFire();
        }
    }

    [Command]
    void CmdStartFire()
    {
        RpcStartFire();
    }

    [ClientRpc]
    void RpcStartFire()
    {
        GameObject.Find("DangerZone").GetComponent<IncidentManager>().FireIncident();
        Debug.Log("Fire event triggered! Evacuate!");
    }
}
