namespace AoT_Vis
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityNpgsql;

    public class DataManager : MonoBehaviour
    {
        public ComponentManager componentManager;

        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string SelectedNodeId { get; set; }
        public string SelectedNodeAddress { get; set; }
        public string HighlightedNode { get; set; }

        //Node Sensor Data
        public Dictionary<string, string> TemperatureReadings { get; set; }
        string[] tempSensors = new string[] { "bmp180", "htu21d", "pr103j2", "tsys01", "tmp112"};

        //Node Info Data
        public List<string> NodeIds { get; private set; }
        public List<string> NodeLocations { get; private set; }
        public List<string> NodeAddresses { get; private set; }
        public List<string> NodeStart { get; private set; }
        public List<string> NodeEnd { get; private set; }
        public List<string> NodeLat { get; private set; }
        public List<string> NodeLon { get; private set; }

        // Use this for initialization
        void Awake()
        {
            NodeLocations = new List<string>();
            NodeIds = new List<string>();
            NodeAddresses = new List<string>();
            NodeStart = new List<string>();
            NodeEnd = new List<string>();
            NodeLat = new List<string>();
            NodeLon = new List<string>();
            TemperatureReadings = new Dictionary<string, string>();

            fetchNodeData();
            SelectedNodeId = NodeIds[0];
            SelectedNodeAddress = NodeAddresses[0];
            StartTime = "2018-06-15 12:00:00";
            EndTime = "2018-06-15 13:00:00";
            getSelectedNodeTempData();
        }

        private void fetchNodeData()
        {
            //Database Stuff
            NpgsqlConnection conn = new NpgsqlConnection("Server=flick.cs.niu.edu;Port=5432;User Id=readonly;Database=aot;CommandTimeout=240");
            conn.Open();
            NpgsqlCommand command = new NpgsqlCommand("SELECT FORMAT('%s, %s', lat, lon) lat_lon, aot_node_id, address, start_timestamp, end_timestamp, lat, lon FROM aot_nodes WHERE project_id = 'AoT_Chicago' AND aot_node_id != '001e0610ef73';", conn);

            try
            {
                NpgsqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    NodeLocations.Add(reader.GetString(0));
                    NodeIds.Add(reader.GetString(1));
                    NodeAddresses.Add(reader.GetString(2));
                    NodeStart.Add(reader.GetTimeStamp(3).ToString());
                    try
                    {
                        NodeEnd.Add(reader.GetTimeStamp(4).ToString());
                    }
                    catch (System.InvalidCastException icex)
                    {
                        NodeEnd.Add("");
                    }
                    NodeLat.Add(reader.GetString(5));
                    NodeLon.Add(reader.GetString(6));
                }
            }
            catch (System.Exception ex)
            {
                print(ex.Message);
            }

            conn.Close();
        }

        private void getSelectedNodeTempData()
        {
            TemperatureReadings.Clear();

            foreach (string sensor in tempSensors)
            {
                //Database Stuff
                NpgsqlConnection conn = new NpgsqlConnection("Server=flick.cs.niu.edu;Port=5432;User Id=readonly;Database=aot;CommandTimeout=240");
                conn.Open();
                NpgsqlCommand command = new NpgsqlCommand("SELECT value_hrf FROM data WHERE aot_node_id = '" + 
                    SelectedNodeId + 
                    "' AND sensor = '" +
                    sensor +
                    "' and parameter = 'temperature' AND timestamp >= '" + 
                     StartTime + 
                     "' AND timestamp < '" +
                     EndTime + "' LIMIT 1;", conn);

                try
                {
                    NpgsqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        TemperatureReadings.Add(sensor, reader.GetString(0));
                    }
                }
                catch (System.Exception ex)
                {
                    print(ex);
                }

                conn.Close();
            }
        }

        public void changeSelectedNode(string newNodeId)
        {
            SelectedNodeId = newNodeId;
            SelectedNodeAddress = NodeAddresses[NodeIds.FindIndex(x => x == newNodeId)];
            getSelectedNodeTempData();
            componentManager.updateNode();
        }
    }
}
