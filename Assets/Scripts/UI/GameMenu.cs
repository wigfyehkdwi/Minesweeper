using UnityEngine;

public class GameMenu : Menu
{
    public MineGrid Grid;
    public Window CustomGridWindow;

    public void NewGameOption()
    {
        Collapse();
        Grid.ResetGrid();
    }

    public void BeginnerOption()
    {
        Collapse();
        Grid.SetMode(MineGrid.Modes.Beginner);
    }

    public void IntermediateOption()
    {
        Collapse();
        Grid.SetMode(MineGrid.Modes.Intermediate);
    }

    public void ExpertOption()
    {
        Collapse();
        Grid.SetMode(MineGrid.Modes.Expert);
    }

    public void CustomOption()
    {
        Collapse();
        CustomGridWindow.gameObject.SetActive(true);
    }

    public void SaveOption()
    {
        Collapse();
        Grid.SaveGame();
    }

    public void LoadOption()
    {
        Collapse();
        Grid.LoadGame();
    }
}
