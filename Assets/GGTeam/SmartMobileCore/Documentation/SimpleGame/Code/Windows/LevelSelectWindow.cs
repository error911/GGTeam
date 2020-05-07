using GGTeam.SmartMobileCore;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectWindow : UIScreen
{
    public GameObject CellPref = null;
    public Transform CellContainer = null;
    bool initedQ = false;
    public override void OnInit()
    {
        
        
        
    }


    void _Init()
    {
        CellPref.SetActive(false);

        for (int i = 1; i <= Game.Levels.Count; i++)
        //for (int i = Game.Levels.Count; i >= 1; i--)
        {
            var data = Game.Levels.LevelData(i);
            GameObject go = Instantiate(CellPref, CellContainer);
            go.SetActive(true);
            Transform normalTr = go.transform.Find("Normal");
            Transform lockTr = go.transform.Find("Lock");
            Text textLevel = normalTr.Find("TextLevel").GetComponent<Text>();
            Button btn = normalTr.Find("Button").GetComponent<Button>();

            Image star1 = normalTr.Find("Star1").GetComponent<Image>();
            Image star2 = normalTr.Find("Star2").GetComponent<Image>();
            Image star3 = normalTr.Find("Star3").GetComponent<Image>();

            if (data.completed || data.played)
            {
                textLevel.text = i.ToString();
                btn.onClick.AddListener(() => StartLevel(i));
                normalTr.gameObject.SetActive(true);
                lockTr.gameObject.SetActive(false);

                if (data.stars > 1)
                {
                    star1.fillAmount = 1;
                }
                else
                {
                    star1.fillAmount = data.stars;
                    star2.fillAmount = 0;
                    star3.fillAmount = 0;
                    continue;
                }

                if (data.stars > 2)
                {
                    star1.fillAmount = 1;
                    star2.fillAmount = 1;
                }
                else
                {
                    star2.fillAmount = data.stars - 1;
                    star3.fillAmount = 0;
                    continue;
                }

                if (data.stars >= 3)
                {
                    star1.fillAmount = 1;
                    star2.fillAmount = 1;
                    star3.fillAmount = 1;
                }
                else
                {
                    star3.fillAmount = data.stars - 2;
                    continue;
                }
            }
            else
            {
                normalTr.gameObject.SetActive(false);
                lockTr.gameObject.SetActive(true);
            }
            /*
            if (data.played)
            {
                textLevel.text = i.ToString();
                normalTr.gameObject.SetActive(true);
                lockTr.gameObject.SetActive(false);
                btn.onClick.AddListener(() => StartLevel(i));
            }
            */

        }
        initedQ = true;
    }

    public void StartLevel(int i)
    {

    }




    public void OnBtnContinue()
    {
        Game.Levels.LoadNext(); //OnLoaded
    }

    public void OnBtnRestart()
    {
        Game.Levels.LoadCurrent();  //OnLoaded
    }

    public override void OnClose()
    {

    }

    

    public override void OnOpen()
    {
        if (!initedQ) _Init();
        //lvlNum.text = "LEVEL " + Game.Levels.CurrentNumber;
    }
}
