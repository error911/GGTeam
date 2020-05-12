// ================================
// Free license: CC BY Murnik Roman
// ================================

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
    [SerializeField] public int number = 0;
    
    /// <summary>
    /// Максимально набранные очки
    /// </summary>
    [SerializeField] public int score = 0;

    /// <summary>
    /// Полученные звезды (желательно 0..3 с шагом 0.5f)
    /// </summary>
    [SerializeField] public float stars = 0;

    /// <summary>
    /// Пройден
    /// </summary>
    [SerializeField] public bool completed = false;
    
    /// <summary>
    /// Открыт
    /// </summary>
    [SerializeField] public bool opened = false;

    public LevelData(int number)
    {
        this.number = number;
        Load(number.ToString());
    }

    /// <summary>
    /// Сохранить прогресс уровеня
    /// </summary>
    public new void Save()
    {
        Save(number.ToString());
    }

}
