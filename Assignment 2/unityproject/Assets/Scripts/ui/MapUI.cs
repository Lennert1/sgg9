
using UnityEngine;
using UnityEngine.Assertions;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MapUI : UI
{
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI usernameText;
    [SerializeField] Button goToMenuButton;

    public void Awake()
    {
       
    }

    public void updateLevel(int level)
    {
        levelText.text = "LVL " + level;
    }

    public void backToMenu()
    {
        SceneManager.LoadScene(0);
    } 

}
