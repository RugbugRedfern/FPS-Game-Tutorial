///////////////////////////////////////////////////////////////////////////
//  Simple Jump (Soldier Animations)                                     //
//  Kevin Iglesias - https://www.keviniglesias.com/       			     //
//  Contact Support: support@keviniglesias.com                           //
///////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KevinIglesias {
	public class JumpScript : MonoBehaviour
	{
		public float jumpHeight = 1;
		public float speed = 3.35f;
		
		private Transform jumper;
		private IEnumerator jumpCO;
		private Vector3 startPosition;
		private Vector3 endPosition;
		
		void Awake()
		{
			jumper = this.transform;
			startPosition = jumper.position;
			jumpCO = StartJump();
		}
		
		
		public void Jump()
		{
			if(jumpCO != null)
            {
                StopCoroutine(jumpCO);
            }
            jumpCO = StartJump();
            StartCoroutine(jumpCO);
		}
		
		public void Land()
		{
			if(jumpCO != null)
            {
                StopCoroutine(jumpCO);
            }
			jumper.position = startPosition;
		}
		
		
		IEnumerator StartJump()
        {

			jumper.position = startPosition;
			
			endPosition = new Vector3(startPosition.x, startPosition.y+jumpHeight, startPosition.z);

            //Going up
            float i = 0;
            while(i < 1)
            {
                i += Time.deltaTime * speed;
                jumper.position = Vector3.Lerp(startPosition, endPosition, Mathf.Sin(i * Mathf.PI * 0.5f));
                yield return 0;
            }
            
            //Going down
            i = 0;
            while(i < 1)
            {
                i += Time.deltaTime * speed;
                jumper.position = Vector3.Lerp(endPosition, startPosition, 1f - Mathf.Cos(i * Mathf.PI * 0.5f));
                yield return 0;
            }
			
			jumper.position = startPosition;
        }

	}
}
