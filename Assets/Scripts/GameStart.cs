using System;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class GameStart : MonoBehaviour
    {
        [SerializeField]
        private GameObject _linePrefab;

        [SerializeField] private string[] _previewAlgorithmList = new string[] { };


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
        }
        
    }
}