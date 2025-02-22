using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public int Width { get; private set; }
    public int Height { get; private set; }
    public int Area => Width * Height;

    public GameObject Tile;

    // Start is called before the first frame update
    void Start()
    {
        Resize(9, 9);
    }

    // Update is called once per frame
    public void Resize(int width, int height)
    {
        Width = width;
        Height = height;

        int resWidth = 9 // Left border
                     + 8 // Right border
                     + 16 * Width; // Tiles (16x16)
        int resHeight = 52 // Top UI/border
                      + 8 // Bottom border
                      + 16 * Height; // Tiles (16x16)

        Debug.Log("Updating resolution to " + resWidth + "x" + resHeight);
        Screen.SetResolution(resWidth, resHeight, FullScreenMode.Windowed);
    }
}
