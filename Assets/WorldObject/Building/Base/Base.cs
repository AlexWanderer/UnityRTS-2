using UnityEngine;
using System.Collections;

public class Base : Building
{

    public override void SetSelection(bool selected, Rect playingArea)
    {
        SetSelectionNoRally(selected, playingArea);
    }

}