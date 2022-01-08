///////////////////////////////////////////////////////////////////////////
//  Melee Warrior Shield Rotator - MonoBehaviour Script		     //
//  Kevin Iglesias - https://www.keviniglesias.com/     			     //
//  Contact Support: support@keviniglesias.com                           //
///////////////////////////////////////////////////////////////////////////

// This script allows the shield to be displayed in a desired rotation when 
// blocking with it. It requires the 'Retargeters' empty gameobjects in the 
// hierarchy of your character and a child called 'ShieldRetargeter'. 
// More information at Documentation PDF file.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KevinIglesias {
	public class MeleeWarriorShieldRotator : MonoBehaviour
	{
        public Transform retargeter;
		public Transform shield;
		public Transform newShieldRotation;
		
		private Quaternion initRotation;

		void Start()
		{
			initRotation = shield.localRotation;
		}

		void Update()
		{
			shield.localRotation = Quaternion.Lerp(initRotation, newShieldRotation.localRotation, retargeter.localPosition.y);
		}
		
	}
}
