using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    public ObstacleController barrel;
    public ObstacleController virus_01;
    public ObstacleController virus_02;
    public ObstacleController virus_03;
    public ObstacleController double_barrel;

    public float maxSpeed = 800;

    public void GenerateObstacle(float elapsedTime)
    {
        float rand = Random.Range(0f, 1f);
        ObstacleController obstacle;

        if (elapsedTime < 20)
        {
            if (rand < 0.5)
            {
                obstacle = barrel;
            }
            else
            {
                obstacle = virus_01;
            }
        }
        else if (elapsedTime < 40)
        {
            double prob_virus_02 = 0.15 + 0.1833 * (elapsedTime - 20) / 40;
            double prob_virus_01 = (1 - prob_virus_02) * 0.5;

            if (rand < prob_virus_02)
            {
                obstacle = virus_02;
            }
            else if (rand < prob_virus_02 + prob_virus_01)
            {
                obstacle = virus_01;
            }
            else
            {
                obstacle = barrel;
            }
        }
        else
        {
            double prob_double_barrel = Mathf.Min((float)(0.05 + 0.25 * (elapsedTime - 40) / 260), 0.3f);
            double prob_virus_03 = Mathf.Min((float)(0.05 + 0.3 * (elapsedTime - 40) / 260), 0.35f);
            double prob_virus_02 = (1 - prob_double_barrel - prob_virus_03) * 0.5;
            double prob_virus_01 = (1 - prob_double_barrel - prob_virus_03 - prob_virus_02) * 0.5;

            if (rand < prob_double_barrel)
            {
                obstacle = double_barrel;
            }
            else if (rand < prob_double_barrel + prob_virus_03)
            {
                obstacle = virus_03;
            }
            else if (rand < prob_double_barrel + prob_virus_03 + prob_virus_02)
            {
                obstacle = virus_02;
            }
            else if (rand < prob_double_barrel + prob_virus_03 + prob_virus_02 + prob_virus_01)
            {
                obstacle = virus_01;
            }
            else
            {
                obstacle = barrel;
            }
        }

        ObstacleController genObstacle = Instantiate(obstacle);
    }
}
