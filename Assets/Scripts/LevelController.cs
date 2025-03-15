using TMPro;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public PlayerController player;
    public ObstacleController obstacle;
    public CollectableController collectable;
    public LivesController livesController;
    public TextMeshProUGUI distanceText;

    private float elapsedTime = 0f;
    private float timeNextObstacle = 3f;
    private float minTimeBetweenObstacles = 3f;
    private float maxTimeBetweenObstacles = 5f;

    private float obstaclesBeforeCollectableCount = 0;
    private float minObstaclesBeforeCollectable = 5;
    private float maxObstaclesBeforeCollectable = 10;

    void Start()
    {
        player.SetCollideObstacleAction(() =>
        {
            livesController.RemoveLife();
        });

        player.SetCollideCollectableAction(() =>
        {
            print("Coleccionable");
        });
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime > timeNextObstacle)
        {
            float prob = Mathf.Max(0, obstaclesBeforeCollectableCount - minObstaclesBeforeCollectable) / (maxObstaclesBeforeCollectable - minObstaclesBeforeCollectable);
            float rand = Random.Range(0f, 1f);

            // Generar coleccionable
            if (rand <= prob)
            {
                Instantiate(collectable);
                obstaclesBeforeCollectableCount = 0;
            }
            else // Generar obstáculo
            {
                Instantiate(obstacle);
                obstaclesBeforeCollectableCount++;
            }

            timeNextObstacle = elapsedTime + Random.Range(minTimeBetweenObstacles, maxTimeBetweenObstacles);
        }

        int distance = (int) (elapsedTime * 10);
        distanceText.SetText(distance + " m.");
    }
}

