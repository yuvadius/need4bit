using UnityEngine;
using System.Collections;

[System.Serializable]
public class LevelData {
    [Tooltip("How much apples you got to eat to level up")]
    public int appReq;
    //[Tooltip("How many banannas you got to eat to level up")]
    //public int banReq;
    [Tooltip("How many points you get rewarded for this level")]
    public int pointsRwd;

    [Tooltip("How many apples maximum room for")]
    public int applesPresent;
    [Tooltip("What is the rate of apple production")]
    public float appleRate;
}

public class LevelingSystem : MonoBehaviour {

    public LevelData[] levels;
    public bool CanLevelUp;
    public bool CanHaveApples;
    [Header("For inspection only")]
    public int currentLevel = 0;
    public int applesAte = 0, bannanasAte = 0;
    public int availablePoints = 0;

    LevelingView view;
    AppleController appleSystem;

    void Start() {
        view = FindObjectOfType<LevelingView>();
        appleSystem = FindObjectOfType<AppleController>();
        appleSystem.SetCanHaveApples(CanHaveApples);
    }

    public void AddApple() {
        if (CanHaveApples)
        {
            applesAte++;
            checkNExecuteLvlUpCondition();
        }
    }

    public void AddBannana() {
        bannanasAte++;
        checkNExecuteLvlUpCondition();
    }

    public void ConsumePoint() {
        availablePoints--;
    }

    void checkNExecuteLvlUpCondition() {
        if (currentLevel < levels.Length - 1 
            && applesAte >= levels[currentLevel].appReq 
            //&& bannanasAte >= levels[currentLevel].banReq
        ) {
            lvlUp();             
        }
    }

    void lvlUp() {
        if (CanLevelUp)
        {
            availablePoints += levels[currentLevel].pointsRwd;
            appleSystem.maxNumOfApples = levels[currentLevel].applesPresent;
            appleSystem.ratePerSecond = levels[currentLevel].appleRate;
            currentLevel++;
            view.SetUpView();
        }
    }

}


