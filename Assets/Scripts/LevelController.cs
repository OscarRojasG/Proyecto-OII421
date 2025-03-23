using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    public PlayerController player;
    public ObstacleController obstacle;
    public CollectableController collectable;
    public LivesController livesController;
    public CollectableBarController collectableBar;
    public TextMeshProUGUI distanceText;

    private GameController gameController;
    private List<Question> questions;

    public Canvas questionPanelCanvas;
    public QuestionPanelController questionPanel;

    public Button pauseButton;
    public PauseScreenController pauseScreen;

    private float elapsedTime = 0f;
    //private float startTime;

    private float timeNextObstacle = 2.5f;
    private float minTimeBetweenObstacles = 2.5f;
    private float maxTimeBetweenObstacles = 4.5f;

    private float obstaclesBeforeCollectableCount = 0;
    private float minObstaclesBeforeCollectable = 4;
    private float maxObstaclesBeforeCollectable = 8;

    void Start()
    {
        gameController = GameController.Instance;
        StartPhysics();

        player.SetCollideObstacleAction(() =>
        {
            livesController.RemoveLife();
            if (livesController.GetLivesLeft() == 0)
            {
                SceneController.Instance.ChangeScene("GameOverScene");
            }
        });

        player.SetCollideCollectableAction(() =>
        {
            Question question = GetQuestion();

            QuestionPanelController questionPanelController = Instantiate(questionPanel, questionPanelCanvas.transform);
            
            questionPanelController.SetQuestion(question);
            questionPanelController.SetCorrectOptionAction(() =>
            {
                collectableBar.AddCollectable();
                ProgressData progressData = gameController.GetProgressData();
                AnsweredQuestion answeredQuestion = new AnsweredQuestion();
                answeredQuestion.playerAnswer = "";
                answeredQuestion.question = question;
                answeredQuestion.level = gameController.GetCurrentLevel();
                answeredQuestion.isCorrect = true;
                answeredQuestion.responseTime = 1;

                AnsweredQuestion[] newAnsweredQuestions = new AnsweredQuestion[progressData.answeredQuestions.Length + 1];

                // Copiar los elementos del array original al nuevo array
                progressData.answeredQuestions.CopyTo(newAnsweredQuestions, 0);

                // Agregar el nuevo elemento
                newAnsweredQuestions[newAnsweredQuestions.Length - 1] = answeredQuestion;

                progressData.answeredQuestions = newAnsweredQuestions;

                gameController.SaveProgressData();
            });

            questionPanelController.SetDismissAction(() =>
            {
                StartPhysics();
            });

            PausePhysics();
        });

        pauseButton.onClick.AddListener(() =>
        {
            PausePhysics();
            pauseScreen.Show();
        });

        pauseScreen.SetContinueAction(() =>
        {
            StartPhysics();
            pauseScreen.Hide();
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
            if (rand <= prob || true)
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

    private Question GetQuestion()
    {
        if (questions == null || questions.Count == 0)
        {
            questions = gameController.GetQuestions();
        }

        int randomIndex = Random.Range(0, questions.Count);
        Question question = questions[randomIndex];
        questions.RemoveAt(randomIndex);

        return question;
    }

    private void PausePhysics()
    {
        Time.timeScale = 0;
        player.enabled = false;
    }
    private void StartPhysics()
    {
        Time.timeScale = 1;
        player.enabled = true;
    }
}