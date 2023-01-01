using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Netcode;

public class PlayerMovement : NetworkBehaviour
{   // Manages these attributes of the Player:
    //                                         - Camera sensitivity
    //                                         - Scoped and Unscoped camera sensitivity (current sensitivity is a place holder which is set equal to which ever
    //                                           sensitivity is currently being utilized)
    //                                         - Rotation of the player camera on x and y axis

    public float currentSensitivity = 100f;
    public float unscopedSensitivity = 100f;
    public float scopeSensitivity = 100f;

    [SerializeField]
    private Transform yAxis;

    private float xAxis = 0;

    // Manages these attributes of the Player:
    //                                         - movement speed
    //                                         - gravity and limits the height of the players thrust from the floor when jumping
    public float walkPace = 10f;
    public float dropForce = -10f;
    public float thrustPower = 3f;

    // Sets private variables to check for players contact with floor
    [SerializeField]
    private float floorTolerance = 0.5f;
    [SerializeField]
    private Transform floorDetect;
    [SerializeField]
    private LayerMask floorLayer;

    //                                           Links with main player to :
    //                                         - allow control of the game object
    //                                         - alter the velocity of its "Vector3" allowing it to change its position
    //                                         - therefore allowing it to move 
    public GameObject Player;
    public Image HealthFill;
    public TextMeshProUGUI CurrentHealth;
    private Vector3 speed;
    //                                           Creates a bool to save the status of the characters relationship to the floor regularly and locally to the script
    private bool onFloor;
    private CharacterController player;
    public takeDamage health;

    //                                           Activated whenever Script is enabled:
    //                                         - Sets player as short cut to alter CharacterController put on the game object for player
    //                                         - locks players cursor to screen and makes it invisible to allow full 360+ head movement
    public AudioSource audioSource;
    public AudioClip Jump;
    private void Awake()
    {
        health.Health.Value = health.MaxHealth;
        player = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    //                                           Activated every fixed frame of framerate, allowing more frequent updates:
    //                                         - Sets player as short cut to alter CharacterController put on the game object for player
    private void Update()
    {
        if (!IsOwner)
            return;
        // calls subroutines for gravity and movement of the player as it is input
        Gravity();
        Movement();
        HealthBar();
    }

    private void Movement()
    {
        //                                           Sets value of players position horizontally and vertically on the axis as floats
        //                                           
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");


        Vector3 move = transform.right * horizontal + transform.forward * vertical;

        //                                           Sets speed of player
        player.Move(move * walkPace * Time.deltaTime);

        //                                           Lets player jump if on floor + lets player climb items on floorLayer
        //if (Input.GetButton("Jump") && onFloor)
        //{
        //    //   This is for if I want to make it so you can hold space to jump, to test if specific leaps ingame are possible when timed
        //    speed.y = Mathf.Sqrt(thrustPower * -1f * dropForce);

        //}

        if (Input.GetButtonDown("Jump") && onFloor)
        {
            speed.y = Mathf.Sqrt(thrustPower * -1f * dropForce);
            audioSource.PlayOneShot(Jump);
        }
    }

    private void Gravity()
    {
        //                                           Sets gravity therefore allowing the code to alter the velocity at which player drops to floor when "OnFloor"
        //                                           is false
        
        onFloor = Physics.CheckSphere(floorDetect.position, floorTolerance, floorLayer);

        if (onFloor && speed.y < 0)
        {
            speed.y = -2f;
        }
        speed.y += dropForce * Time.deltaTime;
        player.Move(speed * Time.deltaTime);

        if (onFloor == false)
        {
            audioSource.Play();
        }
    }
    //                                           Subroutine called when player health is less than or equal to zero, causing players death
    public void Die()
    {
        Destroy(Player);
    }
    public void HealthBar()
    {
        CurrentHealth.text = health.Health + "";
        HealthFill.fillAmount = health.Health.Value / health.MaxHealth;
    }
}
