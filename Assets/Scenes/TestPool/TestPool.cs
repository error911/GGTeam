using GGTeam.SmartMobileCore.Modules.PoolModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPool : MonoBehaviour
{
    public PObj prefObj;
    //PoolModule poolManager = new PoolModule();

    void Start()
    {
        PoolModule poolManager = new PoolModule();
        var pool = poolManager.PutElement(prefObj, 10);

        var el1 = pool.GetElement(transform);
        var el2 = pool.GetElement(transform);

        el2.Return();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
