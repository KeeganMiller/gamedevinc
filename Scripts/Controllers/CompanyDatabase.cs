﻿using System.Collections.Generic;
using Godot;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GameDevInc;

public partial class CompanyDatabase : Node3D
{
    private const string c_CompanyResourcePath = "res://Data/CompanyData.json";
    public Company PlayersCompany { get; private set; }
    private List<Company> m_Companies = new List<Company>();

    private static List<CompanyDataJson> s_CompanyNames = new List<CompanyDataJson>();

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

    public static void LoadCompanyDetails()
    {
        if(FileAccess.FileExists(c_CompanyResourcePath))
        {
            var file = FileAccess.Open(c_CompanyResourcePath, FileAccess.ModeFlags.Read);
            if(file != null && file.IsOpen())
            {
                var text = file.GetAsText();
                var tokens = JArray.Parse(text);

                foreach(var token in tokens)
                {
                    var companyData = JsonConvert.DeserializeObject<CompanyDataJson>(token.ToString());
                    if(companyData != null)
                    {
                        s_CompanyNames.Add(companyData);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Generates a new company
    /// </summary>
    /// <returns></returns>
    public Company GetNewCompany()
    {
        RandomNumberGenerator rand = new RandomNumberGenerator();
        for(var i = 0; i < 100; ++i)
        {
            rand.Randomize();
            var company = s_CompanyNames[rand.RandiRange(0, s_CompanyNames.Count - 1)];
            bool hasCompany = false;
            foreach(var c in m_Companies)
            {
                if (c.CompanyName == company.CompanyName)
                {
                    hasCompany = true;
                    break;
                }
            }

            // Create the new company
            if (!hasCompany)
                return new Company(company.CompanyName);
        }

        // Failed to create company
        return null;
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

public class CompanyDataJson
{
    [JsonProperty]
    public string CompanyName;
    [JsonProperty]
    public string CompanyIconPath;
}