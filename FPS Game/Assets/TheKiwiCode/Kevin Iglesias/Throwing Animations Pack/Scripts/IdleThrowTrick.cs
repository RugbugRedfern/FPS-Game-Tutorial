///////////////////////////////////////////////////////////////////////////
//  IdleThrowTrick                                                       //
//  Kevin Iglesias - https://www.keviniglesias.com/       			     //
//  Contact Support: support@keviniglesias.com                           //
///////////////////////////////////////////////////////////////////////////

/* This Scripts needs 'ThrowingActionSMB' script as StateMachineBehaviour 
in the character Animator Controller state that throws the prop */

using  System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KevinIglesias {
    
    public class IdleThrowTrick : MonoBehaviour {

        //Prop to move
        public Transform propToSpin;
        //Hand that holds the prop
        public Transform hand;
        
        //Character root (for parenting when prop is thrown)
        private Transform characterRoot;
        
        //Needed for getting the prop back
        private Vector3 zeroPosition;
        private Quaternion zeroRotation;
        
        //Needed for calculate prop trajectory
        private Vector3 startPosition;
        private Quaternion startRotation;
        private Vector3 endPosition;
        private Quaternion endRotation;
        
        //Coroutine that will make the prop move
        IEnumerator spinCO;
    
        public void Start()
        {
            characterRoot = this.transform;
            
            zeroPosition = propToSpin.localPosition;
            zeroRotation = propToSpin.localRotation;
        }
    
        //Function called by 'ThrowingActionSMB' script
        public void SpinProp()
        {
            if(spinCO != null)
            {
                StopCoroutine(spinCO);
            }
            spinCO = StartSpin();
            StartCoroutine(spinCO);
        }
        
        IEnumerator StartSpin()
        {
            //Remove prop from hand
            propToSpin.SetParent(characterRoot);
            
            //Get initial position/rotation
            startPosition = propToSpin.position;
            startRotation = propToSpin.localRotation;
            
            //Set end position (highest point the prop will get)
            endPosition = new Vector3(propToSpin.position.x, propToSpin.position.y+1f, propToSpin.position.z);
            
            //Going up
            float i = 0;
            while(i < 1)
            {
                i += Time.deltaTime * 3f;
                propToSpin.position = Vector3.Lerp(startPosition, endPosition, Mathf.Sin(i * Mathf.PI * 0.5f));
                propToSpin.transform.Rotate(0.0f, 0.0f, -9f, Space.World);
                yield return 0;
            }
            
            startPosition = new Vector3(startPosition.x, startPosition.y-0.11f, startPosition.z);

            //Going down
            i = 0;
            while(i < 0.9f)
            {
                i += Time.deltaTime * 3f;
                propToSpin.position = Vector3.Lerp(endPosition, startPosition, 1f - Mathf.Cos(i * Mathf.PI * 0.5f));
                propToSpin.transform.Rotate(0f, 0.0f, -9f, Space.World);
                yield return 0;
            }
            
            //Back to the hand
            propToSpin.SetParent(hand);
            propToSpin.localPosition = zeroPosition;
            propToSpin.localRotation = zeroRotation;
        }
    }
}