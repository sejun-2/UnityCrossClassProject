using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataTableParser<T> where T : IUsableID
{
    public Func<string[], T> Parse;
    [SerializeField] Dictionary<string, T> values;
    public Dictionary<string, T> Values { get { return values; } }

    public DataTableParser(Func<string[], T> Parse)
    {
        this.Parse = Parse;
        values = new Dictionary<string, T>();
    }

    public bool Load(in string csv)
    {
        string[] lines = csv.Split('\n');
        for (int i = 1; i < lines.Length; i++)
        {
            T value = Parse(lines[i].Split(','));
            values.Add(value.GetID(), value);
        }

        return true;
    }
}