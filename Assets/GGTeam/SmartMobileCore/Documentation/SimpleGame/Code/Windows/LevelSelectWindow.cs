// ====================================
// Simple UI Window for SmartMobileCore
// ====================================

using GGTeam.SmartMobileCore;
using GGTeam.Tools.Tween;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectWindow : UIScreen
{
    public bool showScore = true;
    [Space(16)]
    public GameObject CellPref = null;
    public Transform CellContainer = null;
    public Sprite ImageCompleted;
    public Sprite ImagePlayed;
    readonly List<GameObject> rowListGo = new List<GameObject>();
    //bool inited = false;
    
    public override void OnInit()
    {
        
    }

    void RenderRows()
    {
        ClearAllRows();
        CellPref.SetActive(false);

        for (int i = 1; i <= Game.Levels.Count; i++)
        {
            GameObject go = Instantiate(CellPref, CellContainer);
            rowListGo.Add(go);
            go.SetActive(true);
            Transform normalTr = go.transform.Find("Normal");
            Transform lockTr = go.transform.Find("Lock");
            Text textLevel = normalTr.Find("TextLevel").GetComponent<Text>();
            Text textScore = normalTr.Find("TextScore").GetComponent<Text>();
            Button btn = normalTr.Find("Button").GetComponent<Button>();
            Image btnImg = btn.gameObject.GetComponent<Image>();
            Transform imageBottomTr = normalTr.Find("ImageBottom");
            Image star1 = imageBottomTr.Find("Star1").GetComponent<Image>();
            Image star2 = imageBottomTr.Find("Star2").GetComponent<Image>();
            Image star3 = imageBottomTr.Find("Star3").GetComponent<Image>();

            var data = Game.Levels.LevelData(i);

            if (data.completed || data.opened)
            {
                normalTr.localScale = new Vector3(0, 0, 1);
                Tween.TweenVector3((v) => { normalTr.localScale = v; }, new Vector3(0, 0, 1), new Vector3(1, 1, 1), 0.25f, (i - 1) * 0.05f);

                if (data.completed && data.opened)
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
                if (showScore && data.score > 0) textScore.text = data.score.ToString(); else textScore.text = "";
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
            }
            else
            {
                lockTr.localScale = new Vector3(0, 0, 1);
                Tween.TweenVector3((v) => { lockTr.localScale = v; }, new Vector3(0, 0, 1), new Vector3(1, 1, 1), 0.15f, (i * 0.25f - 1) * 0.05f);

                normalTr.gameObject.SetActive(false);
                lockTr.gameObject.SetActive(true);
            }
        }
    }

    public void StartLevel(int i)
    {
        Game.Levels.Load(i, ComplLoaded);
        void ComplLoaded()
        {
            Close();
        }
        
    }

    void ClearAllRows()
    {
        List<GameObject> tmp = new List<GameObject>(rowListGo);
        int i = 0;
        foreach (var item in tmp)
        {
            if (tmp[i] != null) Destroy(tmp[i]);
            i++;
        }
        rowListGo.Clear();

    }


    public void OnBtnContinue()
    {
        Game.Levels.LoadNext();
    }

    public void OnBtnRestart()
    {
        Game.Levels.LoadCurrent();
    }

    public override void OnClose()
    {

    }

    public override void OnOpen()
    {
        RenderRows();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Close();
        }
    }


}
