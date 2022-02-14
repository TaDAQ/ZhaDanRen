using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform player;
    int x,y;
    public void Init(Transform player,int x,int y)
    {
        this.player = player;
        this.x = x;
        this.y = y;
    }
    private void LateUpdate() {
        if(this.player != null)
        {
            float x = Mathf.Lerp(transform.position.x, player.position.x,0.2f);
            float y = Mathf.Lerp(transform.position.y, player.position.y,0.2f);
            transform.position = new Vector3(x, y,transform.position.z);
            transform.position = new Vector3(Mathf.Clamp(transform.position.x,-(this.x-5),this.x-7),Mathf.Clamp(transform.position.y,-(this.y-2),this.y-4), transform.position.z);
        }
    }
}
