using UnityEngine;
using UnityEngine.Events;

public static class GameData
{
    public static Vector3 playerShipPosition = new Vector3();
    public static Vector3 playerPlanetPosition = new Vector3(-50f, 2.2f, -350f);
    public static Vector3 alienShipPosition = new Vector3();
    public static bool alienScanned;
    public static bool fruitCollected = false;
    public static bool wrongFruitCollected = false;
    public static UnityEvent endDraw = new();
    public static bool drawingDone = false;
}