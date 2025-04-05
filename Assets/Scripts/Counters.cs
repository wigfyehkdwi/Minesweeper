using UnityEngine;
using UnityEngine.UI;

public class Counters : MonoBehaviour
{
    public MineGrid Grid;
    public Text RemainingMinesText;
    public Text TimerText;

    private void Update()
    {
        RemainingMinesText.text = Format(Grid.MineCount - Grid.FlaggedMines);
        TimerText.text = Format((int)Grid.TimeSinceReset);
    }

    public string Format(int n)
    {
        return n.ToString().PadLeft(3);
    }

    public void ResetGrid()
    {
        Grid.ResetGrid();
    }
}
