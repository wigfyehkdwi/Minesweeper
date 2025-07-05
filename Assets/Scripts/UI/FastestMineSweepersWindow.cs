using UnityEngine;
using UnityEngine.UI;


public class FastestMineSweepersWindow : Window
{
    public MineGrid Grid;

    public Text BeginnerTime;
    public Text IntermediateTime;
    public Text ExpertTime;

    public Text BeginnerName;
    public Text IntermediateName;
    public Text ExpertName;

    public void UpdateText()
    {
        BeginnerTime.text = GetTime(MineGrid.Modes.Beginner) + " seconds";
        IntermediateTime.text = GetTime(MineGrid.Modes.Intermediate) + " seconds";
        ExpertTime.text = GetTime(MineGrid.Modes.Expert) + " seconds";

        BeginnerName.text = GetName(MineGrid.Modes.Beginner);
        BeginnerName.text = GetName(MineGrid.Modes.Intermediate);
        BeginnerName.text = GetName(MineGrid.Modes.Expert);
    }

    public void OnEnable()
    {
        UpdateText();
    }

    public void ResetScores()
    {
        PlayerPrefs.DeleteAll();
        UpdateText();
    }

    private string GetTime(MineGrid.Modes mode)
    {
        string key = mode.ToString() + ".time";

        if (PlayerPrefs.HasKey(key)) return PlayerPrefs.GetInt(key).ToString().PadLeft(3, '0');
        return "999";
    }

    private string GetName(MineGrid.Modes mode)
    {
        string key = mode.ToString() + ".name";

        if (PlayerPrefs.HasKey(key)) return PlayerPrefs.GetString(key);
        return "Anonymous";
    }
}
