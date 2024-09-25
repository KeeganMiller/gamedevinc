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

    public override void _Ready()
    {
        base._Ready();

        _activeColorRect = GetNode<ColorRect>("ColorRect");
        _buttonRef = GetNode<TextureButton>("TextureRect");
        _selectedRef = GetNode<ReferenceRect>("ReferenceRect");

        if (_buttonRef != null)
            _buttonRef.Connect("pressed", new Callable(this, "OnSelectColor"));
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

            switch(_colorPropType)
            {
                case EColorPropertyType.COL_Skin:
                    if (mesh.GetActiveMaterial(0) is OrmMaterial3D skinOrm)
                        skinOrm.AlbedoColor = color;
                    return;
                case EColorPropertyType.COL_HairOne:
                    if(modelIndex == 0 || modelIndex == 1 || modelIndex == 2)
                    {
                        if (mesh.GetActiveMaterial(2) is OrmMaterial3D hairOneOrm)
                            hairOneOrm.AlbedoColor = color;
                    } else if(modelIndex == 3)
                    {
                        if (mesh.GetActiveMaterial(3) is OrmMaterial3D hairOneOrm)
                            hairOneOrm.AlbedoColor = color;
                    }
                    return;
                case EColorPropertyType.COL_HairTwo:
                    if(modelIndex == 2)
                    {
                        if (mesh.GetActiveMaterial(3) is OrmMaterial3D hairTwoOrm)
                            hairTwoOrm.AlbedoColor = color;
                    }
                    return;
                case EColorPropertyType.COL_ShirtOne:
                    if(modelIndex == 0 || modelIndex == 1)
                    {
                        if(mesh.GetActiveMaterial(3) is OrmMaterial3D shirtOneOrm)
                            shirtOneOrm.AlbedoColor = color;
                    } else if(modelIndex ==  2)
                    {
                        if (mesh.GetActiveMaterial(4) is OrmMaterial3D shirtOneOrm)
                            shirtOneOrm.AlbedoColor = color;
                    } else if(modelIndex == 3)
                    {
                        if (mesh.GetActiveMaterial(1) is OrmMaterial3D shirtOneOrm)
                            shirtOneOrm.AlbedoColor = color;
                    }
                    return;
                case EColorPropertyType.COL_ShirtTwo:
                    if(modelIndex == 2)
                    {
                        if(mesh.GetActiveMaterial(5) is  OrmMaterial3D shirtTwoOrm)
                            shirtTwoOrm.AlbedoColor = color;
                    } else if (modelIndex == 3)
                    {
                        if (mesh.GetActiveMaterial(3) is OrmMaterial3D shirtTwoOrm)
                            shirtTwoOrm.AlbedoColor = color;
                    }
                    return;
                case EColorPropertyType.COL_ShirtThree:
                    if(modelIndex == 3)
                    {
                        if(mesh.GetActiveMaterial(6) is OrmMaterial3D shirtThreeOrm)
                            shirtThreeOrm.AlbedoColor = color;
                    }
                    return;
                case EColorPropertyType.COL_Pants:
                    if(modelIndex == 0 || modelIndex == 1)
                    {
                        if(mesh.GetActiveMaterial(4) is OrmMaterial3D pantsOneOrm)
                            pantsOneOrm.AlbedoColor = color;
                    } else if(modelIndex == 2)
                    {
                        if (mesh.GetActiveMaterial(6) is OrmMaterial3D pantsOneOrm)
                            pantsOneOrm.AlbedoColor = color;
                    } else if(modelIndex == 3)
                    {
                        if (mesh.GetActiveMaterial(5) is OrmMaterial3D pantsOneOrm)
                            pantsOneOrm.AlbedoColor = color;
                    }
                    return;
            }
        }
    }

    public void OnSelectColor()
    {
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