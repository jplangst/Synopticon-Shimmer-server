using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;

namespace ShimmerAPI
{
    class Util
    {
        public static SortedDictionary<string, string> GetAllShimmerID()
        {
            // The WMI query 
            const string Win32_PnPEntity = "SELECT * FROM Win32_PnPEntity ";
            // const string Win32_SerialPort = "Win32_SerialPort";

            // List<string> ShimmerID = new List<string>();
            SortedDictionary<string, string> ShimmerName = new SortedDictionary<string, string>();
            //SortedDictionary<string, string> ShimmerPort = new SortedDictionary<string, string>();

            SelectQuery WMIquery = new SelectQuery(Win32_PnPEntity);
            ManagementObjectSearcher WMIqueryResults = new ManagementObjectSearcher(WMIquery);

            // Make sure results were found
            if (WMIqueryResults == null)
                return null;

            // Scan query results to find port
            ManagementObjectCollection MOC = WMIqueryResults.Get();

            foreach (ManagementObject mo in MOC)
            {
                object captionObject = mo.GetPropertyValue("Caption");
                if (captionObject != null)
                {
                    string caption = captionObject.ToString();
                    if (caption.Contains("Shimmer"))
                    {


                        string path = mo.ToString();
                        int index = path.LastIndexOf("BLUETOOTHDEVICE_");
                        string id = path.Substring(index + 16);
                        id = System.Text.RegularExpressions.Regex.Replace(id, "[^a-zA-Z0-9]+", "", System.Text.RegularExpressions.RegexOptions.Compiled);
                        ShimmerName.Add(caption, id);
                    }
                }
            }

            return ShimmerName;
        }
    }
}
