using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TerrainGeneration : MonoBehaviour
{
    // These do not need to change given the terrain size
    private const int textureWidth = 256;
    private const int textureHeight = 256;

    public GameObject terrain;
    public Image heightMapDisplay;

    void Awake()
    {
        // Terrain Generation on start-up before anything else
        GenerateTerrain();
    }

    private void GenerateTerrain()
    {
        // Randomly generate Fractional Brownian Motion Values
        float scale = UnityEngine.Random.Range(450f, 550f);
        int octaves = UnityEngine.Random.Range(1, 6);
        float gain = UnityEngine.Random.Range(0.5f, 1f);
        float lacunarity = UnityEngine.Random.Range(1f, 3f); 

        Texture2D heightMap = GeneratefBmNoiseMap(textureWidth, textureHeight, scale, octaves, gain, lacunarity);

        heightMapDisplay.material.mainTexture = heightMap;

        GenerateTerrainFromTexture(heightMap);
    }

    // Reference: Procedural Terrain Generation series by Sebastian Lague (specifically Episode 2 and 3)
    public Texture2D GeneratefBmNoiseMap(int mapWidth, int mapHeight, float scale, int octaves, float gain, float lacunarity)
    {
        float[,] noiseMap = new float[mapWidth, mapHeight];

        if (scale <= 0)
        {
            scale = 0.0001f;
        }

        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        // Fill noiseMap 2D array
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                // set these to one because we will be using gain and lacunarity to modify them
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                // Use perlin noise value for each octave
                for (int i = 0; i < octaves; i++)
                {
                    float sampleX = x / scale * frequency;
                    float sampleY = y / scale * frequency;

                    float perlinNoiseValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    noiseHeight += perlinNoiseValue * amplitude;

                    amplitude *= gain;
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

        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
            }
        }

        // Create a Texture2D from noise map and return to generate terrain
        return CreateTextureFromNoiseValues(noiseMap);
    }

    private Texture2D CreateTextureFromNoiseValues(float[,] noiseMap)
    {
        // Set grayscale gradient for the texture using the passed in noice map

        int textureWidth = noiseMap.GetLength(0);
        int textureHeight = noiseMap.GetLength(1);

        Texture2D noiseTexture = new Texture2D(textureWidth, textureHeight);

        Color[] color = new Color[textureWidth * textureHeight];

        for (int x = 0; x < textureWidth; x++)
        {
            for (int y = 0; y < textureHeight; y++)
            {
                color[y * textureWidth + x] = Color.Lerp(Color.black, Color.white, noiseMap[x, y]);
            }
        }

        noiseTexture.SetPixels(color);
        noiseTexture.Apply();

        return noiseTexture;
    }

    // Reference: Unity 5 - Create Terrain From Texture (Works at Runtime too) by Creagines
    private void GenerateTerrainFromTexture(Texture2D heightMap)
    {
        // Loop through the texture and store heights based on grayscale values

        TerrainData terrainData = terrain.GetComponent<Terrain>().terrainData;
        float[,] heights = new float[heightMap.width, heightMap.height];
        for (int x = 0; x < heightMap.width; x++)
        {
            for (int y = 0; y < heightMap.height; y++)
            {
                heights[x, y] = heightMap.GetPixel(x, y).grayscale;
            }
        }

        terrainData.SetHeights(0, 0, heights);
    }
}
