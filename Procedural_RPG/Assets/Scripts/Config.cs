using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum DrawMode { Pixel, Tile };
public enum DrawMap { Height, HeightType, Temperature, Humidity, Biome, City, Structure, Movement, Event }
public enum DrawStyle { Dawn };

[System.Serializable]
public class Config
{
    [Header("General Settings")]
    public bool autoUpdate;
    public DrawMode drawMode;
    public DrawMap drawMap;
    public DrawStyle drawStyle;
    public NoiseType noiseType;

    public const int chunkSize = 257;
    public bool commonOffset;
    public Vector2Int offset;
    public bool commonSeed;
    public int seed;

    [Header("Rendering")]
    public MeshRenderer PixelRenderer;
    public Tilemap TileRenderer;

    // rajouter des vars genre humidity++ temperature++ water++ 
    // qui s'additionne au height pour controle plus simple sur les types
    [Header("Terrain Map Settings")]
    public TerrainMap terrainMap;
    [Header("Temperature Map Settings")]
    public TemperatureMap temperatureMap;
    [Header("Humidity Map Settings")]
    public HumidityMap humidityMap;
    [Header("Biomes Settings")]
    public Biome[] biomes;
}
