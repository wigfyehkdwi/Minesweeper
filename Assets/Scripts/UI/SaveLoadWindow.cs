using System;
using System.IO;
using UnityEngine.UI;


public class SaveLoadWindow : Window
{
    public bool Loading;

    public Text Titlebar;
    public Text PathLabel;
    public InputField PathField;

    public MineGrid Grid;

    private void OnEnable()
    {
        if (Loading)
        {
            Titlebar.text = "Save game";
            PathLabel.text = "Enter the path to save to...";
        }
        else
        {
            Titlebar.text = "Load game";
            PathLabel.text = "Enter the path to load from...";
        }
    }

    public void OkButton()
    {
        string path = PathField.text;

        FileStream file = null;
        var info = new FileInfo(path);
        try
        {
            if (Loading)
            {
                file = info.OpenRead();
                byte[] data = new byte[info.Length];
                file.Read(data);
                Grid.Deserialize(data);
            }
            else
            {
                if (info.Exists) {
                    PathLabel.text = "The file already exists!"; // to prevent accidental overwrites
                    return;
                }

                byte[] data = Grid.Serialize();
                file = info.Create();
                file.Write(data);
            }
            file.Flush();
            Close();
        }
        catch (Exception ex)
        {
            PathLabel.text = ex.Message;
        }
        try
        {
            file?.Dispose();
        }
        catch (Exception ex)
        {
            PathLabel.text = ex.Message;
        }
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
