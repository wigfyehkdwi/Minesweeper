using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeMarker : OptionMarker
{
    public MineGrid Grid;
    public RectTransform[] ModeOptions;

    public override RectTransform Target => ModeOptions[(int)Grid.Mode];
}
