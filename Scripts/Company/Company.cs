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

    public void AddOwnedStudio(Company studio)
    {
        m_OwnedStudios.Add(studio);
        studio.ParentCompany = this;
    }

    public void RemoveOwnedStudio(Company studio)
    {
        if(m_OwnedStudios.Contains(studio))
        {
            studio.ParentCompany = null;
            m_OwnedStudios.Remove(studio);
        }
    }
    
}