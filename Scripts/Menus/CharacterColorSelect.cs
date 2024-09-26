using System.Collections.Generic;
using Godot;

namespace GameDevInc.Menus;

public enum EColorPropertyType
{
    COL_Skin,
    COL_HairOne,
    COL_HairTwo,
    COL_ShirtOne,
    COL_ShirtTwo,
    COL_ShirtThree,
    COL_Pants
}

public partial class CharacterColorSelect : Label
{
    private ColorRect _activeColorRect;
    private TextureButton _buttonRef;
    private ReferenceRect _selectedRef;

    [Export] private CharacterDesignMenu _designMenu;
    [Export] private EColorPropertyType _colorPropType;

    public bool IsActive = true;

    public override void _Ready()
    {
        base._Ready();

        _activeColorRect = GetNode<ColorRect>("ColorRect");
        _buttonRef = GetNode<TextureButton>("TextureRect");
        _selectedRef = GetNode<ReferenceRect>("ReferenceRect");

        if (_buttonRef != null)
            _buttonRef.Connect("pressed", new Callable(this, "OnSelectColor"));
    }

    public void Setup()
    {
        var model = _designMenu.SelectedCharacterController;
        var index = CompanyDatabase.Instance.PlayersStaffMember.ModelIndex;
        var sex = CompanyDatabase.Instance.PlayersStaffMember.Sex;

        if(model != null)
        {
            if(sex == EStaffSex.SEX_Male)
            {
                switch(_colorPropType)
                {
                    case EColorPropertyType.COL_HairTwo:
                        if(index == 0 || index == 1 || index == 3)
                        {
                            Modulate = new Color(1, 1, 1, 0.1f);
                            IsActive = false;
                        }
                        break;
                    case EColorPropertyType.COL_ShirtTwo:
                        if(index == 0 || index == 1)
                        {
                            Modulate = new Color(1, 1, 1, 0.1f);
                            IsActive = false;
                        }
                        break;
                    case EColorPropertyType.COL_ShirtThree:
                        if(index == 0 || index == 1 || index == 2)
                        {
                            Modulate = new Color(1, 1, 1, 0.1f);
                            IsActive = false;
                        }
                        break;
                }
            }
            else
            {
                switch (_colorPropType)
                {
                    case EColorPropertyType.COL_ShirtTwo:
                        if (index != 0)
                        {
                            Modulate = new Color(1, 1, 1, 0.1f);
                            IsActive = false;
                        }
                        break;
                    case EColorPropertyType.COL_ShirtThree:
                        Modulate = new Color(1, 1, 1, 0.1f);
                        IsActive = false;
                        break;
                    case EColorPropertyType.COL_HairTwo:
                        if (index != 0)
                        {
                            Modulate = new Color(1, 1, 1, 0.1f);
                            IsActive = false;
                        }
                        break;
                    case EColorPropertyType.COL_Pants:
                        if (index == 3)
                        {
                            Modulate = new Color(1, 1, 1, 0.1f);
                            IsActive = false;
                        }

                        break;
                }
            }
        }
    }

    public void SetColor(Color color)
    {
        _activeColorRect.Color = color;
        var model = _designMenu.SelectedCharacterController;
        if(model != null)
        {
            var modelIndex = CompanyDatabase.Instance.PlayersStaffMember.ModelIndex;
            var modelSex = CompanyDatabase.Instance.PlayersStaffMember.Sex;
            var mesh = model.Mesh;

            OrmMaterial3D materialToUpdate = null;
            
            switch(_colorPropType)
            {
                case EColorPropertyType.COL_Skin:
                    if (mesh.GetActiveMaterial(0) is OrmMaterial3D skinOrm)
                        materialToUpdate = skinOrm;
                    return;
                case EColorPropertyType.COL_HairOne:
                    if (modelSex == EStaffSex.SEX_Male)
                    {
                        if(modelIndex == 0 || modelIndex == 1 || modelIndex == 2)
                            materialToUpdate = mesh.GetActiveMaterial(2) as OrmMaterial3D;
                        else if(modelIndex == 3)
                            materialToUpdate = mesh.GetActiveMaterial(3) as OrmMaterial3D;
                    } else if (modelSex == EStaffSex.SEX_Female)
                    {
                        if(modelIndex == 1 || modelIndex == 2 || modelIndex == 3)
                            materialToUpdate = mesh.GetActiveMaterial(2) as OrmMaterial3D;
                        else if(modelIndex == 0)
                            materialToUpdate = mesh.GetActiveMaterial(6) as OrmMaterial3D;  
                    }

                    break;
                case EColorPropertyType.COL_HairTwo:
                    if (modelSex == EStaffSex.SEX_Male)
                    {
                        if (modelIndex == 2)
                            materialToUpdate = mesh.GetActiveMaterial(3) as OrmMaterial3D;
                    } else if (modelSex == EStaffSex.SEX_Female)
                    {
                        if(modelIndex == 0)
                            materialToUpdate = mesh.GetActiveMaterial(9) as OrmMaterial3D;
                    }

                    break;
                case EColorPropertyType.COL_ShirtOne:
                    if (modelSex == EStaffSex.SEX_Male)
                    {
                        if(modelIndex == 0 || modelIndex == 1)
                            materialToUpdate = mesh.GetActiveMaterial(3) as OrmMaterial3D;
                        else if(modelIndex == 2)
                            materialToUpdate = mesh.GetActiveMaterial(4) as OrmMaterial3D;
                        else if(modelIndex == 3)
                            materialToUpdate = mesh.GetActiveMaterial(1) as OrmMaterial3D;;
                    } else if (modelSex == EStaffSex.SEX_Female)
                    {
                        materialToUpdate = mesh.GetActiveMaterial(3) as OrmMaterial3D;
                    }

                    break;
                case EColorPropertyType.COL_ShirtTwo:
                    if (modelSex == EStaffSex.SEX_Male)
                    {
                        if(modelIndex == 2)
                            materialToUpdate = mesh.GetActiveMaterial(5) as OrmMaterial3D;
                        else if(modelIndex == 3)
                            materialToUpdate = mesh.GetActiveMaterial(3) as OrmMaterial3D;
                    } else if (modelSex == EStaffSex.SEX_Female)
                    {
                        if(modelIndex == 0)
                            materialToUpdate = mesh.GetActiveMaterial(7) as OrmMaterial3D;
                    }

                    break;
                case EColorPropertyType.COL_ShirtThree:
                    if (modelSex == EStaffSex.SEX_Male)
                    {
                        if(modelIndex == 3)
                            materialToUpdate = mesh.GetActiveMaterial(6) as OrmMaterial3D;
                    }

                    break;
                case EColorPropertyType.COL_Pants:
                    if (modelSex == EStaffSex.SEX_Male)
                    {
                        if (modelIndex == 0 || modelIndex == 1)
                            materialToUpdate = mesh.GetActiveMaterial(4) as OrmMaterial3D;
                        else if(modelIndex == 2)
                            materialToUpdate = mesh.GetActiveMaterial(6) as OrmMaterial3D;
                        else if(modelIndex == 3)
                            materialToUpdate = mesh.GetActiveMaterial(5) as OrmMaterial3D;
                    } else if (modelSex == EStaffSex.SEX_Female)
                    {
                        if(modelIndex == 0 || modelIndex == 1 || modelIndex == 2)
                            materialToUpdate = mesh.GetActiveMaterial(4) as OrmMaterial3D;
                    }

                    break;
            }
            
            if(materialToUpdate != null)
                materialToUpdate.AlbedoColor = color;
            else
                GD.PushError("CharacterColorSelect::SetColor -> Failed to update color");
        }
    }

    public void OnSelectColor()
    {
        if (!IsActive) return;

        if (_designMenu != null)
        {
            _designMenu.SetSelectedColorEdit(this);
            _selectedRef.Visible = true;
        }
    }

    public void Deselect()
        => _selectedRef.Visible = false;

    public void UpdateColorsWithModelData()
    {
        var model = _designMenu.SelectedCharacterController;
        if (model != null)
        {
            var modelIndex = CompanyDatabase.Instance.PlayersStaffMember.ModelIndex;
            var modelSex = CompanyDatabase.Instance.PlayersStaffMember.Sex;
            var mesh = model.Mesh;

            if(modelSex == EStaffSex.SEX_Male)
            {
                if(_colorPropType == EColorPropertyType.COL_Skin)
                {
                    if (mesh.GetActiveMaterial(0) is OrmMaterial3D orm)
                    {
                        _activeColorRect.Color = orm.AlbedoColor;
                    }
                    return;
                }

                switch(_colorPropType)
                {
                    case EColorPropertyType.COL_Skin:
                        if(mesh.GetActiveMaterial(0) is OrmMaterial3D skinOrm)
                            _activeColorRect.Color = skinOrm.AlbedoColor;
                        return;
                    case EColorPropertyType.COL_HairOne:
                        if(modelIndex == 0 || modelIndex == 1 || modelIndex == 2)
                        {
                            if (mesh.GetActiveMaterial(2) is OrmMaterial3D hairOneOrm)
                                _activeColorRect.Color = hairOneOrm.AlbedoColor;
                        } else if(modelIndex == 3)
                        {
                            if (mesh.GetActiveMaterial(2) is OrmMaterial3D hairOneOrm)
                                _activeColorRect.Color = hairOneOrm.AlbedoColor;
                        }
                        return;
                    case EColorPropertyType.COL_HairTwo:
                        if(modelIndex == 2)
                        {
                            if(mesh.GetActiveMaterial(3) is OrmMaterial3D hairTwoOrm)
                                _activeColorRect.Color = hairTwoOrm.AlbedoColor;
                        }
                        return;
                    case EColorPropertyType.COL_ShirtOne:
                        if(modelIndex == 0 || modelIndex == 1)
                        {
                            if(mesh.GetActiveMaterial(3) is OrmMaterial3D shirtOneOrm)
                                _activeColorRect.Color = shirtOneOrm.AlbedoColor;
                        } else if(modelIndex == 2)
                        {
                            if (mesh.GetActiveMaterial(4) is OrmMaterial3D shirtOneOrm)
                                _activeColorRect.Color = shirtOneOrm.AlbedoColor;
                        } else if(modelIndex == 3)
                        {
                            if (mesh.GetActiveMaterial(1) is OrmMaterial3D shirtOneOrm)
                                _activeColorRect.Color = shirtOneOrm.AlbedoColor;
                        }
                        return;
                    case EColorPropertyType.COL_ShirtTwo:
                        if(modelIndex == 2)
                        {
                            if(mesh.GetActiveMaterial(5) is OrmMaterial3D shirtTwoOrm)
                                _activeColorRect.Color = shirtTwoOrm.AlbedoColor;
                        } else if(modelIndex == 3)
                        {
                            if (mesh.GetActiveMaterial(4) is OrmMaterial3D shirtTwoOrm)
                                _activeColorRect.Color = shirtTwoOrm.AlbedoColor;
                        }
                        return;
                    case EColorPropertyType.COL_ShirtThree:
                        if(modelIndex == 3)
                        {
                            if(mesh.GetActiveMaterial(6) is OrmMaterial3D shirtThree)
                                _activeColorRect.Color = shirtThree.AlbedoColor;
                        }
                        return;
                    case EColorPropertyType.COL_Pants:
                        if (modelIndex == 0 || modelIndex == 1)
                        {
                            if (mesh.GetActiveMaterial(4) is OrmMaterial3D pants)
                                _activeColorRect.Color = pants.AlbedoColor;
                        }
                        else if (modelIndex == 2)
                        { 
                            if(mesh.GetActiveMaterial(6) is OrmMaterial3D pants)
                                _activeColorRect.Color = pants.AlbedoColor;
                        } else if(modelIndex == 3)
                        {
                            if (mesh.GetActiveMaterial(5) is OrmMaterial3D pants)
                                _activeColorRect.Color = pants.AlbedoColor;
                        }
                        return;
                }
            }
        }
    }
}