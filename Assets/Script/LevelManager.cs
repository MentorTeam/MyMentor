﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

    int NowState = 1;   //当前状态，不同的状态对应不同的操作
    int level = 0;      //关卡id

    //游戏状态
    public enum LevelStateType
    {
        PlayAnimation,
        Common
    }
    LevelStateType LevelState = LevelStateType.Common;

    ArrayList Bag = new ArrayList();    //背包

    public GameObject CompleteBoard;
    public GameObject FailBoard;
    private RectTransform BagUI;

    private void Awake()
    {
        IntiManager();
    }

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //初始化当前状态
    void IntiManager()
    {
        NowState = 1;
        Bag = new ArrayList();
        BagUI = transform.Find("/GameCanvas/UIlayer/bagUI") as RectTransform;

        if (!int.TryParse(SceneManager.GetActiveScene().name, out level))
            Debug.Log("关卡名称错误：" + SceneManager.GetActiveScene().name);
        Debug.LogFormat("初始化关卡<color=green> {0} </color>成功！", level);  
    }

    void AddItemToBag(int id)
    {
        if (!Bag.Contains(id))
        {
            Bag.Add(id);
        }
        else
        {
            Debug.Log("已经包含了这个道具: " + id);
        }
    }

    public int GetNowState()
    {
        return NowState;
    }

    public int AddNowState()
    {
        Debug.Log("当前状态: " + (NowState + 1));
        return NowState++;
    }

    public void CompleteLevel()
    {
        GameObject cp = Instantiate(CompleteBoard);
        cp.transform.SetParent(transform.Find("/GameCanvas/UIlayer"), false);

        EventTriggerListener.Get(cp.transform.Find("RetryButton")).onClick = GameManager.ReloadNowLevel;
        EventTriggerListener.Get(cp.transform.Find("NextButton")).onClick = GameManager.NextLevel;   
    }

    public void FailLevel()
    {
        GameObject fp = Instantiate(FailBoard);
        fp.transform.SetParent(transform.Find("/GameCanvas/UIlayer"), false);
        EventTriggerListener.Get(fp.transform.Find("RetryButton")).onClick = GameManager.ReloadNowLevel;
    }

    public bool isPlayingAnimation()
    {
        if (LevelState == LevelStateType.PlayAnimation)
            return true;
        else
            return false;
    }

    public void AddItemInBagUI(GameObject item)
    {
        ItemElement itemscript = item.GetComponent<ItemElement>();
        if (itemscript != null) Destroy(itemscript);

        int childcount = BagUI.childCount;
        item.transform.SetParent(BagUI);
        RectTransform trans = item.transform as RectTransform;
        float ratio = 120 / trans.rect.width;
        trans.sizeDelta = new Vector2(trans.rect.size.x * ratio, trans.rect.size.y * ratio);

        MoveToCenter moveeffect = item.AddComponent<MoveToCenter>();
        moveeffect.SetLocalPos(GetBagPos(childcount));
        moveeffect.SetSpeed(3f);
        moveeffect.SetCallback(() =>
        {
            trans.localPosition = GetBagPos(childcount);
            item.AddComponent<ItemDragEffect>();
        });
    }

    Vector2 GetBagPos(int count)
    {
        count = count + 1;
        Vector2 pading = new Vector2(15, 45);
        Vector2 pos = new Vector2((120 + pading.x) * count - 60, 0);
        return pos;
    }


    public LevelStateType GetLevelState()
    {
        return LevelState;
    }

    public void SetLevelState(LevelStateType _levelstate)
    {
        LevelState = _levelstate;
    }
}
