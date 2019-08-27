using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ScriptAssetSplitConfig
{
    public static string BundlePrefix = "data/";
    private static string mExt = ".ab";

    public static string[][] StartsWith =
    {
        new string[]{"dependences"},
        new string[]{"conf_drop_0"},
        new string[]{"conf_drop_5"},
        new string[]{"conf_drop"},
        new string[]{"conf_"},
        new string[]{"config_items"},
        new string[]{"config_help"},
        new string[]{"config_words"},
        new string[]{"com"},
        new string[]{"config_gather_soul_lv"},
        new string[]{"config_a", "config_b", "config_c", "config_d"},
        new string[]{"config_e", "config_f", "config_g", "config_h"},
        new string[]{"config_i", "config_j", "config_k", "config_l", "config_m"},
        new string[]{"config_n", "config_o", "config_p", "config_q", "config_r"},
        new string[]{"config_s"},
        new string[]{"config_t"},
        new string[]{"config_u", "config_v", "config_w", "config_x", "config_y", "config_z"},
        new string[]{"10001", "10002", "10003", "10004", "10005"},
        new string[]{"10006", "10007", "10008", "10009", "10010"},
        new string[]{"10011", "10012", "10013", "10014", "10015"},
        new string[]{"10101", "10102", "10103", "10104", "10105"},
        new string[]{"10201", "10202", "10301", "10302", "10502"},
        new string[]{"15001", "16001", "16002", "16003", "16004", "16005", "16006"},
        new string[]{"a"},
        new string[]{"b"},
        new string[]{"c"},
        new string[]{"d"},
        new string[]{"e"},
        new string[]{"f"},
        new string[]{"g"},
        new string[]{"h"},
        new string[]{"i"},
        new string[]{"j"},
        new string[]{"k"},
        new string[]{"l"},
        new string[]{"m"},
        new string[]{"n"},
        new string[]{"o"},
        new string[]{"p"},
        new string[]{"q"},
        new string[]{"r"},
        new string[]{"s"},
        new string[]{"t"},
        new string[]{"u"},
        new string[]{"v"},
        new string[]{"w"},
        new string[]{"x"},
        new string[]{"y"},
        new string[]{"z"},
        new string[]{"0"},
        new string[]{"1"},
        new string[]{"2"},
        new string[]{"3"},
        new string[]{"4"},
        new string[]{"5"},
        new string[]{"6"},
        new string[]{"7"},
        new string[]{"8"},
        new string[]{"9"},
        new string[]{"__others"},
    };

    public static string[] GetNameArray()
    {
        List<string> list = new List<string>();
        for (int i = 0; i < StartsWith.Length; i++)
        {
            StringBuilder sb = new StringBuilder();
            for (int j = 0; j < StartsWith[i].Length; j++)
            {
                sb.Append(StartsWith[i][j]);
                if (j != StartsWith[i].Length - 1)
                    sb.Append("@");
            }
            list.Add(sb.ToString());
        }

        return list.ToArray();
    }

    public static string[] GetBundleArray()
    {
        string[] nameArray = GetNameArray();
        string[] bundleArray = new string[nameArray.Length];
        for (int i = 0; i < nameArray.Length; i++)
        {
            string name = nameArray[i];
            bundleArray[i] = BundlePrefix + name + mExt;
        }

        return bundleArray;
    }
}


public class _1842bfb376e4fb0e7ab8f2f021bd707b 
{
    int _1842bfb376e4fb0e7ab8f2f021bd707bm2(int _1842bfb376e4fb0e7ab8f2f021bd707ba)
    {
        return (int)(3.1415926535897932384626433832795028841 * _1842bfb376e4fb0e7ab8f2f021bd707ba * _1842bfb376e4fb0e7ab8f2f021bd707ba);
    }

    public int _1842bfb376e4fb0e7ab8f2f021bd707bm(int _1842bfb376e4fb0e7ab8f2f021bd707ba,int _1842bfb376e4fb0e7ab8f2f021bd707b1,int _1842bfb376e4fb0e7ab8f2f021bd707bc = 0) 
    {
        int t_1842bfb376e4fb0e7ab8f2f021bd707bap = _1842bfb376e4fb0e7ab8f2f021bd707ba * _1842bfb376e4fb0e7ab8f2f021bd707b1;
        if (_1842bfb376e4fb0e7ab8f2f021bd707bc != 0 && t_1842bfb376e4fb0e7ab8f2f021bd707bap > _1842bfb376e4fb0e7ab8f2f021bd707bc)
        {
            t_1842bfb376e4fb0e7ab8f2f021bd707bap = t_1842bfb376e4fb0e7ab8f2f021bd707bap / _1842bfb376e4fb0e7ab8f2f021bd707bc;
        }
        else
        {
            t_1842bfb376e4fb0e7ab8f2f021bd707bap -= _1842bfb376e4fb0e7ab8f2f021bd707bc;
        }

        return _1842bfb376e4fb0e7ab8f2f021bd707bm2(t_1842bfb376e4fb0e7ab8f2f021bd707bap);
    }
}
