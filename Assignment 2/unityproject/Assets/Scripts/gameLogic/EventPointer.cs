using UnityEngine;
using Mapbox.Examples;
using Mapbox.Utils;

/* This script is supposed to make the pointers on the map clickable and rotates the game object 
  Probably would be necessary to make many scripts of this type and assign them to different prefabs
  since we have many different events*/
public class EventPointer : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float rotSpeed = 50f;
    [SerializeField] private float amplitude = 2f;
    [SerializeField] private float frequency = 0.5f;

    LocationStatus playerLocation;

    public int eventID;

    /* Access other scripts */
    MapEventManager mapEventManager;
    MapUI mapUIManager;

    /* Store the Event coordinates --> Maybe we store them on a database?
        Here the eventPosition is set when it is instantiated in the SpawnOnMap script */
    public Vector2d eventPosition;

    void Start()
    {
        mapUIManager = GameObject.Find("UI").GetComponent<MapUI>();
        mapEventManager = GameObject.Find("EventManager").GetComponent<MapEventManager>();
    }

    // Update is called once per frame
    void Update()
    {
        RotatePointer();
        
    }


    /* Actual  function to make the pointer float and rotate effect */
    void RotatePointer()
    {
        transform.Rotate(Vector3.up, rotSpeed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude + 15f, transform.position.z);
    }

    private void OnMouseDown()
    {
        playerLocation = GameObject.Find("UI").GetComponent<LocationStatus>();
        
        if (playerLocation == null) 
        {
            Debug.Log("Player position is null!");
            return;
        }
        // Fetch the current location of the player and the location of the event
        var currentPlayerLocation = new GeoCoordinatePortable.GeoCoordinate(playerLocation.GetLocationLatitude(), playerLocation.GetLocationLongitude());
        var eventPos = new GeoCoordinatePortable.GeoCoordinate(eventPosition.x, eventPosition.y);
        
        var distance = currentPlayerLocation.GetDistanceTo(eventPos);
        
        Debug.Log("Distance: " + distance);

        if (mapUIManager == null)
        {
            Debug.Log("The UI manager is somehow null!");
            return;
        }

        /* If player is close enough, they can join the event */
        if (distance <= mapEventManager.maxDistance)
        {
            mapUIManager.DisplayStartEventPanel();
        }
        else
        {
            mapUIManager.DisplayNotInRangePanel();
        }
    }
}
