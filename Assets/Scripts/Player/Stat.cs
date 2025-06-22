using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat<T>
{
    private T _value;
    public T Value
    {
        get => _value;
        set
        {
            if(!_value.Equals(value))
            {
                _value = value;
                OnChanged?.Invoke(_value);
            }
        }
    }

    public event Action<T> OnChanged;
}
