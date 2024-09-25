using System.Collections.Generic;
using System.Diagnostics;
using Godot;

namespace GameDevInc.Menus;

public partial class NewGamePopup : Control
{
    [Export] private Button _exitBtn;
    [Export] private TextureButton _nextFormBtn;
    [Export] private Label _errorMessage;
    [Export] private OptionButton _genderDropdown;
    [Export] private TextureButton _EditCharacterBtn;
    [Export] private LineEdit _ceoNameInput;

    [ExportCategory("Forms References")]
    [Export] private Control _companyInformationContainer;
    [Export] private Control _ceoInformationContainer;
    [Export] private Control _characterCreationContainer;


    #region Company Properties
    [ExportGroup("Logo's")]
    [Export] private NewGameCompanyLogoSelect _companyLogoOne;
    [Export] private NewGameCompanyLogoSelect _companyLogoTwo;
    [Export] private NewGameCompanyLogoSelect _companyLogoThree;
    private NewGameCompanyLogoSelect _selectedCompanyLogo;
    

    [ExportGroup("Specialties")]
    [Export] private NewGameSpecialitySelect _actionBtn;
    [Export] private NewGameSpecialitySelect _rpgBtn;
    [Export] private NewGameSpecialitySelect _simulationBtn;
    [Export] private NewGameSpecialitySelect _strategyBtn;
    [Export] private NewGameSpecialitySelect _platformerBtn;
    [Export] private NewGameSpecialitySelect _casualBtn;
    [Export] private NewGameSpecialitySelect _adventureBtn;
    [Export] private NewGameSpecialitySelect _puzzle;
    private NewGameSpecialitySelect? _currentlySelected = null;
    private ESpeciality _currentlySelectedSpec;


    [ExportGroup("Form Inputs")]
    [Export] private LineEdit _companyNameInput;

    #endregion


    #region CEO Setup

    [Export] private GridContainer _maleCharacters;
    [Export] private GridContainer _femaleCharacters;

    [ExportGroup("Selectable Characters")]
    [ExportCategory("Male")]
    [Export] private NewGameSelectCharacter _maleOne;
    [Export] private NewGameSelectCharacter _maleTwo;
    [Export] private NewGameSelectCharacter _maleThree;
    [Export] private NewGameSelectCharacter _maleFour;

    [ExportCategory("Female")]
    [Export] private NewGameSelectCharacter _femaleOne;
    [Export] private NewGameSelectCharacter _femaleTwo;
    [Export] private NewGameSelectCharacter _femaleThree;
    [Export] private NewGameSelectCharacter _femaleFour;

    

    private NewGameSelectCharacter _selectedCharacter;

    private int _currentlySelectedGenderIndex;                      // Index selected for the dropdown

    #endregion



    public override void _Ready()
    {
        base._Ready();

        if (_exitBtn != null)
            _exitBtn.Connect("pressed", new Callable(this, "ClosePopup"));

        // Link company logo to function
        if (_companyLogoOne != null)
            _companyLogoOne.Connect("pressed", new Callable(this, "SelectCompanyOne"));
        if (_companyLogoTwo != null)
            _companyLogoTwo.Connect("pressed", new Callable(this, "SelectCompanyTwo"));
        if (_companyLogoThree != null)
            _companyLogoThree.Connect("pressed", new Callable(this, "SelectCompanyThree"));

        // Link Specialty buttons to function 
        if (_actionBtn != null && _actionBtn.ButtonRef != null)
            _actionBtn.ButtonRef.Connect("pressed", new Callable(this, "SelectActionSpec"));
        if (_rpgBtn != null && _rpgBtn.ButtonRef != null)
            _rpgBtn.ButtonRef.Connect("pressed", new Callable(this, "SelectRPGSpec"));
        if (_simulationBtn != null && _simulationBtn.ButtonRef != null)
            _simulationBtn.ButtonRef.Connect("pressed", new Callable(this, "SelectSimulationSpec"));
        if (_strategyBtn != null && _strategyBtn.ButtonRef != null)
            _strategyBtn.ButtonRef.Connect("pressed", new Callable(this, "SelectStrategySpec"));
        if (_platformerBtn != null && _platformerBtn.ButtonRef != null)
            _platformerBtn.ButtonRef.Connect("pressed", new Callable(this, "SelectPlatformerSpec"));
        if (_casualBtn != null && _casualBtn.ButtonRef != null)
            _casualBtn.ButtonRef.Connect("pressed", new Callable(this, "SelectCasualSpec"));
        if (_adventureBtn != null && _adventureBtn.ButtonRef != null)
            _adventureBtn.ButtonRef.Connect("pressed", new Callable(this, "SelectAdventureSpec"));
        if (_puzzle != null && _puzzle.ButtonRef != null)
            _puzzle.ButtonRef.Connect("pressed", new Callable(this, "SelectPuzzleSpec"));

        // Complete form btn
        if (_nextFormBtn != null)
            _nextFormBtn.Connect("pressed", new Callable(this, "CompleteForm"));


        // Link the character selection buttons
        if (_maleOne != null && _maleOne.ButtonRef != null)
            _maleOne.ButtonRef.Connect("pressed", new Callable(this, "SelectMaleOne"));
        if (_maleTwo != null && _maleTwo.ButtonRef != null)
            _maleTwo.ButtonRef.Connect("pressed", new Callable(this, "SelectMaleTwo"));
        if (_maleThree != null && _maleThree.ButtonRef != null)
            _maleThree.ButtonRef.Connect("pressed", new Callable(this, "SelectMaleThree"));
        if (_maleFour != null && _maleFour.ButtonRef != null)
            _maleFour.ButtonRef.Connect("pressed", new Callable(this, "SelectMaleFour"));

        if (_femaleOne != null && _femaleOne.ButtonRef != null)
        {
            _femaleOne.ButtonRef.Connect("pressed", new Callable(this, "SelectFemaleOne"));
            _femaleOne.HideWithSelfMod();
        }
        if (_femaleTwo != null && _femaleTwo.ButtonRef != null)
        {
            _femaleTwo.ButtonRef.Connect("pressed", new Callable(this, "SelectFemaleTwo"));
            _femaleTwo.HideWithSelfMod();
        }
        if (_femaleThree != null && _femaleThree.ButtonRef != null)
        {
            _femaleThree.ButtonRef.Connect("pressed", new Callable(this, "SelectFemaleThree"));
            _femaleThree.HideWithSelfMod();
        }
        if (_femaleFour != null && _femaleFour.ButtonRef != null)
        {
            _femaleFour.ButtonRef.Connect("pressed", new Callable(this, "SelectFemaleFour"));
            _femaleFour.HideWithSelfMod();
        }

        if (_genderDropdown != null)
            _genderDropdown.Connect("item_selected", new Callable(this, "OnChangeGender"));

        if (_EditCharacterBtn != null)
            _EditCharacterBtn.Connect("pressed", new Callable(this, "CreateCeoDetails"));
    }

    #region Company Selection

    private void SetSelectedCompanyLogo(NewGameCompanyLogoSelect selected)
    {
        GD.Print("Selecting company");
        if (_selectedCompanyLogo != null)
            _selectedCompanyLogo.ToggleSelectionRect(false);

        _selectedCompanyLogo = selected;
        if (_selectedCompanyLogo != null)
            _selectedCompanyLogo.ToggleSelectionRect(true);
    }

    private void SelectCompanyOne()
        => SetSelectedCompanyLogo(_companyLogoOne);
    private void SelectCompanyTwo()
        => SetSelectedCompanyLogo(_companyLogoTwo);
    private void SelectCompanyThree()
        => SetSelectedCompanyLogo(_companyLogoThree);


    #endregion


    #region Speciality Selection

    private void SetSelectedSpecialty(ESpeciality specialty, NewGameSpecialitySelect selected)
    {
        _currentlySelectedSpec = specialty;

        if (_currentlySelected != null)
            _currentlySelected.ToggleSelectionRect(false);

        _currentlySelected = selected;
        if (_currentlySelected != null)
            _currentlySelected.ToggleSelectionRect(true);
    }

    private void SelectActionSpec()
        => SetSelectedSpecialty(ESpeciality.SPEC_Action, _actionBtn);
    private void SelectRPGSpec()
        => SetSelectedSpecialty(ESpeciality.SPEC_RPGBtn, _rpgBtn);
    private void SelectSimulationSpec()
        => SetSelectedSpecialty(ESpeciality.SPEC_Simulation, _simulationBtn);
    private void SelectStrategySpec()
        => SetSelectedSpecialty(ESpeciality.SPEC_Strategy, _strategyBtn);
    private void SelectPlatformerSpec()
        => SetSelectedSpecialty(ESpeciality.SPEC_Platformer, _platformerBtn);
    private void SelectCasualSped()
        => SetSelectedSpecialty(ESpeciality.SPEC_Casual, _casualBtn);
    private void SelectAdventureSpec()
        => SetSelectedSpecialty(ESpeciality.SPEC_Adventure, _adventureBtn);
    private void SelectPuzzleSpec()
        => SetSelectedSpecialty(ESpeciality.SPEC_Puzzle, _puzzle);

    #endregion

    public void CompleteForm()
    {
        // Validate the form
        if(_companyNameInput.Text == null)
        {
            _errorMessage.Text = "Please enter a company name";
            return;
        }

        if(_selectedCompanyLogo == null)
        {
            _errorMessage.Text = ("Please select a logo for your company");
            return;
        }

        if(_currentlySelected == null)
        {
            _errorMessage.Text = "Please select a specialty for your company";
            return;
        }

        // Form is valid so create and add the company
        var company = new Company(_companyNameInput.Text, _selectedCompanyLogo.TextureNormal, _currentlySelectedSpec);
        var companyDb = GetNode<CompanyDatabase>("/root/CompanyDatabase");
        if (companyDb != null)
            companyDb.SetPlayersCompany(company);

        _companyInformationContainer.Visible = false;
        _ceoInformationContainer.Visible = true;
    }

    #region Selectable Characters

    public void SetSelectedCharacter(NewGameSelectCharacter character)
    {
        if (_selectedCharacter != null)
            _selectedCharacter.ToggleRect(false);

        _selectedCharacter = character;
        if (_selectedCharacter != null)
            _selectedCharacter.ToggleRect(true);
    }

    public void SelectMaleOne()
        => SetSelectedCharacter(_maleOne);
    public void SelectMaleTwo()
        => SetSelectedCharacter(_maleTwo); 
    public void SelectMaleThree()
        => SetSelectedCharacter(_maleThree);
    public void SelectMaleFour()
        => SetSelectedCharacter(_maleFour);

    public void SelectFemaleOne()
        => SetSelectedCharacter(_femaleOne);
    public void SelectFemaleTwo()
        => SetSelectedCharacter(_femaleTwo);
    public void SelectFemaleThree()
        => SetSelectedCharacter (_femaleThree);
    public void SelectFemaleFour()
        => SetSelectedCharacter(_femaleFour);

    #endregion

    public void OnChangeGender(int index)
    {
        if (_currentlySelectedGenderIndex == index)
            return;

        _currentlySelectedGenderIndex = index;

        if(index == 0)
        {
            _maleCharacters.Visible = true;
            _maleOne.FadeState = EFadeState.FADE_In;
            _maleTwo.FadeState = EFadeState.FADE_In;
            _maleThree.FadeState = EFadeState.FADE_In;
            _maleFour.FadeState = EFadeState.FADE_In;

            _femaleCharacters.Visible = false;
            _femaleOne.FadeState = EFadeState.FADE_Out;
            _femaleTwo.FadeState = EFadeState.FADE_Out;
            _femaleThree.FadeState = EFadeState.FADE_Out;
            _femaleFour.FadeState = EFadeState.FADE_Out;
            
        } else
        {
            _maleCharacters.Visible = false;
            _maleOne.FadeState = EFadeState.FADE_Out;
            _maleTwo.FadeState = EFadeState.FADE_Out;
            _maleThree.FadeState = EFadeState.FADE_Out;
            _maleFour.FadeState = EFadeState.FADE_Out;

            _femaleCharacters.Visible = true;
            _femaleOne.FadeState = EFadeState.FADE_In;
            _femaleTwo.FadeState = EFadeState.FADE_In;
            _femaleThree.FadeState = EFadeState.FADE_In;
            _femaleFour.FadeState = EFadeState.FADE_In;
        }
    }

    private void CreateCeoDetails()
    {
        if(ValidateCeoCreation())
        {
            CompanyDatabase.Instance.PlayersStaffMember = new CEO(_ceoNameInput.Text, (EStaffSex)_genderDropdown.Selected, EModuleJobType.JOB_All);
            CompanyDatabase.Instance.PlayersStaffMember.SetCharacterModel(null, _selectedCharacter.ModelIndex);
            if (_characterCreationContainer is CharacterDesignMenu designMenu)
                designMenu.Setup();
            _ceoInformationContainer.Visible = false;
            _characterCreationContainer.Visible = true;
        }
    }

    /// <summary>
    /// Checks if all fields have been selected to create a ceo
    /// </summary>
    /// <returns></returns>
    public bool ValidateCeoCreation()
    {
        if (_ceoNameInput == null || string.IsNullOrEmpty(_ceoNameInput.Text))
        {
            return false;
        }

        if (_selectedCharacter == null)
            return false;

        return true;
    }

    private void ClosePopup()
        => this.QueueFree();
}