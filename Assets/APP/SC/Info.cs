using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using System.Threading.Tasks;

public static class Info
{
    public static User user;

}
public class User
{
    public int id;
    public string name;
    public List<string> tags;
    public bool submited;

    public List<string> tags_to_List(string _tags)
    {
        return _tags.Split('|').ToList();
    }
    public string tags_to_string(List<string> _tags)
    {
        string _s = "";
        foreach (string _tag in _tags)
        {
            _s += _tag + "|";
        }
        return _s[..^1];
    }
    public User(int id, string name, string tags, int submited)
    {
        this.id = id;
        this.name = name;
        this.tags = this.tags_to_List(tags);
        this.submited = (submited != 0);
    }
}
public static class Tools
{
    public static async Task DeleteChildren(Transform trans)
    {
        foreach (Transform obj in trans.GetComponentsInChildren<Transform>())
        {
            if (obj.name != "Content") MonoBehaviour.Destroy(obj.gameObject);
        }
    }
}
public class chanel
{
    public int id;
    public string name;
    public bool noReaded;
    public chanel(int id, string name, bool noread)
    {
        this.id = id;
        this.name = name;
        this.noReaded = noread;
    }
}
