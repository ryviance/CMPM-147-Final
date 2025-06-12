using UnityEditor.UI;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Player stats
    [SerializeField] public float Strength;
    [SerializeField] public float Intelligence;
    [SerializeField] public float Agility;
    [SerializeField] public float Charisma;
    [SerializeField] public float Endurance;

    public Player(int seed)
    {
        GenerateStats(420);
    }

    private void Start()
    {
        GenerateStats(Random.Range(0, 1001));
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Zone")
        {
            string[] myname = {this.gameObject.name};
            NewsManager.Instance.clickHeadline(myname, collision.gameObject.GetComponent<AoeZone>().Type);
        }
    }
}
