using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncidentManager : MonoBehaviour {

	[HeaderAttribute( "Fire incident" )]
	public ParticleSystem Fire;
	public GameObject SafetyRoute;

	public void FireIncident()
	{
		Fire.Play();
		SafetyRoute.SetActive(true);
	}

}
