using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag(Tags.BombEffect))
        {
            ObjectPool.Instance.Add(ObjectType.Wall,gameObject);
        }
    }
}
