using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class StimScript : MonoBehaviour
{
    public PlayerMovement player = new PlayerMovement();
    public takeDamage heal = new takeDamage();

    public Animator animator;
    public int stimAmount = 1;
    public bool isHealing = false;

    public AudioSource audioSource;
    public AudioSource stimSource;
    public AudioClip PullOut;
    public AudioClip UseSound;
    public TextMeshProUGUI currentMag;
    public GameObject ammoLeft;

    private void OnEnable()
    {
        audioSource.PlayOneShot(PullOut, 0.6f);
        animator.Play("GunIdle");
        isHealing = false;
        animator.SetBool("Healing", false);
        player.walkPace = 15f;
        player.dropForce = -30f;
        player.thrustPower = 5f;
        ammoLeft.SetActive(false);
    }

    private void OnDisable()
    {
        animator.Play("GunIdle");
        isHealing = false;
        animator.SetBool("Healing", false);
        ammoLeft.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {
        currentMag.text = stimAmount + "";
        if (isHealing == true)
        {
            return;
        }
        //if (Input.GetKeyDown(KeyCode.Alpha1) || (Input.GetKeyDown(KeyCode.Alpha2)) || (Input.GetKeyDown(KeyCode.Alpha3)))
        //{
        //    Debug.Log("Healing set to false");
        //    animator.SetBool("Healing", false);
        //    StopCoroutine(Delay());
        //}

        if (Input.GetButtonDown("Fire2"))
        {
            if(heal.Health.Value < 150) 
            {
                if (isHealing == false)
                {
                    if (stimAmount > 0)
                    {
                        StimHit();
                    }
                }
            }
        }
    }
    void StimHit()
    {
        stimSource.PlayOneShot(UseSound);
        if (isHealing == false)
        {  
            StartCoroutine(Delay());
        }
        else
        {
            return;
        }
    }

    IEnumerator Delay()
    {
        animator.SetBool("Healing", true);
        yield return new WaitForSeconds(2f);
        animator.SetBool("Healing", false);
        StimHeal();
    }
    void StimHeal()
    {
        float health = heal.Health.Value;
        switch (health)
        {
            case < 100:
                heal.Health.Value += 50;
                break;
            case >= 100:
                heal.Health.Value = 150;
                break;
        }
        Debug.Log("+50 hp added");
        
        stimAmount--;
    }

    
}
