using System.Threading;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public ObstacleController obstacle;

    private float elapsedTime = 0f;
    private float timeNextObstacle = 3f;
    private float minTimeBetweenObstacles = 3f;
    private float maxTimeBetweenObstacles = 5f;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime > timeNextObstacle)
        {
            Instantiate(obstacle);
            timeNextObstacle = elapsedTime + Random.Range(minTimeBetweenObstacles, maxTimeBetweenObstacles);
            print(timeNextObstacle - elapsedTime);
        }
    }
}

