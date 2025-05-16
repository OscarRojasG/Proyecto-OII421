using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class DataLoader
{
    static private QuestionData questionData = null;
    static private QuestionData questionData2 = new QuestionData();

    static public bool Load()
    {
        if (questionData == null)
        {
            TextAsset asset = Resources.Load<TextAsset>("questions");
            questionData = JsonConvert.DeserializeObject<QuestionData>(asset.text);
        }

        foreach (var (key, value) in questionData.data) {
            int l_id = int.Parse(key);
            Question[] qs = value;
            int q_id = 0;
            foreach (Question q in qs)
            {
                // QuestionT qt = new QuestionT(q.question, q.concept, q.image, q.assertions, l_id, q_id);
                q_id++;
            }
        }


        return true;
    }

    public List<Question> GetQuestions(int level)
    {
        return new List<Question>(questionData.data[level.ToString()]);
    }
    
}
