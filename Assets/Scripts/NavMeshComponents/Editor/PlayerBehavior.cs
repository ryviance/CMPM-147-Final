using UnityEngine;

public class Player
{
    // Player stats
    public float Strength { get; private set; }
    public float Intelligence { get; private set; }
    public float Agility { get; private set; }
    public float Charisma { get; private set; }
    public float Endurance { get; private set; }

    public Player(int seed)
    {
        GenerateStats(seed);
    }

    private void GenerateStats(int seed)
    {
        // Use seed to create offset for noise sampling
        float baseOffset = seed * 0.1f;
        float latentFactor = Mathf.PerlinNoise(baseOffset, baseOffset + 100);

        Strength = Mathf.Clamp01(latentFactor + (Mathf.PerlinNoise(baseOffset + 1, baseOffset + 200) - 0.5f) * 0.3f);
        Intelligence = Mathf.Clamp01(1 - latentFactor + (Mathf.PerlinNoise(baseOffset + 2, baseOffset + 300) - 0.5f) * 0.3f);
        Agility = Mathf.Clamp01(latentFactor * 0.8f + (Mathf.PerlinNoise(baseOffset + 3, baseOffset + 400) - 0.5f) * 0.3f);
        Charisma = Mathf.Clamp01(1 - latentFactor * 0.5f + (Mathf.PerlinNoise(baseOffset + 4, baseOffset + 500) - 0.5f) * 0.3f);
        Endurance = Mathf.Clamp01(latentFactor * 0.6f + (Mathf.PerlinNoise(baseOffset + 5, baseOffset + 600) - 0.5f) * 0.3f);

        Strength *= 100;
        Intelligence *= 100;
        Agility *= 100;
        Charisma *= 100;
        Endurance *= 100;
    }

    public void ApplyEffect(string statName, float delta)
    {
        switch (statName.ToLower())
        {
            case "strength":
                Strength = Mathf.Clamp(Strength + delta, 0, 100);
                break;
            case "intelligence":
                Intelligence = Mathf.Clamp(Intelligence + delta, 0, 100);
                break;
            case "agility":
                Agility = Mathf.Clamp(Agility + delta, 0, 100);
                break;
            case "charisma":
                Charisma = Mathf.Clamp(Charisma + delta, 0, 100);
                break;
            case "endurance":
                Endurance = Mathf.Clamp(Endurance + delta, 0, 100);
                break;
            default:
                Debug.LogWarning($"Stat {statName} not found.");
                break;
        }
    }
}
