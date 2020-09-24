﻿using HealthCheck.Database;
using HealthCheck.Models;
using HealthCheck.Models.DTOs.TargetApps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HealthCheck.Services
{
    public class HealthCheckService
    {
        private readonly HealthContext db;

        public HealthCheckService(HealthContext _db)
        {
            db = _db;
        }

        public List<TargetAppDto> All(GetTargetAllAppDto request)
        {
            return db.TargetApps.Where(c => c.CreatedById == request.LoggedInUserId).OrderByDescending(x=>x.Id).Select(app => new TargetAppDto { Id = app.Id, Url = app.Url, Name = app.Name, IsAlive=app.IsAlive, LastCheck= app.LastCheck }).ToList();
        }

        public UpdateTargetAppDto GetOne(GetOneTargetAppDto request)
        {
            return db.TargetApps.Where(c => c.CreatedById == request.LoggedInUserId && c.Id == request.Id).Select(app => new UpdateTargetAppDto { Id = app.Id, Url = app.Url, Name = app.Name, IntervalType=app.IntervalType, IntervalValue=app.IntervalValue }).FirstOrDefault();
        }

        public UpdateTargetAppDto GetOneToCheck()
        {
            return db.TargetApps.OrderBy(x=>x.LastCheck).Select(app => new UpdateTargetAppDto { Id = app.Id, Url = app.Url, Name = app.Name }).FirstOrDefault();
        }

        public TargetAppDto Add(CreateTargetAppDto registerDto)
        {
            var app = new TargetApp { Name = registerDto.Name, Url= registerDto.Url, CreatedById= registerDto.LoggedInUserId, IntervalValue= registerDto.IntervalValue, IntervalType=registerDto.IntervalType };
            db.TargetApps.Add(app);
            db.SaveChanges();

            return new TargetAppDto { Id = app.Id, Url = app.Url, Name = app.Name };
        }

        public TargetAppDto Update(UpdateTargetAppDto registerDto)
        {
            var app = db.TargetApps.FirstOrDefault(c => c.Id == registerDto.Id && c.CreatedById==registerDto.LoggedInUserId);

            app.Name = registerDto.Name;
            app.Url = registerDto.Url;
            app.IntervalType = registerDto.IntervalType;
            app.IntervalValue = registerDto.IntervalValue;

            db.SaveChanges();

            return new TargetAppDto { Id = app.Id, Url = app.Url, Name = app.Name };
        }

        public TargetAppDto MarkAsChecked(UpdateChecksStatusDto checkStatus)
        {
            var app = db.TargetApps.FirstOrDefault(c => c.Id == checkStatus.Id);

            app.IsAlive = checkStatus.IsAlive;
            app.LastCheck = checkStatus.CheckDate;

            db.SaveChanges();

            return new TargetAppDto { Id = app.Id, Url = app.Url, Name = app.Name };
        }

        public bool Delete(DeleteTargetAppDto registerDto)
        {
            var app = db.TargetApps.FirstOrDefault(c => c.Id == registerDto.Id);

            db.TargetApps.Remove(app);
            db.SaveChanges();

            return true;
        }
    }
}
