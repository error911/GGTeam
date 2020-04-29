using GGTeam.SmartMobileCore;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelData : DataModel
{
    [SerializeField] public int num = 0;
    [SerializeField] public int score = 0;
    [SerializeField] public bool completed = false;
    [SerializeField] public bool played = false;

    public LevelData(int number)
    {
        this.num = number;
    }
}
