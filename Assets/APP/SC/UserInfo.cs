using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

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
        this.submited = (submited!=0);
    }


}
