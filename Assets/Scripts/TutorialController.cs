using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class TutorialEvent
{
    public string message { get; set; }

    public TutorialEvent(string message)
    {
        this.message = message;
    }

    public virtual void Start() { }

    public abstract bool Run();
}

public class WelcomeEvent : TutorialEvent
{
    private static new readonly string message = "�Bienvenido a Bio Lab Escape! Este eres t�, un cient�fico atrapado en un laboratorio que debe recuperar los objetos de su equipamiento";
    private float elapsedTime = 0f;
    private float duration = 6f;

    public WelcomeEvent() : base(message) { }

    public override bool Run()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime > duration) return true;

        return false;
    }
}

public class ObstacleEvent : TutorialEvent
{
    private static new readonly string message = "Tu misi�n es recuperar estos objetos, �pero cuidado! Deber�s saltar los obst�culos que aparecen en tu camino";
    private PlayerController player;
    private ObstacleController barrel;
    private ObstacleController virus;

    private float elapsedTime = 0f;
    private float timeNextObstacle = 2f;
    private float minTimeBetweenObstacles = 1.4f;
    private float maxTimeBetweenObstacles = 2.5f;

    private int collisionCount = 0;
    private int obstacleCount = 0;

    public ObstacleEvent(PlayerController player, ObstacleController barrel, ObstacleController virus) : base(message)
    {
        this.player = player;
        this.barrel = barrel;
        this.virus = virus;
    }

    public override void Start()
    {
        player.SetCollideObstacleAction((ObstacleController obstacleController) =>
        {
            player.Blink(0.5f);
            collisionCount++;
        });
    }

    public override bool Run()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime > timeNextObstacle)
        {
            if (obstacleCount <= collisionCount + 3)
            {
                GenerateObstacle();
                obstacleCount++;
                timeNextObstacle = elapsedTime + Random.Range(minTimeBetweenObstacles, maxTimeBetweenObstacles);
            }
            else if (elapsedTime > timeNextObstacle + 1) return true;
        }

        return false;
    }

    private void GenerateObstacle()
    {
        float rand = Random.Range(0f, 1f);

        if (rand <= 0.5)
        {
            GameObject.Instantiate(barrel);
        }
        else
        {
            GameObject.Instantiate(virus);
        }
    }
}

public class CollectableEvent : TutorialEvent
{
    private static new readonly string message = "�Bien hecho! Ahora recoge alguno de tus objetos, pero prep�rate, porque deber�s responder una pregunta para qued�rtelo";
    private static readonly string messageQuestion = "Marca las afirmaciones correctas (�pueden haber varias!). Solo si aciertas todas, podr�s quedarte con el objeto";
    private static readonly string messageFeedbackCorrect = "�Felicitaciones, conseguiste el objeto! En caso de error en tus respuestas, aqu� podr�s revisar las explicaciones de cada una";
    private static readonly string messageFeedbackWrong = "�Casi! Fallaste esta vez, pero no te preocupes, aqu� podr�s ver tus errores y revisar las explicaciones de cada respuesta";

    private CollectableManager collectableManager;
    private float elapsedTime = 0f;
    private float timeNextCollectable = 2f;
    private float timeBetweenCollectables = 3f;

    private List<GameQuestion> tutorialQuestions = new List<GameQuestion>();
    private int currentCollectable = 0;

    private bool finished = false;

    public CollectableEvent(PlayerController player, CollectableManager collectableManager,
        QuestionPanelController questionPanel, FeedbackPanelController feedbackPanel,
        GameObject panelContainer) : base(message)
    {
        this.collectableManager = collectableManager;

        Question question = new Question();
        question.question = "�Qu� afirmaciones son correctas en relaci�n a las partes de la c�lula?";

        question.assertions = new Assertion[4];

        string[] statements = {
            "El n�cleo celular contiene el material gen�tico de la c�lula.",
            "Las mitocondrias se encargan de producir prote�nas.",
            "El ret�culo endoplasm�tico rugoso tiene ribosomas en su superficie.",
            "Los cloroplastos est�n presentes en todas las c�lulas animales."
        };

        bool[] answers = { true, false, true, false };

        string[] feedback =
        {
            "El n�cleo es la parte de la c�lula que guarda el ADN y contiene la informaci�n gen�tica necesaria para controlar las funciones y actividades de la c�lula.",
            "Las mitocondrias son las \"centrales energ�ticas\" de la c�lula; su funci�n principal es producir energ�a en forma de ATP. La s�ntesis de prote�nas la realizan los ribosomas.",
            "El ret�culo endoplasm�tico rugoso se llama as� porque su superficie est� cubierta de ribosomas, que son los encargados de fabricar prote�nas.",
            "Los cloroplastos son organelos que realizan la fotos�ntesis y solo se encuentran en las c�lulas vegetales, no en las c�lulas animales."
        };

        for (int i = 0; i < 4; i++)
        {
            question.assertions[i] = new Assertion();
            question.assertions[i].forms = new AssertionForm[1];
            question.assertions[i].forms[0] = new AssertionForm();
            question.assertions[i].forms[0].statement = statements[i];
            question.assertions[i].forms[0].answer = answers[i];
            question.assertions[i].feedbackText = feedback[i];
        }

        List<QuestionT> questionTList = new List<QuestionT>();
        for (int i = 0; i < 3; i++)
        {
            questionTList.Add(new QuestionT(0, i, question));
        }
        collectableManager.Init(questionTList);

        List<GameAssertion> gameAssertions = new List<GameAssertion>();
        foreach (Assertion assertion in question.assertions)
        {
            AssertionT assertionT = new AssertionT(0, assertion);
            AssertionFormT assertionFormT = new AssertionFormT(0, assertionT.forms[0]);
            GameAssertion gameAssertion = new GameAssertion(assertionT, assertionFormT);
            gameAssertions.Add(gameAssertion);
        }

        for (int i = 0; i < 3; i++)
        {
            tutorialQuestions.Add(new GameQuestion(questionTList[i], gameAssertions));
        }


        player.SetCollideCollectableAction((CollectableController collectableController) =>
        {
            GameQuestion gameQuestion = collectableController.GetGameQuestion();
            QuestionPanelController questionPanelController = GameObject.Instantiate(questionPanel, panelContainer.transform);
            questionPanelController.SetQuestion(gameQuestion);

            base.message = messageQuestion;

            // Pausar
            Time.timeScale = 0;
            player.enabled = false;

            questionPanelController.SetContinueAction((AssertionController[] assertionControllers) =>
            {
                FeedbackPanelController feedbackPanelController = GameObject.Instantiate(feedbackPanel, panelContainer.transform);

                bool allCorrect = true;
                for (int i = 0; i < assertionControllers.Length; i++)
                {
                    AssertionForm assertionForm = assertionControllers[i].GetAssertion();
                    bool playerAnswer = assertionControllers[i].GetPlayerAnswer();
                    string feedbackText = gameQuestion.question.assertions[i].feedbackText;
                    string feedbackImage = gameQuestion.question.assertions[i].feedbackImage;

                    if (playerAnswer != assertionForm.answer) allCorrect = false;

                    feedbackPanelController.AddAssertion(assertionForm, playerAnswer, feedbackText, feedbackImage);
                }

                if (allCorrect)
                {
                    collectableManager.AddCollectable(gameQuestion);
                    base.message = messageFeedbackCorrect;
                }
                else
                {
                    base.message = messageFeedbackWrong;
                }

                feedbackPanelController.SetContinueAction(() =>
                {
                    Time.timeScale = 1;
                    player.enabled = true;
                    finished = true;
                });
            });
        });
    }

    public override bool Run()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime > timeNextCollectable)
        {
            collectableManager.GenerateCollectable(tutorialQuestions[currentCollectable]);
            timeNextCollectable = elapsedTime + timeBetweenCollectables;
            currentCollectable = (currentCollectable + 1) % 3;
        }

        return finished;
    }
}

public class FinishEvent : TutorialEvent
{
    private static new readonly string message = "Para completar el nivel, re�ne los 3 objetos mostrados en la barra. �Mucha suerte!";
    private float elapsedTime = 0f;
    private float duration = 5f;

    public FinishEvent() : base(message) { }

    public override bool Run()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime > duration) return true;

        return false;
    }
}


public class TutorialController : MonoBehaviour
{
    private List<TutorialEvent> events = new List<TutorialEvent>();
    public TextMeshProUGUI messageBox;
    private int currentEvent = 0;

    public PlayerController player;
    public ObstacleController barrel;
    public ObstacleController virus;

    public CollectableManager collectableManager;
    public QuestionPanelController questionPanel;
    public FeedbackPanelController feedbackPanel;
    public GameObject panelContainer;


    void Start()
    {
        events.Add(new WelcomeEvent());
        events.Add(new ObstacleEvent(player, barrel, virus));
        events.Add(new CollectableEvent(player, collectableManager, questionPanel, feedbackPanel, panelContainer));
        events.Add(new FinishEvent());

        events[currentEvent].Start();
        messageBox.text = events[currentEvent].message;
    }

    void Update()
    {
        bool completed = events[currentEvent].Run();
        messageBox.text = events[currentEvent].message;

        if (completed)
        {
            currentEvent++;
            if (currentEvent >= events.Count)
            {
                SceneController.Instance.PreviousScene();
            }
            else
            {
                events[currentEvent].Start();
            }
        }
    }
}
