using UnityEditor;
using UnityEditor.SceneManagement;

public static class SceneMenu
{
    [MenuItem("Scenes/Ship")]
    public static void OpenShip()
    {
        OpenScene("ShipScene");
    }

    [MenuItem("Scenes/Planet")]
    public static void OpenPlanet()
    {
        OpenScene("PlanetScene");
    }

    private static void OpenScene(string sceneName)
    {
        EditorSceneManager.OpenScene("Assets/_Project/Scenes/Persistent.unity", OpenSceneMode.Single);
        EditorSceneManager.OpenScene("Assets/_Project/Scenes/" + sceneName + ".unity", OpenSceneMode.Additive);
    }
}
