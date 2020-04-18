using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGeneration : MonoBehaviour
{
    public int textureWidth = 256;
    public int textureHeight = 256;
    public float scale = 20f;

    public GameObject terrain;

    float total = 0.0f;
    public int octaves = 1;
    public int period = 2;
    [HideInInspector] public float frequency;
    public float amplitude = 2;
    public float lacunarity = 2.0f;
    public float gain = 0.65f;

    Mesh mesh;
    Vector3[] vertices;

    // Start is called before the first frame update
    void Start()
    {
        frequency = 1.0f / period;

        mesh = terrain.GetComponent<MeshFilter>().mesh;
        vertices = mesh.vertices;

        Renderer renderer = terrain.GetComponent<Renderer>();
        renderer.material.mainTexture = GeneratefBm(); // change this

        //GenerateTerrain();
    }

    private Texture2D GeneratefBm()
    {
        Texture2D texture = new Texture2D(textureWidth, textureHeight);

        for (int pixelX = 0; pixelX < textureWidth; pixelX++)
        {
            for (int pixelY = 0; pixelY < textureHeight; pixelY++)
            {
                /*
                 * fBm
                for (int i = 0; i < octaves; i++)
                {
                    total += CalculateNoise((float)pixelX * frequency, (float)pixelY * frequency) * amplitude;

                    frequency *= lacunarity;
                    amplitude *= gain;
                }

                Color pixelColor = new Color(total, total, total);

                texture.SetPixel(pixelX, pixelY, pixelColor);
                */

                Color color = CalculateNoise(pixelX, pixelY);
                texture.SetPixel(pixelX, pixelY, color);
            }
        }

        texture.Apply();
        return texture;
    }

    private Color CalculateNoise(int x, int y)
    {
        float perlinX = (float)x / textureWidth * scale;
        float perlinY = (float)y / textureHeight * scale;

        float noiseResult = Mathf.PerlinNoise(perlinX, perlinY);

        return new Color (noiseResult, noiseResult, noiseResult);

        // return noiseResult;
    }

    private void GenerateTerrain()
    {
        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 vertex = vertices[i];

            Vector3 normalized = (new Vector2(vertex.x, vertex.z)) / 10.0f + Vector2.one * 0.5f;
            // vertex.y = 
        }

        mesh.vertices = vertices;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
