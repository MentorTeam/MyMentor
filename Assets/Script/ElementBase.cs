﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementBase : MonoBehaviour {

    LevelManager levelManager;
    [System.Serializable]
    public enum StateAction
    {
        None,
        Next,
        Complete,
        Fail
    }
    [HideInInspector]
    public Animator ani;

    // Use this for initialization
    virtual public void Awake()
    {
        IntiElement();
    }

    //初始化当前状态
    public void IntiElement()
    {
        //获取关卡管理器
        levelManager = transform.Find("/Main Camera").GetComponent<LevelManager>();
        if (levelManager == null)
        {
            Debug.Log("获取不到关卡管理器! 物件名称为：" + transform);
            return;
        }

        Debug.LogFormat("物件<color=blue> {0} </color>初始化完成！", transform.name);
    }

    //确定是否下一步
    public void CheckAction(StateAction action)
    {
        if (action == StateAction.Next)
        {
            levelManager.AddNowState();
        }
        else if (action == StateAction.Complete)
        {
            levelManager.CompleteLevel();
        }
        else if (action == StateAction.Fail)
        {
            levelManager.FailLevel();
        }
    }

    public LevelManager GetLevelManager()
    {
        return levelManager;
    }
}
