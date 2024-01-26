using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;
using UnityEngine.UI;

public class blobAI : MonoBehaviour
{
    [SerializeField]
    public GameObject player,healthbar;
    [SerializeField]
    public CharacterControl playerStat;
    public int state,health;
    [SerializeField]
    private Animator anim;
    
    // private Color angy, idle, blob;
    [SerializeField]
    public Image healthfill;
    private float healthcurr,timer;

    private bool enter,inHitRange,inActiveRange,gotPoint, attack;//leave,
    [SerializeField]
    public LayerMask pLayer;// p(layer) pun :)
    private Vector3 point;

    UnityEngine.AI.NavMeshAgent agent;
    void Start()
    {
        state = 0;
        health = 3;
        enter = true;
        // leave = true;
        this.gameObject.transform.GetChild(0).gameObject.SetActive(false);// sets blob component off, leaves the butterfly sprite
        this.gameObject.transform.GetChild(1).gameObject.SetActive(false);
        // this.gameObject.transform.GetChild(3).gameObject.SetActive(false);
        // atkhitbox.enabled=false;
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        timer = 0;

    }

    // Update is called once per frame
    void Update()
    {
        inHitRange=Physics.CheckSphere(transform.position, 0.25f, pLayer);
        inActiveRange=Physics.CheckSphere(transform.position, 10f, pLayer);
        if(health ==0 ){
            gameObject.SetActive(false);//it's dead
        }
        if (state== 0){//idle state
            this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
            patrol();

        }

        if (inActiveRange){//this basically replaced state 1 thats why theres no state one. this is active passive state
            if(!inHitRange&&playerStat.state>0){
                agent.destination = player.transform.position; //seeks player
            }
            if(inHitRange){
                agent.destination = transform.position; //stops so it doesn't clip thru u
            }
            if (playerStat.state>0&&inSight()){//aggro state if they see u
                state = 2;
            }

            if (playerStat.state==0&&inSight()){// this is for when u shift, returns to passive state
                patrol();
            }
            if (enter){ // sets enter trasformation so it only runs once

                this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
                anim.SetTrigger("near");       
                waitEnd();
                enter=false;
            }
            if (!Physics.CheckSphere(transform.position, 10f, pLayer)){ //for going to state 0
                state =0;
                enter = false;
            }

        }        
        if(state==2){//aggro state
            this.gameObject.transform.GetChild(0).gameObject.SetActive(false);// graphical stuff
            this.gameObject.transform.GetChild(1).gameObject.SetActive(true);
            this.gameObject.transform.GetChild(3).gameObject.SetActive(true);
            healthbar.transform.LookAt(player.transform);
            healthcurr = healthfill.fillAmount;// healthbar update
            healthfill.fillAmount = Mathf.Lerp(healthcurr, health/3f, Time.deltaTime*10);
            if (CharacterControl.passiveTimer<0){
                state=0;
            }
            if (!attack){// attack cool down so u don't instantly die
                timer-=Time.deltaTime;

            }
            if(inHitRange){
                agent.destination = this.transform.position;
                attack = true;
                if (timer<=0){
                    if(Physics.CheckSphere(transform.position, 0.2f, pLayer)){
                        CharacterControl.health -=19;
                        timer=2f;
                    }
                }
                if (timer>0){
                    attack=false;
                }
            
            
        }
    }
    }
    // Ignore this


    // void OnTriggerEnter(Trigger other){
    //     if (other.gameObject.CompareTag("player")){
    //         state = 1;
    //         transitionIn(1.4f);
    //         agent.destination = other.transform.position;
    //     }
    // }
    // IEnumerator transitionIn(float duration){//failes animation :)
    //     float time= 0;//to store the time
    //     Color blobS = blob;
    //     Color idleS = idle;
    //     while (blob.a>0)
    //     {
    //         blob = Color.Lerp(blobS, blobS + new Color (0,0,0,256), time/duration);
    //         idle = Color.Lerp(idleS, idleS - new Color (0,0,0,256), time/duration);
    //         time += Time.deltaTime;
    //         yield return null;
    //     }
    //         blob = blobS + new Color (0,0,0,256);
    //         idle = idleS - new Color (0,0,0,256);
    //         this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
    //         this.gameObject.transform.GetChild(2).gameObject.SetActive(false);
    // }
    private bool inSight(){// uses raycast to check if they can "see" you
        Vector3 direction= player.transform.position - this.transform.position;
        float angle = Vector3.Angle(direction,this.transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(this.transform.position, direction, out hit,5f))
        {

            return hit.collider.gameObject.CompareTag("Player") && angle < 110f;
        }
        else
        {
            return false;
        }
    }
    IEnumerator waitEnd()// timer
    {
        yield return new WaitForSeconds(2);
        this.gameObject.transform.GetChild(2).gameObject.SetActive(false);
    }
    private void patrol(){ // if i say heavily inspired by harrison would u dock points
        if(!gotPoint||!agent.hasPath){// searches point if theres no destination or valid path
            searchPoint();
        }
        if (gotPoint&&agent.hasPath){//sets destination if there is valid point and path
            agent.destination = point;
        }
        Vector3 distance = transform.position-point;
        if (distance.magnitude<1f)// gets another point once point is nearly reached
        gotPoint=false;
    }
    private void searchPoint(){
        Vector3 ranPoint = transform.position + Random.insideUnitSphere*10;
        UnityEngine.AI.NavMeshHit hit;// makes sure hit is on navMesh
        UnityEngine.AI.NavMeshPath path = new UnityEngine.AI.NavMeshPath();// new path
        if (UnityEngine.AI.NavMesh.SamplePosition(ranPoint, out hit, 1f, UnityEngine.AI.NavMesh.AllAreas)&&UnityEngine.AI.NavMesh.CalculatePath(transform.position,hit.position,UnityEngine.AI.NavMesh.AllAreas, path)){
            point = hit.position;// if point exists and path exists, point = hit.position and agent.path = new path and gotPoint == true
            agent.path=path;
            gotPoint=true;
        }
        
    }
    
    // I HIGHLY DISLIKE COLLIDERS
    // THEY SUCK

    // void OnColliderEnter(Collision other){ 
    //         if (timer<=0){
    //             attack=true;
    //         }
    //         if (attack &&other.gameObject.CompareTag("Player")){
    //         playerStat.health -=23;
    //         timer = 1f;
    //         attack=false;

    //     }
    // }
    // void OnColliderStay(Collision other){
    //         if (attack &&other.gameObject.CompareTag("Player")){
    //         playerStat.health -=23;
    //         timer = 1f;
    //         attack=false;

    //     }

    // }
}


