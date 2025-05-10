using UnityEngine;

public class GameMenu : Menu
{
    public MineGrid Grid;
    public GameObject CustomGridWindow;

    public void NewGameOption()
    {
        Collapse();
        Grid.ResetGrid();
    }

    public void BeginnerOption()
    {
        Collapse();
        Grid.Mode = MineGrid.Modes.Beginner;
        Grid.MineCount = 10;
        Grid.Resize(9, 9);
    }

    public void IntermediateOption()
    {
        Collapse();
        Grid.Mode = MineGrid.Modes.Intermediate;
        Grid.MineCount = 40;
        Grid.Resize(16, 16);
    }

    public void ExpertOption()
    {
        Collapse();
        Grid.Mode = MineGrid.Modes.Expert;
        Grid.MineCount = 99;
        Grid.Resize(16, 30);
    }

    public void CustomOption()
    {
        Collapse();
        CustomGridWindow.SetActive(true);
    }
}
