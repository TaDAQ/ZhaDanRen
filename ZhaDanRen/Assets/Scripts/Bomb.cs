using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Bomb : MonoBehaviour
{
    // public GameObject boomEffect;
    private int range;
    private float delayTime;
    private Action aniFinishAction;
    public void Init(int range,float delayTime,Action action)
    {
        this.range = range;
        this.delayTime = delayTime;
        aniFinishAction = action;
        StartCoroutine("DelayBoom",delayTime);


    }
    IEnumerator DelayBoom(float time)
    {
        yield return new WaitForSeconds(time);
        if(aniFinishAction != null)
        {
            aniFinishAction();
        }
        // Instantiate(boomEffect,transform.position,Quaternion.identity);
        ObjectPool.Instance.Get(ObjectType.BombEffect,transform.position);
        Boom(Vector2.left);
        Boom(Vector2.right);
        Boom(Vector2.up);
        Boom(Vector2.down);
        AudioController.Instance.PlayBoom();
        ObjectPool.Instance.Add(ObjectType.Bomb,gameObject);
    }
    private void Boom(Vector2 dir){
        for(int i =1;i<=this.range;i++)
        {
            Vector2 pos = (Vector2)(transform.position) + i* dir;
            if(!GameController.Instance.IsSuperWall(pos))
            {
                // GameObject effect = Instantiate(boomEffect);
                // effect.transform.position = pos;
                ObjectPool.Instance.Get(ObjectType.BombEffect,pos);
            }
            
        }
    }
}
