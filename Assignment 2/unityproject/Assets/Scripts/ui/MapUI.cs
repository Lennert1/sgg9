
using Mapbox.Unity.Location;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MapUI : UI
{
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI usernameText;
    [SerializeField] Button goToMenuButton;

    [SerializeField] private GameObject eventPanelInRange;
    [SerializeField] private GameObject eventPanelNOTInRange;

    /* Ensures that eventPanelInRange and eventPanelNOTInRange cannot be active at the same time */
    bool isEventPanelActive = false;


    public void updateLevel(int level)
    {
        levelText.text = "LVL " + level;
    }

    public void backToMenu()
    {
        SceneManager.LoadScene(0);
    }

    /* If the user is in range while clicking on a event pointer, they can start the event */
    public void DisplayStartEventPanel()
    {
        if (!isEventPanelActive)
        {
            eventPanelInRange.SetActive(true);
            isEventPanelActive = true;
        }
    }

    /* If the user is NOT in range while clicking on a event pointer, another panel is shown */
    public void DisplayNotInRangePanel()
    {
        if (!isEventPanelActive)
        {
            eventPanelNOTInRange.SetActive(true);
            isEventPanelActive = true;
        }
    }
    public void CloseButtonClicked()
    {
        eventPanelInRange.SetActive(false);
        eventPanelNOTInRange.SetActive(false);
        isEventPanelActive = false;
    }


}
