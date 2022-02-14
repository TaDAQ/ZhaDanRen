using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectType
{
    SuperWall,
    Wall,
    Prop,
    Bomb,
    Enemy,
    BombEffect

}
[System.Serializable]
public class Type_Prefab
{
    public ObjectType type;
    public GameObject prefab;
}
public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;
    public List<Type_Prefab> typePrefabs = new List<Type_Prefab>();
    /// <summary
    ///物体类型对应的对象池关系字典
    private Dictionary<ObjectType,List<GameObject>> dic = new Dictionary<ObjectType,List<GameObject>>();

    private void Awake() {
        Instance = this;
    }

    /// <summary>
    ///通过物体类型，从对象池中获取对象
    public GameObject Get(ObjectType type)
    {
        GameObject temp = null;
        if(dic.ContainsKey(type) == false)
        {
          List<GameObject> gameObjs = new List<GameObject>();
          dic.Add(type,gameObjs);
        }
        if(dic[type].Count > 0)
        {
            int index = dic[type].Count - 1;
            temp = dic[type][index];
            dic[type].RemoveAt(index);
        }else
        {
            GameObject prefab = GetPreByType(type);
            if(prefab != null)
            {
                temp = Instantiate(prefab,transform);
            }
        }
        temp.SetActive(true);
        return temp;
    }
    // <summary>
    ///通过物体类型，从对象池中获取对象
    public GameObject Get(ObjectType type,Vector2 position)
    {
        GameObject temp = null;
        if(dic.ContainsKey(type) == false)
        {
          List<GameObject> gameObjs = new List<GameObject>();
          dic.Add(type,gameObjs);
        }
        if(dic[type].Count > 0)
        {
            int index = dic[type].Count - 1;
            temp = dic[type][index];
            dic[type].RemoveAt(index);
        }else
        {
            GameObject prefab = GetPreByType(type);
            if(prefab != null)
            {
                temp = Instantiate(prefab,transform);
            }
        }
        // GameObject prefab = GetPreByType(type);
        // if(prefab != null)
        // {
        //         temp = Instantiate(prefab,transform);
        // }
        // temp = Instantiate(prefab,transform);
        temp.SetActive(true);
        temp.transform.position = position;
        temp.transform.rotation = Quaternion.identity;
        return temp;
    }
    public void Add(ObjectType type,GameObject gameObj)
    {
        if(dic.ContainsKey(type) && dic[type].Contains(gameObj) == false)
        {
            dic[type].Add(gameObj);
        }
        gameObj.SetActive(false);
    }
    ///通过物体类型获取对应的预制体
    private GameObject GetPreByType(ObjectType type)
    {
        foreach(Type_Prefab item in typePrefabs)
        {
            if(item.type == type)
            {
                return item.prefab;
            }
        }
        return null;
    }
}
