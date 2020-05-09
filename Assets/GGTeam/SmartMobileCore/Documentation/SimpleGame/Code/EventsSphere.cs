using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventsSphere : MonoBehaviour
{
    public bool Complete = false;
    MyGameplay level;

    private void Awake()
    {
        level = FindObjectOfType<MyGameplay>();
    }

    private void OnMouseDown()
    {
        if (Complete)
        {
            level.Score.ScoreAdd(150);
            level.LevelComplete(1.8f);
        }
        else level.LevelFailed();
    }

}
