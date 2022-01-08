using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastWeaponTest : MonoBehaviour
{
    public bool isFiring = false;

    public Transform raycastOrigin;

    public Transform raycastDestination;

    public Transform crossHair;

    public ParticleSystem hitEffect;

    public ParticleSystem bloodEffect;

    Ray ray;

    RaycastHit hitInfo;

    public new Camera camera;


    public AudioClip audioClip;

    private void Start()
    {
        
    }

    public void StartFiring()
    {
        isFiring = true;

        AudioSource.PlayClipAtPoint(audioClip,raycastOrigin.transform.position);

        // RaycastHit hit;
        ray.origin = raycastOrigin.transform.position;
        ray.direction =
            (raycastDestination.position - raycastOrigin.position).normalized;
        if (Physics.Raycast(ray, out hitInfo, 100.0f))
        {
            print("I'm looking at " + hitInfo.transform.name);
            Debug.DrawLine(ray.origin, hitInfo.point, Color.red, 1.0f);
            TargetTest target = hitInfo.transform.GetComponent<TargetTest>();

            if (target != null)
            {
                print(target);
                target.TakeDamage(15);
                bloodEffect.transform.position = hitInfo.point;
                bloodEffect.transform.forward = hitInfo.normal;
                bloodEffect.Emit(1);
            }
            else
            {
                hitEffect.transform.position = hitInfo.point;
                hitEffect.transform.forward = hitInfo.normal;
                hitEffect.Emit(1);
            }
        }
        else
            print("I'm looking at nothing!");
    }

    public void StopFiring()
    {
        isFiring = false;
    }
}
