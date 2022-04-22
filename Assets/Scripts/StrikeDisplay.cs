using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrikeDisplay : MonoBehaviour
{
    public GameObject[] strikes;
    int strikeAmt;
    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject strike in strikes)
        {
            strike.SetActive(false);
        }
    }

    public bool UpdateStrikes()
    {
        if (strikeAmt < strikes.Length)
        {
            strikes[strikeAmt].SetActive(true);
            strikeAmt++;
        }       

        return (strikeAmt >= strikes.Length);
    }
}
