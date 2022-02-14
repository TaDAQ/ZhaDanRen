using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    private int X,Y;
    // public GameObject superWallPre,wallPre,doorPre,propPre,enemyPre;
    public GameObject doorPre;
    private GameObject door;
    List<Vector2> nullPoints = new List<Vector2>(); 
    List<Vector2> wallPoints = new List<Vector2>();  
    List<Vector2> superWallPoints = new List<Vector2>();
    Dictionary<ObjectType,List<GameObject>> objectPoolDic = new Dictionary<ObjectType,List<GameObject>>();
    // private void Awake() {
    //     InitMap(3,2);
    // }
    public Vector2 GetPlayerPosition(){
        return new Vector2(-(X+1),Y-1);
    }
    public void InitMap(int x,int y,int wallCount,int enemyCount){
        ResetMap();
        this.X = x;
        this.Y = y;
        CreateSuperWall(); 
        FindNullPoints();
        CreateWall(wallCount);
        CreateDoor();
        CreateProps();
        CreateEnemy(enemyCount);
        Debug.Log(nullPoints.Count);
    }
    private void ResetMap(){
        nullPoints.Clear();
        superWallPoints.Clear();
        wallPoints.Clear();
        foreach(var  item in objectPoolDic)
        {
            Debug.Log("item key="+item.Key);
            Debug.Log("item Count="+item.Value.Count);
            foreach(var obj in item.Value)
            {
                ObjectPool.Instance.Add(item.Key,obj);
            }
        }
        objectPoolDic.Clear();
    }

    private void CreateSuperWall(){
        for(int x = -X ;x < X;x+=2)
        {
            for(int y = -Y;y<Y;y+=2)
            {
                SpawnSuperWall(new Vector2(x,y));
            } 
        }

        for(int x = -(X+2);x<=X;x++)
        {
            SpawnSuperWall(new Vector2(x,Y));
            SpawnSuperWall(new Vector2(x,-Y-2));
        }
        for(int y = -(Y+1);y<=Y-1;y++)
        {
            SpawnSuperWall(new Vector2(-(X+2),y));
            SpawnSuperWall(new Vector2(X,y));
        }

    }
    public bool IsSuperWall(Vector2 position)
    {
        return superWallPoints.Contains(position);
    }
    private void SpawnSuperWall(Vector2 pos)
    {
         GameObject superWall = ObjectPool.Instance.Get(ObjectType.SuperWall,pos);
         superWallPoints.Add(pos);
         if(objectPoolDic.ContainsKey(ObjectType.SuperWall) == false)
         {
             objectPoolDic.Add(ObjectType.SuperWall,new List<GameObject>());
         }
         objectPoolDic[ObjectType.SuperWall].Add(superWall);
    }

    private void FindNullPoints()
    {
        int result = -(X+1) % 2;
        for(int x = -(X+1);x<=X-1;x++)
        {
            if(x%2 == -result || x%2 == result)
            {
                for(int y = -(Y+1);y<=Y-1;y++)
                {
                    nullPoints.Add(new Vector2(x,y));
                }
            }else
            {
                for(int y = -(Y+1);y<=Y-1;y+=2)
                {
                    nullPoints.Add(new Vector2(x,y));
                }
            }
        }
        nullPoints.Remove(new Vector2(-(X+1),Y-1));
        nullPoints.Remove(new Vector2(-(X+1),Y-2));
        nullPoints.Remove(new Vector2(-X,Y-1));
    }

    private void CreateWall(int wallCount)
    {
        if(wallCount > nullPoints.Count)
           wallCount = (int)(nullPoints.Count * 0.7f);
        for(int i =0;i<wallCount;i++)
        {
            int index = Random.Range(0,nullPoints.Count);
            GameObject wall =  ObjectPool.Instance.Get(ObjectType.Wall,nullPoints[index]);
            nullPoints.RemoveAt(index);
            if(objectPoolDic.ContainsKey(ObjectType.Wall) == false)
            {
                objectPoolDic.Add(ObjectType.Wall,new List<GameObject>());
            }
            objectPoolDic[ObjectType.Wall].Add(wall);
        }
    }
    /// <summary>
    /// 创建门
    /// </summary>
    private void CreateDoor()
    {
        int index = Random.Range(0,nullPoints.Count);
        if(door == null)door = Instantiate(doorPre,transform);
        door.GetComponent<Door>().Reset();
        door.transform.position = nullPoints[index];
        nullPoints.RemoveAt(index);
    }
    /// 创建道具
    private void CreateProps()
    {
        int count = Random.Range(0,2+(int)(nullPoints.Count*0.05f));
        for(int i = 0 ;i < count ;i++)
        {
            // GameObject prop = Instantiate(propPre,transform);
            int index = Random.Range(0,nullPoints.Count);
            // prop.transform.position = nullPoints[index];
            GameObject prop =  ObjectPool.Instance.Get(ObjectType.Prop,nullPoints[index]);
            nullPoints.RemoveAt(index);
            if(objectPoolDic.ContainsKey(ObjectType.Prop) == false)
            {
                objectPoolDic.Add(ObjectType.Prop,new List<GameObject>());
            }
            objectPoolDic[ObjectType.Prop].Add(prop);
        }
    }
    private void CreateEnemy(int count)
    {
        for(int i = 0 ;i < count ;i++)
        {
            // GameObject enemy = Instantiate(enemyPre,transform);
            int index = Random.Range(0,nullPoints.Count);
            // enemy.transform.position = nullPoints[index];
            GameObject enemy = ObjectPool.Instance.Get(ObjectType.Enemy,nullPoints[index]);
            enemy.GetComponent<EnemyAI>().Init();
            nullPoints.RemoveAt(index);
            if(objectPoolDic.ContainsKey(ObjectType.Enemy) == false)
            {
                objectPoolDic.Add(ObjectType.Enemy,new List<GameObject>());
            }
            objectPoolDic[ObjectType.Enemy].Add(enemy);
        }
    }
}
