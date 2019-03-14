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
		[SerializeField]
		AbstractMap _map;
		[SerializeField]
		[Geocode]
		List<string> locationStrings;
		[SerializeField]
        float _spawnScale = 100f;
		[SerializeField]
        GameObject _markerPrefab;

        Vector2d[] _locations;
        List<GameObject> _spawnedObjects;
        public List<string> nodes;

        void Start()
		{
            _spawnedObjects = new List<GameObject>();
            nodes = new List<string>();

            getNodes(nodes);

            _locations = new Vector2d[locationStrings.ToArray().Length];
            for (int i = 0; i < locationStrings.ToArray().Length - 1; i++)
			{
				var locationString = locationStrings[i];
				_locations[i] = Conversions.StringToLatLon(locationString);
				var instance = Instantiate(_markerPrefab);
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

        private void getNodes(List<string> nodes)
        {
            //Database Stuff
            NpgsqlConnection conn = new NpgsqlConnection("Server=flick.cs.niu.edu;Port=5432;User Id=readonly;Database=aot;CommandTimeout=240");
            conn.Open();
            NpgsqlCommand command = new NpgsqlCommand("SELECT FORMAT('%s, %s', lat, lon) lat_lon, aot_node_id FROM aot_nodes WHERE project_id = 'AoT_Chicago';", conn);

            try
            {
                NpgsqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    locationStrings.Add(reader.GetString(0));
                    nodes.Add(reader.GetString(1));
                }
            }
            catch (System.Exception ex)
            {
                Console.print(ex.ToString());
                Console.print("Error reading query for node locations.");
            }

            conn.Close();
        }
	}
}