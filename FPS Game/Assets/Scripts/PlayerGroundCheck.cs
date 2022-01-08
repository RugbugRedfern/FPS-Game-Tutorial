using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundCheck : MonoBehaviour
{
	CharacterAimingTest characterAimingTest;

	void Awake()
	{
		characterAimingTest = GetComponentInParent<CharacterAimingTest>();
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject == characterAimingTest.gameObject)
			return;

		characterAimingTest.SetGroundedState(true);
	}

	void OnTriggerExit(Collider other)
	{
		if(other.gameObject == characterAimingTest.gameObject)
			return;

		characterAimingTest.SetGroundedState(false);
	}

	void OnTriggerStay(Collider other)
	{
		if(other.gameObject == characterAimingTest.gameObject)
			return;

		characterAimingTest.SetGroundedState(true);
	}
}