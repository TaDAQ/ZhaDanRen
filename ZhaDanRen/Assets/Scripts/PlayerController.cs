using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

   private float speed = 0.1f;
   public  int HP = 3;
   public  int bombCount = 1;
   public  int range = 0;

   private Animator anim;
   
   public  GameObject bombPre;
   private Rigidbody2D rig;
   private SpriteRenderer spriteRenderer;
   private Color color;
   private bool isInjured = false;
   
   private float bombDelay = 0.0f;
   /// <summary>
   ///当前场景中的炸弹数目
   private List<GameObject> bombList = new List<GameObject>();
   private void Awake()
   {
       spriteRenderer = GetComponent<SpriteRenderer>();
       color = spriteRenderer.color;
       anim = GetComponent<Animator>();
       rig = GetComponent<Rigidbody2D>();
   }
   private void Update()
   {
       Move();
       CreateBomb();
   }
   public void Init(int HP,int range,float bombDelay)
   {
       this.HP = HP;
       this.range = range;
       this.bombDelay = bombDelay;
   }
   private void CreateBomb()
   {
       if(Input.GetKeyDown(KeyCode.Space) && bombCount > 0)
       {
           AudioController.Instance.PlayFire();
           bombCount--;
           GameObject bomb = ObjectPool.Instance.Get(ObjectType.Bomb,new Vector2(Mathf.RoundToInt(transform.position.x),Mathf.RoundToInt(transform.position.y)));
           bomb.GetComponent<Bomb>().Init(this.range,this.bombDelay,()=>{
                bombCount++;
                bombList.Remove(bomb);
           });
           bombList.Add(bomb);
       }
   }
   public void AddSpeed(float value = 0.03f)
   {
       speed += value;
       if(speed > 0.2f) speed = 0.2f;
   }
   private void BombFinished(){
    //    bombCount++;
    //    bombList.Remove(bomb);
   }
   public void ResetPlayer()
   {
       foreach(var item in  bombList)
       {
           bombCount++;
           item.GetComponent<Bomb>().StopAllCoroutines();
           ObjectPool.Instance.Add(ObjectType.Bomb,item);
       }
       bombList.Clear();

       StopCoroutine("Injured");
       color.a = 1;
       spriteRenderer.color = color;
   }
   private void Move()
   {
       float h = Input.GetAxis("Horizontal");
       float v = Input.GetAxis("Vertical");
       anim.SetFloat("Horizontal",h);
       anim.SetFloat("Vertical",v);
       rig.MovePosition(transform.position + new Vector3(h,v) * speed);
   }
   private void OnTriggerEnter2D(Collider2D other) {
       if(isInjured)return;
        if(other.CompareTag(Tags.Enemy) || other.CompareTag(Tags.BombEffect))
       {
           HP--;
           if(HP == 0)
           {
               PlayDieAni();
               return;
           }
           StartCoroutine("Injured",2);
       }
   }
   public void PlayDieAni()
   {
       Time.timeScale = 0;
       anim.SetTrigger("Die");
   }
   private void DieFinish()
   {
       GameController.Instance.GameOver();
   }
   IEnumerator Injured(int time)
   {
       isInjured = true;
       for(int i =0;i<time*2;i++)
       {
           color.a = 0;
           spriteRenderer.color = color;
           yield return new WaitForSeconds(0.25f);
           color.a = 1;
           spriteRenderer.color = color;
           yield return new WaitForSeconds(0.25f);

       }
       isInjured = false;
   }
}
