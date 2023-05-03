using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;
public class clement : MonoBehaviour
{
    string test = "(111.46, 12121.60, 111.00)";

    void Start()
    {
        Vector3 zeub = StringToVector3Int(test);
    }

    public Vector3 StringToVector3Int(string stringVector)
    {

        if (stringVector.StartsWith("(") && stringVector.EndsWith(")"))
        {
            stringVector = stringVector.Substring(1, stringVector.Length - 1);
        }

        string[] sArray = stringVector.Split(',');

        for (int i = 0; i < sArray.Length; i++)
        {
            string s = sArray[i];
            s = s.Replace(")", "");
            sArray[i] = s;
        }

        var a = float.Parse(sArray[0], CultureInfo.InvariantCulture);
        var b = float.Parse(sArray[1], CultureInfo.InvariantCulture);
        var c = float.Parse(sArray[2], CultureInfo.InvariantCulture);

        Vector3 result = new Vector3(a, b, c);

        Debug.Log(result);

        return result;
    }
}
