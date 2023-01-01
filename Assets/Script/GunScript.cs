using UnityEngine;
using System.Collections;
using TMPro;
using Unity.Netcode;

// NSS: Commenting this out.
// Really, if you are assigning the audio source itself then you don't need to require it.
//[RequireComponent(typeof(AudioSource))]
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
    public GameObject ShotGunOverlay;
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

    // NSS: Added Properties
    private bool IsSelected;
    public enum WeaponTypes
    {
        AR,
        SMG,
        Snotgun,
        DotCH,
        CrossCH,
        PlusCH,
    }

    public WeaponTypes WeaponType;

    public GameObject WeaponAsset;

    //private void OnEnable()
    //{
    //    isReloading = false;
    //    animator.SetBool("CurrentlyReloading", false);
    //    animator.SetBool("Healing", false);
    //    FireInterval = Time.time + 1f / fireRate;
    //    audioSource.PlayOneShot(equipSound, 0.5f);
    //    player.walkPace = walkPaceSet;
    //    player.dropForce = dropForceSet;
    //    player.thrustPower = thrustPowerSet;
    //}



    public void SelectedStatus(bool isSelected)
    {
        IsSelected = isSelected;

        // Activate the weapon's visual and audio components
        WeaponAsset.SetActive(IsSelected);
        isReloading = false;
        animator.SetBool("CurrentlyReloading", false);
        animator.SetBool("Healing", false);
        if (IsSelected)
        {
            FireInterval = Time.time + 1f / fireRate;
            audioSource.PlayOneShot(equipSound, 0.5f);
            player.walkPace = walkPaceSet;
            player.dropForce = dropForceSet;
            player.thrustPower = thrustPowerSet;
            WeaponSelected();
        }
    }

    void Update()
    {
        // If not spawned or not selected exit early
        if (!IsSpawned || !IsSelected)
        {
            return;
        }


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
                if (hit.collider.tag == PlayerTag)
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


                //EquipAR(hit.transform.name);
                //EquipSMG(hit.transform.name);
                //EquipSnotgun(hit.transform.name);
                //EquipDotCH(hit.transform.name);
                //EquipCrossCH(hit.transform.name);
                //EquipPlusCH(hit.transform.name);
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


                //EquipAR(hit.transform.name);
                //EquipSMG(hit.transform.name);
                //EquipSnotgun(hit.transform.name);
                //EquipDotCH(hit.transform.name);
                //EquipCrossCH(hit.transform.name);
                //EquipPlusCH(hit.transform.name);
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

    void WeaponSelected()
    {
        switch (WeaponType)
        {
            case WeaponTypes.AR:
                {
                    Debug.Log("AR equipped");
                    if (ShotGunOverlay) { ShotGunOverlay.SetActive(false); }
                    if (SnotGunCharacter) { SnotGunCharacter.SetActive(false); }

                    if (SMGOverlay) { SMGOverlay.SetActive(false); }
                    if (SMGCharacter) { SMGCharacter.SetActive(false); }

                    if (AROverlay) { AROverlay.SetActive(true); }
                    if (ARCharacter) { ARCharacter.SetActive(true); }

                    break;
                }
            case WeaponTypes.SMG:
                {
                    Debug.Log("SMG equipped");
                    if (ShotGunOverlay) { ShotGunOverlay.SetActive(false); }
                    if (SnotGunCharacter) { SnotGunCharacter.SetActive(false); }

                    if (SMGOverlay) { SMGOverlay.SetActive(true); }
                    if (SMGCharacter) { SMGCharacter.SetActive(true); }

                    if (AROverlay) { AROverlay.SetActive(false); }
                    if (ARCharacter) { ARCharacter.SetActive(false); }

                    break;
                }
            case WeaponTypes.Snotgun:
                {
                    Debug.Log("Shotgun equipped");
                    if (ShotGunOverlay) { ShotGunOverlay.SetActive(true); }
                    if (SnotGunCharacter) { SnotGunCharacter.SetActive(true); }

                    if (SMGOverlay) { SMGOverlay.SetActive(false); }
                    if (SMGCharacter) { SMGCharacter.SetActive(false); }

                    if (AROverlay) { AROverlay.SetActive(false); }
                    if (ARCharacter) { ARCharacter.SetActive(false); }

                    break;
                }
            case WeaponTypes.DotCH:
                {
                    if (dotOverlay) { dotOverlay.SetActive(true); };
                    if (crossOverlay) { crossOverlay.SetActive(false); };
                    if (plusOverlay) { plusOverlay.SetActive(false); };
                    break;
                }
            case WeaponTypes.CrossCH:
                {
                    if (dotOverlay) { dotOverlay.SetActive(false); };
                    if (crossOverlay) { crossOverlay.SetActive(true); };
                    if (plusOverlay) { plusOverlay.SetActive(false); };
                    break;
                }
            case WeaponTypes.PlusCH:
                {
                    if (dotOverlay) { dotOverlay.SetActive(false); };
                    if (crossOverlay) { crossOverlay.SetActive(false); };
                    if (plusOverlay) { plusOverlay.SetActive(true); };

                    break;
                }
        }
    }
}
