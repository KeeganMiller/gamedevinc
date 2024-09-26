using System.Collections.Generic;
using Godot;

namespace GameDevInc;

public class CEO : StaffMember
{
    public CEO(EModuleJobType jobType) : base(jobType)
    {
    }

    public CEO(string name, EStaffSex sex, EModuleJobType jobType = EModuleJobType.JOB_All) : base(name, sex, jobType)
    {
    }

    protected override void GenerateStats()
    {
        base.GenerateStats();
    }
}