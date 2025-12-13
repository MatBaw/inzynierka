using System.Linq;
using System.Reflection;
using UnityEngine;
using Tobii.GameIntegration.Net;

public class TobiiCsBindingsDump : MonoBehaviour
{
    void Start()
    {
        var t = typeof(TobiiGameIntegrationApi);

        var methods = t.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static)
                       .OrderBy(m => m.Name)
                       .Select(m => m.ToString());

        Debug.Log("TobiiGameIntegrationApi STATIC methods:\n" + string.Join("\n", methods));
    }
}
