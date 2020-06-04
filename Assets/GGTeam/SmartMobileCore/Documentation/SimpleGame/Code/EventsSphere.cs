using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventsSphere : MonoBehaviour
{
    public TestState State = TestState.None;
    MyGameplay level;

    private void Awake()
    {
        level = FindObjectOfType<MyGameplay>();
    }

    private void OnMouseDown()
    {
        if (State == TestState.Complete)
        {
            level.Progress.MoneyAdd(4);
            level.Progress.ScoreAdd(1500);
            level.LevelComplete(2.5f);
        }

        if (State == TestState.Failed) level.LevelFailed();
        if (State == TestState.MoneyAdd) { Debug.Log("Money"); level.Progress.MoneyAdd(1); }
        if (State == TestState.ScoreAdd) { Debug.Log("Score"); level.Progress.ScoreAdd(1); }
    }

    public enum TestState
    {
        None,
        Complete,
        Failed,
        MoneyAdd,
        ScoreAdd,
    }

}
