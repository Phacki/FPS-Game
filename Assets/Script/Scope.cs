using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

[RequireComponent(typeof(AudioSource))]
public class Scope : MonoBehaviour
{
    public GunScript gunscript3;
    public CamMovement camMovement;
    public Animator animator;
    public GameObject snotgunScope;
    public GameObject fullHUD;
    public GameObject CrosshairFolder;
    public GameObject scopeCamera;
    public GameObject rayCamera;
    private bool CurrentlyScoped = false;
    public AudioSource audioSource;
    public AudioClip scopeIn;
    public AudioClip scopeOut;

    private void Start()
    {
        rayCamera.SetActive(true);
        scopeCamera.SetActive(false);
    }

    private void OnDisable()
    {
        if (CurrentlyScoped)
        {
            CurrentlyScoped = !CurrentlyScoped;
            animator.SetBool("Scoped", CurrentlyScoped);
            audioSource.PlayOneShot(scopeOut, 1f);
            snotgunScope.SetActive(false);
            fullHUD.SetActive(true);
            CrosshairFolder.SetActive(true);
            rayCamera.SetActive(true);
            scopeCamera.SetActive(false);
            gunscript3.damage = 75;
            gunscript3.fireRate = 1f;
            camMovement.currentSensitivity = camMovement.unscopedSensitivity;
        }
        
    }

    public void Update()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            CurrentlyScoped = !CurrentlyScoped;
            animator.SetBool("Scoped", CurrentlyScoped);

            if (CurrentlyScoped)
                StartCoroutine(OnScoped());
            else
                OnUnscoped();
        }

        if (Input.GetButtonDown("Fire1") && CurrentlyScoped)
        {
            CurrentlyScoped = !CurrentlyScoped;
            animator.SetBool("Scoped", CurrentlyScoped);
            OnUnscoped();

        }

        void OnUnscoped()
        {
            audioSource.PlayOneShot(scopeOut, 1f);
            snotgunScope.SetActive(false);
            fullHUD.SetActive(true);
            CrosshairFolder.SetActive(true);
            rayCamera.SetActive(true);
            scopeCamera.SetActive(false);
            zoomOut();
        }
        IEnumerator OnScoped()
        {
            audioSource.PlayOneShot(scopeIn, 1f);
            yield return new WaitForSeconds(.1f);
            rayCamera.SetActive(false);
            scopeCamera.SetActive(true);
            CrosshairFolder.SetActive(false);
            fullHUD.SetActive(false);
            snotgunScope.SetActive(true);
            zoomIn();
        }

        void zoomIn()
        {   
            gunscript3.damage = 150;
            gunscript3.fireRate = 0.8f;
            camMovement.currentSensitivity = camMovement.scopeSensitivity;
        }

        void zoomOut()
        {
            gunscript3.damage = 75;
            gunscript3.fireRate = 1f;
            camMovement.currentSensitivity = camMovement.unscopedSensitivity;
        }
    }
}
