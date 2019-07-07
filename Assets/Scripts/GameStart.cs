using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class GameStart : MonoBehaviour
    {
        private void Start()
        {
            GameObject.Find("SelectActionButton").GetComponent<SelectActionButtonScript>().OnEditStart();
        }
    }
}