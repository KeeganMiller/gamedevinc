﻿using System.Collections.Generic;
using Godot;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GameDevInc;

public partial class CompanyDatabase : Node3D
{
    public static CompanyDatabase Instance { get; private set; }
    public Company PlayersCompany { get; private set; }
    private List<Company> _companies = new List<Company>();

    private List<CompanyDataJson> _companyNames = new List<CompanyDataJson>();

    
    public StaffMember PlayersStaffMember;
    

    public override void _Ready()
    {
        base._Ready();
        Instance = this;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        // Update the players company
        if(PlayersCompany != null)
            PlayersCompany._Update((float)delta);
            foreach (var company in _companies)
                company._Update((float)delta);

        // TODO: Update all other companies

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
            var company = _companyNames[rand.RandiRange(0, _companyNames.Count - 1)];
            bool hasCompany = false;
            foreach(var c in _companies)
            {
                if (c.CompanyName == company.CompanyName)
                {
                    hasCompany = true;
                    break;
                }
            }
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

        foreach(var company in _companies)
        {
            if(company.CompanyName == companyName)
                return company;
        }

        return null;
    }

    public void SetPlayersCompany(Company company, bool createSave = false)
    {
        PlayersCompany = company;
        if(createSave)
        {
            // TODO: Create Save File
        }
    }
}

public class CompanyDataJson
{
    [JsonProperty]
    public string CompanyName;
    [JsonProperty]
    public string CompanyIconPath;
}