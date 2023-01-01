using UnityEngine;
using System.Collections;
using TMPro;
using Unity.Netcode;

[RequireComponent(typeof(AudioSource))]
public class GunScript : NetworkBehaviour
{
    public float damage = 10f;
    public float fireRate = 15f;
    public bool Automatic = false;
    public int magCapacity = 30;
    public int magCounter = 30;
    public int ammo;
    public float timeReload = 1f;
    private bool isReloading = false;
    public Animator animator;
    public int classSelected = 5;
    public bool FMJ;

    public PlayerMovement player;
    private const string PlayerTag = "Player";
    public float walkPaceSet = 10f;
    public float dropForceSet = -20f;
    public float thrustPowerSet = 3f;
    public TextMeshProUGUI currentMag;
    public TextMeshProUGUI ammoLeft;

    public Camera mainCamera;
    public ParticleSystem muzzleFlash;
    public GameObject objHit;
    public GameObject AR;
    public GameObject SMG;
    public GameObject Snotgun;
    public GameObject DotCH;
    public GameObject CrossCH;
    public GameObject PlusCH;
    public GameObject dotOverlay;
    public GameObject crossOverlay;
    public GameObject plusOverlay;
    public GameObject AROverlay;
    public GameObject SMGOverlay;
    public GameObject SnotGunOverlay;
    public GameObject ARCharacter;
    public GameObject SMGCharacter;
    public GameObject SnotGunCharacter;

    public LayerMask fmjMask;
    public LayerMask normalMask;
    public AudioSource audioSource;
    public AudioSource reloadSource;
    public AudioClip shootSound;
    public AudioClip equipSound;
    public AudioClip reloadSound;
    private float FireInterval;
    [SerializeField]
    private float shotVolume = 1f;

    private void OnEnable()
    {
        isReloading = false;
        animator.SetBool("CurrentlyReloading", false);
        animator.SetBool("Healing", false);
        FireInterval = Time.time + 1f / fireRate;
        audioSource.PlayOneShot(equipSound, 0.5f);
        player.walkPace = walkPaceSet;
        player.dropForce = dropForceSet;
        player.thrustPower = thrustPowerSet;
    }

    void Update()
    {
            currentMag.text = magCounter + "";
            ammoLeft.text = ammo + "";

            if (Time.time >= FireInterval)
            {
                animator.SetInteger("fire", -1);
            }
            if (isReloading == true)
            {
                return;
            }
            if (magCounter <= 0)
            {
                StartCoroutine(Reload());
                return;
            }
            if (Automatic == true)
            {
                if (Input.GetButton("Fire1") && Time.time >= FireInterval)
                {
                    animator.SetInteger("fire", 2);
                    FireInterval = Time.time + 1f / fireRate;
                    ShootClientRpc();
                }
            }
            if (Automatic == false)
            {
                if (Input.GetButtonDown("Fire1") && Time.time >= FireInterval)
                {
                    animator.SetInteger("fire", 2);
                    FireInterval = Time.time + 1f / fireRate;
                    ShootClientRpc();
                }
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                StartCoroutine(Reload());
                return;
            }
    }
    IEnumerator Reload()
    {
        reloadSource.PlayOneShot(reloadSound, 0.4f);
        isReloading = true;

        Debug.Log("Reloading");

        animator.SetBool("CurrentlyReloading", true);

        yield return new WaitForSeconds(timeReload);

        animator.SetBool("CurrentlyReloading", false);

        if (ammo >= 1)
        {
            if (ammo >= (magCapacity - magCounter))
            {
                ammo -= (magCapacity - magCounter);
                magCounter = magCapacity;
            }
            else
            {
                magCounter += ammo;
                ammo = 0;

            }
        }
        else
        {
            animator.Play("GunReload");
        }

        isReloading = false;
    }

    [ClientRpc]
    void ShootClientRpc() 
    {
        audioSource.PlayOneShot(shootSound, shotVolume);
        muzzleFlash.Play();
        magCounter--;
        RaycastHit hit;
        if (FMJ == true)
        {
            if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, (Mathf.Infinity), ~fmjMask))
            {
                if(hit.collider.tag == PlayerTag)
                {
                    PlayerHitServerRpc(hit.collider.name, damage);
                }

                //takeDamage target = hit.transform.GetComponent<takeDamage>();
                //if (target != null)
                //{
                //    target.TakeDamage(damage);
                //}

                //bullet contact on animation
                GameObject hitobj = Instantiate(objHit, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(hitobj, 2f);


                EquipAR(hit.transform.name);
                EquipSMG(hit.transform.name);
                EquipSnotgun(hit.transform.name);
                EquipDotCH(hit.transform.name);
                EquipCrossCH(hit.transform.name);
                EquipPlusCH(hit.transform.name);
            }
        }
        else if (FMJ == false)
        {
            if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, (Mathf.Infinity), ~normalMask))
            {
                takeDamage target = hit.transform.GetComponent<takeDamage>();
                if (target != null)
                {
                    target.TakeDamage(damage);
                }

                //bullet contact on animation
                GameObject hitobj = Instantiate(objHit, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(hitobj, 2f);


                EquipAR(hit.transform.name);
                EquipSMG(hit.transform.name);
                EquipSnotgun(hit.transform.name);
                EquipDotCH(hit.transform.name);
                EquipCrossCH(hit.transform.name);
                EquipPlusCH(hit.transform.name);
            }
        }




    }
    [ServerRpc]
    void PlayerHitServerRpc(string userID, float damage)
    {
        Debug.Log(userID + "has been hit");

        takeDamage user = GameManager.GetUser(userID);
        user.TakeDamage(damage);
    }
    public void EquipAR(string name)
    {
        if (name == AR.name)
        {
            classSelected = 0;
            Debug.Log("AR equiped");
            SnotGunOverlay.SetActive(false);
            SnotGunCharacter.SetActive(false);
            SMGOverlay.SetActive(false);
            SMGCharacter.SetActive(false);
            AROverlay.SetActive(true);
            ARCharacter.SetActive(true);
        }
    }

    public void EquipSMG(string name)
    {
        if (name == SMG.name)
        {
            classSelected = 1;
            Debug.Log("SMG equiped");
            SnotGunOverlay.SetActive(false);
            SnotGunCharacter.SetActive(false);
            SMGOverlay.SetActive(true);
            SMGCharacter.SetActive(true);
            AROverlay.SetActive(false);
            ARCharacter.SetActive(false);
        }
    }

    public void EquipSnotgun(string name)
    {
        if (name == Snotgun.name)
        {
            classSelected = 2;
            Debug.Log("Snotgun equiped");
            SnotGunOverlay.SetActive(true);
            SnotGunCharacter.SetActive(true);
            SMGOverlay.SetActive(false);
            SMGCharacter.SetActive(false);
            AROverlay.SetActive(false);
            ARCharacter.SetActive(false);
        }
    }

    public void EquipDotCH(string name)
    {
        if (name == DotCH.name)
        {
            dotOverlay.SetActive(true);
            crossOverlay.SetActive(false);
            plusOverlay.SetActive(false);
        }
    }

    public void EquipCrossCH(string name)
    {
        if (name == CrossCH.name)
        {
            crossOverlay.SetActive(true);
            dotOverlay.SetActive(false);
            plusOverlay.SetActive(false);
        }
    }

    public void EquipPlusCH(string name)
    {
        if (name == PlusCH.name)
        {
            plusOverlay.SetActive(true);
            dotOverlay.SetActive(false);
            crossOverlay.SetActive(false);
        }
    }
}
