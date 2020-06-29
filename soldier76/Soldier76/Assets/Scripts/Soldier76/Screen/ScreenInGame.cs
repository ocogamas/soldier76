using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenInGame : MonoBehaviour
{
    private readonly uint TILE_X = 10;
    private readonly uint TILE_Y = 16;

    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private GameObject tilesGameObject;

    void Start()
    {
        for (int x = 0; x < TILE_X; x++)
        {
            for (int y = 0; y < TILE_Y; y++)
            {
                GameObject obj = Instantiate(this.tilePrefab);

                obj.transform.parent = this.tilesGameObject.transform;

                obj.transform.localPosition = new Vector3(x*0.5f, y*0.5f, 0);
            }
        }
    }

    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown (KeyCode.Escape))
        {
            SceneManager.LoadScene("Title");
        }
#endif
    }
}
