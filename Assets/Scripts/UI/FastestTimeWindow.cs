using System;
using UnityEngine;
using UnityEngine.UI;


public class FastestTimeWindow : Window
{
    public Text NameLabel;
    public InputField NameField;
    public Window FastestMineSweepers;

    public MineGrid Grid;

    private string Mode;
    private int Time;

    private void OnEnable()
    {
        Mode = Grid.Mode.ToString();
        Time = Math.Clamp((int)Grid.TimeSinceReset, 0, 999);
        NameLabel.text = "You have the fastest time for " + Mode.ToLowerInvariant() + " level. Please enter your name.";
    }

    public void OkButton()
    {
        string name = string.IsNullOrEmpty(NameField.text) ? "Anonymous" : NameField.text;
        PlayerPrefs.SetInt(Grid.Mode.ToString() + ".time", Time);
        PlayerPrefs.SetString(Grid.Mode.ToString() + ".name", name);

        FastestMineSweepers.gameObject.SetActive(true);
        Close();
    }
}
