using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class StartScene : MonoBehaviour
{
    public Button btn_start,btn_exit;
    private void Awake() {
        btn_start.onClick.AddListener(()=>{
            SceneManager.LoadScene("Game");
        });
        btn_exit.onClick.AddListener(()=>{
            Application.Quit(); 
        });
    }
   
}
