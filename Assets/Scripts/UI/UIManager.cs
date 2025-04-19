using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public MineGrid Grid;

    // Counters
    public Text RemainingMinesText;
    public Text TimerText;

    // New game button management
    public Button NewGameButton;
    public Sprite NewGameSprite;
    public Sprite NewGameDeadSprite;

    // Used to resize the whole UI
    public RectTransform CountersTransform;

    private void Update()
    {
        RemainingMinesText.text = Format(Grid.MineCount - Grid.FlaggedMines);
        if (!Grid.GameEnded) TimerText.text = Format((int)Grid.TimeSinceReset);

        NewGameButton.image.sprite = Grid.GameEnded ? NewGameDeadSprite : NewGameSprite;
    }

    public void Resize(int width, int height)
    {
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
