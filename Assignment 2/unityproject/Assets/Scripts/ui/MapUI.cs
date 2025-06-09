
using UnityEngine;
using UnityEngine.Assertions;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MapUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI usernameText;
    [SerializeField] Button goToMenuButton;

    public void Awake()
    {
        Assert.IsNotNull(levelText);
        Assert.IsNotNull(usernameText);
        Assert.IsNotNull(goToMenuButton);
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
