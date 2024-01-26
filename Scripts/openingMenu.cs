using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class openingMenu : MonoBehaviour
{
    [SerializeField]
    public Button start, instr, back; // assigns buttons, I'm too lazy to call for them in script so drag it from editor
    [SerializeField]
    public GameObject Menu, Instr, blob, hand, shine, main,screen;
    [SerializeField]
    public CanvasRenderer whiteScreen; // this is suppoused to be full screen white but noooooo

    void Start()
    {
        Instr.SetActive(false); // menus are no longer scenes and are gameobjects within a scene, this definetly improved loading time
        // blob.SetActive(false);
        hand.SetActive(false);
        main.SetActive(false);
        screen.SetActive(false);
    }
    public void goToInstr(){
        Menu.SetActive(false);
        Instr.SetActive(true);
    }

    public void goToMenu(){
        Menu.SetActive(true);
        Instr.SetActive(false);
    }

    public void startGame(){
        Menu.SetActive(false);
        Instr.SetActive(false);
        blob.SetActive(true);
        hand.SetActive(true);
        main.SetActive(true);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Maze");
        // animations();
    }
    private void animations(){
        screen.SetActive(true);
        StartCoroutine(Fade(5f));
        Debug.Log('1');
        StartCoroutine(wait());

    }
    IEnumerator Fade(float duration)//doesn't work :/
    {
      Debug.Log('2');
        float time = 0;
        while (time < duration)
        {
                  Debug.Log('3');
            whiteScreen.SetAlpha(Mathf.Lerp(254, 0, time / duration));
            time += Time.deltaTime;
            yield return null;
        }
        whiteScreen.SetAlpha(0);
    }
    IEnumerator wait(){
        yield return new WaitForSeconds(5);
        main.GetComponent<Animator>().SetTrigger("transition");
        bool run = true;//argument so this only runs once
        if (run==true){
        run=false;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Maze");
        }
    }
}
