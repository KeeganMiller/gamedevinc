using System.Collections.Generic;
using Godot;

namespace GameDevInc;

public class Company
{
    public string CompanyName { get; protected set; }
    public Image CompanyLogo { get; protected set; }

    public Company ParentCompany;

    public int CompanyFans = 0;
    public int CompanyCurrency = 200000;

    public List<StaffMember> StaffMembers = new List<StaffMember>();

    private List<Company> m_OwnedStudios = new List<Company>();                     // List of children studios
    // TODO: Add List of games

    /// <summary>
    /// Adds a new game studio to this as owned
    /// </summary>
    /// <param name="studio">Studio to set as owned</param>
    public void AddOwnedStudio(Company studio)
    {
        m_OwnedStudios.Add(studio);
        studio.ParentCompany = this;
    }

    /// <summary>
    /// Removes a studio from this company
    /// </summary>
    /// <param name="studio">Studio to remove</param>
    public void RemoveOwnedStudio(Company studio)
    {
        if(m_OwnedStudios.Contains(studio))
        {
            studio.ParentCompany = null;
            m_OwnedStudios.Remove(studio);
        }
    }

    /// <summary>
    /// Called each frame
    /// </summary>
    /// <param name="deltaTime"></param>
    public virtual void _Update(float deltaTime)
    {
        foreach(var staff in StaffMembers)
        {
            staff._Update(deltaTime);
        }
    }
    
}