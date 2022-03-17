using System;
using UnityEngine;

public class RandomUtil
{
    static RandomUtil instance = null;
    System.Random rand;

    public static RandomUtil Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new RandomUtil();
            }
            return instance;
        }
    }

    RandomUtil()
    {
		rand = new System.Random();
	}

	public void Seed(int seed)
    {
        rand = new System.Random(seed);
    }

    public int Range(int minInclusive, int maxExclusive)
    {
        return rand.Next(minInclusive, maxExclusive);
    }

    public float Range(float minInclusive, float maxInclusive)
    {
        double range = (double)maxInclusive - (double)minInclusive;
        double sample = rand.NextDouble();
        double scaled = (sample * range) + (double)minInclusive;
        float f = (float)scaled;
        return f;
    }

    public float Value()
    {
        return (float)rand.NextDouble();
    }
}
