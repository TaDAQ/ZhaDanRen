using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    public GameObject playerPre;
    private MapController mapController;
    public int enemyCount;
    private GameObject player;
    private PlayerController playerController;
    private int level = 0;
    [HideInInspector]
    public int time = 130;
    private float timer = 0;

    // Start is called before the first frame update
    private void Awake() {
        Instance = this;
    }
    void Start()
    {
        mapController = GetComponent<MapController>();
        LevelControl();
    }
    void Update()
    {
        UIController.Instance.Refresh(playerController.HP,level,time,enemyCount);
        // if(Input.GetKeyDown(KeyCode.N))
        // {
            
        //     LevelControl();
        // }
        LevelTimer();
    }

    public void LoadNextLevel()
    {
        if(enemyCount <= 0 )
        {
            LevelControl();
        }
    }
    private void LevelControl()
    {
        time = level * 50 + 130;
        int x = 6 +2 * (level/3);
        int y = 3 +2 * (level/3);
        if(x>18) x = 18;
        if(y>15) y = 15;
        enemyCount = (int)(level * 1.5) + 1;
        if(enemyCount > 40) enemyCount = 40;
        mapController.InitMap(x,y,x*y,enemyCount);
        if(player == null) 
        {
            player = Instantiate(playerPre);
            playerController =  player.GetComponent<PlayerController>();
            playerController.Init(3,1,2);
        }
        player.transform.position = mapController.GetPlayerPosition();
        level++;
        UIController.Instance.PlayLevelFade(level);
        Camera.main.GetComponent<CameraFollow>().Init(player.transform,x,y);
        playerController.ResetPlayer();

        GameObject[] effects = GameObject.FindGameObjectsWithTag(Tags.BombEffect);
        foreach (var item in effects)
        {
            ObjectPool.Instance.Add(ObjectType.BombEffect,item);
        }
    }
    public bool IsSuperWall(Vector2 position)
    {
        return mapController.IsSuperWall(position);
    }
    /// <summary>
    public void LevelTimer()
    {
        if(time <= 0)
        {
            if(playerController.HP > 0)
            {
                playerController.HP --;
                time = 200;
                return;
            }
            playerController.PlayDieAni();
            return;
        }
        timer += Time.deltaTime;
        if(timer >= 1.0f)
        {
            time --;
            timer = 0;
        }
    }
    public void GameOver()
    {
         
            //显示游戏结束界面
        UIController.Instance.ShowGameOver();
    }
}
