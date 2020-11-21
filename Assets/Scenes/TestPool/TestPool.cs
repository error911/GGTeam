using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPool : MonoBehaviour
{
    public GameObject goPuuled;
    public PObj pObj;
    PoolManager poolManager = new PoolManager();

    void Start()
    {
        //IPool pool = poolManager.PutElement<PObj>(pObj, 1);
        IPool pool = poolManager.PutElement(pObj, 10);


        pool.GetElement(transform);
        pool.GetElement(transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
