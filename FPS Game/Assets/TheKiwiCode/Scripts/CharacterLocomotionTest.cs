using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Cinemachine;

public class CharacterLocomotionTest : MonoBehaviour
{
    Animator animator;
    Vector2 input;

    PhotonView PV;

    GameObject parent;

    // Start is called before the first frame update
    void Start()
    {
        parent = gameObject.transform.parent.gameObject;
        PhotonNetwork.AutomaticallySyncScene = true;
        PV = GetComponent<PhotonView>();
        animator = GetComponent<Animator>();

        if(!PV.IsMine) {
           Destroy(parent.GetComponentInChildren<Camera>().gameObject); 
           Destroy(parent.GetComponentInChildren<CinemachineFreeLook>().gameObject); 
        }

    }

    // Update is called once per frame
    void Update()
    {
        
        if(!PV.IsMine)
            return;
        

        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");

        animator.SetFloat("InputX", input.x);
        animator.SetFloat("InputY",input.y);

        

    }
}
