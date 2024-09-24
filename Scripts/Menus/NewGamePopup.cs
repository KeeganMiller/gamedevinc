﻿using System.Collections.Generic;
using System.Diagnostics;
using Godot;

namespace GameDevInc.Menus;

public partial class NewGamePopup : Control
{
    [Export] private Button m_ExitBtn;
    [Export] private TextureButton m_NextFormBtn;
    [Export] private Label m_ErrorMessage;

    [ExportGroup("Logo's")]
    [Export] private NewGameCompanyLogoSelect m_CompanyLogoOne;
    [Export] private NewGameCompanyLogoSelect m_CompanyLogoTwo;
    [Export] private NewGameCompanyLogoSelect m_CompanyLogoThree;
    private NewGameCompanyLogoSelect m_SelectedCompanyLogo;
    

    [ExportGroup("Specialties")]
    [Export] private NewGameSpecialitySelect m_ActionBtn;
    [Export] private NewGameSpecialitySelect m_RPGBtn;
    [Export] private NewGameSpecialitySelect m_SimulationBtn;
    [Export] private NewGameSpecialitySelect m_StrategyBtn;
    [Export] private NewGameSpecialitySelect m_PlatformerBtn;
    [Export] private NewGameSpecialitySelect m_CasualBtn;
    [Export] private NewGameSpecialitySelect m_AdventureBtn;
    [Export] private NewGameSpecialitySelect m_Puzzle;
    private NewGameSpecialitySelect? m_CurrentlySelected = null;
    private ESpeciality m_CurrentlySelectedSpec;


    [ExportGroup("Form Inputs")]
    [Export] private LineEdit m_CompanyNameInput;

    

    public override void _Ready()
    {
        base._Ready();

        if (m_ExitBtn != null)
            m_ExitBtn.Connect("pressed", new Callable(this, "ClosePopup"));

        // Link company logo to function
        if (m_CompanyLogoOne != null)
            m_CompanyLogoOne.Connect("pressed", new Callable(this, "SelectCompanyOne"));
        if (m_CompanyLogoTwo != null)
            m_CompanyLogoTwo.Connect("pressed", new Callable(this, "SelectCompanyTwo"));
        if (m_CompanyLogoThree != null)
            m_CompanyLogoThree.Connect("pressed", new Callable(this, "SelectCompanyThree"));

        // Link Specialty buttons to function 
        if (m_ActionBtn != null && m_ActionBtn.ButtonRef != null)
            m_ActionBtn.ButtonRef.Connect("pressed", new Callable(this, "SelectActionSpec"));
        if (m_RPGBtn != null && m_RPGBtn.ButtonRef != null)
            m_RPGBtn.ButtonRef.Connect("pressed", new Callable(this, "SelectRPGSpec"));
        if (m_SimulationBtn != null && m_SimulationBtn.ButtonRef != null)
            m_SimulationBtn.ButtonRef.Connect("pressed", new Callable(this, "SelectSimulationSpec"));
        if (m_StrategyBtn != null && m_StrategyBtn.ButtonRef != null)
            m_StrategyBtn.ButtonRef.Connect("pressed", new Callable(this, "SelectStrategySpec"));
        if (m_PlatformerBtn != null && m_PlatformerBtn.ButtonRef != null)
            m_PlatformerBtn.ButtonRef.Connect("pressed", new Callable(this, "SelectPlatformerSpec"));
        if (m_CasualBtn != null && m_CasualBtn.ButtonRef != null)
            m_CasualBtn.ButtonRef.Connect("pressed", new Callable(this, "SelectCasualSpec"));
        if (m_AdventureBtn != null && m_AdventureBtn.ButtonRef != null)
            m_AdventureBtn.ButtonRef.Connect("pressed", new Callable(this, "SelectAdventureSpec"));
        if (m_Puzzle != null && m_Puzzle.ButtonRef != null)
            m_Puzzle.ButtonRef.Connect("pressed", new Callable(this, "SelectPuzzleSpec"));

        // Complete form btn
        if (m_NextFormBtn != null)
            m_NextFormBtn.Connect("pressed", new Callable(this, "CompleteForm"));
    }

    #region Company Selection

    private void SetSelectedCompanyLogo(NewGameCompanyLogoSelect selected)
    {
        GD.Print("Selecting company");
        if (m_SelectedCompanyLogo != null)
            m_SelectedCompanyLogo.ToggleSelectionRect(false);

        m_SelectedCompanyLogo = selected;
        if (m_SelectedCompanyLogo != null)
            m_SelectedCompanyLogo.ToggleSelectionRect(true);
    }

    private void SelectCompanyOne()
        => SetSelectedCompanyLogo(m_CompanyLogoOne);
    private void SelectCompanyTwo()
        => SetSelectedCompanyLogo(m_CompanyLogoTwo);
    private void SelectCompanyThree()
        => SetSelectedCompanyLogo(m_CompanyLogoThree);


    #endregion


    #region Speciality Selection

    private void SetSelectedSpecialty(ESpeciality specialty, NewGameSpecialitySelect selected)
    {
        m_CurrentlySelectedSpec = specialty;

        if (m_CurrentlySelected != null)
            m_CurrentlySelected.ToggleSelectionRect(false);

        m_CurrentlySelected = selected;
        if (m_CurrentlySelected != null)
            m_CurrentlySelected.ToggleSelectionRect(true);
    }

    private void SelectActionSpec()
        => SetSelectedSpecialty(ESpeciality.SPEC_Action, m_ActionBtn);
    private void SelectRPGSpec()
        => SetSelectedSpecialty(ESpeciality.SPEC_RPGBtn, m_RPGBtn);
    private void SelectSimulationSpec()
        => SetSelectedSpecialty(ESpeciality.SPEC_Simulation, m_SimulationBtn);
    private void SelectStrategySpec()
        => SetSelectedSpecialty(ESpeciality.SPEC_Strategy, m_StrategyBtn);
    private void SelectPlatformerSpec()
        => SetSelectedSpecialty(ESpeciality.SPEC_Platformer, m_PlatformerBtn);
    private void SelectCasualSped()
        => SetSelectedSpecialty(ESpeciality.SPEC_Casual, m_CasualBtn);
    private void SelectAdventureSpec()
        => SetSelectedSpecialty(ESpeciality.SPEC_Adventure, m_AdventureBtn);
    private void SelectPuzzleSpec()
        => SetSelectedSpecialty(ESpeciality.SPEC_Puzzle, m_Puzzle);

    #endregion

    public void CompleteForm()
    {
        // Validate the form
        if(m_CompanyNameInput.Text == null)
        {
            m_ErrorMessage.Text = "Please enter a company name";
            return;
        }

        if(m_SelectedCompanyLogo == null)
        {
            m_ErrorMessage.Text = ("Please select a logo for your company");
            return;
        }

        if(m_CurrentlySelected == null)
        {
            m_ErrorMessage.Text = "Please select a specialty for your company";
            return;
        }

        // Form is valid so create and add the company
        var company = new Company(m_CompanyNameInput.Text, m_SelectedCompanyLogo.TextureNormal, m_CurrentlySelectedSpec);
        var companyDb = GetNode<CompanyDatabase>("/root/CompanyDatabase");
        if (companyDb != null)
            companyDb.SetPlayersCompany(company);
    }

    private void ClosePopup()
        => this.QueueFree();
}