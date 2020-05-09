// ====================================
// Simple UI Window for SmartMobileCore
// ====================================

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
    public Sprite ImageCompleted;
    public Sprite ImagePlayed;
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
//data.Load();
            
            GameObject go = Instantiate(CellPref, CellContainer);
            go.SetActive(true);
            Transform normalTr = go.transform.Find("Normal");
            Transform lockTr = go.transform.Find("Lock");
            Text textLevel = normalTr.Find("TextLevel").GetComponent<Text>();
            Button btn = normalTr.Find("Button").GetComponent<Button>();
            Image btnImg = btn.gameObject.GetComponent<Image>();
            Transform imageBottomTr = normalTr.Find("ImageBottom");
            Image star1 = imageBottomTr.Find("Star1").GetComponent<Image>();
            Image star2 = imageBottomTr.Find("Star2").GetComponent<Image>();
            Image star3 = imageBottomTr.Find("Star3").GetComponent<Image>();

            if (data.completed || data.played)
            {
                if (data.completed && data.played)
                {
                    imageBottomTr.gameObject.SetActive(true);
                    btnImg.sprite = ImageCompleted;
                }
                else
                {
                    imageBottomTr.gameObject.SetActive(false);
                    btnImg.sprite = ImagePlayed;
                }

                textLevel.text = i.ToString();
                int m = i;
                btn.onClick.AddListener(() => StartLevel(m));
                normalTr.gameObject.SetActive(true);
                lockTr.gameObject.SetActive(false);

                if (data.stars > 0 && data.stars <= 1)
                {
                    star1.fillAmount = data.stars;
                    star2.fillAmount = 0;
                    star3.fillAmount = 0;
                }
                else
                if (data.stars > 1 && data.stars <= 2)
                {
                    star1.fillAmount = 1;
                    star2.fillAmount = data.stars - 1;
                    star3.fillAmount = 0;
                }
                else
                if (data.stars > 2 && data.stars <= 3)
                {
                    star1.fillAmount = 1;
                    star2.fillAmount = 1;
                    star3.fillAmount = data.stars - 2;
                }
                else
                {
                    star1.fillAmount = 1;
                    star2.fillAmount = 1;
                    star3.fillAmount = 1;
                }


                /*
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
                */
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
        Game.Levels.Load(i, ComplLoaded);
        void ComplLoaded()
        {
            Close();
        }
        
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
