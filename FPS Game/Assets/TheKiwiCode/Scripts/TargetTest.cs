using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.UI;
using Photon.Pun;

public class TargetTest : MonoBehaviour
{
    public float health = 100f;

    public Image crosshair;

    public GameObject Rifle;

    CharacterAimingTest characterAiming;

    Animator animator;

    PhotonView PV;

      private void Awake() {
        PV = GetComponentInParent<PhotonView>();
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private IEnumerator wait()
    {
        yield return new WaitForSeconds(1f);
        Die();
    }

    public void TakeDamage(float amount)
    {
        health = health - amount;
        print("Take Damage function");
        if (health <= 101)
        {
            StartCoroutine(wait());
        }
    }

    [PunRPC]
    void Die()
    {
        if(!PV.IsMine) {
            RigBuilder death_rigBuilder = gameObject.GetComponent<RigBuilder>();
            Destroy(death_rigBuilder);
            Destroy(Rifle);
            return;
        }
        
        print("DEATH");
        animator.SetBool("isDeath", true);
        CharacterLocomotionTest locomotion =
            gameObject.GetComponent<CharacterLocomotionTest>();
        CharacterAimingTest aiming =
            gameObject.GetComponent<CharacterAimingTest>();
        RigBuilder rigBuilder = gameObject.GetComponent<RigBuilder>();
        MeshCollider meshCollider = Rifle.GetComponent<MeshCollider>();

        characterAiming = GetComponent<CharacterAimingTest>();
        locomotion.enabled = false;
        aiming.enabled = false;
        rigBuilder.enabled = false;
        crosshair.enabled = false;
        meshCollider.enabled = true;
        characterAiming.disableFollowCamera();

        Rigidbody gameObjectsRigidBody = Rifle.AddComponent<Rigidbody>();

        aiming.enableDeathScreen();
    }

    public void getDeath() {
      PV.RPC("Die", RpcTarget.All);
      StartCoroutine(RestartGame());
    }


    IEnumerator RestartGame() {
        yield return new WaitForSeconds(5f);
        print("Restarting Game...");
    }
}
