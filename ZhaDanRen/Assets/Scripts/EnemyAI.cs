using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float speed = 0.04f;
    private Rigidbody2D rig;
    private SpriteRenderer spriteRenderer;
    private Color color;
    private float rayDistance = 0.7f;

    public bool canMove = true;

    /// <summary>
    ///方向：0 上 1下 2 左 3 右
    /// </summary>
    private int dirId = 0 ;
    private Vector2 dirVector;
    void Awake()
    {
        spriteRenderer =GetComponent<SpriteRenderer>();
        color = spriteRenderer.color;
        rig = GetComponent<Rigidbody2D>();
        InitDir(Random.Range(0,4));
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void Init()
    {
        color.a = 1;
        spriteRenderer.color = color;
        canMove = true;
        InitDir(Random.Range(0,4));
    }
    private void InitDir(int dir)
    {
        dirId = dir;
        switch(dir)
        {
            case 0:
            dirVector = Vector2.up;
            break;
            case 1:
            dirVector = Vector2.down;
            break;
            case 2:
            dirVector = Vector2.left;
            break;
            case 3:
            dirVector = Vector2.right;
            break;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(canMove)
        {
            rig.MovePosition((Vector2)(transform.position) +dirVector*speed);
        }else
        {
            ChangeDir();
        }
        
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag(Tags.BombEffect) && gameObject.activeSelf)
        {
            GameController.Instance.enemyCount -- ;
            ObjectPool.Instance.Add(ObjectType.Enemy,gameObject);
        }
        if(other.CompareTag(Tags.Enemy))
        {
            color.a = 0.3f;
            spriteRenderer.color = color;
        }
        if(other.CompareTag(Tags.Wall) || other.CompareTag(Tags.SuperWall))
        {
            ChangeDir();
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        if(other.CompareTag("Enemy"))
        {
            color.a = 1f;
            spriteRenderer.color = color;
        }
    }
    private void ChangeDir(){
        transform.position = new Vector2(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
        List<int> dirList = new List<int>();
        if(Physics2D.Raycast(transform.position,Vector2.up,rayDistance,1<<6) == false)//上面是否有墙壁
        {
            dirList.Add(0);
        }
        if(Physics2D.Raycast(transform.position,Vector2.down,rayDistance,1<<6) == false)//下面是否有墙壁
        {
            dirList.Add(1);
        }
        if(Physics2D.Raycast(transform.position,Vector2.left,rayDistance,1<<6) == false)//左面是否有墙壁
        {
            dirList.Add(2);
        }
        if(Physics2D.Raycast(transform.position,Vector2.right,rayDistance,1<<6) == false)//右面是否有墙壁
        {
            dirList.Add(3);
        }
        if(dirList.Count > 0){
            // Debug.Log("dirList:"+dirList.Count);
            int index = Random.Range(0,dirList.Count);
            // Debug.Log("index:"+index);
            // Debug.Log("dir:"+dirList[index]);
            InitDir(dirList[index]);
            canMove = true;
        }else
        {
            canMove = false;
        }
    }
    private void OnDrawGizmos() {
        Gizmos.DrawLine(transform.position,transform.position+new Vector3(0,rayDistance,0));
        Gizmos.DrawLine(transform.position,transform.position+new Vector3(0,-rayDistance,0));
        Gizmos.DrawLine(transform.position,transform.position+new Vector3(rayDistance,0,0));
        Gizmos.DrawLine(transform.position,transform.position+new Vector3(-rayDistance,0));
    }
}
