using UnityEngine;

public class WeaponSwitch : MonoBehaviour
{
    public float multiplier = 2;
    public int gunHeld = 0;
    // Start is called before the first frame update
    void Start()
    {
        SelectWeapon();
    }

    // Update is called once per frame
    void Update()
    {
        int lastgunHeld = gunHeld;

        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (gunHeld >= transform.childCount - 1)
                gunHeld = 0;
            else
                gunHeld++;
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (gunHeld <= 0)
                gunHeld = transform.childCount - 1;
            else
                gunHeld--   ;
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            gunHeld = 0;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && transform.childCount >= 2)
        {
            gunHeld = 1;
        }

        if (lastgunHeld != gunHeld)
        {
            SelectWeapon();
        }
    }

    void SelectWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (i == gunHeld)
                weapon.gameObject.SetActive(true);
            else
                weapon.gameObject.SetActive(false);
            i++;
        }
    }
}
