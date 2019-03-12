namespace Mapbox.Examples
{
	using UnityEngine;
	using Mapbox.Utils;
	using Mapbox.Unity.Map;
	using Mapbox.Unity.MeshGeneration.Factories;
	using Mapbox.Unity.Utilities;
	using System.Collections.Generic;
    using UnityNpgsql;
    using TMPro;

	public class SpawnNodeMarkers : MonoBehaviour
	{
		[SerializeField]
		AbstractMap _map;
        [SerializeField]
        bool TempVisualization;
        [SerializeField]
        bool LightVisualization;
		[SerializeField]
		[Geocode]
		List<string> locationStrings;
		[SerializeField]
        float _spawnScale = 100f;
		[SerializeField]
        GameObject _markerPrefab;

        Vector2d[] _locations;
        List<GameObject> _spawnedObjects;
        List<string> nodes;
        Dictionary<string, string> temperatures;
        Dictionary<string, string> light_intensities;

        void Start()
		{

            _spawnedObjects = new List<GameObject>();
            nodes = new List<string>();

            getNodes(nodes);

            if (TempVisualization)
            {
                getTemp();
            }
            else if (LightVisualization)
            {
                
                getLight();
            }

            _locations = new Vector2d[locationStrings.ToArray().Length];
            for (int i = 0; i < locationStrings.ToArray().Length - 1; i++)
			{
				var locationString = locationStrings[i];
				_locations[i] = Conversions.StringToLatLon(locationString);
				var instance = Instantiate(_markerPrefab);
				instance.transform.localPosition = _map.GeoToWorldPosition(_locations[i], true);
				instance.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);

                //set Temp Enabled Shader
                if (TempVisualization)
                {
                    setUpTempMarker(instance, i);
                    instance.GetComponentInChildren<TextMeshPro>().enabled = false;
                }
                else if (LightVisualization)
                {
                    setUpLightEnabledMarker(instance, i);
                }
                else
                {
                    instance.GetComponentInChildren<MeshRenderer>().material.color = Color.white;
                    instance.GetComponentInChildren<TextMeshPro>().SetText(nodes[i]);
                    instance.GetComponentInChildren<TextMeshPro>().enabled = false;
                }

                
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

        private void getTemp()
        {
            temperatures = new Dictionary<string, string>();

            NpgsqlConnection conn = new NpgsqlConnection("Server=flick.cs.niu.edu;Port=5432;User Id=readonly;Database=aot;CommandTimeout=240;");
            conn.Open();

            string template = "SELECT aot_node_id, CAST(ROUND(AVG(CAST(NULLIF(value_hrf, 'NA') AS NUMERIC)), 2) AS TEXT) FROM data WHERE parameter='temperature' AND sensor='pr103j2' AND timestamp >= '2019-01-01 00:00:00' AND timestamp < '2020-01-01 00:00:00' GROUP BY aot_node_id;";
            NpgsqlCommand command = new NpgsqlCommand(string.Format(template), conn);

            try
            {
                NpgsqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    temperatures.Add(reader.GetString(0), reader.GetString(1));
                }
            }
            catch (System.Exception ex)
            {
                Console.print(ex.ToString());
                Console.print("Error reading query for temp-enabled nodes.");
            }

            conn.Close();
        }

        private void getLight()
        {
            light_intensities = new Dictionary<string, string>();

            NpgsqlConnection conn = new NpgsqlConnection("Server=flick.cs.niu.edu;Port=5432;User Id=readonly;Database=aot;CommandTimeout=240;");
            conn.Open();

            string template = " SELECT aot_node_id, CAST(ROUND(MAX(CAST(NULLIF(value_hrf, 'NA') AS NUMERIC)), 2) AS TEXT) FROM data WHERE sensor='apds_9006_020' AND timestamp >= '2018-06-01 00:00:00' AND timestamp < '2018-06-02 00:00:00' GROUP BY aot_node_id;";
            NpgsqlCommand command = new NpgsqlCommand(string.Format(template), conn);

            try
            {
                NpgsqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if (reader.GetString(1) != "5267.41")
                        light_intensities.Add(reader.GetString(0), reader.GetString(1));
                }
            }
            catch (System.Exception ex)
            {
                Console.print(ex.ToString());
                Console.print("Error reading query for light-enabled nodes.");
            }

            conn.Close();
        }

        private void setUpTempMarker(GameObject instance, int i)
        {
            if (temperatures.ContainsKey(nodes[i]))
            {
                instance.GetComponentInChildren<MeshRenderer>().material.color = Color.green;
                instance.GetComponentInChildren<TextMeshPro>().SetText(temperatures[nodes[i]]);
            }
            else
            {
                instance.GetComponentInChildren<MeshRenderer>().material.color = Color.red;
                instance.GetComponentInChildren<TextMeshPro>().SetText("");
            }
        }

        private void setUpLightEnabledMarker(GameObject instance, int i)
        {
            if (light_intensities.ContainsKey(nodes[i]))
            {
                instance.GetComponentInChildren<Light>().intensity = float.Parse(light_intensities[nodes[i]]) / 1000 * 8;
                instance.GetComponentInChildren<Light>().color = Color.cyan;
            }
        }
	}
}