using System.Collections.Generic;
using Godot;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GameDevInc;

public enum EStaffSex
{
    SEX_Male,
    SEX_Female,
}

public enum ENameType
{
    NAME_First,
    NAME_Last,  
}

public abstract class StaffMember
{
    public const int c_MaxStatLevel = 25;
    public string Name { get; protected set; }                    // Name of the staff member
    public EModuleJobType JobType { get; protected set; }                 // Reference to the staff members job type
    public EStaffSex Sex { get; protected set; }

    public StaffMemberController Controller { get; protected set; }
    

    // == Leveling Properties == //
    // XP Leveling
    public float XP { get; protected set; }
    protected float m_NextLevelXP = 125;
    protected const float c_NextLevelXPModifier = 1.4f;
    // Level properties
    public int Level { get; protected set; }
    public int SkillPoints { get; protected set; }
    public float XP_Modifier = 1f;

    // == Stats == //
    public GeneralSkills GeneralStats { get; protected set; }

    // == Current Task Data == //
    private List<BaseModule> m_CurrentWorkList = new List<BaseModule>();                    // List of current task queued
    private Timer m_WorkTimer;

    // Assigned Desk

    // Boss/Team
    // Burnout
    // Hours

    // == Model Properties == //
    protected StaffMemberModelColors _clothingColors;
    public int ModelIndex { get; protected set; }
    protected PackedScene _modelScene;

    public StaffMember(EModuleJobType jobType)
    {
        // Generate Random Name
        RandomNumberGenerator rand = new RandomNumberGenerator();
        rand.Randomize();
        Name = StaffDatabase.Instance.GetNewName(rand.Randi() > 0 ? EStaffSex.SEX_Male : EStaffSex.SEX_Female);

        JobType = jobType;              // Set the job type
        GenerateLevelDetails();                 // Generate the staff members level
        GenerateStats();                    // Call the generate stats method on the inherited classes

        m_WorkTimer = new Timer(3f, WorkOnModules, true, true);
    }

    public StaffMember(string name, EStaffSex sex, EModuleJobType jobType = EModuleJobType.JOB_All)
    {
        Name = name;
        Sex = sex;
        JobType = jobType;
    }

    /// <summary>
    /// Adds XP to the staff member and than determines if they level up
    /// </summary>
    /// <param name="xp">Experience Points</param>
    public void AddXP(float xp)
    {
        var lastXP = XP;
        XP += xp * XP_Modifier;
        while(XP < m_NextLevelXP)
        {
            Level += 1;
            SkillPoints += 1;
            m_NextLevelXP = lastXP * c_NextLevelXPModifier;
        }
    }

    /// <summary>
    /// Handles generating of the stats
    /// </summary>
    protected virtual void GenerateStats()
    {
        GeneralStats = new GeneralSkills(JobType);
    }

    /// <summary>
    /// Determines the initial stats for the staff member when created
    /// </summary>
    protected virtual void GenerateLevelDetails()
    {
        RandomNumberGenerator rand = new RandomNumberGenerator();
        rand.Randomize();
        AddXP(rand.RandfRange(1, 1000));
    }

    /// <summary>
    /// Called each frame
    /// </summary>
    /// <param name="deltaTime"></param>
    public virtual void _Update(float deltaTime)
    {
        if (m_WorkTimer != null)
            m_WorkTimer._Update(deltaTime);
    }

    /// <summary>
    /// Handles working on a modules as the staff member
    /// It's called on the completion of the work timer
    /// This functions is called every frame even if there is no module assigned
    /// </summary>
    public void WorkOnModules()
    {
        // Check if there are task in the work list
        // If not don't continue with this module
        if (m_CurrentWorkList.Count <= 0)
            return;

        // Work on the module and remove it if it's complete
        if (m_CurrentWorkList[0].WorkOnModule(this))
            m_CurrentWorkList.RemoveAt(0);

        // Generate the length of time to work
        RandomNumberGenerator rand = new RandomNumberGenerator();
        rand.Randomize();
        if (m_WorkTimer != null)
            m_WorkTimer.TimerLength = rand.RandfRange(1, 7);

        // TODO: Determine work time based on the module requirements itself
    }

    // Add and remove modules from the work list
    public void AddModule(BaseModule module)
        => m_CurrentWorkList.Add(module);
    public void RemoveModule(BaseModule module)
        => m_CurrentWorkList.Remove(module);

    

    /// <summary>
    /// Sets the colors of the model and the index of the model to select
    /// </summary>
    /// <param name="colors">Colors of the staff member</param>
    /// <param name="modelIndex">Index of the model</param>
    public void SetCharacterModel(StaffMemberModelColors colors, int modelIndex)
    {
        // Set valies
        _clothingColors = colors;
        ModelIndex = modelIndex;
        
        // Get reference to the module
        if(Sex == EStaffSex.SEX_Male)
        {
            _modelScene = StaffDatabase.Instance.MaleModels[ModelIndex];
        } else if(Sex == EStaffSex.SEX_Female)
        {
            _modelScene = StaffDatabase.Instance.FemaleModels[ModelIndex];
        }

        // If we failed to set reference let the editor know
        if(_modelScene == null)
        {
            GD.PushWarning("StaffMember::SetCharacterModel -> Failed to select a model");
        }
    }

    /// <summary>
    /// Returns a character model
    /// </summary>
    /// <param name="sex">Sex of the character model</param>
    /// <param name="index">Index point in the list</param>
    /// <returns></returns>
    public static PackedScene GetCharacterModel(EStaffSex sex, int index)
        => sex == EStaffSex.SEX_Male ? StaffDatabase.Instance.MaleModels[index] : StaffDatabase.Instance.FemaleModels[index];

    /// <summary>
    /// Handles creating staff member controller
    /// </summary>
    public void CreateController()
    {
        if(_modelScene != null)
        {
            Controller = (StaffMemberController)GameController.Instance.SpawnToWorld(_modelScene, null);
            if (Controller != null)
                Controller.Setup(this, _clothingColors);
        }
    }
}

public class GeneralStaffStats
{
    public float WorkSpeed { get; private set; }
    public float WorkQuality { get; private set; }

    public bool IncreaseWorkSpeed(int amount = 1)
    {
        if (WorkSpeed + amount <= StaffMember.c_MaxStatLevel)
        {
            WorkSpeed += amount;
            return true;
        }

        return false;
    }

    public bool IncreaseWorkQuality(int amount = 1)
    {
        if(WorkQuality + 1 <= StaffMember.c_MaxStatLevel)
        {
            WorkQuality += amount;
            return true;
        }

        return false;
    }
}

public class StaffMemberModelColors
{
    public Color SkinColor;
    public Color HairOne;
    public Color HairTwo;
    public Color ShirtOne;
    public Color ShirtTwo;
    public Color ShirtThree;
    public Color Pants;

    public void SetColorProperties(MeshInstance3D characterMesh, EStaffSex sex, int modelIndex)
    {
        if(characterMesh != null)
        {
            if(sex == EStaffSex.SEX_Male)
            {
                if(characterMesh.GetActiveMaterial(0) is OrmMaterial3D skin)
                {
                    SkinColor = skin.AlbedoColor;
                }

                if(modelIndex == 0 || modelIndex == 1)
                {
                    if(characterMesh.GetActiveMaterial(2) is OrmMaterial3D hair) HairOne = hair.AlbedoColor;
                    if (characterMesh.GetActiveMaterial(3) is OrmMaterial3D shirt) ShirtOne = shirt.AlbedoColor;
                    if (characterMesh.GetActiveMaterial(4) is OrmMaterial3D pants) Pants = pants.AlbedoColor;
                } else if(modelIndex == 2)
                {
                    if (characterMesh.GetActiveMaterial(2) is OrmMaterial3D hairOne) HairOne = hairOne.AlbedoColor;
                    if(characterMesh.GetActiveMaterial(3) is OrmMaterial3D hairTwo) HairTwo = hairTwo.AlbedoColor;
                    if (characterMesh.GetActiveMaterial(4) is OrmMaterial3D shirtOne) ShirtOne = shirtOne.AlbedoColor;
                    if (characterMesh.GetActiveMaterial(5) is OrmMaterial3D shirtTwo) ShirtTwo = shirtTwo.AlbedoColor;
                    if (characterMesh.GetActiveMaterial(6) is OrmMaterial3D pantsOne) Pants = pantsOne.AlbedoColor;
                } else if(modelIndex == 3)
                {
                    if (characterMesh.GetActiveMaterial(1) is OrmMaterial3D shirtOne) ShirtOne = shirtOne.AlbedoColor;
                    if (characterMesh.GetActiveMaterial(4) is OrmMaterial3D shirtTwo) ShirtTwo = shirtTwo.AlbedoColor;
                    if (characterMesh.GetActiveMaterial(6) is OrmMaterial3D shirtThree) ShirtThree = shirtThree.AlbedoColor;
                    if (characterMesh.GetActiveMaterial(5) is OrmMaterial3D pants) Pants = pants.AlbedoColor;
                    if (characterMesh.GetActiveMaterial(3) is OrmMaterial3D hair) HairOne = hair.AlbedoColor;
                }
            }
        }
    }

}