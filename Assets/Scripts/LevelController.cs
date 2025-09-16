using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    public PlayerController player;
    public LivesController livesController;
    public TextMeshProUGUI distanceText;
    private PlayerData playerData;

    public CollectableManager collectableManager;
    public ObstacleManager obstacleManager;

    private GameController gameController;
    private QuestionManager questionManager;

    public Canvas panelCanvas;
    public QuestionPanelController questionPanelController;
    public FeedbackMainController feedbackMainController;

    public Button pauseButton;
    public PauseScreenController pauseScreen;

    public GameObject continueCanvas;

    // Quieres continuar?
    public Button continueButton;
    public Button exitButton;

    public bool autoComplete = false;

    private float elapsedTime = 0f;
    private float elapsedTimeMoving = 0f;
    private float timeNextObstacle = 2f;
    private float minTimeBetweenObstacles = 1.4f;
    private float maxTimeBetweenObstacles = 2.5f;

    private float obstaclesBeforeCollectableCount = 0;
    private float minObstaclesBeforeCollectable = 2;
    private float maxObstaclesBeforeCollectable = 5;

    private int errorCount = 0;
    private int assertionCount = 0;
    private int collisionCount = 0;

    private int levelCompleted = 0;

    void Start()
    {
        gameController = GameController.Instance;
        playerData = PlayerData.Instance;
        continueCanvas.gameObject.SetActive(false);

        // Reset run data on new level start !
        playerData.RunData = new RunData();
        // Populate run data on start
        playerData.RunData.playerId = playerData.Data.playerId;
        playerData.RunData.levelId = GameController.Instance.GetCurrentLevel();

        continueCanvas.gameObject.SetActive(false);

        List<QuestionT> questions = gameController.GetQuestions();
        questionManager = new QuestionManager(questions);
        collectableManager.Init(questions);

        StartPhysics();

        player.SetCollideObstacleAction((ObstacleController obstacleController) =>
        {
            collisionCount++;
            player.Blink(0.5f);
            livesController.RemoveLife();
            if (livesController.GetLivesLeft() == 0)
            {
                playerData.OnLevelFinish(collisionCount, assertionCount, errorCount, (int) (elapsedTime * 10), levelCompleted, collectableManager.GetObtainedCount());
                SceneController.Instance.ChangeScene("GameOverScene");
            }
        });

        player.SetCollideCollectableAction((CollectableController collectableController) =>
        {
            GameQuestion gameQuestion = collectableController.GetGameQuestion();
            float startTime = Time.unscaledTime;

            questionPanelController.SetQuestion(gameQuestion);
            questionPanelController.gameObject.SetActive(true);
            // Cuando se responde una pregunta (Boton continuar) -> Guardar los datos

            OutQuestion oq = OutQuestionT.FromGameQuestion(gameQuestion);

            questionPanelController.SetContinueAction((AssertionController[] assertionControllers) =>
            {
                bool allCorrect = true;
                FeedbackAssertion[] feedbackAssertions = new FeedbackAssertion[4];

                for (int i = 0; i < assertionControllers.Length; i++)
                {
                    AssertionForm assertionForm = assertionControllers[i].GetAssertion();
                    bool playerAnswer = assertionControllers[i].GetPlayerAnswer();
                    oq.assertions[i].correct = playerAnswer == assertionForm.answer;

                    if (playerAnswer != assertionForm.answer) {
                        allCorrect = false;
                        if (playerData != null && playerData.Data != null)
                        {
                        if (playerData.Data.assertionErrors < 10)
                            {
                                playerData.Data.assertionErrors++; // Logro
                            }
                            errorCount++; // Total errores
                        }
                    }

                    assertionCount++;
                    feedbackAssertions[i] = new FeedbackAssertion
                    {
                        feedbackText = gameQuestion.assertions[i].assertion.feedbackText,
                        feedbackImage = gameQuestion.assertions[i].assertion.feedbackImage,
                        assertionForm = assertionControllers[i].GetAssertion(),
                        playerAnswer = assertionControllers[i].GetPlayerAnswer()
                    };
                }

                feedbackMainController.SetAssertions(feedbackAssertions);
                feedbackMainController.SetQuestionText(gameQuestion.question.question);
                feedbackMainController.SetQuestionImage(gameQuestion.question.image);
                feedbackMainController.gameObject.SetActive(true);

                oq.answerTime = Time.unscaledTime - startTime;
                PlayerData.Instance.Data.answeredQuestions[gameController.GetCurrentLevel()].Add(oq);
                PlayerData.Instance.RunData.questions.Add(oq);


                if (allCorrect)
                {
                    collectableManager.AddCollectable(gameQuestion);
                    questionManager.MarkQuestionAsSolved(gameQuestion);
                }
                // if (allCorrect && collectableManager.AllCollectablesObtained())
                if (autoComplete || collectableManager.AllCollectablesObtained())
                {
                    PausePhysics();
                    continueCanvas.gameObject.SetActive(true);
                    levelCompleted = 1;
                }

                feedbackMainController.SetContinueAction(() =>
                {
                    if (!collectableManager.AllCollectablesObtained())
                        StartPhysics();
                    if (autoComplete)
                    {
                        PausePhysics();
                    }
                });
            });

            PausePhysics();
        });

        pauseButton.onClick.AddListener(() =>
        {
            pauseScreen.Show();
            PausePhysics();
        });

        pauseScreen.SetContinueAction(() =>
        {
            StartPhysics();
            pauseScreen.Hide();
        });

        continueButton.onClick.AddListener(() =>
        {
            StartPhysics();
            continueCanvas.gameObject.SetActive(false);
        });

        exitButton.onClick.AddListener(() =>
        {
            playerData.OnLevelFinish(collisionCount, assertionCount, errorCount, (int)(elapsedTime * 10), levelCompleted, collectableManager.GetObtainedCount());
            StartPhysics(); // Importante setear TimeScale = 1 antes de cambiar de escena
            SceneController.Instance.ChangeScene("GameOverScene");
        });
    }

    /*
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
    */

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (Time.timeScale > 0)
        {
            elapsedTimeMoving += Time.unscaledDeltaTime;
            if (elapsedTimeMoving > 30)
            {
                Time.timeScale = Mathf.Min(1 + (elapsedTimeMoving - 30) / 270, 2);
            }
        }

        if (elapsedTime > timeNextObstacle)
        {
            float prob = Mathf.Max(0, obstaclesBeforeCollectableCount - minObstaclesBeforeCollectable) / (maxObstaclesBeforeCollectable - minObstaclesBeforeCollectable);
            float rand = Random.Range(0f, 1f);

            // Generar coleccionable
            if (rand <= prob)
            {
                GameQuestion gameQuestion = questionManager.GetQuestion();
                collectableManager.GenerateCollectable(gameQuestion);
                obstaclesBeforeCollectableCount = 0;
            }
            else // Generar obstáculo
            {
                obstacleManager.GenerateObstacle(elapsedTimeMoving); 
                obstaclesBeforeCollectableCount++;
            }

            float adjustedMaxTimeBetweenObstacles = maxTimeBetweenObstacles;
            if (elapsedTimeMoving > 30)
            {
                adjustedMaxTimeBetweenObstacles = minTimeBetweenObstacles + (maxTimeBetweenObstacles - minTimeBetweenObstacles) * Mathf.Max(1 - 0.8f * (elapsedTimeMoving - 30) / 270, 0.2f);
            }
            timeNextObstacle = elapsedTime + Random.Range(minTimeBetweenObstacles, adjustedMaxTimeBetweenObstacles) * Time.timeScale;
        }

        int distance = (int) (elapsedTime * 10);
        distanceText.SetText(distance + " m.");
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