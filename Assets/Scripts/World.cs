using UnityEngine;
using System.Collections.Generic;
using System;

public class World : MonoBehaviour
{
    [SerializeField]
    private GameObject hexPrefab;
    [SerializeField]
    private float hexSpacing = 0.02f;
    private List<GameObject> hexes = new List<GameObject>();

    public void Start()
    {
        GameManager.onRebuildWorld += InstantiateHexes;
    }

    private void InstantiateHexes()
    {
        Debug.Log("[WORLD] Creating world");

        var hexWidth = 0.866f + hexSpacing;
        var hexHeight = 1.00f + hexSpacing;
        var interlockHeight = 0.75f;
        var xStart = -(GameManager.gameState.worldWidth / 2) * hexWidth;
        var yStart = -(GameManager.gameState.worldHeight / 2) * interlockHeight * hexHeight;

        Func<int, int, Vector3> getPosition = (int x, int y) => {
            var offset = y % 2 == 0 ? 0 : 0.5f;
            return new Vector3(hexWidth * (x + offset) + xStart, 0, hexHeight * y * interlockHeight + yStart);
        };

        foreach (var obj in GameManager.gameState.hexByPosition)
        {
            var key = obj.Key;
            var value = obj.Value;

            var position = getPosition(key.x, key.y);
            var hex = Instantiate(hexPrefab, position, Quaternion.identity);
            hex.transform.SetParent(transform);
            hex.name = $"Hex({key.x}, {key.y})";
        }
    }
}