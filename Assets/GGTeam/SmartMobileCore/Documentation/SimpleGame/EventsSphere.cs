using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventsSphere : MonoBehaviour
{
    //public UnityEvent act;
    public bool Complete = false;
    LevelLogic level;

    private void Awake()
    {
        level = FindObjectOfType<LevelLogic>();
    }

    private void OnMouseDown()
    {
        //act?.Invoke();

        if (Complete) level.LevelComplete();
        else level.LevelFailed();



    }

}
