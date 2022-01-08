using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class CharacterAimingTest : MonoBehaviourPunCallbacks, IDamageable
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

    public GameObject deathText;

    // Jumping
    Rigidbody rb;

    bool grounded;

    float jumpForce = 300f;

    const float maxHealth = 100f;

    float currentHealth = maxHealth;

    public GameObject crosshair;

    public Image healthbarImage;

    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();
        PV = GetComponent<PhotonView>();
    }

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        mainCamera = Camera.main;
        weapon = GetComponentInChildren<RayCastWeaponTest>();

        if (!PV.IsMine)
        { 
            Destroy (rb);
            Destroy (crosshair);
            Destroy(healthbarImage);
        }
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
        Jump();
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            rb.AddForce(transform.up * jumpForce);
        }
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

    public void disableFollowCamera()
    {
        mainCamera.GetComponent<CinemachineBrain>().enabled = false;
    }

    public void enableDeathScreen()
    {
        PostProcessVolume volume = mainCamera.GetComponent<PostProcessVolume>();
        volume.profile.settings[1].active = true;
        deathText.SetActive(true);
    }

    public void SetGroundedState(bool grounded)
    {
        this.grounded = grounded;
    }

    public void TakeDamage(float damage)
    {
        PV.RPC("RPC_TakeDamage", RpcTarget.All, damage);
    }

    [PunRPC]
    void RPC_TakeDamage(float damage)
    {
        if (!PV.IsMine) return;
        print("Take damage... RPC, " + damage);
        currentHealth -= damage;

        healthbarImage.fillAmount = currentHealth / maxHealth;

        if (healthbarImage.fillAmount < 0.3)
        {
            healthbarImage.color = new Color32(255,0,8,255);
        }

        if (currentHealth <= 0)
        {
            gameObject.GetComponent<TargetTest>().getDeath();
        }
    }
}
