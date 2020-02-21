using UnityEngine;
using Mapbox.Utils;
using Mapbox.Unity.Map;
using Mapbox.Unity.MeshGeneration.Factories;
using Mapbox.Unity.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


	public class SpawnOnMap : MonoBehaviour
	{
		[SerializeField]
		AbstractMap _map;

		[SerializeField]
		[Geocode]
		//public string[] _locationStrings;
		public Vector2d[] _locations;

        [SerializeField]
        public int[] IDs;

		[SerializeField]
		float _spawnScale = 100f;

		[SerializeField]
		GameObject _markerPrefab;

		List<GameObject> _spawnedObjects;

        JSONDataClass jsnData;

        string jsonURL = "http://roo-api.herokuapp.com/pairings/";

		void Start()
		{
            StartCoroutine(getdata(jsonURL));			
		}

		private void Update()
		{
            _spawnScale = (float)(0.3 * _map.AbsoluteZoom);
			int count = _spawnedObjects.Count;
			for (int i = 0; i < count; i++)
			{
				var spawnedObject = _spawnedObjects[i];
				var location = _locations[i];
				spawnedObject.transform.localPosition = _map.GeoToWorldPosition(location, true);
                spawnedObject.transform.Translate(new Vector3(0, 1, 0));
				spawnedObject.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
			}
		}

        IEnumerator getdata(string _url)
        {
            UnityWebRequest _uwr = UnityWebRequest.Get(_url);
            yield return _uwr.SendWebRequest();
            if (_uwr.error == null) {
                processJsonData(_uwr.downloadHandler.text);
            } else {
                Debug.Log("Oops something went wrong" + _uwr.error);
            }
            postCoroutine();
        }

        private void postCoroutine() 
        {
            _spawnedObjects = new List<GameObject>();
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
                    instance.transform.Find("Canvas").transform.Find("Button").GetComponent<ButtonController>().id = IDs[i];
                    foreach (var pairing in jsnData.pairings)
                    {
                        if (pairing.rider.id == IDs[i])
                        {
                            instance.transform.Find("Canvas").transform.Find("Button").GetComponent<ButtonController>().restLocations.Add(new Vector2d(pairing.restaurant.location.lat, pairing.restaurant.location.@long));
                        }
                    }
                    // Debug.Log("Locations.x:");
                    // Debug.Log(_locations[i].x);
                } else {
                    Debug.Log("Can't find 'Button Controller'");
                }
				_spawnedObjects.Add(instance);
			}
        }

        private void processJsonData(string _url)
        {
            jsnData = JsonUtility.FromJson<JSONDataClass>(_url);
            int i = 0;
            foreach (var pairing in jsnData.pairings)
            {
                _locations[i].x = pairing.rider.location.lat;
                _locations[i].y = pairing.rider.location.@long;
                IDs[i] = pairing.rider.id;
                i = i + 1;
            }
        }
	}