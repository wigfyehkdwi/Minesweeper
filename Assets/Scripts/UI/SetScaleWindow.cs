using UnityEngine;
using UnityEngine.UI;


public class SetScaleWindow : Window
{
    public Text ScaleLabel;
    public InputField ScaleField;

    public MineGrid Grid;

    public void OkButton()
    {
        ScaleLabel.text = "Enter the new scale...";

        if (float.TryParse(ScaleField.text, out float scale))
        {
            var ui = Grid.UI;
            if (ui != null)
            {
                ui.UIScale = scale;
                Grid.UI?.HandleResize();
            }
            Close();
        }
        else
        {
            ScaleLabel.text = "Invalid scale number!";
        }

        
    }
}
