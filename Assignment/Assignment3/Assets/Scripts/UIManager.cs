using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public TextMeshProUGUI algorithmText;

    void Awake()
    {
        Instance = this;
    }

    public void SetAlgorithmText(string mode)
    {
        algorithmText.text = "Algorithm: " + mode;
    }
}
