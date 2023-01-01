using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;


public class takeDamage : NetworkBehaviour
{
    public AudioSource audioSource;
    public AudioClip damageSound;
    public AudioClip criticalSound;
    public AudioClip offlineSound;

    public NetworkVariable<float> Health = new NetworkVariable<float>();
    public float MaxHealth = 150f;
    public enum collisionType { head , body}
    public collisionType bulletcollision;

    public WeaponSwapV2 gunMultiplier;
    public PlayerMovement controller;
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

        Health.Value -= amount * gunMultiplier.multiplier;
        if (Health.Value <= 50f)
        {
            audioSource.PlayOneShot(criticalSound);
        }
            if (Health.Value <= 0f)
            {
            audioSource.PlayOneShot(offlineSound);
            controller.Die();
            }
    }
}
