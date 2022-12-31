using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class takeDamage : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip damageSound;
    public AudioClip criticalSound;
    public AudioClip offlineSound;
    public enum collisionType { head , body}
    public collisionType bulletcollision;

    public PlayerMovement controller;
    public WeaponSwapV2 gunMultiplier;
    public void TakeDamage(float amount)
    {
        audioSource.PlayOneShot(damageSound, 0.2f);
        if (bulletcollision == collisionType.head)
        {
            gunMultiplier.multiplier = 2;
        }
        else if (bulletcollision == collisionType.body)
        {
            gunMultiplier.multiplier = 1;
        }

        controller.Health -= amount * gunMultiplier.multiplier;
        if (controller.Health <= 50f)
        {
            audioSource.PlayOneShot(criticalSound);
        }
            if (controller.Health <= 0f)
            {
            audioSource.PlayOneShot(offlineSound);
            controller.Die();
            }
    }
}
