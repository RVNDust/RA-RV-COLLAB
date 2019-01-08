using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ControllerInput : MonoBehaviour
{
    public SteamVR_Input_Sources inputSource;
    private SteamVR_Behaviour_Pose behaviourPose;

    public delegate void OnTriggerPressed(GameObject controller);
    public static event OnTriggerPressed onTriggerPressed;
    public delegate void OnTriggerReleased(GameObject controller);
    public static event OnTriggerReleased onTriggerReleased;

    private GameObject SelectedObject;


    void Awake()
    {
        behaviourPose = GetComponent<SteamVR_Behaviour_Pose>();
        inputSource = behaviourPose.inputSource;
    }

    void Update()
    {
        if (SteamVR_Input._default.inActions.GrabPinch.GetStateDown(inputSource) && SelectedObject)
        {
            GrabSelectedObject(SelectedObject);
        }
        if (SteamVR_Input._default.inActions.GrabPinch.GetStateUp(inputSource))
        {
           UnGrabSelectedObject(SelectedObject);
        }
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<GrabbableObject>())
        {
            SelectedObject= other.gameObject;
        } 
    }

    private void OnTriggerExit(Collider other)
    {
        if (SelectedObject == other.gameObject)
        {
            SelectedObject= null;
        }
    }

    private void GrabSelectedObject(GameObject myGrab)
    {
        if(!gameObject.GetComponent<FixedJoint>()){
            FixedJoint fx = gameObject.AddComponent<FixedJoint>();
            fx.breakForce = 20000;
            fx.breakTorque = 20000;
            fx.connectedBody = myGrab.GetComponent<Rigidbody>();
        }
	}

    private void UnGrabSelectedObject(GameObject myGrab){
        if (gameObject.GetComponent<FixedJoint>()){
            Destroy(gameObject.GetComponent<FixedJoint>());
        }
    }




}
