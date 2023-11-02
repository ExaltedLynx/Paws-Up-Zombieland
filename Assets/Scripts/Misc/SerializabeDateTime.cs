using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializabeDateTime : ISerializationCallbackReceiver
{
    [SerializeField] private int month;
    [SerializeField] private int day;
    [SerializeField] private int year;
    public DateTime dateTime = DateTime.Today;

    public void OnAfterDeserialize()
    {
        month = dateTime.Month;
        day = dateTime.Day;
        year = dateTime.Year;
    }

    public void OnBeforeSerialize()
    {
        month = dateTime.Month;
        day = dateTime.Day;
        year = dateTime.Year;
    }
}
