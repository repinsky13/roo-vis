using System;
using UnityEngine;
using Mapbox.Utils;
using Mapbox.Unity.Map;
using Mapbox.Unity.MeshGeneration.Factories;
using Mapbox.Unity.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


	public class SpawnOnMapZoom : MonoBehaviour
	{
		[SerializeField]
		AbstractMap _map;

		[SerializeField]
		[Geocode]
		//public string[] _locationStrings;
		public Vector2d[] _locations;

        public Vector2d riderLocation;

		[SerializeField]
		float _spawnScale = 100f;

		[SerializeField]
		GameObject _markerPrefab;

        [SerializeField]
        GameObject _riderPrefab;

		List<GameObject> _spawnedObjects;

		void Start()
		{
            retrievePlayerPrefsData();
            _map.Initialize(riderLocation, 8);
            _spawnedObjects = new List<GameObject>();
            var riderLogo = Instantiate(_riderPrefab);
			riderLogo.transform.localPosition = _map.GeoToWorldPosition(riderLocation, true);
			riderLogo.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
            _spawnedObjects.Add(riderLogo);
			for (int i = 0; i < _locations.Length; i++)
			{
				//var locationString = _locationStrings[i];
				//_locations[i] = Conversions.StringToLatLon(locationString);
                Debug.Log(_locations[i]);
				var instance = Instantiate(_markerPrefab);
				instance.transform.localPosition = _map.GeoToWorldPosition(_locations[i], true);
				instance.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
                if (instance.transform.Find("Canvas").transform.Find("Button").GetComponent<ButtonController>() != null) {
                    instance.transform.Find("Canvas").transform.Find("Button").GetComponent<ButtonController>().location.Set(_locations[i].x, _locations[i].y);
                    // Debug.Log("Locations.x:");
                    // Debug.Log(_locations[i].x);
                } else {
                    Debug.Log("Can't find 'Button Controller'");
                }
				_spawnedObjects.Add(instance);
			}			
		}

		private void Update()
		{
            _spawnScale = (float)(0.3 * _map.AbsoluteZoom);
			int count = _spawnedObjects.Count;
            var spawnedObjectRider = _spawnedObjects[0];
			var locationRider = riderLocation;
			spawnedObjectRider.transform.localPosition = _map.GeoToWorldPosition(locationRider, true);
            spawnedObjectRider.transform.Translate(new Vector3(0, 1, 0));
			spawnedObjectRider.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
			for (int i = 1; i < count; i++)
			{
				var spawnedObject = _spawnedObjects[i];
				var location = _locations[i];
				spawnedObject.transform.localPosition = _map.GeoToWorldPosition(location, true);
                spawnedObject.transform.Translate(new Vector3(0, 1, 0));
				spawnedObject.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
			}
		}

        private void retrievePlayerPrefsData()
        {
            riderLocation.x = Convert.ToDouble(PlayerPrefs.GetFloat("rider_x"));
            riderLocation.y = Convert.ToDouble(PlayerPrefs.GetFloat("rider_y"));
            int restNumber = PlayerPrefs.GetInt("rest_number");
            for (int i = 1; i <= restNumber; i++)
            {
                double xcoordinate = Convert.ToDouble(PlayerPrefs.GetFloat("restaurant" + Convert.ToString(i) + "x"));
                double ycoordinate = Convert.ToDouble(PlayerPrefs.GetFloat("restaurant" + Convert.ToString(i) + "y"));
                _locations[i].x = xcoordinate;
                _locations[i].y = ycoordinate;
                Debug.Log(_locations[i]);
            }
        }
	}