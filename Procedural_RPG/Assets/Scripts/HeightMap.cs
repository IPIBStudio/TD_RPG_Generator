using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//public enum HeightMapType { Terrain, Humidity, Temperature }

[System.Serializable]
public class HeightMap
{
    public float[,] heightMap;

    [Header("Noise Settings")]
    public int seed;
    public Vector2Int offset;
    public bool layeringNoise;
    [Range(5, 500)]
    public int scale;
    [Range(0, 10)]
    public int octaves;
    [Range(0, 1)]
    public float persistance;
    [Range(0, 50)]
    public float lacunarity;

}

[System.Serializable]
public class TerrainMap : HeightMap
{
    [Range(0, 50)]
    public int steps;
    public bool drawUnderwaterSteps;
    public TerrainType[] terrainTypes;
}

[System.Serializable]
public class TemperatureMap : HeightMap
{
    // public bool latitudeOriented;
    public TemperatureType[] temperatureTypes;
}

[System.Serializable]
public class HumidityMap : HeightMap
{
    public HumidityType[] humidityTypes;
    // need to use temperature
}
