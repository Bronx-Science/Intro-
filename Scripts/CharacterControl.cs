using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class CharacterControl : MonoBehaviour
{
    // I should have named this class something short like player I DID'NT THINK THIS WOULD BE THIS LARGE
    public float speed,state;
    [SerializeField][Range(0f, 20f)]
    public float sensX;// copied harrison and added mouse sensitivity ajst in editor
    [SerializeField][Range(0f, 20f)]
    public float sensY;
    [SerializeField]
    GameObject Camera, Player, Hand;
    public float x,y,ypos,h,v,timer;
    [SerializeField]
    Animator anim;
    public static float health ,passiveTimer;
    Vector3 mid ;//middle of screen
    public LayerMask end;
    void Start()
    {
        passiveTimer=5f;
        timer=0;
        state = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        speed = 1f;
        ypos= Player.transform.position.y;
        health = 100f;
        mid= new Vector3(( Camera.GetComponent<Camera>().pixelWidth-1)/2,( Camera.GetComponent<Camera>().pixelHeight-1)/2);//gets the center of screen
    }

    void Update()
    {
        timer-=Time.deltaTime;//hit timer, is always counting down 
        if (health<=0){
            UnityEngine.SceneManagement.SceneManager.LoadScene("Ending");
        }
         if (Input.GetKeyDown(KeyCode.LeftShift)){//sneak mechanics
            speed = 0.5f*speed;
            Camera.transform.position -= new Vector3(0,.5f);
            state = 0;//related to mob AI, it references this to determin state
            passiveTimer-=Time.deltaTime;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift)){
            speed = 2f*speed;
            Camera.transform.position += new Vector3(0,.5f);
            state = 1;
            passiveTimer=5f;
        }
        if (Input.GetButtonDown("Jump")){// i don't like to code jumping
            
        }
        if (Input.GetMouseButtonDown(0)){
            anim.SetTrigger("hit");
            RaycastHit hit;//uses raycast because COLLIDERS SUCK
            Ray ray = Camera.GetComponent<Camera>().ScreenPointToRay(mid);
            if (Physics.Raycast(ray, out hit,2f)&&timer<=0){
                if (hit.transform.gameObject.CompareTag("blob")){
                    hit.transform.gameObject.GetComponent<blobAI>().state = 2;
                    hit.transform.gameObject.GetComponent<blobAI>().health -=1;
                }
            }
            timer=0.5f;
        }
        if (Input.GetKeyDown(KeyCode.LeftControl)){//sprint, you will agro AI immedietly
            speed = 1.5f*speed;
            state =2;

        }
        h = Input.GetAxis("Horizontal");//A+D keys input
        v = Input.GetAxis("Vertical");//W+S keys input
        x+= Input.GetAxisRaw("Mouse X")*sensX;//mouse input
        y-= Input.GetAxisRaw("Mouse Y")*sensY;
        // x = Mathf.Clamp(x,-90f,90f);
        y= Mathf.Clamp(y,-90,90f);//clamps verticle mouse input so it doesn't go crazy
        Player.transform.Translate(new Vector3(h,0,v)*speed*Time.deltaTime);
        Player.transform.rotation = Quaternion.Euler(0,x,0);//I do not know why this is using x value but it doesn't work if I use y value, turns player
        Camera.transform.rotation = Quaternion.Euler(y,x,0);//turns camera, but not player up and down, for movement i think? i don't remember
         if(Physics.CheckSphere(transform.position, 1f, end)){// uses this because COLLODERS SUCK
            Debug.Log("work");
            UnityEngine.SceneManagement.SceneManager.LoadScene("Ending");
        }
    }

    //COLLIDERS SUCK

    // void LateUpdate(){
    //     if(Physics.CheckSphere(transform.position, 0.2f, end)){
    //         UnityEngine.SceneManagement.SceneManager.LoadScene("Ending");
    //     }
    // // }
    // void OnColliderEnter(Collision other){
    //     if (other.gameObject.CompareTag("hitbox")){
    //         health -=23;
    //     }
    //     if (other.gameObject.CompareTag("endScene")){
    //         Debug.Log("why");
    //         UnityEngine.SceneManagement.SceneManager.LoadScene("Ending");
    //     }
    // }

    public void OnGUI(){//bugged health sorry
        GUIStyle healthStyle = new GUIStyle();
        healthStyle.fontSize=60;
        GUI.Label(new Rect(10,Screen.height-50,300,500),""+health,healthStyle);
    }
}
