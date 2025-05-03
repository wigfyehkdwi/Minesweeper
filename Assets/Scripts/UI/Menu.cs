using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public GameObject Options;

    public void Collapse()
    {
        Options.SetActive(false);
    }

    public void ToggleMenu()
    {
        Options.SetActive(!Options.activeSelf);
    }
}
