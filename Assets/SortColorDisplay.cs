using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SortColorDisplay : MonoBehaviour
{
    public Image colorIndicator;

    public void UpdateColor(Color32 color) 
    { 
        colorIndicator.color = color;
    }
}
