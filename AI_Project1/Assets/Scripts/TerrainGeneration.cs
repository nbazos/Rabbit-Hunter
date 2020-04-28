using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TerrainGeneration : MonoBehaviour
{
    private const int textureWidth = 256;
    private const int textureHeight = 256;

    public GameObject terrain;
    public Image heightMapDisplay;

    private Texture2D heightMap;

    // Start is called before the first frame update
    void Awake()
    {
        // Terrain Generation on start-up
        GenerateTerrain();
    }

    private void GenerateTerrain()
    {
        float scale = UnityEngine.Random.Range(450, 550);
        int octaves = UnityEngine.Random.Range(1, 6);
        int period = UnityEngine.Random.Range(1, 6); ;
        float frequency = 1.0f / period;
        float amplitude = UnityEngine.Random.Range(1, 4);
        float lacunarity = UnityEngine.Random.Range(1, 3); 
        float gain = UnityEngine.Random.Range(0.5f, 1f);

        heightMap = GeneratefBmNoiseMap(textureWidth, textureHeight, scale, octaves, gain, lacunarity);

        heightMapDisplay.material.mainTexture = heightMap;

        GenerateTerrainFromTexture(heightMap);
    }

    public Texture2D GeneratefBmNoiseMap(int mapWidth, int mapHeight, float scale, int octaves, float gain, float lacunarity)
    {
        float[,] noiseMap = new float[mapWidth, mapHeight];

        if (scale <= 0)
        {
            scale = 0.0001f;
        }

        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for (int i = 0; i < octaves; i++)
                {
                    float sampleX = x / scale * frequency;
                    float sampleY = y / scale * frequency;

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    noiseHeight += perlinValue * amplitude;

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

        return CreateTextureFromNoiseValues(noiseMap);
    }

    private Texture2D CreateTextureFromNoiseValues(float[,] noiseMap)
    {
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

    private void GenerateTerrainFromTexture(Texture2D heightMap)
    {
        TerrainData terrainData = terrain.GetComponent<Terrain>().terrainData;
        float[,] heights = new float[heightMap.width, heightMap.height];
        for (int x = 0; x < heightMap.width; x++)
        {
            for (int y = 0; y < heightMap.width; y++)
            {
                heights[x, y] = heightMap.GetPixel(x, y).grayscale;
            }
        }

        terrainData.SetHeights(0, 0, heights);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
