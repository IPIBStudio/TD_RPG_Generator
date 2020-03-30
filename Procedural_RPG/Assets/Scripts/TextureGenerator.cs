using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureGenerator
{
    // en faire une pour le terrain et une pour temperature / humidity ou whatever ? 
    // pour pouvoir en afficher plusieurs d'un coup

    public static Texture2D GetTexture(TileData[,] tileDataMap, DrawMap drawMap, bool drawUnderwaterSteps, Biome[] biomes)
    {
        //TileData[,] tileDataMap = mapData.tileDataMap;

        int width = tileDataMap.GetLength(0);
        int height = tileDataMap.GetLength(1);

        Color biomeBorderColor = Color.black; // à mettre avec la suivante en public inspector terrain setting
        Color edgeColor = Color.white;
        Color cityColor = Color.magenta; // en fonction du thème couleur de la ville ?

        var texture = new Texture2D(width, height);
        Color[] pixels = new Color[width * height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // moyen de generaliser le drawing des edges et borders ici
                switch (drawMap)
                {
                    case DrawMap.Height:
                        //Set color range, 0 = black, 1 = white
                        if (tileDataMap[x, y].isEdge)
                        {
                            pixels[x + y * width] = edgeColor;
                        }
                        else
                        {
                            pixels[x + y * width] = Color.Lerp(Color.black, Color.white, tileDataMap[x, y].terrainHeight);
                        }
                        break;
                    case DrawMap.HeightType:
                        if (tileDataMap[x, y].isBiomeBorder)
                        {
                            pixels[x + y * width] = biomeBorderColor;
                        }
                        else if (tileDataMap[x, y].isEdge && drawUnderwaterSteps)
                        {
                            pixels[x + y * width] = edgeColor;
                        }
                        else
                        {
                            pixels[x + y * width] = tileDataMap[x, y].terrainType.color;
                        }
                        break;
                    // sur les trois qui suivent c chiant de pas voir les heights faut refaire des f°
                    case DrawMap.Humidity:
                        if (tileDataMap[x, y].isBiomeBorder)
                        {
                            pixels[x + y * width] = biomeBorderColor;
                        }
                        else if (tileDataMap[x, y].isEdge && drawUnderwaterSteps)
                        {
                            pixels[x + y * width] = edgeColor;
                        }
                        else
                        {
                            pixels[x + y * width] = tileDataMap[x, y].humidityType.color;
                        }
                        break;
                    case DrawMap.Temperature:
                        if (tileDataMap[x, y].isBiomeBorder)
                        {
                            pixels[x + y * width] = biomeBorderColor;
                        }
                        else if (tileDataMap[x, y].isEdge && drawUnderwaterSteps)
                        {
                            pixels[x + y * width] = edgeColor;
                        }
                        else
                        {
                            pixels[x + y * width] = tileDataMap[x, y].temperatureType.color;
                        }
                        break;
                    case DrawMap.Biome:
                        if (tileDataMap[x, y].isBiomeBorder)
                        {
                            pixels[x + y * width] = biomeBorderColor;
                        }
                        else if (tileDataMap[x, y].isEdge && drawUnderwaterSteps)
                        {
                            pixels[x + y * width] = edgeColor;
                        }
                        else
                        {
                            for (int i = 0; i < biomes.Length; i++)
                            {
                                if (biomes[i].biomeType == tileDataMap[x, y].biomeType)
                                {
                                    pixels[x + y * width] = biomes[i].pixelColor;
                                    break;
                                }
                            }
                            if (tileDataMap[x, y].terrainType.terrainTypeName == TerrainTypeName.DeepWater)
                            {
                                pixels[x + y * width] = tileDataMap[x, y].terrainType.color;
                            }
                            else if (tileDataMap[x, y].terrainType.terrainTypeName == TerrainTypeName.ShallowWater)
                            {
                                pixels[x + y * width] = tileDataMap[x, y].terrainType.color;
                            }
                        }
                        break;
                    case DrawMap.City:
                        if (tileDataMap[x, y].isBiomeBorder)
                        {
                            pixels[x + y * width] = biomeBorderColor;
                        }
                        else if (tileDataMap[x, y].isEdge && drawUnderwaterSteps)
                        {
                            pixels[x + y * width] = edgeColor;
                        }
                        else if (tileDataMap[x, y].isCity)
                        {
                            pixels[x + y * width] = cityColor;
                        }
                        else
                        {
                            for (int i = 0; i < biomes.Length; i++)
                            {
                                if (biomes[i].biomeType == tileDataMap[x, y].biomeType)
                                {
                                    pixels[x + y * width] = biomes[i].pixelColor;
                                    break;
                                }
                            }
                            if (tileDataMap[x, y].terrainType.terrainTypeName == TerrainTypeName.DeepWater)
                            {
                                pixels[x + y * width] = tileDataMap[x, y].terrainType.color;
                            }
                            else if (tileDataMap[x, y].terrainType.terrainTypeName == TerrainTypeName.ShallowWater)
                            {
                                pixels[x + y * width] = tileDataMap[x, y].terrainType.color;
                            }
                        }
                        break;
                }
            }
        }

        texture.SetPixels(pixels);
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.Apply();
        return texture;
    }
}
