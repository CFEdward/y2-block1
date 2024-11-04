using UnityEngine;
using UnityEngine.SceneManagement;

public class SwapMenus : MonoBehaviour
{
    [SerializeField] private GameObject objectsMenu;
    [SerializeField] private GameObject dictionaryMenu;

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            objectsMenu.SetActive(true);
            dictionaryMenu.SetActive(false);
        }
        else
        {
            objectsMenu.SetActive(false);
            dictionaryMenu.SetActive(true);
        }
    }
}
