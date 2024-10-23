﻿using Microsoft.Extensions.Logging;
using UniCabinet.Application.Interfaces.Repository;
using UniCabinet.Application.Services;
using UniCabinet.Domain.DTO;
using UniCabinet.Domain.Entities;
using UniCabinet.Infrastructure.BackgroundServices;
using UniCabinet.Infrastructure.Data;

namespace UniCabinet.Infrastructure.Repository
{
    public class SemesterRepository : ISemesterRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<SemesterRepository> _logger;
        public SemesterRepository(ApplicationDbContext context, ILogger<SemesterRepository> logger)
        {
            _context = context;
            _logger = logger;
            
    }

        public List<SemesterDTO> GetAllSemesters()
        {
            var semesterEntities = _context.Semesters.ToList();

            return semesterEntities.Select(d => new SemesterDTO
            {
                Id = d.Id,
                DayStart = d.DayStart,
                DayEnd = d.DayEnd,
                MounthStart = d.MounthStart,
                MounthEnd = d.MounthEnd,
                Number = d.Number,
            }).ToList();
        }

        public SemesterDTO GetSemesterById(int id)
        {
            var semesterEntity = _context.Semesters.Find(id);
            if (semesterEntity == null) return null;

            return new SemesterDTO
            {
                Id = semesterEntity.Id,
                DayStart = semesterEntity.DayStart,
                DayEnd = semesterEntity.DayEnd,
                MounthStart = semesterEntity.MounthStart,
                MounthEnd = semesterEntity.MounthEnd,
                Number = semesterEntity.Number,
            };
        }

        public SemesterDTO GetCurrentSemester(DateTime currentDate)
        {
            var semesters = _context.Semesters.ToList();
            Semester semesterEntity = null;

            foreach (var s in semesters)
            {
                var startDate = new DateTime(currentDate.Year, s.MounthStart, s.DayStart);
                var endDate = new DateTime(currentDate.Year, s.MounthEnd, s.DayEnd);

                // Если период семестра пересекает новый год
                if (endDate < startDate)
                {
                    endDate = endDate.AddYears(1);
                }

                _logger.LogInformation($"Проверяем семестр №{s.Number}: {startDate.ToShortDateString()} - {endDate.ToShortDateString()}");

                if (currentDate >= startDate && currentDate <= endDate)
                {
                    _logger.LogInformation($"Текущая дата {currentDate.ToShortDateString()} попадает в семестр №{s.Number}");
                    semesterEntity = s;
                    break;
                }

            }

            if (semesterEntity == null)
            {
                throw new InvalidOperationException("Текущий семестр не найден.");
            }

            _logger.LogInformation($"Определён текущий семестр: №{semesterEntity.Number}, период: {semesterEntity.DayStart}.{semesterEntity.MounthStart} - {semesterEntity.DayEnd}.{semesterEntity.MounthEnd}");

            return new SemesterDTO
            {
                Id = semesterEntity.Id,
                Number = semesterEntity.Number,
                DayStart = semesterEntity.DayStart,
                MounthStart = semesterEntity.MounthStart,
                DayEnd = semesterEntity.DayEnd,
                MounthEnd = semesterEntity.MounthEnd,
            };
        }

        public Semester GetSemesterEntityById(int id)
        {
            return _context.Semesters.Find(id);
        }

        public void Add(Semester semester)
        {
            _context.Semesters.Add(semester);
        }

        public void Update(Semester semester)
        {
            _context.Semesters.Update(semester);
        }

        public void Remove(Semester semester)
        {
            _context.Semesters.Remove(semester);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
