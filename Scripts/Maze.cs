using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;

public class Maze : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject[] cells;
    private GameObject[,] cellnbr;
    [SerializeField]
    int length;
    [SerializeField]
    float gap;
    [SerializeField]
    public GameObject cellBase;
    [SerializeField]
    public NavMeshSurface NavMesh;
    [SerializeField]
    public Transform mesh;
   void Start()
    {   
        //gen(cells[0], Random.Range(0,3));
        cells = new GameObject[length*length];
        cellnbr = new GameObject[length*length,4];
        populate();
        gen(0);
        NavMesh.BuildNavMesh();
        cells[length/2].transform.GetChild(3).gameObject.SetActive(false);
        cells[length*length-(length/2)].transform.GetChild(4).gameObject.SetActive(false);

    }
    public void populate()
    {

        for (int i=0; i < length * length; i++)// creating cells
        {
            GameObject cellb = Instantiate(cellBase, new Vector3( i%length *gap, 1.5f,-i / length *gap), Quaternion.Euler(0, 0, 0),mesh); //base
            cells[i] = cellb;
        }
        for (int i = 0; i < length * length; i++)// assigning neighbors to each cell via a seperate array
        {
            if (i - 1 > -1 && (i/length==(i-1)/length))//left nbr
            {
                cellnbr[i,0]=cells[i - 1];
            }
            if (i + 1 < length*length && (i /length == (i + 1) / length))//right nbr
            {
                cellnbr[i,1] = cells[i + 1];
            }
            if (i-length > -1)//top nbr
            {
                cellnbr[i, 2] = cells[i-length];
            }
            if (i+length<length*length)//bottom nbr
            {
                cellnbr[i, 3] = cells[i+length];
            }
        }
    }
    public void gen(int i)
    {   
        cells[i].transform.GetChild(0).gameObject.SetActive(false);//turns visited off
        if (!(i<0||i>length*length)&&allNbrVisited(i)){//breaks loop if no unvisited nbr
            return;
        }
        if( i<0||i>length*length)
        {//breaks if i is greater than array (technically not needed because the while loop gets rid of it already)
            return;
        }       
        // if (allNbrVisited(i)){
        //     return;
        // }
        while (!allNbrVisited(i)){
        ArrayList unvisited = new ArrayList();
        for (int j = 0; j<4;j++){
            if(!visited(cellnbr[i,j])){
                unvisited.Add(j);
            }
        }

        //  while (visited(cellnbr[i,ran]))//random number while cell neigbor doesn't exist or is visited
        //  {
        //     ran = Random.Range(0, 3);
        //  }
        // while (unvisited.Count!= 0){
        int ran = Random.Range(0,unvisited.Count);
        ran = (int)unvisited[ran];
        // unvisited.Remove(ran);
        
        cells[i].transform.GetChild(ran+1).gameObject.SetActive(false);//turns wall off
        if (ran==0){//if left wall turn off next cell's right wall
            // Debug.Log("0");
            cellnbr[i,0].transform.GetChild(ran+2).gameObject.SetActive(false);
            gen(i-1);
        }
        if (ran==2){//if top wall, turn off next bottom
            // Debug.Log("2");
            cellnbr[i,2].transform.GetChild(ran+2).gameObject.SetActive(false);
            gen(i-length);
        }
        if (ran==1){//if right wall, turn off next left
            // Debug.Log("1");
            cellnbr[i,1].transform.GetChild(ran).gameObject.SetActive(false);
            gen(i+1);
        }
        if (ran==3){//if bottom, turn off next top
            // Debug.Log("3");
            cellnbr[i,3].transform.GetChild(ran).gameObject.SetActive(false);
            gen(i+length);
        }
        }
        // }
        // return;
        //cell.
    }
    private bool allNbrVisited(int j){
        for (int i = 0; i<4; i++){
            if (!visited(cellnbr[j,i])){//if nbr is not null and not visited return false
                return false;
            }
        }
        return true;
    }
    private bool visited(GameObject cell){
        if (cell==null){
            return true;
        }
        return !cell.transform.GetChild(0).gameObject.activeSelf;//checks if nbr's visited gameobject is active or not, if active it's not visited
    }

}