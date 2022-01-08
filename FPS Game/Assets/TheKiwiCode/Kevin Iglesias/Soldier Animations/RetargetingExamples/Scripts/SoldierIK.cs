///////////////////////////////////////////////////////////////////////////
//  SoldierIK - MonoBehaviour Script				         			 //
//  Kevin Iglesias - https://www.keviniglesias.com/     			     //
//  Contact Support: support@keviniglesias.com                           //
///////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace KevinIglesias {
	
	public class SoldierIK : MonoBehaviour
	{
		
		[HideInInspector]
		public Animator animator;
	
		public Transform leftHandIK;
		
		private float iKWeight = 0;
		private IEnumerator iKCO;
		
		void Awake()
		{
			iKWeight = 0;
			animator = GetComponent<Animator>();
			
			//Check animator
			if(animator == null)
			{
				Debug.LogError("SoldierIK Error: Animator not found on character.");
			}
			
			//Check humanoid avatar animator
			if(!animator.isHuman)
			{
				Debug.Log("SoldierIK Warning: Animator Avatar is not Human. IK may not work properly.");
			}
		}
		
		//Function called by SMB
		public void PerformIK(bool isStart, float speed, int interpolation)
		{
			if(iKCO != null)
            {
                StopCoroutine(iKCO);
            }
			
			switch (interpolation){
				case 0:
					iKCO = LinearIK(isStart, speed);
				break;
				case 1:
					iKCO = EaseOutIK(isStart, speed);
				break;
				case 2:
					iKCO = EaseInIK(isStart, speed);
				break;
				case 3:
					iKCO = SmoothIK(isStart, speed);
				break;
				default:
				return;
				
			}
            StartCoroutine(iKCO);
		}
		
		//IK Weight Linear interpolation (same pace all the time)
		IEnumerator LinearIK(bool isStart, float speed)
        {
			
			float initWeight = iKWeight;
			float endWeight = 1;
			
			if(!isStart)
			{
				endWeight = 0;
			}

            float i = 0;
            while(i < 1)
            {
                i += Time.deltaTime * speed;
                iKWeight = Mathf.Lerp(initWeight, endWeight, i);
                yield return 0;
            }
        }

		//IK Weight Ease Out interpolation (movement starts fast and ends slower)
		IEnumerator EaseOutIK(bool isStart, float speed)
        {
			
			float initWeight = iKWeight;
			float endWeight = 1;
			
			if(!isStart)
			{
				endWeight = 0;
			}

            float i = 0;
            while(i < 1)
            {
                i += Time.deltaTime * speed;
                iKWeight = Mathf.Lerp(initWeight, endWeight, Mathf.Sin(i * Mathf.PI * 0.5f));
                yield return 0;
            }
        }
		
		//IK Weight Ease In interpolation (movement starts slow and ends faster)
		IEnumerator EaseInIK(bool isStart, float speed)
        {
			
			float initWeight = iKWeight;
			float endWeight = 1;
			
			if(!isStart)
			{
				endWeight = 0;
			}

            float i = 0;
            while(i < 1)
            {
                i += Time.deltaTime * speed;
                iKWeight = Mathf.Lerp(initWeight, endWeight, 1f - Mathf.Cos(i * Mathf.PI * 0.5f));
                yield return 0;
            }
        }
		
		//IK Weight Smooth interpolation (slow movement at the start and at the end)
		IEnumerator SmoothIK(bool isStart, float speed)
        {
			
			float initWeight = iKWeight;
			float endWeight = 1;
			
			if(!isStart)
			{
				endWeight = 0;
			}

            float i = 0;
            while(i < 1)
            {
                i += Time.deltaTime * speed;
                iKWeight = Mathf.Lerp(initWeight, endWeight, i*i*i * (i * (6f*i - 15f) + 10f));
                yield return 0;
            }
        }
		

		//Set IK using interpolated weight
        void OnAnimatorIK(int layerIndex)
		{
			animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, iKWeight);
			animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, iKWeight);
			animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandIK.position);  
			animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandIK.rotation);
        }
	}
}
