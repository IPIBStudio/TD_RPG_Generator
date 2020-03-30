using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// https://www.hackromtools.info/advance-map/

public enum Structure { Water, WaterEdge, Land, Tree, T1, T2h, T2v, T4, Rock, R1, R4, Grass, Flower, Edge, Building, Object }
public enum Movement { Able, Unable, Surf, Jump, Stairs }
public enum Event { Door, Signpost, Character, Script, Landing }
public enum Edge { None, NWe, Ne, NEe, Ee, SEe, Se, SWe, We, NWi, Ni, NEi, Ei, SEi, Si, SWi, Wi }

public class TileData
{
    public int x, y;

    public float terrainHeight;
    public float humidityHeight;
    public float temperatureHeight;

    public TerrainType terrainType;
    public TemperatureType temperatureType;
    public HumidityType humidityType;
    public BiomeType biomeType;

    public Structure structure;
    public Edge edge;
    public Movement movement;
    public Event triggerEvent;

    public bool isBiomeBorder;
    public bool isEdge;
    public int edgeHeight;

    public bool isCity;

    public TileData topLeft;
    public TileData top;
    public TileData topRight;
    public TileData right;
    public TileData bottomRight;
    public TileData bottom;
    public TileData bottomLeft;
    public TileData left;

    //Generator
    public TileData(int x, int y, float terrainHeight, float temperatureHeight, float humidityHeight)
    {
        this.x = x;
        this.y = y;
        this.terrainHeight = terrainHeight;
        this.temperatureHeight = temperatureHeight;
        this.humidityHeight = humidityHeight;
        this.isBiomeBorder = false;
        this.isCity = false;
    }

}
