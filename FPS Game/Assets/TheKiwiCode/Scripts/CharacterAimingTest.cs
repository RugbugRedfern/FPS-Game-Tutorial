using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Rendering.PostProcessing;

public class CharacterAimingTest : MonoBehaviour
{
    public float turnSpeed = 20;

    Camera mainCamera;

    public float aimDuration = 0.3f;

    private float zoomDuration = 13.3f;

    private int zoom1 = 40;

    private int zoom2 = 27;

    public float timeBetweenShots = 0f;

    public Rig aimLayer;

    public CinemachineFreeLook vcam;

    public ParticleSystem flame;

    public ParticleSystem smoke;

    public ParticleSystem flashdistriction;

    public ParticleSystem shoot;

    PhotonView PV;

    RayCastWeaponTest weapon;

    public GameObject  deathText;

    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PV = GetComponent<PhotonView>();
        mainCamera = Camera.main;

        weapon = GetComponentInChildren<RayCastWeaponTest>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!PV.IsMine)
        {
            return;
        }
        float yawCamera = mainCamera.transform.eulerAngles.y;
        transform.rotation =
            Quaternion
                .Slerp(transform.rotation,
                Quaternion.Euler(0, yawCamera, 0),
                turnSpeed * Time.fixedDeltaTime);

        OnMouseButtonsPressed();
    }

    public void OnMouseButtonsPressed()
    {
        if (Input.GetMouseButton(0))
        {
            aimLayer.weight += Time.deltaTime / aimDuration;

            if (!Input.GetMouseButton(1))
            {
                while (vcam.m_Lens.FieldOfView < zoom1)
                {
                    vcam.m_Lens.FieldOfView += Time.deltaTime / zoomDuration;
                }
            }
            if (aimLayer.weight >= 0.95f)
            {
                timeBetweenShots += Time.deltaTime;

                if (timeBetweenShots > 0.2f)
                {
                    PlayShootVFX();
                    weapon.StartFiring();
                    timeBetweenShots = 0f;
                }
            }
        }
        else if (Input.GetMouseButton(1))
        {
            aimLayer.weight += Time.deltaTime / aimDuration;
            while (vcam.m_Lens.FieldOfView > zoom2)
            {
                vcam.m_Lens.FieldOfView -= Time.deltaTime / zoomDuration;
            }
        }
        else
        {
            weapon.StopFiring();
            aimLayer.weight -= Time.deltaTime / aimDuration;
            while (vcam.m_Lens.FieldOfView < zoom1)
            {
                vcam.m_Lens.FieldOfView += Time.deltaTime / zoomDuration;
            }
        }
    }

    public void PlayShootVFX()
    {
        flame.Play();
        smoke.Play();
        shoot.Play();
        flashdistriction.Play();
    }

    public void disableFollowCamera() {
        vcam.gameObject.SetActive(false);
    }

    public void enableDeathScreen() {
        PostProcessVolume volume = mainCamera.GetComponent<PostProcessVolume>();
        volume.enabled = true;
        deathText.SetActive(true);
        
    }
}
