using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TerrainTypeName { DeepWater, ShallowWater, Sand, Grass, Forest, Rock, Snow }
public enum TemperatureTypeName { Coldest, Colder, Cold, Warm, Warmer, Warmest }
public enum HumidityTypeName { Wettest, Wetter, Wet, Dry, Dryer, Dryest }

[System.Serializable]
public class HeightType
{
    public int id;
    [Range(0, 1)]
    public float height;
    public Color color;
}

[System.Serializable]
public class TerrainType : HeightType
{
    public TerrainTypeName terrainTypeName;
}

[System.Serializable]
public class TemperatureType : HeightType
{
    public TemperatureTypeName temperatureTypeName;
}

[System.Serializable]
public class HumidityType : HeightType
{
    public HumidityTypeName humidityTypeName;
}