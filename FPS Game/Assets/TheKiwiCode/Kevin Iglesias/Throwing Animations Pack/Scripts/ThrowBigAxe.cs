///////////////////////////////////////////////////////////////////////////
//  ThrowBigAxe                                                          //
//  Kevin Iglesias - https://www.keviniglesias.com/       			     //
//  Contact Support: support@keviniglesias.com                           //
///////////////////////////////////////////////////////////////////////////

/* This Scripts needs 'ThrowingActionSMB' script as StateMachineBehaviour 
in the character Animator Controller state that throws the prop */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KevinIglesias {
    
    public class ThrowBigAxe : MonoBehaviour {

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

            //Set end position (farthest point the prop will get)
            endPosition = new Vector3(propToSpin.position.x-5f, propToSpin.position.y, propToSpin.position.z);
            
            //Going away
            float i = 0;
            while(i < 1f)
            {
                
                i += Time.deltaTime * 1.3f;

                propToSpin.position = Vector3.Lerp(startPosition, endPosition, Mathf.Sin(i * Mathf.PI * 0.5f));
                propToSpin.transform.Rotate(0.0f, -15f, 0.0f, Space.World);
                yield return 0;
            }
            
            //Coming back
            i = 0;
            while(i < 1f)
            {
                i += Time.deltaTime * 1.3f;
                
                propToSpin.position = Vector3.Lerp(endPosition, startPosition, 1f - Mathf.Cos(i * Mathf.PI * 0.5f));
                propToSpin.transform.Rotate(0f, -16f, 0.0f, Space.World);
                
                yield return 0;
            }
            
            //Back to the hand
            propToSpin.SetParent(hand);
            propToSpin.localPosition = zeroPosition;
            propToSpin.localRotation = zeroRotation;
        }
        
    }
}