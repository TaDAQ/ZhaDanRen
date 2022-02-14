using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public static UIController Instance;
    public Text txt_hp,txt_level,txt_time,txt_enemy;
    public GameObject gameOverPanel;
    public Animator levelFadeAnim;
    public Text txtLevel;
    public void Refresh(int hp,int level,int time,int enemy)
    {
        txt_hp.text = "HP:"+hp.ToString();
        txt_level.text = "Level:"+level.ToString();
        txt_time.text = "Time:"+time.ToString();
        txt_enemy.text = "Enemy:"+enemy.ToString();
    }

    private void Init()
    {
        gameOverPanel.transform.Find("btn_again").GetComponent<Button>().onClick.AddListener(()=>{
            Time.timeScale = 1;
            //重新加载当前场景
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        });
        gameOverPanel.transform.Find("btn_home").GetComponent<Button>().onClick.AddListener(()=>{
            Time.timeScale = 1;
            SceneManager.LoadScene("Start");
        });
    }

    private void Awake() {
        Instance = this;
        Init();
    }
    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
    }
    public  void PlayLevelFade(int level)
    {
        Time.timeScale = 0;
        txtLevel.text = "Level:" + level.ToString();
        levelFadeAnim.Play("LevelFade",0,0);
        startDealy = true;
    }
    bool startDealy = false;
    float timer = 0;
    public void Update()
    {
        if(startDealy)
        {
            timer += Time.unscaledDeltaTime;
            if(timer > 0.7f)
            {
                startDealy = false;
                Time.timeScale = 1;
                timer = 0;
            }
        }
    }
}
