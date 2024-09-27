using System.Collections.Generic;
using Godot;

namespace GameDevInc;

public class CEO : StaffMember
{
    public CEO(EModuleJobType jobType) : base(jobType)
    {
    }

    public CEO(string name, EStaffSex sex, EModuleJobType jobType = EModuleJobType.JOB_All, bool generateStats = false) : base(name, sex, jobType, generateStats)
    {
    }

    protected override void GenerateStats()
    {
        base.GenerateStats();
    }
}