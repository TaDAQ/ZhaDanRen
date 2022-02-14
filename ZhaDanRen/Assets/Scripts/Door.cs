using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Sprite doorSprite,defaultSprite;
    private SpriteRenderer spriteRenderer;
    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void Reset() {
        spriteRenderer.sprite = defaultSprite;
        GetComponent<Collider2D>().isTrigger = false;
        tag = "Wall";
        gameObject.layer = 6;
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag(Tags.BombEffect))
        {
            tag = "Untagged";
            gameObject.layer = 0;
            spriteRenderer.sprite = doorSprite;
            GetComponent<Collider2D>().isTrigger = true;
        }
        if(other.CompareTag(Tags.Player))
        {
            //当前场景敌人已经全部被消灭 开始下一关卡
            print("Next Level");
            GameController.Instance.LoadNextLevel();
        }
    }
}
