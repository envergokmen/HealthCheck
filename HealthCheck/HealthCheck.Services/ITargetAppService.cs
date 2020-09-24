using HealthCheck.Models.DTOs.TargetApps;
using System.Collections.Generic;

namespace HealthCheck.Services
{
    public interface ITargetAppService
    {
        TargetAppDto Add(CreateTargetAppDto registerDto);
        List<TargetAppDto> All(GetTargetAllAppDto request);
        bool Delete(DeleteTargetAppDto registerDto);
        UpdateTargetAppDto GetOne(GetOneTargetAppDto request);
        UpdateTargetAppDto GetOneToCheck();
        TargetAppDto MarkAsChecked(UpdateChecksStatusDto checkStatus);
        TargetAppDto Update(UpdateTargetAppDto updateDto);
    }
}