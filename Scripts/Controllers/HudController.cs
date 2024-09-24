using System.Collections.Generic;
using Godot;

namespace GameDevInc.UI;

public partial class HudController : Control
{
    [ExportCategory("Action Buttons")]
    [Export] private TextureButton m_NewDevBtn;
    [Export] private TextureButton m_ResearchBtn;
    [Export] private TextureButton m_MarketingBtn;
    [Export] private TextureButton m_InventoryBtn;
    [Export] private TextureButton m_StaffBtn;
    [Export] private TextureButton m_MarketplaceBtn;
    [Export] private TextureButton m_ChartsBtn;
    [Export] private TextureButton m_TownBtn;

    [ExportCategory("Company Details")]
    [Export] private Label m_CompanyName;
    [Export] private Label m_CompanyFunds;
    [Export] private Label m_CurrentDate;

    private Company m_PlayerCompany;

    public override void _Ready()
    {
        base._Ready();

        var companyDb = GetNode<CompanyDatabase>("/root/CompanyDatabase");
        if(companyDb != null )
        {
            m_PlayerCompany = companyDb.PlayersCompany;
        }

        // Setup the company details
        m_CompanyName.Text = m_PlayerCompany != null ? m_PlayerCompany.CompanyName : "No Company Set";
        m_CompanyFunds.Text = m_PlayerCompany != null ? m_PlayerCompany.CompanyFunds.ToString() : "$0";
        m_PlayerCompany.e_FundsChange += UpdateFundsText;
        // TODO: Load current date

        // TODO: Connect buttons to method
    }

    public void UpdateFundsText(int funds)
    {
        if (m_CompanyFunds != null)
            m_CompanyFunds.Text = funds.ToString();
    }
}