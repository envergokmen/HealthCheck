using HealthCheck.Database;
using HealthCheck.Models;
using HealthCheck.Models.DTOs.TargetApps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HealthCheck.Services
{
    public class TargetAppService : ITargetAppService
    {
        private readonly HealthContext _db;

        public TargetAppService(HealthContext db)
        {
            _db = db;
        }

        public List<TargetAppDto> All(GetTargetAllAppDto request)
        {
            return _db.TargetApps.Where(c => c.CreatedById == request.LoggedInUserId).OrderByDescending(x => x.Id).Select(app => new TargetAppDto {  IntervalType=app.IntervalType, IntervalValue=app.IntervalValue, CreatedById=app.CreatedById, Id = app.Id, Url = app.Url, Name = app.Name, IsAlive = app.IsAlive, LastCheck = app.LastCheck }).ToList();
        }

        public UpdateTargetAppDto GetOne(GetOneTargetAppDto request)
        {
            return _db.TargetApps.Where(c => c.CreatedById == request.LoggedInUserId && c.Id == request.Id).Select(app => new UpdateTargetAppDto { Id = app.Id, Url = app.Url, Name = app.Name, IntervalType = app.IntervalType, IntervalValue = app.IntervalValue }).FirstOrDefault();
        }

        public UpdateTargetAppDto GetOneToCheck()
        {
            return _db.TargetApps.OrderBy(x => x.LastCheck).Select(app => new UpdateTargetAppDto { Id = app.Id, Url = app.Url, Name = app.Name }).FirstOrDefault();
        }

        public TargetAppDto Add(CreateTargetAppDto registerDto)
        {
            var app = new TargetApp { Name = registerDto.Name, Url = registerDto.Url, CreatedById = registerDto.LoggedInUserId, IntervalValue = registerDto.IntervalValue, IntervalType = registerDto.IntervalType };
            _db.TargetApps.Add(app);
            _db.SaveChanges();

            return new TargetAppDto { Id = app.Id, Url = app.Url, Name = app.Name, CreatedById = registerDto.LoggedInUserId, IntervalType=app.IntervalType, IntervalValue= app.IntervalValue, IsAlive=app.IsAlive, LastCheck=app.LastCheck };
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

            return new TargetAppDto { Id = app.Id, Url = app.Url, Name = app.Name, CreatedById = updateDto.LoggedInUserId };
        }

        public TargetAppDto MarkAsChecked(UpdateChecksStatusDto checkStatus)
        {
            var app = _db.TargetApps.FirstOrDefault(c => c.Id == checkStatus.Id);

            app.IsAlive = checkStatus.IsAlive;
            app.LastCheck = checkStatus.CheckDate;

            _db.SaveChanges();

            return new TargetAppDto { Id = app.Id, Url = app.Url, Name = app.Name };
        }

        public bool Delete(DeleteTargetAppDto registerDto)
        {
            var app = _db.TargetApps.FirstOrDefault(c => c.Id == registerDto.Id);

            _db.TargetApps.Remove(app);
            _db.SaveChanges();

            return true;
        }
    }
}
