using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectActionButtonScript : MonoBehaviour
{
    private Action<string> _callback = null;

    public Action<string> Callback
    {
        set => _callback = value;
    }


    public void OnPlayStart()
    {
        // ゲームが開始されたらボタンを無効化する。
        var actionButtons = GetComponentsInChildren<Button>();
        foreach (var actionButton in actionButtons)
        {
            actionButton.enabled = false;
            actionButton.interactable = false;
        }
    }

    public static Dictionary<string, string> DefineButtons = new Dictionary<string, string>()
    {
        {"Walk", "前に進む"},
        {"TurnRight", "右を向く"},
        {"TurnLeft", "左を向く"},
        {"Jump", "ジャンプ"},
        {"Treasure", "ひろう"},
    };

    public void OnEditStart()
    {
        // 編集モードが開始されたらボタンを有効化する。
        var actionButtons = GetComponentsInChildren<Button>();
        foreach (var actionButton in actionButtons)
        {
            actionButton.enabled = true;
            actionButton.interactable = true;

            var clickedEvent = new Button.ButtonClickedEvent();
            clickedEvent.AddListener(() =>
            {
                EventCatch(actionButton.name);
            });
            actionButton.onClick = clickedEvent;
        }
    }

    private void EventCatch(string buttonName)
    {
        Debug.Log(buttonName);
        if (_callback != null)
        {
            _callback(buttonName);
        }
    }
}
