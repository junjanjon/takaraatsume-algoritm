using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class GameStart : MonoBehaviour
    {
        [SerializeField]
        private GameObject _linePrefab;

        [SerializeField] private string[] _previewAlgorithmList = new string[] { };


        private List<int> _algorithmList = new List<int>();
        private Dictionary<int, string> _hashList = new Dictionary<int, string>();

        private SelectActionButtonScript _selectActionButtonScript = null;

        private void Start()
        {
            _selectActionButtonScript = GameObject.Find("SelectActionButton").GetComponent<SelectActionButtonScript>();
            _selectActionButtonScript.OnEditStart();
            _selectActionButtonScript.Callback = GetSelectActionButtonEvent;
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
}