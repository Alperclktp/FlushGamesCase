using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public TMP_Text currentMoneyText;

    private void Awake()
    {
        Instance = this;
    }
}
