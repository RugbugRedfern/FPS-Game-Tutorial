///////////////////////////////////////////////////////////////////////////
//  Simple Jump SMB (Soldier Animations)                                  //
//  Kevin Iglesias - https://www.keviniglesias.com/       			     //
//  Contact Support: support@keviniglesias.com                           //
///////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KevinIglesias {
	public class JumpScriptSMB : StateMachineBehaviour
	{
		private JumpScript jS;
		
		public bool isLand = false;
		
		override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if(jS == null)
			{
				jS = animator.GetComponent<JumpScript>();
			}
			
			if(jS != null)
			{
			
				if(isLand)
				{
					jS.Land();
				}else{
					jS.Jump();
				}
			}
		}
	}
}
