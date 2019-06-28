using UnityEngine;
using System.Collections.Generic;

public class World : MonoBehaviour
{
    [SerializeField]
    private GameObject hexPrefab;
    private List<GameObject> hexes = new List<GameObject>();

    public void Start()
    {
        GameManager.onRebuildWorld += InstantiateHexes;
    }

    private void InstantiateHexes()
    {
        Debug.Log("[WORLD] Creating world");
        foreach (var obj in GameManager.gameState.hexByPosition)
        {
            var key = obj.Key;
            var value = obj.Value;
           
            var position = new Vector3(key.x, 0, key.y);
            var hex = Instantiate(hexPrefab, position, Quaternion.identity);
            hex.transform.SetParent(transform);
            hex.name = $"Hex({key.x}, {key.y})";
        }
    }
}