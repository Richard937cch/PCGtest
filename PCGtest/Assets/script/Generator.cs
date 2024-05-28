using System;
using UnityEngine;
using Unity;
using System.Collections;
using Random=System.Random;
using Unity.Mathematics;

/*
 * Unity Noise-based Terrain Generator
 * 
 * Based on original code by Brackeys (https://www.youtube.com/watch?v=vFvwyu_ZKfU)
 */
public class Generator : MonoBehaviour
{
    public Terrain terrain;

    public MapType mapType = MapType.Flat;

    [Range(1, 10000)]
    public int randomSeed = 10; // Seed for RNG

    [Header("Terrain Size")]
    public int width = 256;
    public int depth = 256;
    [Range(0, 100)]
    public int height = 20;

    public enum MapType
    {
        Flat, Slope, Random, Simplex, Perlin, PerlinOctave
    };

    [Header("Perlin Noise")]
    [Range(0f, 100f)]
    public float frequency = 20f;
    [Range(0f, 10000f)]
    public float offsetX = 100f;
    [Range(0f, 10000f)]
    public float offsetY = 100f;

    public bool animateOffset = false;

    [Header("Octaves")]
    [Range(1, 8)]
    public int octaves = 3;
    [Range(0, 4)]
    public float amplitudeModifier;
    [Range(0, 4)]
    public float frequencyModifier;

    private Random rng;

    public void Start()
    {
        // Get a reference to the terrain component
        terrain = GetComponent<Terrain>();
        

    }

    // Update is called every frame
    public void Update()
    {
        // Generate the terrain according to current parameters
        terrain.terrainData = GenerateTerrain(terrain.terrainData);

        // Move along the X axis
        if (animateOffset)
            offsetX += Time.deltaTime * 5f;
    }

    // Update the terrain height values
    public TerrainData GenerateTerrain(TerrainData data)
    {
        // Set size and resolution for the terrain data
        data.heightmapResolution = width + 1;
        data.size = new Vector3(width, height, depth);

        float[,] heightMap;


        // Generate a height map
        switch (mapType)
        {
            case (MapType.PerlinOctave):
                heightMap = PerlinOctaveMap();
                break;
            
            case (MapType.Perlin):
                heightMap = PerlinMap();
                break;

            case (MapType.Simplex):
                heightMap = SimplexMap();
                break;
            
            case (MapType.Random):
                heightMap = RandomMap();
                break;

            case (MapType.Slope):
                heightMap = SlopingMap();
                break;

            default:
                heightMap = FlatMap();
                break;
        }

        // Set the terrain data to the new height map
        data.SetHeights(0, 0, heightMap);

        return data;
    }

    // Generate a flat height map (all zero)
    public float[,] FlatMap()
    {
        float[,] heights = new float[width, depth];

        return heights;
    }

    // Generate a sloping height map - you need to fix this!
    public float[,] SlopingMap()
    {
        float[,] heights = new float[width, depth];

        // Iterate over map positions
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < depth; y++)
            {
                // Set height at this position
                
                //float e = (float)x / (float)width;
                heights[x, y] = (float)((float)x/(float)width + (float)y/(float)depth)/2;
            }
        }

        return heights;
    }

    //Generate randon height
    public float[,] RandomMap()
    {
        float[,] heights = new float[width, depth];
        rng = new Random(randomSeed);
        // Iterate over map positions
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < depth; y++)
            {
                // Set height at this position
                double randomDouble = rng.NextDouble();
                float randomFloat = (float)randomDouble;
                heights[x, y] = randomFloat;
            }
        }

        return heights;
    }

    //simplex
    public float[,] SimplexMap()
    {
        float[,] heights = new float[width, depth];
        rng = new Random(randomSeed);
        // Iterate over map positions
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < depth; y++)
            {
                // Set height at this position
                float sampleX = frequency * (float)x / width;
                sampleX += offsetX;
                float sampleY = frequency * (float)y / depth;
                sampleY += offsetX;
                
                //heights[x, y] = noise.snoise(sampleX, sampleY);
            }
        }

        return heights;
    }

    //PERLIN
    public float[,] PerlinMap()
    {
        float[,] heights = new float[width, depth];
        rng = new Random(randomSeed);
        // Iterate over map positions
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < depth; y++)
            {
                // Set height at this position
                float sampleX = frequency * (float)x / width;
                sampleX += offsetX;
                float sampleY = frequency * (float)y / depth;
                sampleY += offsetX;
                
                heights[x, y] = Mathf.PerlinNoise(sampleX, sampleY);
            }
        }

        return heights;
    }

    //perlinoctave
    public float[,] PerlinOctaveMap()
    {
        float[,] heights = new float[width, depth];
        rng = new Random(randomSeed);
        // Iterate over map positions
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < depth; y++)
            {
                // Set height at this position
                float sampleX = frequency * (float)x / width;
                sampleX += offsetX;
                float sampleY = frequency * (float)y / depth;
                sampleY += offsetX;
                float a=1.0f;
                float f=1.0f;
                
                for (int i=1;i<=octaves;i++)
                {
                    heights[x, y] += a*Mathf.PerlinNoise(f*sampleX, f*sampleY);
                    a/=amplitudeModifier;
                    f*=frequencyModifier;
                }
                
            }
        }

        return heights;
    }




}
