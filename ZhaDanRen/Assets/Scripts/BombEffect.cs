using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombEffect : MonoBehaviour
{
    public void AnimFinish()
    {
        ObjectPool.Instance.Add(ObjectType.BombEffect,gameObject);
    }
}
