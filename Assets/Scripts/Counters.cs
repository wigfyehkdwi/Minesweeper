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
        if (!Grid.GameEnded) TimerText.text = Format((int)Grid.TimeSinceReset);
    }

    public string Format(int n)
    {
        if (n < 0) return '-' + n.ToString().Substring(1).PadLeft(2, '0');
        return n.ToString().PadLeft(3, '0');
    }

    public void ResetGrid()
    {
        Grid.ResetGrid();
    }
}
