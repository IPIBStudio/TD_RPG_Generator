using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
public enum DrawMode { Pixel, Tile, StructureMap, MovementMap, EventMap };
// juste les deux premiers ==> Pixel Tile, les autres sont en dessous (je garde pour garder le précédent)
public enum DrawMap { Height, HeightType, Temperature, Humidity, Biome, City, Structure, Movement, Event }
public enum DrawStyle { Dawn };
public enum Edge { None, NWe, Ne, NEe, Ee, SEe, Se, SWe, We, NWi, Ni, NEi, Ei, SEi, Si, SWi, Wi }

public class Generator : MonoBehaviour
{
    TileData[,] tileDataMap;

    [Header("General Settings")]
    public bool autoUpdate;
    public DrawMode drawMode;
    public DrawMap drawMap;
    public DrawStyle drawStyle;
    public NoiseType noiseType;

    [Range(1, 4)]
    // public int mapScale;
    public const int chunkSize = 257;
    Vector2Int mapSize;
    public bool commonOffset;
    public Vector2Int offset;
    public bool commonSeed;
    public int seed;

    // chunk handling

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

    BiomeType[,] BiomeTable = new BiomeType[6, 6] {
    { BiomeType.Ice, BiomeType.Tundra, BiomeType.Grassland,    BiomeType.Desert,              BiomeType.Desert,              BiomeType.Desert },              //DRYEST
    { BiomeType.Ice, BiomeType.Tundra, BiomeType.Grassland,    BiomeType.Desert,              BiomeType.Desert,              BiomeType.Desert },              //DRYER
    { BiomeType.Ice, BiomeType.Tundra, BiomeType.Woodland,     BiomeType.Woodland,            BiomeType.Savanna,             BiomeType.Savanna },             //DRY
    { BiomeType.Ice, BiomeType.Tundra, BiomeType.BorealForest, BiomeType.Woodland,            BiomeType.Savanna,             BiomeType.Savanna },             //WET
    { BiomeType.Ice, BiomeType.Tundra, BiomeType.BorealForest, BiomeType.SeasonalForest,      BiomeType.TropicalRainforest,  BiomeType.TropicalRainforest },  //WETTER
    { BiomeType.Ice, BiomeType.Tundra, BiomeType.BorealForest, BiomeType.TemperateRainforest, BiomeType.TropicalRainforest,  BiomeType.TropicalRainforest }}; //WETTEST
    //COLDEST        //COLDER          //COLD                  //HOT                          //HOTTER                       //HOTTEST

    [Header("Cities Settings")]
    [Range(0, 100)]
    public int citiesDensity;
    //public int citySeed; //unimpletend, doesn't work
    [Range(0, 100)]
    public int townsRatio;
    [Range(0, 100)]
    public int citiesRatio;
    [Range(0, 100)]
    public int metropolesRatio;
    public Vector3 ratios;
    List<City> cities;

    void Start()
    {
        Dictionary<Vector2, EndlessWorld.TerrainChunk> terrainChunkDictionary = EndlessWorld.terrainChunkDictionary;
        DrawMap();
        //DrawPixelMap(EndlessWorld.terrainChunkDictionary);
    }

    public MapData GenerateMapData(Vector2Int chunkOffset)
    {
        // chunkSize = (int)Mathf.Pow(2, mapScale + 6) + 1; // 3 = 512
        mapSize = new Vector2Int(chunkSize, chunkSize);

        //terrainMap = new TerrainMap();
        //temperatureMap = new TemperatureMap();
        //humidityMap = new HumidityMap();

        // TERRAIN MAPS
        // Offset handle
        if (commonOffset)
        {
            terrainMap.offset = offset + chunkOffset;
            temperatureMap.offset = offset + chunkOffset;
            humidityMap.offset = offset + chunkOffset;
        }
        else
        {
            terrainMap.offset += chunkOffset;
            temperatureMap.offset += chunkOffset;
            humidityMap.offset += chunkOffset;
        }
        // Seed handle
        if (commonSeed)
        {
            terrainMap.seed = seed;
            temperatureMap.seed = seed;
            humidityMap.seed = seed;
        }

        // Height
        if (terrainMap.layeringNoise)
        {
            terrainMap.heightMap = Noises.Layering2Noise(
                Noises.Perlin(mapSize.x, mapSize.y, terrainMap.seed, terrainMap.scale, terrainMap.octaves, terrainMap.persistance, terrainMap.lacunarity, terrainMap.offset),
                Noises.Perlin(mapSize.x, mapSize.y, terrainMap.seed, terrainMap.scale * 2, terrainMap.octaves, terrainMap.persistance, terrainMap.lacunarity, terrainMap.offset));
        }
        else
        {
            terrainMap.heightMap = Noises.Perlin(mapSize.x, mapSize.y, terrainMap.seed, terrainMap.scale, terrainMap.octaves, terrainMap.persistance, terrainMap.lacunarity, terrainMap.offset);
        }

        // Temperature
        temperatureMap.heightMap = Noises.Perlin(mapSize.x, mapSize.y, temperatureMap.seed, temperatureMap.scale, temperatureMap.octaves, temperatureMap.persistance, temperatureMap.lacunarity, temperatureMap.offset);

        // Humidity
        humidityMap.heightMap = Noises.Perlin(mapSize.x, mapSize.y, humidityMap.seed, humidityMap.scale, humidityMap.octaves, humidityMap.persistance, humidityMap.lacunarity, humidityMap.offset);

        // Grass

        //Forest

        // TECH MAPS
        // struct
        // mvmt

        // CITIES

        tileDataMap = new TileData[mapSize.x, mapSize.y];

        //tileDataMap Analyze
        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                tileDataMap[x, y] = new TileData(x, y,
                    terrainMap.heightMap[x, y],
                    temperatureMap.heightMap[x, y],
                    humidityMap.heightMap[x, y]);

                // setting heights by type
                for (int i = 0; i < terrainMap.terrainTypes.Length; i++)
                {
                    if (tileDataMap[x, y].terrainHeight <= terrainMap.terrainTypes[i].height)
                    {
                        tileDataMap[x, y].terrainType = terrainMap.terrainTypes[i];
                        break;
                    }
                }

                //setting heights by range
                float step = 1f / (float)terrainMap.steps;

                for (int i = 0; i < terrainMap.steps; i++)
                {
                    if (tileDataMap[x, y].terrainHeight <= step * i)
                    {
                        tileDataMap[x, y].edgeHeight = i;
                        break;
                    }
                }

                // setting temperature by type
                // need to use height
                // + bool latitudeOriented
                // ICI FAUT INVERSER C PAS LOGIQUE LORDRE - dans l'inspector
                for (int i = 0; i < temperatureMap.temperatureTypes.Length; i++)
                {
                    if (tileDataMap[x, y].temperatureHeight <= temperatureMap.temperatureTypes[i].height)
                    {
                        tileDataMap[x, y].temperatureType = temperatureMap.temperatureTypes[i];
                        break;
                    }
                }

                // setting humidity by type
                // need to use temperature
                for (int i = 0; i < humidityMap.humidityTypes.Length; i++)
                {
                    if (tileDataMap[x, y].humidityHeight <= humidityMap.humidityTypes[i].height)
                    {
                        tileDataMap[x, y].humidityType = humidityMap.humidityTypes[i];
                        break;
                    }
                }

                //setting biome by temperature & humidity
                tileDataMap[x, y].biomeType = BiomeTable[tileDataMap[x, y].humidityType.id, tileDataMap[x, y].temperatureType.id];
            }
        }

        //Biome & Height edges Analyze
        //both need to handle iEdges for smoothness
        for (int x = 1; x < mapSize.x - 1; x++)
        {
            for (int y = 1; y < mapSize.y - 1; y++)
            {
                tileDataMap[x, y].topLeft = tileDataMap[x + 1, y - 1];
                tileDataMap[x, y].top = tileDataMap[x, y - 1];
                tileDataMap[x, y].topRight = tileDataMap[x - 1, y - 1];
                tileDataMap[x, y].right = tileDataMap[x - 1, y];
                tileDataMap[x, y].bottomRight = tileDataMap[x - 1, y + 1];
                tileDataMap[x, y].bottom = tileDataMap[x, y + 1];
                tileDataMap[x, y].bottomLeft = tileDataMap[x + 1, y + 1];
                tileDataMap[x, y].left = tileDataMap[x + 1, y];

                //Biome borders Analyze
                // peut être rendre ça plus clair avec : 
                // https://docs.microsoft.com/fr-fr/dotnet/api/system.enum.getvalues?view=netframework-4.8
                // valoriser HeightName name;
                //exterior --- logiquement interior c'est pareil mais > à la place de < ?
                if (tileDataMap[x, y].left.terrainType.id == tileDataMap[x, y].terrainType.id &&
                    tileDataMap[x, y].top.terrainType.id == tileDataMap[x, y].terrainType.id &&
                    tileDataMap[x, y].right.terrainType.id < tileDataMap[x, y].terrainType.id &&
                    tileDataMap[x, y].bottom.terrainType.id < tileDataMap[x, y].terrainType.id)
                {
                    //structMap[x, y] = Structure.Edge;
                    //tileDataMap[x, y].edge = Edge.SEe;
                    tileDataMap[x, y].isBiomeBorder = true;
                }
                else if (tileDataMap[x, y].left.terrainType.id == tileDataMap[x, y].terrainType.id &&
                    tileDataMap[x, y].bottom.terrainType.id == tileDataMap[x, y].terrainType.id &&
                    tileDataMap[x, y].right.terrainType.id < tileDataMap[x, y].terrainType.id &&
                    tileDataMap[x, y].top.terrainType.id < tileDataMap[x, y].terrainType.id)
                {
                    //structMap[x, y].terrainType.id = Structure.Edge;
                    //tileDataMap[x, y].edge = Edge.NEe;
                    tileDataMap[x, y].isBiomeBorder = true;
                }
                else if (tileDataMap[x, y].right.terrainType.id == tileDataMap[x, y].terrainType.id &&
                    tileDataMap[x, y].bottom.terrainType.id == tileDataMap[x, y].terrainType.id &&
                    tileDataMap[x, y].left.terrainType.id < tileDataMap[x, y].terrainType.id &&
                    tileDataMap[x, y].top.terrainType.id < tileDataMap[x, y].terrainType.id)
                {
                    //structMap[x, y] = Structure.Edge;
                    //tileDataMap[x, y].edge = Edge.NWe;
                    tileDataMap[x, y].isBiomeBorder = true;
                }
                else if (tileDataMap[x, y].right.terrainType.id == tileDataMap[x, y].terrainType.id &&
                    tileDataMap[x, y].top.terrainType.id == tileDataMap[x, y].terrainType.id &&
                    tileDataMap[x, y].left.terrainType.id < tileDataMap[x, y].terrainType.id &&
                    tileDataMap[x, y].bottom.terrainType.id < tileDataMap[x, y].terrainType.id)
                {
                    //structMap[x, y] = Structure.Edge;
                    //tileDataMap[x, y].edge = Edge.SWe;
                    tileDataMap[x, y].isBiomeBorder = true;
                }
                else if (tileDataMap[x, y].right.terrainType.id < tileDataMap[x, y].terrainType.id &&
                    tileDataMap[x, y].top.terrainType.id == tileDataMap[x, y].terrainType.id &&
                    tileDataMap[x, y].bottom.terrainType.id == tileDataMap[x, y].terrainType.id)
                {
                    //structMap[x, y] = Structure.Edge;
                    //tileDataMap[x, y].edge = Edge.Ee;
                    tileDataMap[x, y].isBiomeBorder = true;
                }
                else if (tileDataMap[x, y].left.terrainType.id < tileDataMap[x, y].terrainType.id &&
                    tileDataMap[x, y].top.terrainType.id == tileDataMap[x, y].terrainType.id &&
                    tileDataMap[x, y].bottom.terrainType.id == tileDataMap[x, y].terrainType.id)
                {
                    //structMap[x, y] = Structure.Edge;
                    //tileDataMap[x, y].edge = Edge.We;
                    tileDataMap[x, y].isBiomeBorder = true;
                }
                else if (tileDataMap[x, y].top.terrainType.id < tileDataMap[x, y].terrainType.id &&
                    tileDataMap[x, y].right.terrainType.id == tileDataMap[x, y].terrainType.id &&
                    tileDataMap[x, y].left.terrainType.id == tileDataMap[x, y].terrainType.id)
                {
                    //structMap[x, y] = Structure.Edge;
                    //tileDataMap[x, y].edge = Edge.Ne;
                    tileDataMap[x, y].isBiomeBorder = true;
                }
                else if (tileDataMap[x, y].bottom.terrainType.id < tileDataMap[x, y].terrainType.id &&
                    tileDataMap[x, y].right.terrainType.id == tileDataMap[x, y].terrainType.id &&
                    tileDataMap[x, y].left.terrainType.id == tileDataMap[x, y].terrainType.id)
                {
                    //structMap[x, y] = Structure.Edge;
                    //tileDataMap[x, y].edge = Edge.Se;
                    tileDataMap[x, y].isBiomeBorder = true;
                }
                // fuck lonely boys
                else if (tileDataMap[x, y].left.terrainType.id < tileDataMap[x, y].terrainType.id &&
                    tileDataMap[x, y].right.terrainType.id < tileDataMap[x, y].terrainType.id)
                {
                    tileDataMap[x, y].terrainType = tileDataMap[x, y].left.terrainType;
                }
                else if (tileDataMap[x, y].top.terrainType.id < tileDataMap[x, y].terrainType.id &&
                    tileDataMap[x, y].bottom.terrainType.id < tileDataMap[x, y].terrainType.id)
                {
                    tileDataMap[x, y].terrainType = tileDataMap[x, y].top.terrainType;
                }
                else
                {
                    tileDataMap[x, y].edge = Edge.None;
                }

                //Edges Analyze
                //exterior
                if (tileDataMap[x, y].left.edgeHeight == tileDataMap[x, y].edgeHeight &&
                    tileDataMap[x, y].top.edgeHeight == tileDataMap[x, y].edgeHeight &&
                    tileDataMap[x, y].right.edgeHeight < tileDataMap[x, y].edgeHeight &&
                    tileDataMap[x, y].bottom.edgeHeight < tileDataMap[x, y].edgeHeight)
                {
                    //structMap[x, y] = Structure.Edge;
                    tileDataMap[x, y].edge = Edge.SEe;
                    tileDataMap[x, y].isEdge = true;
                }
                else if (tileDataMap[x, y].left.edgeHeight == tileDataMap[x, y].edgeHeight &&
                    tileDataMap[x, y].bottom.edgeHeight == tileDataMap[x, y].edgeHeight &&
                    tileDataMap[x, y].right.edgeHeight < tileDataMap[x, y].edgeHeight &&
                    tileDataMap[x, y].top.edgeHeight < tileDataMap[x, y].edgeHeight)
                {
                    //structMap[x, y] = Structure.Edge;
                    tileDataMap[x, y].edge = Edge.NEe;
                    tileDataMap[x, y].isEdge = true;
                }
                else if (tileDataMap[x, y].right.edgeHeight == tileDataMap[x, y].edgeHeight &&
                    tileDataMap[x, y].bottom.edgeHeight == tileDataMap[x, y].edgeHeight &&
                    tileDataMap[x, y].left.edgeHeight < tileDataMap[x, y].edgeHeight &&
                    tileDataMap[x, y].top.edgeHeight < tileDataMap[x, y].edgeHeight)
                {
                    //structMap[x, y] = Structure.Edge;
                    tileDataMap[x, y].edge = Edge.NWe;
                    tileDataMap[x, y].isEdge = true;
                }
                else if (tileDataMap[x, y].right.edgeHeight == tileDataMap[x, y].edgeHeight &&
                    tileDataMap[x, y].top.edgeHeight == tileDataMap[x, y].edgeHeight &&
                    tileDataMap[x, y].left.edgeHeight < tileDataMap[x, y].edgeHeight &&
                    tileDataMap[x, y].bottom.edgeHeight < tileDataMap[x, y].edgeHeight)
                {
                    //structMap[x, y] = Structure.Edge;
                    tileDataMap[x, y].edge = Edge.SWe;
                    tileDataMap[x, y].isEdge = true;
                }
                else if (tileDataMap[x, y].top.edgeHeight == tileDataMap[x, y].edgeHeight &&
                    tileDataMap[x, y].bottom.edgeHeight == tileDataMap[x, y].edgeHeight &&
                    tileDataMap[x, y].right.edgeHeight < tileDataMap[x, y].edgeHeight)
                {
                    //structMap[x, y] = Structure.Edge;
                    tileDataMap[x, y].edge = Edge.Ee;
                    tileDataMap[x, y].isEdge = true;
                }
                else if (tileDataMap[x, y].top.edgeHeight == tileDataMap[x, y].edgeHeight &&
                    tileDataMap[x, y].bottom.edgeHeight == tileDataMap[x, y].edgeHeight &&
                    tileDataMap[x, y].left.edgeHeight < tileDataMap[x, y].edgeHeight)
                {
                    //structMap[x, y] = Structure.Edge;
                    tileDataMap[x, y].edge = Edge.We;
                    tileDataMap[x, y].isEdge = true;
                }
                else if (tileDataMap[x, y].right.edgeHeight == tileDataMap[x, y].edgeHeight &&
                    tileDataMap[x, y].left.edgeHeight == tileDataMap[x, y].edgeHeight &&
                    tileDataMap[x, y].top.edgeHeight < tileDataMap[x, y].edgeHeight)
                {
                    //structMap[x, y] = Structure.Edge;
                    tileDataMap[x, y].edge = Edge.Ne;
                    tileDataMap[x, y].isEdge = true;
                }
                else if (tileDataMap[x, y].right.edgeHeight == tileDataMap[x, y].edgeHeight &&
                    tileDataMap[x, y].left.edgeHeight == tileDataMap[x, y].edgeHeight &&
                    tileDataMap[x, y].bottom.edgeHeight < tileDataMap[x, y].edgeHeight)
                {
                    //structMap[x, y] = Structure.Edge;
                    tileDataMap[x, y].edge = Edge.Se;
                    tileDataMap[x, y].isEdge = true;
                }

            }
        }

        return new MapData(tileDataMap, drawMap, terrainMap.drawUnderwaterSteps, biomes);
    }

    public void DrawMap()
    {
        MapData mapData = GenerateMapData(Vector2Int.zero);

        terrainMap.HeightMapRenderer.materials[0].mainTexture = TextureGenerator.GetTexture(
        mapData.tileDataMap,
        drawMap,
        terrainMap.drawUnderwaterSteps,
        biomes);


    }

    // nul
    public void DrawPixelMap(Dictionary<Vector2, EndlessWorld.TerrainChunk> terrainChunkDictionary)
    {
        Vector2 currentChunkCoord = new Vector2(Mathf.RoundToInt(EndlessWorld.viewerPosition.x / chunkSize),
                                                Mathf.RoundToInt(EndlessWorld.viewerPosition.y / chunkSize));

        for (int xOffset = -EndlessWorld.chunkVisibleInViewDst; xOffset <= EndlessWorld.chunkVisibleInViewDst; xOffset++)
        {
            for (int yOffset = -EndlessWorld.chunkVisibleInViewDst; yOffset <= EndlessWorld.chunkVisibleInViewDst; yOffset++)
            {
                Vector2 viewedChunkCoord = new Vector2(currentChunkCoord.x + xOffset - chunkSize / 2, currentChunkCoord.y + yOffset - chunkSize / 2);

                if (terrainChunkDictionary[viewedChunkCoord].IsVisible())
                {
                    MapData mapData = GenerateMapData(new Vector2Int(
                        Mathf.RoundToInt(terrainChunkDictionary[viewedChunkCoord].position.x),
                        Mathf.RoundToInt(terrainChunkDictionary[viewedChunkCoord].position.y)));

                    terrainMap.HeightMapRenderer.materials[0].mainTexture = TextureGenerator.GetTexture(
                    mapData.tileDataMap,
                    drawMap,
                    terrainMap.drawUnderwaterSteps,
                    biomes);
                }
            }
        }
    }
}


public struct MapData
{
    public TileData[,] tileDataMap;
    public DrawMap drawMap;
    public bool drawUnderwaterSteps;
    public Biome[] biomes;
    public Texture2D texture2D;
    // DrawMap drawMap; bool drawUnderwaterSteps; Biome[] biomes;

    public MapData(TileData[,] tileDataMap, DrawMap drawMap, bool drawUnderwaterSteps, Biome[] biomes)
    {
        this.tileDataMap = tileDataMap;
        this.drawMap = drawMap;
        this.drawUnderwaterSteps = drawUnderwaterSteps;
        this.biomes = biomes;
        this.texture2D = TextureGenerator.GetTexture(
                    tileDataMap,
                    drawMap,
                    drawUnderwaterSteps,
                    biomes);
    }
}
*/