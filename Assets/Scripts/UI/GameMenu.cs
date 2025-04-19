using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenu : Menu
{
    public MineGrid Grid;
    public GameObject CustomGridWindow;

    public void NewOption()
    {
        Grid.ResetGrid();
    }

    public void BeginnerOption()
    {
        Grid.MineCount = 10;
        Grid.Resize(9, 9);
    }

    public void IntermediateOption()
    {
        Grid.MineCount = 40;
        Grid.Resize(16, 16);
    }

    public void ExpertOption()
    {
        Grid.MineCount = 99;
        Grid.Resize(16, 30);
    }

    public void CustomOption()
    {
        CustomGridWindow.SetActive(true);
    }
}
