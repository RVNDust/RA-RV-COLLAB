using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ValveBehaviour : NetworkBehaviour {


	public GameObject particules;
	public GameObject valveToReplace;
	
	public float minAngle = 0;
	public float maxAngle = 720;

	public float CurrentAngle = 0;
	public Vector3 lastEuler;
	void Start()
	{
		lastEuler = gameObject.transform.rotation.eulerAngles;
	}

	//Si la rotation en X est -720, alors changer tag de la valve + faire disparaitre particules.
	void Update()
	{
		float angle = gameObject.transform.rotation.eulerAngles.x - lastEuler.x;
		CurrentAngle += angle;
		lastEuler = gameObject.transform.rotation.eulerAngles;
		//valveRotation();
	}

	[ClientRpc]
	public void RpcRotationComplete()
	{
		
	}



}
