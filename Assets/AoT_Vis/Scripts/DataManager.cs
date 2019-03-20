namespace AoT_Vis
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityNpgsql;

    public class DataManager : MonoBehaviour
    {
        public ComponentManager componentManager;

        public string ViewTime { get; set; }
        public string ViewDate { get; set; }
        public string SelectedNodeId { get; set; }
        public string HighlightedNode { get; set; }

        //Node Info Data
        public List<string> NodeIds { get; private set; }
        public List<string> NodeLocations { get; private set; }
        public List<string> NodeAddresses { get; private set; }
        public List<string> NodeStart { get; private set; }
        public List<string> NodeEnd { get; private set; }

        // Use this for initialization
        void Awake()
        {
            NodeLocations = new List<string>();
            NodeIds = new List<string>();
            NodeAddresses = new List<string>();
            NodeStart = new List<string>();
            NodeEnd = new List<string>();

            fetchNodeData();
            //fakeNodeData();
            SelectedNodeId = NodeIds[0];
            ViewDate = "2018-06-01";
            ViewTime = "12:00:00";
        }

        // Update is called once per frame
        void Update()
        {
            
        }


        private void fakeNodeData()
        {
            NodeLocations.Add("41.878377, -87.627678");
            NodeIds.Add("001e0610ba46");
            NodeAddresses.Add("State St & Jackson Blvd Chicago IL");
            NodeStart.Add("2017-10-09 00:00:00");
            NodeEnd.Add("");

            NodeLocations.Add("41.923996, -87.761072");
            NodeIds.Add("001e0610ee5d");
            NodeAddresses.Add("Long Ave & Fullerton Ave Chicago IL");
            NodeStart.Add("2018-02-23 00:00:00");
            NodeEnd.Add("");
        }

        private void fetchNodeData()
        {
            //Database Stuff
            NpgsqlConnection conn = new NpgsqlConnection("Server=flick.cs.niu.edu;Port=5432;User Id=readonly;Database=aot;CommandTimeout=240");
            conn.Open();
            NpgsqlCommand command = new NpgsqlCommand("SELECT FORMAT('%s, %s', lat, lon) lat_lon, aot_node_id, address, start_timestamp, end_timestamp FROM aot_nodes WHERE project_id = 'AoT_Chicago' AND aot_node_id != '001e0610ef73';", conn);

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
                }
            }
            catch (System.Exception ex)
            {
                print(ex.Message);
            }

            conn.Close();
        }

        public void changeSelectedNode(string newNodeId)
        {
            SelectedNodeId = newNodeId;
            componentManager.updateWorldPosition();
        }

    }
}
