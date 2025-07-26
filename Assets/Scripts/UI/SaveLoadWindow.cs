using System.IO;
using UnityEngine.UI;


public class SaveLoadWindow : Window
{
    public bool Loading { get; set; }

    public Text PathLabel;
    public InputField PathField;

    public MineGrid Grid;

    public void OkButton()
    {
        string path = PathLabel.text;
        try
        {
            if (Loading)
            {
                var info = new FileInfo(path);
                var file = info.OpenRead();
                byte[] data = new byte[info.Length];
                file.Read(data);
                Grid.Deserialize(data);
            }
            else
            {
                byte[] data = Grid.Serialize();
                var file = File.Create(path);
                file.Write(data);
            }
        }
        catch (IOException ex)
        {
            PathLabel.text = ex.Message;
        }
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
