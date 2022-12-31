    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Unity.Netcode;


    public class WeaponSwapV2 : MonoBehaviour
    {
        public float multiplier = 2;
        public float opacitySelected = 1;
        public float opacityUnselected = 0.25f;
        public GunScript gScript = new GunScript();
        int itemHeld = 1;
        int classNo;

    
    
        //public GameObject primary;
        public GameObject assaultRifle;
        public GameObject subMachineGun;
        public GameObject snotGun;
        public GameObject secondary;
        public GameObject stimShot;

        public GameObject secondaryOverlay;
        public GameObject stimOverlay;

        public CanvasGroup primarys;

        private void Start()
        {
            primarys.alpha = opacityUnselected;
        }
        void Update()
        {
            //if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            //{
            //    if (itemHeld >= 2)
            //        itemHeld = 2;
            //    else
            //        itemHeld++;
            //}
            //if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            //{
            //    gunSwitch(3);
            //    if (itemHeld <= 0)
            //        itemHeld = 2;
            //    else
            //        itemHeld--;
            //}

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                if (itemHeld != 0)
                {
                    primarys.alpha = opacitySelected;
                    secondaryOverlay.SetActive(false);
                    stimOverlay.SetActive(false);
                    itemHeld = 0;
                    checkSwitch();
                }
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                if (itemHeld != 1)
                {
                    primarys.alpha = opacityUnselected;
                    secondaryOverlay.SetActive(true);
                    stimOverlay.SetActive(false);
                    checkSwitch();
                    itemHeld = 1;
                    checkSwitch();
                }
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                if (itemHeld != 2)
                {
                    primarys.alpha = opacityUnselected;
                    secondaryOverlay.SetActive(false);
                    stimOverlay.SetActive(true);
                    itemHeld = 2;
                    checkSwitch();
                }
            }
        }

        void gunSwitch(int PorS)
        {
            classNo = gScript.classSelected;
            if (PorS == 0)
            {

                switch (classNo)
                {
                    case 0:
                        //Debug.Log(gunScript.classSelected);
                        Debug.Log("AR Active");
                        assaultRifle.SetActive(true);
                        break;
                    case 1:
                        subMachineGun.SetActive(true);
                        break;
                    case 2:
                        snotGun.SetActive(true);
                        break;
                }
            
                secondary.SetActive(false);
                stimShot.SetActive(false);

            }

            if (PorS == 1)
            {
                switch (classNo)
                {
                    case 0:
                        assaultRifle.SetActive(false);
                        break;
                    case 1:
                        subMachineGun.SetActive(false);
                        break;
                    case 2:
                        snotGun.SetActive(false);
                        break;    
                }
                secondary.SetActive(true);
                stimShot.SetActive(false);

            }

            if (PorS == 2)
            {
                switch (classNo)
                {
                    case 0:
                        assaultRifle.SetActive(false);
                        break;
                    case 1:
                        subMachineGun.SetActive(false);
                        break;
                    case 2:
                        snotGun.SetActive(false);
                        break;
                }
                secondary.SetActive(false);
                stimShot.SetActive(true);
            }
        }

        void checkSwitch()
        {
            switch (itemHeld)
            {
                case 0:
                    gunSwitch(0);
                    break;
                case 1:
                    gunSwitch(1);
                    break;
                case 2:
                    gunSwitch(2);
                    break;
            }
        }
    }
