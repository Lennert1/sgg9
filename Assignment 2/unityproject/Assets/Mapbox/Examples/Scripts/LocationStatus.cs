namespace Mapbox.Examples
{
	using Mapbox.Unity.Location;
	using Mapbox.Utils;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.UI;

	/* This is a script to track the player's location 
	 */
	public class LocationStatus : MonoBehaviour
	{

		[SerializeField]
		Text _statusText;

		private AbstractLocationProvider _locationProvider = null;

		Location currLoc;


        void Start()
		{
			if (null == _locationProvider)
			{
				_locationProvider = LocationProviderFactory.Instance.DefaultLocationProvider as AbstractLocationProvider;
			}
		}

		void Update()
		{
			currLoc = _locationProvider.CurrentLocation;

			if (currLoc.IsLocationServiceInitializing)
			{
				_statusText.text = "location services are initializing";
			}
			else
			{
				if (!currLoc.IsLocationServiceEnabled)
				{
					_statusText.text = "location services not enabled";
				}
				else
				{
					if (currLoc.LatitudeLongitude.Equals(Vector2d.zero))
					{
						_statusText.text = "Waiting for location ....";
					}
					else
					{
						_statusText.text = string.Format("{0}", currLoc.LatitudeLongitude);
					}
				}
			}

		}

		/* Functions to access the player's location */
		public double GetLocationLatitude()
		{
			return currLoc.LatitudeLongitude.x;
		}
        public double GetLocationLongitude()
        {
            return currLoc.LatitudeLongitude.y;
        }

    }
}
