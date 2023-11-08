using System;
using UnityEngine;

[Serializable]
public class SerializableDateTime : ISerializationCallbackReceiver
{
    [SerializeField] private int month;
    [SerializeField] private int day;
    [SerializeField] private int year;
    public DateTime dateTime;

    public SerializableDateTime()
    {
        this.dateTime = DateTime.Today;
    }

    public void OnAfterDeserialize()
    {
        dateTime = new DateTime(year, month, day);
    }

    public void OnBeforeSerialize()
    {
        month = dateTime.Month;
        day = dateTime.Day;
        year = dateTime.Year;
    }
}
