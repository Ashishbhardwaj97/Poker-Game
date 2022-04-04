using System;
using UnityEngine;

public class TableInfoScript : MonoBehaviour
{

    [Serializable]
    public class TableData
    {
        public string table_type;
        public string end_time;
        public string game_type;
        public string table_name;
        public string table_id;
        public string start_time;
        public string rule_id;

        public int small_blind;
        public int big_blind;
        public int min_buy_in;
        public int min_auto_start;
        public int blinds_up;
        public int table_size;
        public int action_time;

        public bool mississippi_straddle;
        public bool auto_start;
        public bool buy_in_authorization;
        public bool video_mode;
    }

    [SerializeField]
    public TableData tableData;

}