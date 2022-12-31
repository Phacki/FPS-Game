using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;


public class WeaponSwapV3 : NetworkBehaviour
{
    public float multiplier = 2;
    public float opacitySelected = 1;
    public float opacityUnselected = 0.25f;

    /// <summary>
    /// This will allow the player (owner) to update this network variable.
    /// Useful for the server and other clients to know what weapon the player currently has selected
    /// </summary>
    private NetworkVariable<int> WeaponSelected = new NetworkVariable<int>(-1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    // The current weapons the player can select from
    public List<GunScript> Weapons;

    public GameObject secondaryOverlay;
    public GameObject stimOverlay;

    public CanvasGroup primarys;

    // Use a weapon selection key map for a 1:1 assignment of weapon to key input
    private List<KeyCode> WeaponSelectionKeys = new List<KeyCode>()
    {
        KeyCode.Alpha1,
        KeyCode.Alpha2,
        KeyCode.Alpha3,
    };

    private void Start()
    {
        primarys.alpha = opacityUnselected;
    }

    public override void OnNetworkSpawn()
    {
        if (IsLocalPlayer)
        {
            // When the player spawns, they default select the first weapon in the list
            // Optionally: you could comment this out and start with "no weapons" selected
            SwitchWeapon(0);
        }
        base.OnNetworkSpawn();
    }

    void Update()
    {
        if (!IsSpawned || !IsLocalPlayer)
        {
            return;
        }
        var mouseScroll = Input.mouseScrollDelta.y;
        // If the mouse scroll delta value is not zero
        if (mouseScroll != 0)
        {
            // Get the direction it is scrolling and set its value in units of +/- 1
            var scrollDirection = mouseScroll > 0 ? 1 : -1;

            var selectedWeapon = WeaponSelected.Value;

            // Increment our current item held
            selectedWeapon += scrollDirection;
            // Use modulus to "roll over" the value back to zero if we exceeded our
            // maximum assigned weapons
            selectedWeapon = selectedWeapon % Weapons.Count;

            // Attempt to switch to that weapon
            SwitchWeapon(selectedWeapon);
        }

        // Check for user key input to switch to another weapon
        for(int i = 0; i < WeaponSelectionKeys.Count; i++)
        {
            if (Input.GetKeyDown(WeaponSelectionKeys[i]))
            {
                // if the associated key is down then attempt to switch
                SwitchWeapon(i);
                // break out of the for loop
                break;
            }
        }

        // Stow away the current weapon and have "no weapon" selected.
        if (Input.GetKeyDown(KeyCode.Tilde))
        {
            SwitchWeapon(-1);
        }
    }

    void SwitchWeapon(int weaponIndex)
    {
        // if we are already holding this weapon or we try to select a value outside
        // of our weapons assigned range then do nothing.
        if (WeaponSelected.Value == weaponIndex || weaponIndex >= Weapons.Count)
        {
            return;
        }

        // As long as it is a valid index value
        if (WeaponSelected.Value >= 0)
        {
            // Set the currently selected weapon to "unselected"
            Weapons[WeaponSelected.Value].SelectedStatus(false);
        }

        // Anything less than zero, the player "put the weapon away"
        // If not < 0 then activate the newly selected weapon
        if (weaponIndex >= 0)
        {
            // Selected the new weapon type
            Weapons[weaponIndex].SelectedStatus(true);
        }

        // Keep track of what weapon is selected and synchronize this
        // update with the server and clients connected
        WeaponSelected.Value = weaponIndex;

        // Update weapon HUD related things
        UpdateWeaponHud();
    }

    /// <summary>
    /// Update weapon HUD Stuff
    /// Note: You might migrate this into a new component on each weapon individually
    /// </summary>
    private void UpdateWeaponHud()
    {
        // Handle ancillary HUD related things
        switch (WeaponSelected.Value)
        {
            // No weapon selected HUD stuff
            case -1:
                {
                    // TODO: Fists? Cut-Away scene? Interacting with world objects?
                    // This is when no weapon is selected
                    break;
                }
            // Handle valid weapon selection HUD stuff below
            case 0:
                {
                    primarys.alpha = opacitySelected;
                    secondaryOverlay.SetActive(false);
                    stimOverlay.SetActive(false);
                    break;

                }
            case 1:
                {
                    primarys.alpha = opacityUnselected;
                    secondaryOverlay.SetActive(true);
                    stimOverlay.SetActive(false);
                    break;
                }
            case 2:
                {
                    primarys.alpha = opacityUnselected;
                    secondaryOverlay.SetActive(false);
                    stimOverlay.SetActive(true);
                    break;
                }
        }
    }
}
