using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Windows;

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
        string[] lines = Regex.Split(csv, @"\n(?=(?:[^$]*\$[^$]*\$)*[^$]*$)");
        for (int i = 1; i < lines.Length; i++)
        {
            Regex csvSplitRegex = new Regex(@",(?=(?:[^$]*\$[^$]*\$)*[^$]*$)");

            string[] fields = csvSplitRegex.Split(lines[i]);

            // 따옴표 제거 (선택)
            for (int j = 0; j < fields.Length; j++)
            {
                fields[j] = fields[j].Trim().Trim('"').Trim('$');
            }

            

            T value = Parse(fields);

            if (value == null || string.IsNullOrEmpty(value.GetID())) continue;

            values.Add(value.GetID(), value);
        }

        return true;
    }
}