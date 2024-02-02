using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WeaponState_UI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI weaponTxt;
    private void Start()
    {
        PlayerCharacter.Instance.OnWeaponStateChanged += Instance_OnWeaponStateChanged;
    }

    private void Instance_OnWeaponStateChanged(object sender, PlayerCharacter.OnWeaponStateChangedArgs e)
    {
        weaponTxt.SetText(e.weaponState.ToString());
    }
}
