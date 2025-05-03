using UnityEngine;
using System.Collections.Generic;

public class SpawnCircles : MonoBehaviour
{
    public GameObject circlePrefab; 
    public int minCircles = 5;
    public int maxCircles = 10;
    public List<GameObject> AllCircles = new List<GameObject>();

    void Start()
    {
        SpawnAllCircles();
    }

    void ClearAllCircles()
    {
        if(AllCircles.Count > 0)
        {
            foreach(var objs in AllCircles)
            {
                DestroyImmediate(objs.gameObject);
            }
            
        }
        AllCircles.Clear();
    }

    public void SpawnAllCircles()
    {
        ClearAllCircles();
        int count = Random.Range(minCircles, maxCircles + 1);

    
        float radius = circlePrefab.GetComponent<SpriteRenderer>().bounds.extents.x;

        for (int i = 0; i < count; i++)
        {
            float minX = radius;
            float maxX = Screen.width - radius;
            float minY = radius;
            float maxY = Screen.height - radius;

            Vector2 screenPosition = new Vector2(
                Random.Range(minX, maxX),
                Random.Range(minY, maxY)
            );

            Vector2 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
           

            GameObject newCircle = Instantiate(circlePrefab, worldPosition, Quaternion.identity);
            AllCircles.Add(newCircle);
        }
    }

}
