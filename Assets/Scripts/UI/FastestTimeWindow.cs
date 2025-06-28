using UnityEngine;
using UnityEngine.UI;


public class FastestTimeWindow : Window
{
    public Text NameLabel;
    public InputField NameField;

    public MineGrid Grid;

    private void OnEnable()
    {
        NameLabel.text = "You have the fastest time for " + Grid.Mode.ToString().ToLowerInvariant() + " level. Please enter your name.";
    }

    public void OkButton()
    {
        string name = string.IsNullOrEmpty(NameField.text) ? "Anonymous" : NameField.text;

    }
}
