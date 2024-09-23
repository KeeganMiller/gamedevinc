using System.Collections.Generic;
using Godot;

namespace GameDevInc;

public partial class CompanyDatabase : Node3D
{
    public Company PlayersCompany { get; private set; }
    private List<Company> m_Companies = new List<Company>();

    // == Debug Properties == //
    private StaffMember staff;
    private BaseModule module;

    public override void _Ready()
    {
        base._Ready();

        staff = new Programmer();

        // Create the players company
        PlayersCompany = new Company("YeahNah Studios");
        PlayersCompany.StaffMembers.Add(staff);
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        // Update the companies
        PlayersCompany._Update((float)delta);
        foreach(var company in m_Companies)
            company._Update((float)delta);

    }

    /// <summary>
    /// Finds the company with a specific name (Players company included)
    /// </summary>
    /// <param name="companyName">Name of the company</param>
    /// <returns>Company reference</returns>
    public Company GetCompany(string companyName)
    {
        if (PlayersCompany.CompanyName == companyName)
            return PlayersCompany;

        foreach(var company in m_Companies)
        {
            if(company.CompanyName == companyName)
                return company;
        }

        return null;
    }
}