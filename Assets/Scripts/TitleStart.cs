using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleStart : MonoBehaviour
{
    [SerializeField]
    private GameObject stageSelectButtonPrefab;
    
    void Start()
    {
        for (int i = 1; i <= 5; i ++)
        {
            var contents = GameObject.Find("StageSelectContent");
            var line = Instantiate(stageSelectButtonPrefab, contents.transform, true);
            line.transform.Find("Button/Text").gameObject.GetComponent<Text>().text = string.Format("Stage {0}", i);
            var clickedEvent = new Button.ButtonClickedEvent();
            var id = i;
            clickedEvent.AddListener(() =>
            {
                SELECT_STAGE_ID_KEY = id;
                SceneManager.LoadSceneAsync(DefineScene[Scene.GAME], LoadSceneMode.Single);
            });
            line.transform.Find("Button").gameObject.GetComponent<Button>().onClick = clickedEvent;
        }
    }

    
    public enum Scene
    {
        TITLE,
        GAME,
    } 
    
    public static Dictionary<Scene, string> DefineScene = new Dictionary<Scene, string>()
    {
        { Scene.TITLE , "Assets/Scenes/_Title.unity" },
        { Scene.GAME , "Assets/Scenes/Game.unity" }
    };

    public static int SELECT_STAGE_ID_KEY = 0;
}
