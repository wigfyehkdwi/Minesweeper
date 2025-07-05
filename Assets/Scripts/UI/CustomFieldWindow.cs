using UnityEngine;
using UnityEngine.UI;


public class CustomFieldWindow : Window
{
    public InputField GridWidthField;
    public InputField GridHeightField;
    public InputField MineCountField;

    public Window FastestMineSweepers;

    public MineGrid Grid;

    public void OkButton()
    {
        Grid.Mode = MineGrid.Modes.Custom;
        Grid.MineCount = Parse(MineCountField, 10);
        Grid.Resize(Parse(GridWidthField, 9), Parse(GridHeightField, 9));
        Close();
    }

    public int Parse(InputField field, int def)
    {
        if (int.TryParse(field.text, out int val))
        {
            return val;
        }
        else
        {
            return def;
        }
    }
}
