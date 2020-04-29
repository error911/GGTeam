using GGTeam.SmartMobileCore;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelData : DataModel
{
    /// <summary>
    /// Номер
    /// </summary>
    [SerializeField] public int num = 0;
    
    /// <summary>
    /// Максимально набранные очки
    /// </summary>
    [SerializeField] public int score = 0;
    
    /// <summary>
    /// Пройден
    /// </summary>
    [SerializeField] public bool completed = false;
    
    /// <summary>
    /// Игрался
    /// </summary>
    [SerializeField] public bool played = false;

    public LevelData(int number)
    {
        this.num = number;
    }
}
