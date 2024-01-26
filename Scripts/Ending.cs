using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ending : MonoBehaviour
{
    [SerializeField]
    public GameObject win ,lose;
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        if (CharacterControl.health<=0)//checks static var to determine win or lose
            win.SetActive(false);
        else
            lose.SetActive(false);
    }
    public void quit(){
        Application.Quit();
    }

    public void newG(){
        UnityEngine.SceneManagement.SceneManager.LoadScene("Opening");
    }
    public void tryAgain(){
        UnityEngine.SceneManagement.SceneManager.LoadScene("Maze");
    }
}
