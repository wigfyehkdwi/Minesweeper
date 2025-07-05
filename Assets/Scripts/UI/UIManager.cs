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

    public RectTransform UITransform;
    public RectTransform CountersTransform;
    public RectTransform MenuBarTransform;

    // winning
    public Window FastestTime;

    private void Update()
    {
        RemainingMinesText.text = Format(Grid.MineCount - Grid.FlaggedMines);
        if (!Grid.GameEnded) TimerText.text = Format((int)Grid.TimeSinceReset);

        NewGameButton.image.sprite = Grid.GameEnded ? NewGameDeadSprite : NewGameSprite;
    }

    public void HandleResize()
    {
        int gridWidth = MineGrid.TileWidth * Grid.Width;
        int gridHeight = MineGrid.TileHeight * Grid.Height;

        int resWidth = 9 // Left border
                     + 8 // Right border
                     + gridWidth;
        int resHeight = 20 // Toolbar
                      + 52 // Top UI/border
                      + 8 // Bottom border
                      + gridHeight;

        Debug.Log("Updating resolution to " + resWidth + "x" + resHeight);
        Screen.SetResolution(resWidth, resHeight, FullScreenMode.Windowed);

        var gridRectTransform = Grid.gameObject.GetComponent<RectTransform>();
        gridRectTransform.position = new Vector3(9, 8, 0);
        gridRectTransform.sizeDelta = new Vector2(gridWidth, gridHeight);

        UITransform.sizeDelta = new Vector2(resWidth, resHeight);
        
        MenuBarTransform.anchoredPosition = new Vector3(0, resHeight - MenuBarTransform.sizeDelta.y);
        MenuBarTransform.sizeDelta = new Vector2(resWidth, MenuBarTransform.sizeDelta.y);

        CountersTransform.anchoredPosition = new Vector3(0, gridHeight - 14);
    }

    public void HandleWin()
    {
        string key = Grid.Mode.ToString() + ".time";
        if (PlayerPrefs.HasKey(key) && (int)Grid.TimeSinceReset > PlayerPrefs.GetInt(key)) FastestTime.gameObject.SetActive(true);
    }

    public static string Format(int n)
    {
        if (n < 0) return '-' + n.ToString().Substring(1).PadLeft(2, '0');
        return n.ToString().PadLeft(3, '0');
    }

    public void ResetGrid()
    {
        Grid.ResetGrid();
    }
}
