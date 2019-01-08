using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Valve.VR;
using Valve.VR.InteractionSystem;

// Make the SteamVR available in multiplayer by deactivate script for UserOther
// Supports version # SteamVR Unity Plugin - v2.0.1
public class VR_CameraRigMultiuser_SteamVR : NetworkBehaviour {
    // Reference to SteamController
    public GameObject SteamVRCamera, SteamVRLeftHand, SteamVRRightHand;
    public GameObject UserOtherLeftHandModel, UserOtherRightHandModel;
    private GameObject goFreeLookCameraRig;

    void Start()
    {
        updateGoFreeLookCameraRig();
        SteamVRactivation();
        if(isLocalPlayer){
            gameObject.tag = "VRLocalPlayer";
            SteamVRCamera.AddComponent<AudioListener>();
        }
    }

    // Deactivate de FreeLookCameraRig since we are uisng the HTC version
    // Execute only in client side
    protected void updateGoFreeLookCameraRig()
    {
        // Client execution ONLY LOCAL
        if (!isLocalPlayer) return;
        goFreeLookCameraRig = null;

        try
        {
            // Get the Camera to set as the follow camera
            goFreeLookCameraRig = transform.Find("/FreeLookCameraRig").gameObject;
            goFreeLookCameraRig.SetActive(false);
        } catch (System.Exception ex)
        {
            Debug.LogWarning("Warning, no goFreeLookCameraRig found\n" + ex);
        }
    }

    // If we are the client who is using the HTC, activate component of SteamVR in the client using it
    // If we are not the client using this specific HTC, deactivate some scripts
    protected void SteamVRactivation()
    {
        // Client exceution for ALL
        if (!isLocalPlayer)
        {
            // Left && right activation if userMe, Deactivate if UserOther
            // Left && Right SteamVR_Rendermodel activationn if USerme, deactivate if UserOther3
            Player player = GetComponent<Player>();
            Destroy(SteamVRLeftHand.GetComponent<Hand>());
            Destroy(SteamVRLeftHand.GetComponent<SteamVR_Behaviour_Pose>());
            Destroy(SteamVRLeftHand.GetComponent<OurTeleport>());

            Destroy(SteamVRRightHand.GetComponent<Hand>());
            Destroy(SteamVRRightHand.GetComponent<SteamVR_Behaviour_Pose>());
            Destroy(SteamVRRightHand.GetComponent<OurTeleport>());

            Destroy(SteamVRCamera.GetComponent<Camera>());
        } else {
            Destroy(UserOtherLeftHandModel);
            Destroy(UserOtherRightHandModel);
        }
    }

    [Command]
     public void CmdSetAuth(NetworkInstanceId objectId, NetworkIdentity player)
     {
         var iObject = NetworkServer.FindLocalObject(objectId);
         var networkIdentity = iObject.GetComponent<NetworkIdentity>();
         var otherOwner = networkIdentity.clientAuthorityOwner;        
 
         if (otherOwner == player.connectionToClient)
         {
             Debug.Log("Authority already available on " + objectId);
             return;
         }else
         {
             if (otherOwner != null)
             {
                 networkIdentity.RemoveClientAuthority(otherOwner);
             }
             networkIdentity.AssignClientAuthority(player.connectionToClient);
             Debug.Log("Authority assigned for " + objectId);
         }        
     }

}
