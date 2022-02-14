using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  enum PropType {
    HP,
    Speed,
    Bomb,
    Range,
    Time,
    NONE
}
[System.Serializable]
public class PropTypeSprite
{
    public PropType type;
    public Sprite sp;
}
public class Prop : MonoBehaviour
{
    public PropTypeSprite[] propTypeSprites;
    SpriteRenderer spriteRenderer;
    public Sprite defaultSprite;
    PropType propType = PropType.NONE;
    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Reset() {
        spriteRenderer.sprite = defaultSprite;
        tag = "Wall";
        gameObject.layer = 6;
        GetComponent<Collider2D>().isTrigger = false;
        propType = PropType.NONE;
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag(Tags.BombEffect) && propType == PropType.NONE)
        {
            tag = "Untagged";
            gameObject.layer = 0;
            GetComponent<Collider2D>().isTrigger = true;
            int index = Random.Range(0,propTypeSprites.Length);
            spriteRenderer.sprite = propTypeSprites[index].sp;
            propType = propTypeSprites[index].type;

            StartCoroutine(PropAni());
        }
        if(other.CompareTag(Tags.Player))
        {
            PlayerController playerController = other.gameObject.GetComponent<PlayerController>();
            switch(propType){
                case PropType.HP:
                playerController.HP++;
                break;
                case PropType.Speed:
                playerController.AddSpeed();
                break;
                case PropType.Bomb:
                playerController.bombCount += 1;
                break;
                case PropType.Range:
                playerController.range += 1;
                break;
                case PropType.Time:
                GameController.Instance.time += 50;
                break;
            }
            Reset();
            ObjectPool.Instance.Add(ObjectType.Prop,gameObject);
        }
    }
    IEnumerator PropAni()
    {
        for(int i = 0; i <2 ;i++)
        {
            spriteRenderer.color = Color.yellow;
            yield return new WaitForSeconds(0.25f);
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(0.25f);
        }
      
    }
}
