using GGTeam.SmartMobileCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGameplay : Level
{
    public override void OnLevelComplete(LevelData levelData) 
    {
        //Debug.Log("OnLevelComplete");
    }

    public override void OnLevelFailed(LevelData levelData)
    {
        //Debug.Log("OnLevelFailed");
    }

    public override void OnLevelStart()
    {
        //Debug.Log("Level started " + Data.number); //!  CurrentNumber
    }

    void Start()
    {
        //Debug.Log("Start " + Data.number);    //!  CurrentNumber
    }


    void Update()
    {
        //Debug.Log("Start!! " + CurrentNumber);
    }

}