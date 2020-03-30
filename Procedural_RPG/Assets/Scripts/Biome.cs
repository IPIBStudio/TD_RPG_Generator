using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum ThemeColor
{
    OrangeLight, Orange, OrangeDark,
    YellowLight, Yellow, YellowDark,
    BlueLight, Blue, BlueDark,
    RedLight, Red, RedDark,
    GreenLight, Green, GreenDark,
    PurpleLight, Purple, PurpleDark
}

public enum BiomeType
{
    /*
    Default, Ice, Snow1, Snow2, ColdForest, Forest, Spider, Fungus,
    Swamp, Pound, // ça c'est plus des biomes techniques, à revoir pour l'implémentation ?
    Land1, Land2, Land3,Tropical1, Tropical2,Savane, Desert1, Desert2
    */
    /*
    HumidTropicalForest,DryTropicalForest,ConiferousTropicalForest,TropicalSavana,TemperateSavana,
    Desert, Mediterranean,TemperateForest,ConiferousTemperateForest,Taiga,Toundra,Ice
    */

    Desert, Savanna, TropicalRainforest,
    Grassland, Woodland, SeasonalForest, TemperateRainforest,
    BorealForest, Tundra, Ice
}

public class Biome
{
    public BiomeType[,] BiomeTable = new BiomeType[6, 6] {
    { BiomeType.Ice, BiomeType.Tundra, BiomeType.Grassland,    BiomeType.Desert,              BiomeType.Desert,              BiomeType.Desert },              //DRYEST
    { BiomeType.Ice, BiomeType.Tundra, BiomeType.Grassland,    BiomeType.Desert,              BiomeType.Desert,              BiomeType.Desert },              //DRYER
    { BiomeType.Ice, BiomeType.Tundra, BiomeType.Woodland,     BiomeType.Woodland,            BiomeType.Savanna,             BiomeType.Savanna },             //DRY
    { BiomeType.Ice, BiomeType.Tundra, BiomeType.BorealForest, BiomeType.Woodland,            BiomeType.Savanna,             BiomeType.Savanna },             //WET
    { BiomeType.Ice, BiomeType.Tundra, BiomeType.BorealForest, BiomeType.SeasonalForest,      BiomeType.TropicalRainforest,  BiomeType.TropicalRainforest },  //WETTER
    { BiomeType.Ice, BiomeType.Tundra, BiomeType.BorealForest, BiomeType.TemperateRainforest, BiomeType.TropicalRainforest,  BiomeType.TropicalRainforest }}; //WETTEST
    //COLDEST        //COLDER          //COLD                  //HOT                          //HOTTER                       //HOTTEST

    [Header("General Settings")]
    public string type;
    public BiomeType biomeType;

    [Range(0, 100)]
    public int temperature;

    public bool cityBiome; // ???
    public ThemeColor biomeColor;
    public Color pixelColor;

    // public enum Edge { None, NWe, Ne, NEe, Ee, SEe, Se, SWe, We, NWi, Ni, NEi, Ei, SEi, Si, SWi, Wi }
    [Header("Terrain Tiles")]

    public Tile groundTile; // central, int, ext // = à edger
    public Tile sandTile;
    public Tile NWe;
    public Tile Ne;
    public Tile NEe;
    public Tile Ee;
    public Tile SEe;
    public Tile Se;
    public Tile SWe;
    public Tile We;
    public Tile NWi;
    public Tile Ni;
    public Tile NEi;
    public Tile Ei;
    public Tile SEi;
    public Tile Si;
    public Tile SWi;
    public Tile Wi;

    public Tile waterTile; // int, ext // = à edger
    public Tile wNWe;
    public Tile wNe;
    public Tile wNEe;
    public Tile wEe;
    public Tile wSEe;
    public Tile wSe;
    public Tile wSWe;
    public Tile wWe;
    public Tile wNWi;
    public Tile wNi;
    public Tile wNEi;
    public Tile wEi;
    public Tile wSEi;
    public Tile wSi;
    public Tile wSWi;
    public Tile wWi;

    [Header("Structs Tiles")]
    public Tile tree1x1Tile; // 1 tile, 2 tiles, 4 tiles, 9 tiles, 16 tiles, 20 tiles
    public Tile tree1x2Tile;
    public Tile tree2x1Tile;
    public Tile tree2x3Tile;
    public Tile grassTile; // short, medium, high
    public Tile flowerTile; // 1 tile +++
    public Tile rock1Tile;
    public Tile rock2Tile;
    public Tile rock4Tile;

    [Header("Building Settings")]
    public Tile[] smallHouses; // open house, closed house, open house with flower, closed house with flower, open house with floor
    public Tile bigHouse;
    public Tile arena;
    public Tile smallShop;
    public Tile bigShop;
    public Tile center;

    // public Building arena;
    // class Building : Tile tile; Vector2 doorPos;
    // truc à faire
}
