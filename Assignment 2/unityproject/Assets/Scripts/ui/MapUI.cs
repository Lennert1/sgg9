
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

    [SerializeField] private TextMeshProUGUI joinLabel;

    /* Ensures that eventPanelInRange and eventPanelNOTInRange cannot be active at the same time */
    bool isEventPanelActive = false;

    MarkerType currentMarker = MarkerType.DUNGEON;
    int currentEventID = 0;

    /* Access other scripts */
    GameManager gameManager;

    protected override void Start()
    {
        base.Start();
        gameManager = GameObject.Find("UI").GetComponent<GameManager>();
        // Load User data to the UI
        User userData = _api.LoadUserDataFromFile();
        if (userData != null)
        {
            levelText.text = "LVL " + userData.lvl.ToString();
            usernameText.text = userData.name;
        }
    }

    /* If the user is in range while clicking on a event pointer, they can start the event */
    public void DisplayStartEventPanel()
    {
        // Debug.Log(currentMarker);
        if (!isEventPanelActive)
        {
            joinLabel.text = "Enter the " + currentMarker.ToString() + "?";
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

    /*When Yes-Button is clicked the scene is changed accordingly to the Type of the Marker*/
    public void EnterButtonClicked()
    {
        eventPanelInRange.SetActive(false);
        isEventPanelActive = false;

        if (gameManager == null)
        {
            Debug.Log("Game manager could not be found!");
            return;
        }

        // When player clicked on enter, id in GameManager is updated
        gameManager.currentPoiID = currentEventID;

        switch (currentMarker) { 
            case MarkerType.DUNGEON:
                DungeonUI dungeonUI = GameObject.Find("UI").GetComponentInChildren<DungeonUI>(true);
                if (dungeonUI != null) 
                {
                    LoadUI(dungeonUI);
                    // Socket call here
                }
                break;
            case MarkerType.TAVERN:
                TavernUI tavernUI = GameObject.Find("UI").GetComponentInChildren<TavernUI>(true);
                if(tavernUI != null)
                {
                    LoadUI(tavernUI);
                    // Socket call here
                }
                break;
            case MarkerType.SHOP:
                ShopUI shopUI = GameObject.Find("UI").GetComponentInChildren<ShopUI>(true);
                if (shopUI != null)
                {
                    LoadUI(shopUI);
                    // Socket call here
                }
                break;
            default:
                break;
        }
    }
    public void CloseButtonClicked()
    {
        eventPanelInRange.SetActive(false);
        eventPanelNOTInRange.SetActive(false);
        isEventPanelActive = false;
    }

    public void SetMarkerType(MarkerType markerType)
    {
        currentMarker = markerType;
    }

    public void SetCurrentEventID(int eventID)
    {
        currentEventID = eventID;
    }

    public bool GetIsEventPanelActive()
    {
        return isEventPanelActive;
    }


}
