using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ColorManager : MonoBehaviour
{
    public GameObject optionBase;
    public DraggableParent draggableParent;
    public TextAsset jsonFile;
    public SortColorDisplay sortColorDisplay;
    public StrikeDisplay strikeDisplay;
    public ResultsDisplay resultsDisplay;
    List<GameObject> options = new List<GameObject>();
    Levels levels;
    int levelIndex = 0;

    enum ColorOptions 
    {
        Green,
        Red,
        Blue
    }

    ColorOptions colorChecker;
    // Start is called before the first frame update
    void Start()
    {
        levels = JsonUtility.FromJson<Levels>(jsonFile.text);
        SetUpLevel(levels.levels[levelIndex]);
    }

    void SetUpLevel(Level level) 
    {
        colorChecker = ConvertLevelColorTypetoColorOption(level.colorType);
        List<Color> colors = ConvertStingColorsToColors(level.colorAmounts, colorChecker, Color.black);

        draggableParent.DestoryAllChildren();
        options = GenerateOptions(colors);
        draggableParent.ShuffleAllChildren();

        //shuffle the options so that it can't be won right away
        do 
        {
            draggableParent.SetupChildren(options);
        } while(IsCorrectResult(
            options.OrderBy(option => option.transform.GetSiblingIndex()),
            colorChecker
        ));
        

        //TODO: Change Connection to UI Elements To Work Through Events
        sortColorDisplay.UpdateColor(ConvertColorOptionsToColor(colorChecker));
    }

    ColorOptions ConvertLevelColorTypetoColorOption(string colorType) 
    {
        colorType = colorType.ToLower();
        switch (colorType)
        {
            case "green":
                return ColorOptions.Green;
            case "blue":
                return ColorOptions.Blue;
            case "red":
                return ColorOptions.Red;
            default:
                return ColorOptions.Green;
        }
    }

    Color32 ConvertColorOptionsToColor(ColorOptions colorOption)
    {
        switch (colorOption)
        {
            case ColorOptions.Green:
                return new Color32(0, 255, 0, 255);
            case ColorOptions.Blue:
                return new Color32(0, 0, 255, 255);
            case ColorOptions.Red:
                return new Color32(255, 0, 0, 255);
            default:
                return new Color32();
        }
    }

    List<Color> ConvertStingColorsToColors(string[] colorAmounts, ColorOptions colorOption, Color32 baseColor)
    { 
        List<Color> convertedColors = new List<Color>();
        foreach (string color in colorAmounts) 
        {
            Color newColor = new Color();
            string[] colorElements = color.Split(",");

            switch (colorElements.Length) 
            {
                //if there's only one option, then we presume that's it is just the wanted color changing on top of the base color
                case 1:
                    if (colorOption == ColorOptions.Green) 
                        newColor = new Color32(baseColor.r, byte.Parse(colorElements[0]), baseColor.b, baseColor.a);
                    else if (colorOption == ColorOptions.Blue)
                        newColor = new Color32(baseColor.r, baseColor.g, byte.Parse(colorElements[0]), baseColor.a);
                    else if (colorOption == ColorOptions.Red) 
                        newColor = new Color32(byte.Parse(colorElements[0]), baseColor.g, baseColor.b, baseColor.a);
                    break;
                case 3:
                    newColor = new Color32(byte.Parse(colorElements[0]), byte.Parse(colorElements[1]), byte.Parse(colorElements[2]), baseColor.a);
                    break;
                case 4:
                    newColor = new Color32(byte.Parse(colorElements[0]), byte.Parse(colorElements[1]), byte.Parse(colorElements[2]), byte.Parse(colorElements[3]));
                    break;
                default:
                    throw new System.Exception("Incorrect amount of characters used for the color generation. Check the JSON file to see if the colors were created correctly.");
            }

            convertedColors.Add(newColor);
        }

        return convertedColors;
    }

    public List<GameObject> GenerateOptions(List<Color> colors)
    {
        List<GameObject> newOptions = new List<GameObject>();
        foreach (Color color in colors)
        {
            GameObject option = Object.Instantiate(optionBase, draggableParent.transform);
            option.GetComponent<Image>().color = color;
            newOptions.Add(option);
        }
        return newOptions;
    }

    public void CheckResult() 
    {
        bool isCorrect = IsCorrectResult(
            options.OrderBy(option => option.transform.GetSiblingIndex()),
            colorChecker
        );

        if (isCorrect)
        {
            levelIndex++;

            if (levelIndex >= levels.levels.Length)
            {
                draggableParent.DestoryAllChildren();
                resultsDisplay.DisplayWinResults();
            }
            else
            {
                SetUpLevel(levels.levels[levelIndex]);
            }
        }
        else
        {
            bool isFinished = strikeDisplay.UpdateStrikes();
            if (isFinished)
            {
                draggableParent.DestoryAllChildren();
                resultsDisplay.DisplayLoseResults();
            }
        }
    }

    bool IsCorrectResult(IEnumerable<GameObject> sortedOptions, ColorOptions colorChecker) 
    {
        Color32 previousColor = new Color32();

        foreach (GameObject option in sortedOptions)
        {
            Color32 optionColor = option.GetComponent<Image>().color;
            if (Color32.Equals(previousColor, new Color32()))
            {
                previousColor = optionColor;
                continue;
            }

            if (CompareColors(previousColor, optionColor, colorChecker) < 0)
            {
                return false;
            }

            previousColor = optionColor;
        }

        return true;
    }

    private float CompareColors(Color32 previousColor, Color32 currentColor, ColorOptions colorOption)
    {
        switch (colorOption)
        {
            case ColorOptions.Green:
                return (previousColor.g - currentColor.g);
            case ColorOptions.Blue:
                return (previousColor.b - currentColor.b);
            case ColorOptions.Red:
                return (previousColor.r - currentColor.r);
            default:
                return 0;
        }
    }

    public void NextLevel()
    { 
    }
}

[System.Serializable]
class Level
{
    //these variables are case sensitive and must match the strings "firstName" and "lastName" in the JSON.
    public string colorType;
    public string[] colorAmounts;
}

[System.Serializable]
class Levels
{
    //employees is case sensitive and must match the string "employees" in the JSON.
    public Level[] levels;
}

