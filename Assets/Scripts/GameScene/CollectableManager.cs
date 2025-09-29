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
    public CollectableController collectablePrefab;
    public GameObject collectableBar;
    public Image collectableIconPrefab;

    private CollectableSlot[] slots;

    private Dictionary<Question, CollectableSlot> questionSlots = new Dictionary<Question, CollectableSlot>();

    private string[] collectableNames = { "bata", "gafas", "microscopio", "matraz", "lupa" };

    public void Init(List<QuestionT> questions)
    {
        slots = new CollectableSlot[questions.Count];

        // Asumiendo que questions.Count <= collectableNames 
        for (int i = 0; i < questions.Count; i++)
        {
            string spriteFilledName = "Coleccionables/" + collectableNames[i];
            string spriteEmptyName = "Coleccionables/" + $"{collectableNames[i]}_contorno";

            slots[i] = new CollectableSlot();
            slots[i].spriteFilled = Resources.Load<Sprite>(spriteFilledName);
            slots[i].spriteEmpty = Resources.Load<Sprite>(spriteEmptyName);

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
