using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InputType
{
    Keyboard,
    Joystick,
}

public class PlayerInput : MonoBehaviour
{
    public static PlayerInput Instance;

    [TabGroup("Options")][HideInInspector] public float horizontal;
    [TabGroup("Options")][HideInInspector] public float vertical;

    [FoldoutGroup("Debug")] public InputType inputType;

    private void Awake()
    {
        Instance = this;
    }
}
