using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;


public class CustomFieldWindow : Window
{
    public TextField GridWidthField;
    public TextField GridHeightField;
    public TextField MineCountField;

    public MineGrid Grid;

    public void OkButton()
    {
        Grid.MineCount = Parse(MineCountField, 10);
        Grid.Resize(Parse(GridWidthField, 9), Parse(GridHeightField, 9));
    }

    public int Parse(TextField field, int def)
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
