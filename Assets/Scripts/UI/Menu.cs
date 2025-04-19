using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public GameObject Options;
    public void ToggleMenu()
    {
        Options.SetActive(!Options.activeSelf);
    }
}
