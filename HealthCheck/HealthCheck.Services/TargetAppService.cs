using HealthCheck.Database;
using HealthCheck.Models.DTOs.TargetApps;
using HealthCheck.Services.Extensions;
using HealthCheck.Services.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HealthCheck.Services
{
    public class TargetAppService : ITargetAppService
    {
        private readonly HealthContext _db;
        private readonly IJobScheduler _jobScheduler;

        public TargetAppService(HealthContext db, IJobScheduler jobScheduler)
        {
            _db = db;
            _jobScheduler = jobScheduler;
        }

        public List<TargetAppDto> All(GetTargetAllAppDto request)
        {
            var apps = _db.TargetApps.Where(c => c.CreatedById == request.LoggedInUserId).OrderByDescending(x => x.Id).ToList();
            return Mapper.MapToAppDto(apps);
        }

        public UpdateTargetAppDto GetOne(GetOneTargetAppDto request)
        {
            var app = _db.TargetApps.Where(c => c.CreatedById == request.LoggedInUserId && c.Id == request.Id).FirstOrDefault();
            return Mapper.MapToUpdateDto(app);
        }

        public TargetAppDto GetOneToCheck(int appId)
        {
            var app = _db.TargetApps.Where(x => x.Id == appId).FirstOrDefault();
            return Mapper.MapToAppDto(app);
        }

        public TargetAppDto GetOneToCheck(int appId, int createdById)
        {
            var app = _db.TargetApps.Where(x => x.Id == appId && x.CreatedById == createdById).FirstOrDefault();
            return Mapper.MapToAppDto(app);
        }

        public TargetAppDto Add(CreateTargetAppDto registerDto)
        {
            if (!registerDto.Url.IsValidUrl())
            {
                throw new Exception("Invalid Url Please Check");
            }

            var app = Mapper.MapToAppFromCreateDto(registerDto);
            _db.TargetApps.Add(app);
            _db.SaveChanges();
            var created = Mapper.MapToAppDto(app);
            _jobScheduler.AddOrUpdate(created);

            return created;
        }

        public TargetAppDto Update(UpdateTargetAppDto updateDto)
        {
            if (!updateDto.Url.IsValidUrl())
            {
                throw new Exception("Invalid Url Please Check");
            }

            var app = _db.TargetApps.FirstOrDefault(c => c.Id == updateDto.Id && c.CreatedById == updateDto.LoggedInUserId);
            Mapper.SetPropsFromDto(app, updateDto);
            _db.SaveChanges();
            var updated = Mapper.MapToAppDto(app);
            _jobScheduler.AddOrUpdate(updated);
            return updated;
        }

        public bool Delete(DeleteTargetAppDto registerDto)
        {
            var app = _db.TargetApps.FirstOrDefault(c => c.Id == registerDto.Id);
            _db.TargetApps.Remove(app);
            _db.SaveChanges();
            _jobScheduler.RemoveIfExists(app.Id);

            return true;
        }
    }
}
