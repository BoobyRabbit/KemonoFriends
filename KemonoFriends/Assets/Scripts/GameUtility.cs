using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

static public class GameUtility
{
    /// <summary>
    /// 文字列をint型に変換して返します。
    /// </summary>
    static public int StringToInt(string value)
    {
        int number = 0;
        if(int.TryParse(value, out number))
        {
            return number;
        }
        Debug.LogError($"\"{value}\" is not int.");
        return number;
    }

    /// <summary>
    /// 文字列をfloat型に変換して返します。
    /// </summary>
    static public float StringToFloat(string value)
    {
        float number = 0;
        if(float.TryParse(value, out number))
        {
            return number;
        }
        Debug.LogError($"\"{value}\" is not float.");
        return number;
    }

    /// <summary>
    /// 文字列をbool型に変換して返します。
    /// </summary>
    static public bool StringToBool(string value)
    {
        bool boolean = false;
        if(bool.TryParse(value, out boolean))
        {
            return boolean;
        }
        Debug.LogError($"\"{value}\" is not float.");
        return boolean;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="Type"></typeparam>
    /// <param name="filepath"></param>
    /// <returns></returns>
    static public Type Load<Type>(string filepath)
        where Type : UnityEngine.Object
    {
        Type type = Resources.Load<Type>(filepath);
        if(type == null)
        {
            Debug.LogError($"filepath = \"{filepath}\" is not found.");
        }
        return type;
    }
}
