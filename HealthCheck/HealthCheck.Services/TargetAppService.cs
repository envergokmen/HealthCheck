using HealthCheck.Database;
using HealthCheck.Models;
using HealthCheck.Models.DTOs.TargetApps;
using HealthCheck.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            return _db.TargetApps.Where(c => c.CreatedById == request.LoggedInUserId).OrderByDescending(x => x.Id).Select(app => new TargetAppDto {  IntervalType=app.IntervalType, IntervalValue=app.IntervalValue, CreatedById=app.CreatedById, Id = app.Id, Url = app.Url, Name = app.Name, IsAlive = app.IsAlive, LastCheck = app.LastCheck }).ToList();
        }

        public UpdateTargetAppDto GetOne(GetOneTargetAppDto request)
        {
            return _db.TargetApps.Where(c => c.CreatedById == request.LoggedInUserId && c.Id == request.Id).Select(app => new UpdateTargetAppDto { Id = app.Id, Url = app.Url, Name = app.Name, IntervalType = app.IntervalType, IntervalValue = app.IntervalValue }).FirstOrDefault();
        }

        public TargetAppDto GetOneToCheck(int appId)
        {
            return _db.TargetApps.Where(x=>x.Id==appId).Select(app => new TargetAppDto { Id = app.Id, Url = app.Url, Name = app.Name, CreatedById=app.CreatedById, IntervalType= app.IntervalType, IntervalValue=app.IntervalValue, IsAlive=app.IsAlive }).FirstOrDefault();
        }

        public TargetAppDto GetOneToCheck(int appId, int createdById)
        {
            return _db.TargetApps.Where(x => x.Id == appId && x.CreatedById==createdById).Select(app => new TargetAppDto { Id = app.Id, Url = app.Url, Name = app.Name, CreatedById = app.CreatedById, IntervalType = app.IntervalType, IntervalValue = app.IntervalValue, IsAlive = app.IsAlive }).FirstOrDefault();
        }

        public TargetAppDto Add(CreateTargetAppDto registerDto)
        {
            var app = new TargetApp { Name = registerDto.Name, Url = registerDto.Url, CreatedById = registerDto.LoggedInUserId, IntervalValue = registerDto.IntervalValue, IntervalType = registerDto.IntervalType };
            _db.TargetApps.Add(app);
            _db.SaveChanges();
            var created = new TargetAppDto { Id = app.Id, Url = app.Url, Name = app.Name, CreatedById = registerDto.LoggedInUserId, IntervalType = app.IntervalType, IntervalValue = app.IntervalValue, IsAlive = app.IsAlive, LastCheck = app.LastCheck };

            _jobScheduler.AddOrUpdate(created);

            return created;
        }

        public TargetAppDto Update(UpdateTargetAppDto updateDto)
        {
            var app = _db.TargetApps.FirstOrDefault(c => c.Id == updateDto.Id && c.CreatedById == updateDto.LoggedInUserId);

            app.Name = updateDto.Name;
            app.Url = updateDto.Url;
            app.IntervalType = updateDto.IntervalType;
            app.IntervalValue = updateDto.IntervalValue;
            app.IsAlive = null;

            _db.SaveChanges();
            var updated= new TargetAppDto { Id = app.Id, Url = app.Url, Name = app.Name, CreatedById = updateDto.LoggedInUserId, IntervalType=app.IntervalType, IntervalValue=app.IntervalValue, IsAlive=app.IsAlive };

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
