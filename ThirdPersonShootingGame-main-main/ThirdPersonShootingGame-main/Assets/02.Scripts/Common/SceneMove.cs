using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneMove : MonoBehaviour
{
   
    public void PlayGame()
    {
        SceneManager.LoadScene("Level");
        SceneManager.LoadScene("ThridPersonCtrlScene", LoadSceneMode.Additive);

    }
    public void QuitGame()
    {
#if UNITY_EDITOR
      UnityEditor.EditorApplication.isPlaying = false;
#else
   Application.Quit();
#endif


    }
}
