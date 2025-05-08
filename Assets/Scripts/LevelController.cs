using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    public PlayerController player;
    public LivesController livesController;
    public CollectableBarController collectableBar;
    public TextMeshProUGUI distanceText;

    public ObstacleController barrel;
    public ObstacleController virus;


    public CollectableController collectable;

    private GameController gameController;
    private List<QuestionNew> questions;

    public Canvas panelCanvas;
    public QuestionPanelController questionPanel;
    public FeedbackPanelController feedbackPanel;

    public Button pauseButton;
    public PauseScreenController pauseScreen;

    private float elapsedTime = 0f;
    private float timeNextObstacle = 2f;
    private float minTimeBetweenObstacles = 1.4f;
    private float maxTimeBetweenObstacles = 2.5f;

    private float obstaclesBeforeCollectableCount = 0;
    private float minObstaclesBeforeCollectable = 2;
    private float maxObstaclesBeforeCollectable = 5;

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
            QuestionNew question = GetQuestion();

            QuestionPanelController questionPanelController = Instantiate(questionPanel, panelCanvas.transform);
            float startTime = Time.unscaledTime;
            
            questionPanelController.SetQuestion(question, 0);
            questionPanelController.SetContinueAction((AssertionController[] assertionControllers) =>
            {
                FeedbackPanelController feedbackPanelController = Instantiate(feedbackPanel, panelCanvas.transform);
                feedbackPanelController.SetContinueAction(() =>
                {
                    StartPhysics();
                });

                bool allCorrect = true;
                for (int i = 0; i < assertionControllers.Length; i++)
                {
                    AssertionForm assertionForm = assertionControllers[i].GetAssertion();
                    bool playerAnswer = assertionControllers[i].GetPlayerAnswer();
                    string feedbackText = question.assertions[i].feedbackText;
                    string feedbackImage = question.assertions[i].feedbackImage;

                    if (playerAnswer != assertionForm.answer) allCorrect = false;

                    feedbackPanelController.AddAssertion(assertionForm, playerAnswer, feedbackText, feedbackImage);
                }

                if (allCorrect)
                {
                    collectableBar.AddCollectable();
                }
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

    private void SaveAnswer(Question question, string playerAnswer, bool isCorrect, double time)
    {
        ProgressData progressData = gameController.GetProgressData();
        AnsweredQuestion answeredQuestion = new AnsweredQuestion();
        answeredQuestion.playerAnswer = playerAnswer;
        answeredQuestion.question = question;
        answeredQuestion.level = gameController.GetCurrentLevel();
        answeredQuestion.isCorrect = isCorrect;
        answeredQuestion.responseTime = time;

        // Agregar pregunta respondida al arreglo progressData.answeredQuestions
        AnsweredQuestion[] newAnsweredQuestions = new AnsweredQuestion[progressData.answeredQuestions.Length + 1];
        progressData.answeredQuestions.CopyTo(newAnsweredQuestions, 0);
        newAnsweredQuestions[newAnsweredQuestions.Length - 1] = answeredQuestion;
        progressData.answeredQuestions = newAnsweredQuestions;

        gameController.SaveProgressData();
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
                GenerateObstacle(); 
                obstaclesBeforeCollectableCount++;
            }

            timeNextObstacle = elapsedTime + Random.Range(minTimeBetweenObstacles, maxTimeBetweenObstacles);
        }

        int distance = (int) (elapsedTime * 10);
        distanceText.SetText(distance + " m.");
    }

    private void GenerateObstacle()
    {
        float rand = Random.Range(0f, 1f);

        if (rand <= 0.5)
        {
            Instantiate(barrel);
        }
        else
        {
            Instantiate(virus);
        }
    }

    private QuestionNew GetQuestion()
    {
        if (questions == null || questions.Count == 0)
        {
            questions = gameController.GetQuestions();
        }

        int randomIndex = Random.Range(0, questions.Count);
        QuestionNew question = questions[randomIndex];
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