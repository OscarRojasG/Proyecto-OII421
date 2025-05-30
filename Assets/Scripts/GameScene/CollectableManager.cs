using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectableSlot
{
    public Image icon;
    public Sprite spriteFilled;
    public Sprite spriteEmpty;
    public bool filled = false;
}

public class CollectableManager : MonoBehaviour
{
    private static readonly int MAX_COLLECTABLES = 3;

    public CollectableController collectablePrefab;
    public GameObject collectableBar;
    public Image collectableIconPrefab;

    public Sprite[] spritesFilled = new Sprite[MAX_COLLECTABLES];
    public Sprite[] spritesEmpty = new Sprite[MAX_COLLECTABLES];
    private CollectableSlot[] slots = new CollectableSlot[MAX_COLLECTABLES];

    private Dictionary<Question, CollectableSlot> questionSlots = new Dictionary<Question, CollectableSlot>(); 

    public void Init(List<QuestionT> questions)
    {
        for (int i = 0; i < MAX_COLLECTABLES; i++)
        {
            slots[i] = new CollectableSlot();
            slots[i].spriteFilled = spritesFilled[i];
            slots[i].spriteEmpty = spritesEmpty[i];

            slots[i].icon = Instantiate(collectableIconPrefab, collectableBar.transform);
            slots[i].icon.sprite = slots[i].spriteEmpty;
        }

        for (int i = 0; i < questions.Count; i++)
        {
            questionSlots[questions[i]] = slots[i % slots.Length];
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(collectableBar.transform as RectTransform);
    }

    public CollectableController GenerateCollectable(GameQuestion gameQuestion)
    {
        CollectableController collectable = Instantiate(collectablePrefab);
        collectable.SetSprite(questionSlots[gameQuestion.question].spriteFilled);
        collectable.SetGameQuestion(gameQuestion);
        return collectable;
    }

    public void AddCollectable(GameQuestion gameQuestion)
    {
        CollectableSlot slot = questionSlots[gameQuestion.question];
        slot.icon.sprite = slot.spriteFilled;
        slot.filled = true;
    }

    public bool AllCollectablesObtained()
    {
        return GetObtainedCount() == questionSlots.Count;
    }

    public int GetObtainedCount()
    {
        int count = 0;
        foreach (var (key, value) in questionSlots)
        {
            if (value.filled == true) count++;
        }
        return count;
    }
}
