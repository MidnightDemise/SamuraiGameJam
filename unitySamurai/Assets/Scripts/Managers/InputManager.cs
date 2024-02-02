using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance {  get; private set; }

    public PlayerControls PlayerControls { get; private set; }

    private void Awake()
    {
        Instance = this;
        PlayerControls = new PlayerControls();
        PlayerControls.Enable();
    }


}
