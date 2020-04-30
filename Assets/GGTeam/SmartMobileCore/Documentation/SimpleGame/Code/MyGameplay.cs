using GGTeam.SmartMobileCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGameplay : Level
{
    public override void OnLevelComplete(int score)
    {
        Debug.Log("OnLevelComplete");
    }

    public override void OnLevelFailed(int score)
    {
        Debug.Log("OnLevelFailed");
    }

    public override void OnLevelStart()
    {
        Debug.Log("Level started " + CurrentNumber);
    }

    void Start()
    {
        Debug.Log("Start " + CurrentNumber);
            
    }


    void Update()
    {
        //Debug.Log("Start!! " + CurrentNumber);
    }

}