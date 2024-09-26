using System.Collections.Generic;
using Godot;

namespace GameDevInc;


public class GeneralSkills
{
    public const int MAX_SKILL_POINTS = 25;                           // Max skill points any category can have
    public int Speed { get; private set; }
    public int Quality { get; private set; }
    public int AttentionToDetail { get; private set; }

    public bool AddSpeedPoints(int points = 1)
    {
        if(Speed + points <= MAX_SKILL_POINTS)
        {
            Speed += points;
            return true;
        }

        return false;
    }

    public bool AddQualityPoints(int points = 1)
    { 
        if(Quality + points <= MAX_SKILL_POINTS)
        {
            Quality += points;
            return true;
        }

        return false;
    }

    public bool AddAttentionToDetailPoints(int points = 1)
    {
        if(AttentionToDetail + points <= MAX_SKILL_POINTS)
        {
            AttentionToDetail += points;
            return true;
        }

        return false;
    }
}