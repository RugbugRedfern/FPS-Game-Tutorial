///////////////////////////////////////////////////////////////////////////
//  Archer Animations - BowLoadScript                                    //
//  Kevin Iglesias - https://www.keviniglesias.com/     			     //
//  Contact Support: support@keviniglesias.com                           //
///////////////////////////////////////////////////////////////////////////

// This script makes the bow animate when pulling arrow, also it makes thes
// arrow look ready to shot when drawing it from the quivers.

// To do the pulling animation, the bow mesh needs a blenshape named 'Load' 
// and the character needs an empty Gameobject in your Unity scene named 
// 'ArrowLoad' as a child, see character dummies hierarchy from the demo 
// scene as example. More information at Documentation PDF file.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KevinIglesias {
	public class BowLoadScript : MonoBehaviour
	{
	   
		public Transform bow;
		public Transform arrowLoad;
		
		//Bow Blendshape
		SkinnedMeshRenderer bowSkinnedMeshRenderer;
		
		//Arrow draw & rotation
		public bool arrowOnHand;
		public Transform arrowToDraw;
		public Transform arrowToShoot; 
	   
		void Awake()
		{
			
			if(bow != null)
			{
				bowSkinnedMeshRenderer = bow.GetComponent<SkinnedMeshRenderer>();
			}
			
			if(arrowToDraw != null)
			{
				arrowToDraw.gameObject.SetActive(false);
			}
			if(arrowToShoot != null)
			{
				arrowToShoot.gameObject.SetActive(false);
			}
		}

		void Update()
		{
			//Bow blendshape animation
				if(bowSkinnedMeshRenderer != null && bow != null && arrowLoad != null)
				{
					float bowWeight = Mathf.InverseLerp(0, -0.7f, arrowLoad.localPosition.z);
					bowSkinnedMeshRenderer.SetBlendShapeWeight(0, bowWeight*100);
				}
			
			//Draw arrow from quiver and rotate it
				if(arrowToDraw != null && arrowToShoot != null && arrowLoad != null)
				{
					if(arrowLoad.localPosition.y == 0.5f)
					{
						if(arrowToDraw != null)
						{
							arrowOnHand = true;
							arrowToDraw.gameObject.SetActive(true);
						}
					}
					
					if(arrowLoad.localPosition.y > 0.5f)
					{
						if(arrowToDraw != null && arrowToShoot != null)
						{
							arrowToDraw.gameObject.SetActive(false);
							arrowToShoot.gameObject.SetActive(true);
						}
					}
					
					if(arrowLoad.localScale.z < 1f)
					{
						if(arrowToShoot != null)
						{
							arrowToShoot.gameObject.SetActive(false);
							arrowOnHand = false;
						}
					}
				}
		}
	}
}
