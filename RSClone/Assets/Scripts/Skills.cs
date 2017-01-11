using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skills : MonoBehaviour
{

    static Skills playerSkills;
    public string[] skillNames;
    public Dictionary<string, Skill> skillList;

    // Use this for initialization
    void Awake()
    {
        if (playerSkills != null)
        {
            Debug.LogError("Can't have multiple instances of playerSkills!");
        }
        playerSkills = this;
        for (int i = 0; i < skillNames.Length; i++)
            addSkill(skillNames[i]);
    }

    private void addSkill(string _skill)
    {
        skillList.Add(_skill, new Skill());
    }

    public void gainExp(string _skill, int _exp)
    {
        skillList[_skill].gainExp(_exp);
    }

    public int getLevel(string _skill)
    {
        return skillList[_skill].GetLevel();
    }

    public int getExp(string _skill)
    {
        return skillList[_skill].GetExp();
    }

}

public class Skill
{
    public Skill(int startlevel = 1)
    {
        Level = startlevel;
        SetMod(0);
    }

    private int Level;
    private int Mod;
    private int currExp;
    private int nextExp;

    public void gainExp(int _exp)
    {
        currExp += _exp;
    }

    private void CheckforLevelUp()
    {
        if(currExp <= nextExp)
        {
            Level++;
            nextExp = 100 * Level;
        }
    }

    public int GetLevel()
    {
        return Level;
    }

    public int GetExp()
    {
        return currExp;
    }

    public int GetCurrSkill()
    {
        return Level + Mod;
    }

    public int GetMod()
    {
        return Mod;
    }

    public void SetMod(int _Mod)
    {
        Mod = _Mod;
    }
}