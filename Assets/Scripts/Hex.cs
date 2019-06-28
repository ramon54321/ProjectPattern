using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Weather
{
    public float temperature;
    public float humidity;
}

[Serializable]
public class Hex
{
    public Vector2Int position;
    public Weather weather;
}
