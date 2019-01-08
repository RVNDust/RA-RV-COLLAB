using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class OurTeleport : MonoBehaviour {

    public SteamVR_Input_Sources inputSource;
    private SteamVR_Behaviour_Pose behaviourPose;

    void Awake()
    {
        behaviourPose = GetComponent<SteamVR_Behaviour_Pose>();
        inputSource = behaviourPose.inputSource;
    }

    void Update()
    {
        if (SteamVR_Input._default.inActions.Teleport.GetStateDown(inputSource))
        {
            TeleportPressed();
        }
        if (SteamVR_Input._default.inActions.Teleport.GetStateUp(inputSource))
        {
            TeleportReleased();
        }
    }

    private void TeleportPressed()
    {
        ControllerPointer ContPt = gameObject.AddComponent<ControllerPointer>();
        ContPt.RaycastMask = LayerMask.GetMask("TeleportableFloor");
    }

    private void TeleportReleased()
    {
        ControllerPointer ContPt = gameObject.GetComponent<ControllerPointer>();
        if(ContPt.CanTeleport) 
        {
            GameObject.FindGameObjectWithTag("VRLocalPlayer").transform.position = GetComponent<ControllerPointer>().TargetPosition;
        }
        ContPt.DesactivatePointer();
        Destroy(ContPt);
    }

}
