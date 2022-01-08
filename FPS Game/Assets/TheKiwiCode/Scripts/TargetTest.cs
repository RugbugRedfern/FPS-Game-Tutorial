using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Animations.Rigging;
using UnityEngine.UI;
public class TargetTest : MonoBehaviour
{
    public float health = 100f;

    public Image crosshair;

    public GameObject Rifle;

    CharacterAimingTest characterAiming;

    Animator animator;

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
        print("hello");
        if (health <= 0)
        {
            StartCoroutine(wait());
        }
    }

    public void Die()
    {
        animator.SetBool("isDeath", true);
        CharacterLocomotionTest locomotion =
            gameObject.GetComponent<CharacterLocomotionTest>();
        CharacterAimingTest aiming = gameObject.GetComponent<CharacterAimingTest>();
        RigBuilder rigBuilder = gameObject.GetComponent<RigBuilder>();
        MeshCollider meshCollider = Rifle.GetComponent<MeshCollider>();

        characterAiming = GetComponent<CharacterAimingTest>();

        if (
            locomotion != null &&
            aiming != null &&
            rigBuilder != null &&
            characterAiming != null
        )
        {
            locomotion.enabled = false;
            aiming.enabled = false;
            rigBuilder.enabled = false;
            crosshair.enabled = false;
            meshCollider.enabled = true;
            characterAiming.disableFollowCamera();

            Rigidbody gameObjectsRigidBody = Rifle.AddComponent<Rigidbody>();

            aiming.enableDeathScreen();
        }
    }
}
