///////////////////////////////////////////////////////////////////////////
//  ThrowProp                                                            //
//  Kevin Iglesias - https://www.keviniglesias.com/       			     //
//  Contact Support: support@keviniglesias.com                           //
///////////////////////////////////////////////////////////////////////////

/* This Scripts needs 'ThrowingActionSMB' script as StateMachineBehaviour 
in the character Animator Controller state that throws the prop */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KevinIglesias {

    public class ThrowProp : MonoBehaviour {

        //Prop to move
        public Transform propToThrow;
        //Hand that holds the prop
        public Transform hand;

        //Target to throw the prop
        public Transform targetPos;

        //Speed of the prop
        public float speed = 10;
        
        //Maximum arc the prop will make
        public float arcHeight = 1;
        
        //Needed for checking if prop was thrown or not
        public bool launched = false;

        //Character root (for parenting when prop is thrown)
        private Transform characterRoot;
        
        //Different movements for different prop types
        private bool spear;
        private bool tomahawk;
        
        //Needed for calculate prop trajectory
        private Vector3 startPos; 
        private Vector3 zeroPosition;
        private Quaternion zeroRotation;
        private Vector3 nextPos;
        
        void Start() 
        {
            characterRoot = this.transform;
            
            zeroPosition = propToThrow.localPosition;
            zeroRotation = propToThrow.localRotation;
        }
        
        //This will make the prop move when launched
        void Update() 
        {
            //Arc throw facing the target
            if(launched && spear)
            {
                float x0 = startPos.x;
                float x1 = targetPos.position.x;
                float dist = x1 - x0;
                float nextX = Mathf.MoveTowards(propToThrow.position.x, x1, speed * Time.deltaTime);
                float baseY = Mathf.Lerp(startPos.y, targetPos.position.y, (nextX - x0) / dist);
                float arc = arcHeight * (nextX - x0) * (nextX - x1) / (-0.25f * dist * dist);
                Vector3 nextPos = new Vector3(nextX, baseY + arc, propToThrow.position.z);
            
                propToThrow.rotation = LookAt2D(nextPos - propToThrow.position);
                propToThrow.position = nextPos;
     
                float currentDistance = Mathf.Abs(targetPos.position.x - propToThrow.position.x);
                if(currentDistance < 0.5f)
                {
                    launched = false;
                }
            }
            
            //Arc throw rotating forwards
            if(launched && tomahawk)
            {
                float x0 = startPos.x;
                float x1 = targetPos.position.x;
                float dist = x1 - x0;
                float nextX = Mathf.MoveTowards(propToThrow.position.x, x1, speed * Time.deltaTime);
                float baseY = Mathf.Lerp(startPos.y, targetPos.position.y, (nextX - x0) / dist);
                float arc = arcHeight * (nextX - x0) * (nextX - x1) / (-0.25f * dist * dist);
                Vector3 nextPos = new Vector3(nextX, baseY + arc, propToThrow.position.z);
            
                propToThrow.transform.Rotate(19f, 0.0f, 0.0f, Space.Self);
                propToThrow.position = nextPos;
     
                float currentDistance = Mathf.Abs(targetPos.position.x - propToThrow.position.x);
                if(currentDistance < 0.5f)
                {
                    launched = false;
                }
            }
        }
        
        static Quaternion LookAt2D(Vector3 forward) {
            return Quaternion.Euler(0, 0, (Mathf.Atan2(forward.y, forward.x) * Mathf.Rad2Deg)-90f);
        }   
        
        //Function called by 'ThrowingActionSMB' script
        public void ThrowSpear()
        {
            tomahawk = false;
            spear = true;
            startPos = propToThrow.position;
            propToThrow.SetParent(characterRoot);
            launched = true;
        }
        
        //Function called by 'ThrowingActionSMB' script
        public void ThrowTomahawk()
        {
            spear = false;
            tomahawk = true;
            startPos = propToThrow.position;
            propToThrow.SetParent(characterRoot);
            launched = true;
        }
        
        //Function called by 'ThrowingActionSMB' script
        public void RecoverProp()
        {
            launched = false;
            propToThrow.SetParent(hand);
            propToThrow.localPosition = zeroPosition;
            propToThrow.localRotation = zeroRotation;
        }
    }

}