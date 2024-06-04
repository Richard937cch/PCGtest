using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LayerGen : MonoBehaviour
{
    public GameObject Player;

    //private int[,,] Grid;
    private Grid3D Grid;
    private Vector3 spawnpoint;
    private Vector3 startpos;
    private Queue<Vector3> gridQueue;

    public int Seed = 1234;

    public int width = 100;
    public int height = 100;

    public int depth = 2;

    public GameObject hallway;

    public GameObject roomTile;

    public GameObject token;

    public int branchCount =2;


    // Start is called before the first frame update
    void Start()
    {
        Random.InitState(Seed);
        gridQueue = new Queue<Vector3>();
        
        Grid = new Grid3D(width, depth, height);
        Grid.InitializeGrid();


        startpoint();
        GameObject player = Instantiate(Player, spawnpoint, Quaternion.identity);

        MapGen();
        tileGen();

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void startpoint()
    {
        spawnpoint = new Vector3(Random.Range(3,width),0.58f,0);
        startpos = new Vector3(spawnpoint.x, 0, 0);
        gridQueue.Enqueue(new Vector3(spawnpoint.x, 0, 0));
        Grid[startpos] = 1;
    }

    void MapGen()
    {
        
         while (gridQueue.Count > 0 && branchCount > 0)
         {
            print(gridQueue.Count);
            Vector3 nextPos = gridQueue.Dequeue();
             switch (Grid[nextPos])
             {
                case 1:
                    print("forward");
                    forward(nextPos);
                    branchCount--;
                    break;
                case 2:
                    print("backward");
                    backward(nextPos);
                    branchCount--;
                    break;
                case 3:
                    print("right");
                    right(nextPos);
                    branchCount--;
                    break;
                case 4:
                    print("left");
                    left(nextPos);
                    branchCount--;
                    break;
                default:
                    break;
             }
         }

        List<Vector3> cellsWithValue5 = Grid.FindCellsWithValue(5);
        List<Vector3> randomCells = Grid.PickRandomCells(cellsWithValue5, 3);
        foreach (Vector3 cell in randomCells)
        {
            for (int i=1;i<5;i++)
            {
                for (int j=1;j<5;j++)
                {
                    if (Grid[new Vector3(cell.x-i,cell.y,cell.z-j)]==0)
                    {
                        Grid[new Vector3(cell.x-i,cell.y,cell.z-j)]=7;
                    }
                }
            }
            //Grid[cell] = 7;
        }

        List<Vector3> cellsWithValue8 = Grid.FindCellsWithValue(7);
        List<Vector3> randomCells8 = Grid.PickRandomCells(cellsWithValue8, 5);
        foreach (Vector3 cell in randomCells8)
        {
            
            Grid[cell] = 8;
        }
    }

    void tileGen()
    {
        Quaternion rotation = Quaternion.Euler(90, 0, 0);
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                for (int y = 0; y < depth; y++)
                {
                    if (Grid[x,y,z]!=0 && Grid[x,y,z]!=7 && Grid[x,y,z]!=8)
                    {
                        GameObject newhallway = Instantiate(hallway, new Vector3(x, y, z), rotation);
                        newhallway.transform.parent = transform;
                    }
                    if (Grid[x,y,z]==7 || Grid[x,y,z]==8)
                    {
                        GameObject newroom = Instantiate(roomTile, new Vector3(x, y, z), rotation);
                        newroom.transform.parent = transform;
                    }
                    if (Grid[x,y,z]==8)
                    {
                        GameObject newtoken = Instantiate(token, new Vector3(x, y+0.1f, z), rotation);
                        newtoken.transform.parent = transform;
                    }
                }
            }
        }
    }
    void forward(Vector3 nextPos)
    {
        int h = Random.Range((int)nextPos.z+2,height-2);
        Vector3 newPos = new Vector3(nextPos.x,nextPos.y,h); // new branch point
        print (nextPos);
        print (newPos);
        //if (Grid[nextPos]==1) print (Grid[nextPos]);
        //Grid[newPos] = 1;
        setgrid(newPos,1);
        for (int i = (int)nextPos.z+1; i<h; i++) // fill the hallway
        {
            setgrid(new Vector3(nextPos.x-1,nextPos.y,i),5); 
            setgrid(new Vector3(nextPos.x+1,nextPos.y,i),5);
            setgrid(new Vector3(nextPos.x,nextPos.y,i),1);  
        }

        //left?
        ll(newPos);
        //right?
        rr(newPos);
        //up?
        uu(newPos);

        Grid[new Vector3(newPos.x-1,newPos.y,newPos.z+1)] = 6;
        Grid[new Vector3(newPos.x+1,newPos.y,newPos.z+1)] = 6;

    }

    void backward(Vector3 nextPos)
    {
        int h = Random.Range(2,(int)nextPos.z-1);
        Vector3 newPos = new Vector3(nextPos.x,nextPos.y,h); // new branch point
        print (nextPos);
        print (newPos);
        setgrid(newPos,2);
        for (int i = (int)nextPos.z-1; i>h; i--) // fill the hallway
        {
            setgrid(new Vector3(nextPos.x-1,nextPos.y,i),5); 
            setgrid(new Vector3(nextPos.x+1,nextPos.y,i),5);
            setgrid(new Vector3(nextPos.x,nextPos.y,i),2);  
        }

        //left?
        ll(newPos);
        //right?
        rr(newPos);
        //down?
        dd(newPos);

        Grid[new Vector3(newPos.x-1,newPos.y,newPos.z-1)] = 6;
        Grid[new Vector3(newPos.x+1,newPos.y,newPos.z-1)] = 6;
    }

    void left(Vector3 nextPos)
    {
        int w = Random.Range(2,(int)nextPos.x-1);
        Vector3 newPos = new Vector3(w,nextPos.y,nextPos.z); // new branch point
        print (nextPos);
        print (newPos);
        setgrid(newPos,4);
        for (int i = (int)nextPos.x-1; i>w; i--) // fill the hallway
        {
            setgrid(new Vector3(i,nextPos.y,nextPos.z-1),5); 
            setgrid(new Vector3(i,nextPos.y,nextPos.z+1),5);
            setgrid(new Vector3(i,nextPos.y,nextPos.z),4);  
        }

        //left?
        ll(newPos);
        //up?
        uu(newPos);
        //down?
        dd(newPos);

        Grid[new Vector3(newPos.x-1,newPos.y,newPos.z-1)] = 6;
        Grid[new Vector3(newPos.x-1,newPos.y,newPos.z+1)] = 6;
    }

    void right(Vector3 nextPos)
    {
        int w = Random.Range((int)nextPos.x+2,width-2);
        Vector3 newPos = new Vector3(w,nextPos.y,nextPos.z); // new branch point
        print (nextPos);
        print (newPos);
        setgrid(newPos,4);
        for (int i = (int)nextPos.x+1; i<w; i++) // fill the hallway
        {
            setgrid(new Vector3(i,nextPos.y,nextPos.z-1),5); 
            setgrid(new Vector3(i,nextPos.y,nextPos.z+1),5);
            setgrid(new Vector3(i,nextPos.y,nextPos.z),4);  
        }

        //right?
        rr(newPos);
        //up?
        uu(newPos);
        //down?
        dd(newPos);

        Grid[new Vector3(newPos.x+1,newPos.y,newPos.z-1)] = 6;
        Grid[new Vector3(newPos.x+1,newPos.y,newPos.z+1)] = 6;
    }

    void uu(Vector3 newPos)
    {
        if (Random.Range(0,3) > 0 && newPos.z+5<=height)
        {
            setgrid(new Vector3(newPos.x,newPos.y,newPos.z+1),1);
            gridQueue.Enqueue(new Vector3(newPos.x,newPos.y,newPos.z+1));
            print("u");
        }
        else
        {
            setgrid(new Vector3(newPos.x,newPos.y,newPos.z+1),6);
        }
    }

    void dd(Vector3 newPos)
    {
        if (Random.Range(0,3) > 0 && newPos.z-5>0)
        {
            setgrid(new Vector3(newPos.x,newPos.y,newPos.z-1),2);
            gridQueue.Enqueue(new Vector3(newPos.x,newPos.y,newPos.z-1));
            print("d");
        }
        else
        {
            setgrid(new Vector3(newPos.x,newPos.y,newPos.z-1),6);
        }
    }

    void ll(Vector3 newPos)
    {
        if (Random.Range(0,3) > 0 && newPos.x-5>0)
        {
            setgrid(new Vector3(newPos.x-1,newPos.y,newPos.z),4);
            gridQueue.Enqueue(new Vector3(newPos.x-1,newPos.y,newPos.z));
            print("l");
        }
        else
        {
            setgrid(new Vector3(newPos.x-1,newPos.y,newPos.z),6);
        }
    }

    void rr(Vector3 newPos)
    {
        if (Random.Range(0,3) > 0 && newPos.x+5<=width)
        {
            setgrid(new Vector3(newPos.x+1,newPos.y,newPos.z),3);
            gridQueue.Enqueue(new Vector3(newPos.x+1,newPos.y,newPos.z));
            print("r");
        }
        else
        {
            setgrid(new Vector3(newPos.x+1,newPos.y,newPos.z),6);
        }
    }

    void setgrid(Vector3 vec,int value) 
    {
        
        if (Grid[vec]==0)
        {
            //print (Grid[vec]);
            Grid[vec] = value;
            //print (Grid[vec]);
        } 
        
        else Grid[vec] = 6;
    }

    


}
