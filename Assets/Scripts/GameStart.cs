using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameStart : MonoBehaviour
{
    [SerializeField]
    private GameObject _linePrefab;
    
    private List<int> _algorithmList = new List<int>();
    private Dictionary<int, string> _hashList = new Dictionary<int, string>();

    private SelectActionButtonScript _selectActionButtonScript = null;
    private Map2dStart _map2d = null;
    
    private enum State
    {
        Play,
        Edit,
    }

    private State current = State.Edit;

    private void Start()
    {
        MapReset();
    }

    private void MapStart()
    {
        {
            var button = GameObject.Find("StartButton").GetComponent<Button>();
            var clickedEvent = new Button.ButtonClickedEvent();
            clickedEvent.AddListener(() =>
            {
                MapReset();
            });
            button.onClick = clickedEvent;
        }
        
        _selectActionButtonScript = GameObject.Find("SelectActionButton").GetComponent<SelectActionButtonScript>();
        _selectActionButtonScript.OnPlayStart();

        _map2d = GameObject.Find("Map2d").GetComponent<Map2dStart>();
        _map2d.SetMapData(MapDatabase.LoadMapDataByStageId(1));
        _map2d.PlayStart(_algorithmList.Select((id => _hashList[id])).ToList());
        current = State.Play;
    }
    
    private void MapReset()
    {
        {
            var button = GameObject.Find("StartButton").GetComponent<Button>();
            var clickedEvent = new Button.ButtonClickedEvent();
            clickedEvent.AddListener(() =>
            {
                MapStart();
            });
            button.onClick = clickedEvent;
        }

        _selectActionButtonScript = GameObject.Find("SelectActionButton").GetComponent<SelectActionButtonScript>();
        _selectActionButtonScript.OnEditStart();
        _selectActionButtonScript.Callback = GetSelectActionButtonEvent;

        _map2d = GameObject.Find("Map2d").GetComponent<Map2dStart>();
        _map2d.SetMapData(MapDatabase.LoadMapDataByStageId(1));
        current = State.Edit;
    }
        
    private void GetSelectActionButtonEvent(string buttonName)
    {
        var actionString = SelectActionButtonScript.DefineButtons[buttonName];
        var contents = GameObject.Find("AlgorithmListContents");
        var line = Instantiate(_linePrefab, contents.transform, true);
        line.transform.Find("Image/ActionText").gameObject.GetComponent<Text>().text = actionString;

        var uniqueId = line.GetHashCode();
        line.name = uniqueId.ToString();
            
        {
            var button = line.transform.Find("UpButton").gameObject.GetComponent<Button>();
            var clickedEvent = new Button.ButtonClickedEvent();
            clickedEvent.AddListener(() =>
            {
                MoveButton(uniqueId, -1);
            });
            button.onClick = clickedEvent;
        }
        {
            var button = line.transform.Find("DownButton").gameObject.GetComponent<Button>();
            var clickedEvent = new Button.ButtonClickedEvent();
            clickedEvent.AddListener(() =>
            {
                MoveButton(uniqueId, +1);
            });
            button.onClick = clickedEvent;
        }
        {
            var button = line.transform.Find("TrashButton").gameObject.GetComponent<Button>();
            var clickedEvent = new Button.ButtonClickedEvent();
            clickedEvent.AddListener(() =>
            {
                TrashButton(uniqueId);
            });
            button.onClick = clickedEvent;
        }
            
        _algorithmList.Add(uniqueId);
        _hashList[uniqueId] = buttonName;
    }

    private void MoveButton(int id, int move)
    {
        if (current == State.Play)
        {
            return;
        }
        
        // view
        {
            var target = GameObject.Find(id.ToString()); 
            var siblingIndex = target.transform.GetSiblingIndex();

            if (siblingIndex + move < 0)
            {
                return;
            }
            
            target.transform.SetSiblingIndex(siblingIndex + move);
        }
        // data
        {
            var nowIndex = _algorithmList.IndexOf(id);
            _algorithmList.RemoveAt(nowIndex);
            var nextIndex = Math.Min(nowIndex + move, _algorithmList.Count); 
            _algorithmList.Insert(nextIndex, id);
                
        }
    }
        
    private void TrashButton(int id)
    {
        if (current == State.Play)
        {
            return;
        }

        // view
        {
            var target = GameObject.Find(id.ToString()); 
            Destroy(target);
        }
        // data
        {
            _algorithmList.RemoveAt(_algorithmList.IndexOf(id));
            _hashList.Remove(id);
        }
    }
        
}