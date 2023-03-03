using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboCalculator
{
    public int currentMultiplier = 1;

    public int ScoreCalulator(int scoreToCalculate)
    {
        int score = scoreToCalculate * currentMultiplier;
        currentMultiplier += 1;
        return score;
    }

    public void ResetMultiplier()
    {
        currentMultiplier = 1;
    }
}
