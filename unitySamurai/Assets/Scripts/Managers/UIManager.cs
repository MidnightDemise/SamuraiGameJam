using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject rifle;
    public GameObject shield;
    //public GameObject BasicLookCamera;
    //public GameObject CombatLookCamera;
    public GameObject Crosshair;
    public GameObject Sword;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        RifleEquip();
        SheildEquip();
        SwordEquip();


    }

    public void RifleEquip()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) )
        {

            if (rifle.activeSelf && !shield.activeSelf && !Sword.activeSelf)
            {
                //Third.currentStyle = Third.CameraStyle.Basic;
                Crosshair.SetActive(false);
                rifle.SetActive(false);

            }
            else if (!rifle.activeSelf && (shield.activeSelf || Sword.activeSelf))
            {
                //Third.currentStyle = Third.CameraStyle.Combat;
                Crosshair.SetActive(true);
                shield.SetActive(false);
                Sword.SetActive(false);
                rifle.SetActive(true);

            }
            else
            {
                rifle.SetActive(true);
                Crosshair.SetActive(true);
            }
        } 
       
    }


    public void SheildEquip()
    {
        if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (shield.activeSelf && !rifle.activeSelf && !Sword.activeSelf)
            {
                //Third.currentStyle = Third.CameraStyle.Basic;
                Crosshair.SetActive(false);
                shield.SetActive(false);

            }
            else if (!shield.activeSelf && (rifle.activeSelf || Sword.activeSelf))
            {
                //Third.currentStyle = Third.CameraStyle.Combat;
                Crosshair.SetActive(true);
                shield.SetActive(true);
                Sword.SetActive(false);
                rifle.SetActive(false);

            }
            else
            {
                shield.SetActive(true);
                Crosshair.SetActive(true);

            }
        }
        
    }


    public void SwordEquip()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (Sword.activeSelf && !shield.activeSelf && !rifle.activeSelf)
            {
                Sword.SetActive(false);
                Crosshair.SetActive(false);
            }
            else if (!Sword.activeSelf && (rifle.activeSelf || shield.activeSelf))
            {
                Crosshair.SetActive(true);
                Sword.SetActive(true);
                shield.SetActive(false);
                rifle.SetActive(false);
            }
            else
            {
                Sword.SetActive(true);
                Crosshair.SetActive(true);
            }
        }
           
    }
}