namespace Mapbox.Examples
{
	using UnityEngine;
	using Mapbox.Utils;
	using Mapbox.Unity.Map;
	using Mapbox.Unity.MeshGeneration.Factories;
	using Mapbox.Unity.Utilities;
	using System.Collections.Generic;

	public class SpawnOnMap : MonoBehaviour
	{
		[SerializeField]
		AbstractMap _map;

		[SerializeField]
		[Geocode]
		string[] _locationStrings;
		Vector2d[] _locations;

		[SerializeField]
		float _spawnScale = 100f;

		[SerializeField]
		GameObject _markerPrefab;

		List<GameObject> _spawnedObjects;

		[SerializeField] Transform markerContainer;

		void Start()
		{
			_locations = new Vector2d[_locationStrings.Length];
			_spawnedObjects = new List<GameObject>();
			for (int i = 0; i < _locationStrings.Length; i++)
			{
				var locationString = _locationStrings[i];
				_locations[i] = Conversions.StringToLatLon(locationString);
				var instance = Instantiate(_markerPrefab, markerContainer);

				// Store the event position in the EventPointer script
				instance.GetComponent<EventPointer>().eventPosition = _locations[i];

				// Assigning eventIDs with following system: 1 to 99 dungeon, 100 to 199 taverns, 200 to 299 shops
				// Undefined behaviour if more than 99 dungeons/taverns/shops are defined in the inspector...
				switch(instance.GetComponent<EventPointer>().markerType)
				{
					case MarkerType.DUNGEON:
                        instance.GetComponent<EventPointer>().eventID = i + 1; break;
					case MarkerType.TAVERN:
                        instance.GetComponent<EventPointer>().eventID = i + 100; break;
					case MarkerType.SHOP:
                        instance.GetComponent<EventPointer>().eventID = i + 200; break;
					default:
						instance.GetComponent<EventPointer>().eventID = -1;
						Debug.Log("Something went wrong while assigning the eventID");
						break;
                }

				instance.transform.localPosition = _map.GeoToWorldPosition(_locations[i], true);
				instance.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
				_spawnedObjects.Add(instance);
			}
		}

		private void Update()
		{
			int count = _spawnedObjects.Count;
			for (int i = 0; i < count; i++)
			{
				var spawnedObject = _spawnedObjects[i];
				var location = _locations[i];
				spawnedObject.transform.localPosition = _map.GeoToWorldPosition(location, true);
				spawnedObject.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
			}
		}
	}
}