using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private RowUI rowui;

    private void OnEnable()
    {
        var scores = ScoreManager.Instance.GetHighScores().ToArray();
        for (int i = 0; i < scores.Length; i++)
        {
            var row = Instantiate(rowui, transform).GetComponent<RowUI>();
            row.rank.text = (i + 1).ToString();
            row.name.text = scores[i].name;
            row.score.text = scores[i].score.ToString();
        }
    }
}
