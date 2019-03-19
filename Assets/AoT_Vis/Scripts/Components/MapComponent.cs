namespace AoT_Vis
{
	using UnityEngine;
	using Mapbox.Utils;
	using Mapbox.Unity.Map;
	using Mapbox.Unity.MeshGeneration.Factories;
	using Mapbox.Unity.Utilities;
	using System.Collections.Generic;
    using UnityNpgsql;
    using TMPro;

	public class MapComponent : MonoBehaviour
	{
        public DataManager dataManager;
        public ComponentManager componentManager;

        [SerializeField]
		AbstractMap _map;
		[Geocode]
		List<string> locationStrings;
		[SerializeField]
        float _spawnScale = 1f;
		[SerializeField]
        GameObject _markerPrefab;
        [SerializeField]
        GameObject _selectedMarkerPrefab;

        Vector2d[] _locations;
        List<GameObject> _spawnedObjects = new List<GameObject>();
        public List<string> nodes;
        GameObject selectedMarkerInstance;

        void Start()
		{
            nodes = dataManager.NodeIds;
            locationStrings = dataManager.NodeLocations;

            _locations = new Vector2d[locationStrings.ToArray().Length];
            for (int i = 0; i < locationStrings.ToArray().Length; i++)
			{
				var locationString = locationStrings[i];
				_locations[i] = Conversions.StringToLatLon(locationString);
				var instance = Instantiate(_markerPrefab);
                instance.GetComponent<MapMarker>().node_id = nodes[i];
				instance.transform.localPosition = _map.GeoToWorldPosition(_locations[i], true);
				instance.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
                
                _spawnedObjects.Add(instance);
			}
            selectedMarkerInstance = Instantiate(_selectedMarkerPrefab);
            highlightSelectedNode();
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
            highlightSelectedNode();
        }

        private void highlightSelectedNode()
        {
            string selectedNodeId = dataManager.SelectedNodeId;

            Vector2d selectedLocation = _locations[dataManager.NodeIds.FindIndex(x => x == selectedNodeId)];
            selectedMarkerInstance.transform.localPosition = _map.GeoToWorldPosition(selectedLocation, true);
            selectedMarkerInstance.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
        }
	}
}