using UnityEngine;
using UnityEngine.UI;


public class FastestTimeWindow : Window
{
    public Text NameLabel;
    public InputField NameField;

    public MineGrid Grid;

    private string Mode;
    private int Time;

    private void OnEnable()
    {
        Mode = Grid.Mode.ToString();
        Time = (int)Grid.TimeSinceReset;
        NameLabel.text = "You have the fastest time for " + Mode.ToLowerInvariant() + " level. Please enter your name.";
    }

    public void OkButton()
    {
        string name = string.IsNullOrEmpty(NameField.text) ? "Anonymous" : NameField.text;
        PlayerPrefs.SetInt(Grid.Mode.ToString() + ".time", Time);
        PlayerPrefs.SetString(Grid.Mode.ToString() + ".name", Name);

        FastestMineSweepers.gameObject.SetActive(true);
        Close();
    }
}
