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
    public PlayerData playerData;

    public ObstacleController barrel;
    public ObstacleController virus;

    public CollectableManager collectableManager;

    private GameController gameController;
    private QuestionManager questionManager;

    public Canvas panelCanvas;
    public QuestionPanelController questionPanel;
    public FeedbackPanelController feedbackPanel;

    public Button pauseButton;
    public PauseScreenController pauseScreen;

    public GameObject continueCanvas;
    public Button continueButton;
    public Button exitButton;

    private float elapsedTime = 0f;
    private float timeNextObstacle = 2f;
    private float minTimeBetweenObstacles = 1.4f;
    private float maxTimeBetweenObstacles = 2.5f;

    private float obstaclesBeforeCollectableCount = 0;
    private float minObstaclesBeforeCollectable = 2;
    private float maxObstaclesBeforeCollectable = 5;

    private int errorCount = 0;
    private int collisionCount = 0;

    void Start()
    {
        gameController = GameController.Instance;
        playerData = gameController.GetComponent<PlayerData>();
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
                playerData.OnLevelFinish(collisionCount, errorCount, (int)elapsedTime*10, 0);
                SceneController.Instance.ChangeScene("GameOverScene");
            }
        });

        player.SetCollideCollectableAction((CollectableController collectableController) =>
        {
            GameQuestion gameQuestion = collectableController.GetGameQuestion();

            QuestionPanelController questionPanelController = Instantiate(questionPanel, panelCanvas.transform);
            float startTime = Time.unscaledTime;

            questionPanelController.SetQuestion(gameQuestion);
            // Cuando se responde una pregunta (Boton continuar) -> Guardar los datos

            OutQuestion oq = OutQuestionT.FromGameQuestion(gameQuestion);

            questionPanelController.SetContinueAction((AssertionController[] assertionControllers) =>
            {
                FeedbackPanelController feedbackPanelController = Instantiate(feedbackPanel, panelCanvas.transform);

                bool allCorrect = true;
                for (int i = 0; i < assertionControllers.Length; i++)
                {
                    AssertionForm assertionForm = assertionControllers[i].GetAssertion();
                    bool playerAnswer = assertionControllers[i].GetPlayerAnswer();
                    oq.assertions[i].correct = playerAnswer == assertionForm.answer;
                    string feedbackText = gameQuestion.question.assertions[i].feedbackText;
                    string feedbackImage = gameQuestion.question.assertions[i].feedbackImage;

                    if (playerAnswer != assertionForm.answer) {
                        allCorrect = false;
                        if (playerData != null && playerData.data != null && playerData.data.assertionErrors < 10)
                        {
                            playerData.data.assertionErrors++;
                            errorCount++;
                        }
                    }

                    feedbackPanelController.AddAssertion(assertionForm, playerAnswer, feedbackText, feedbackImage);
                }
                oq.answerTime = Time.unscaledTime - startTime;
                playerData.outQuestions.Add(oq);


                if (allCorrect)
                {
                    collectableManager.AddCollectable(gameQuestion);
                    questionManager.MarkQuestionAsSolved(gameQuestion);
                }
                if (allCorrect && collectableManager.AllCollectablesObtained())
                {
                    PausePhysics();
                    continueCanvas.gameObject.SetActive(true);
                }

                feedbackPanelController.SetContinueAction(() =>
                {

                    if (!collectableManager.AllCollectablesObtained())
                        StartPhysics();
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
            playerData.OnLevelFinish(collisionCount, errorCount, (int)elapsedTime*10, 1);
            Time.timeScale = 1;
            SceneController.Instance.ChangeScene("MainScene");
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
            else // Generar obst�culo
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