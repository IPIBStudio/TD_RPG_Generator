using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NoiseType { Perlin }

public class Noises
{

    public static float[,] Perlin(int mapWidth, int mapHeight, int seed, float scale, int octaves,
        float persistance, float lacunarity, Vector2 offset)
    {
        float[,] noiseMap = new float[mapWidth, mapHeight];

        System.Random prng = new System.Random(seed);
        Vector2[] octaveOffsets = new Vector2[octaves];
        for (int i = 0; i < octaves; i++)
        {
            float offsetX = prng.Next(-100000, 100000) + offset.x;
            float offsetY = prng.Next(-100000, 100000) - offset.y;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }

        if (scale <= 0)
        {
            scale = 0.0001f;
        }

        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        float halfWidth = mapWidth / 2f;
        float halfHeight = mapHeight / 2f;


        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {

                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for (int i = 0; i < octaves; i++)
                {
                    float sampleX = (x - halfWidth + octaveOffsets[i].x) / scale * frequency;
                    float sampleY = (y - halfHeight + octaveOffsets[i].y) / scale * frequency;

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    noiseHeight += perlinValue * amplitude;

                    amplitude *= persistance;
                    frequency *= lacunarity;
                }

                if (noiseHeight > maxNoiseHeight)
                {
                    maxNoiseHeight = noiseHeight;
                }
                else if (noiseHeight < minNoiseHeight)
                {
                    minNoiseHeight = noiseHeight;
                }
                noiseMap[x, y] = noiseHeight;
            }
        }

        return InverseLerpMap(minNoiseHeight, maxNoiseHeight, noiseMap);
    }

    // Others noise functions to implement
    /*
     * https://github.com/ricardojmendez/LibNoise.Unity
     * https://github.com/TinkerWorX/AccidentalNoiseLibrary
     * https://gametorrahod.com/various-noise-functions/
     */

    // it's just so laggy :(
    public static float[,] LayeringPerlinNoise(int mapWidth, int mapHeight, int seed, float scale, int octaves,
        float persistance, float lacunarity, Vector2 offset, int numberOfLayers)
    {
        float[,] noiseMap = Perlin(mapWidth, mapHeight, seed, scale, octaves, persistance, lacunarity, offset);

        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        for (int x = 0; x < noiseMap.GetLength(0); x++)
        {
            for (int y = 0; y < noiseMap.GetLength(1); y++)
            {
                for (int i = 1; i < numberOfLayers + 1; i++)
                {
                    noiseMap[x, y] += Perlin(mapWidth, mapHeight, seed * i, scale, octaves, persistance, lacunarity, offset)[x, y];
                }

                noiseMap[x, y] /= numberOfLayers;

                if (noiseMap[x, y] > maxNoiseHeight)
                {
                    maxNoiseHeight = noiseMap[x, y];
                }
                else if (noiseMap[x, y] < minNoiseHeight)
                {
                    minNoiseHeight = noiseMap[x, y];
                }
            }
        }

        return noiseMap;
    }

    public static float[,] Layering2Noise(float[,] noiseMap1, float[,] noiseMap2)
    {
        float[,] noiseMap = new float[noiseMap1.GetLength(0), noiseMap1.GetLength(1)];

        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        for (int x = 0; x < noiseMap.GetLength(0); x++)
        {
            for (int y = 0; y < noiseMap.GetLength(1); y++)
            {
                noiseMap[x, y] = (noiseMap1[x, y] + noiseMap2[x, y]) / 2;

                if (noiseMap[x, y] > maxNoiseHeight)
                {
                    maxNoiseHeight = noiseMap[x, y];
                }
                else if (noiseMap[x, y] < minNoiseHeight)
                {
                    minNoiseHeight = noiseMap[x, y];
                }
            }
        }

        return InverseLerpMap(minNoiseHeight, maxNoiseHeight, noiseMap);
    }

    static float[,] InverseLerpMap(float minNoiseHeight, float maxNoiseHeight, float[,] map)
    {
        for (int x = 0; x < map.GetLength(0); x++)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                map[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, map[x, y]);
            }
        }

        return map;
    }
}

