﻿using Microsoft.EntityFrameworkCore;
using UniCabinet.Application.Interfaces.Repository;
using UniCabinet.Domain.DTO;
using UniCabinet.Domain.Entities;
using UniCabinet.Infrastructure.Data;

namespace UniCabinet.Infrastructure.Repository
{
    public class LectureVisitRepository : ILectureVisitRepository
    {
        private readonly ApplicationDbContext _context;
        public LectureVisitRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public LectureVisitDTO GetLectureVisitById(int id)
        {
            var lectureVisitEntity = _context.LectureVisits.Find(id);
            if (lectureVisitEntity == null) return null;

            return new LectureVisitDTO
            {
                LectureId = lectureVisitEntity.LectureId,
                PointsCount = lectureVisitEntity.PointsCount,
                StudentId = lectureVisitEntity.StudentId,
                isVisit = lectureVisitEntity.IsVisit,
            };
        }

        public List<LectureVisitDTO> GetAllLectureVisits()
        {
            var lectureVisitEntity = _context.LectureVisits
                .Include(lv => lv.Lecture)
                .Include(lv => lv.Student)
                .AsNoTracking();

            return lectureVisitEntity.Select(d => new LectureVisitDTO
            {
                Id = d.Id,
                isVisit = d.IsVisit,
                LectureId = d.LectureId,
                LectureNumber = d.Lecture.Number,
                StudentId = d.StudentId,
                SudentFirstName = d.Student.FirstName,
                StudentLastName = d.Student.LastName,
                StudentPatronymic = d.Student.Patronymic,
                PointsCount = d.PointsCount,
            }).ToList();
        }

        public void AddLectureVisit(LectureVisitDTO lectureVisitDTO)
        {
            var lectureVisitEntity = new LectureVisit
            {
                IsVisit = lectureVisitDTO.isVisit,
                LectureId = lectureVisitDTO.LectureId,
                PointsCount = lectureVisitDTO.PointsCount,
                StudentId = lectureVisitDTO.StudentId,
            };

            _context.LectureVisits.Add(lectureVisitEntity);
            _context.SaveChanges();
        }

        public void DeleteLectureVisit(int id)
        {
            var lectureVisitEntity = _context.LectureVisits.Find(id);
            if (lectureVisitEntity != null)
            {
                _context.LectureVisits.Remove(lectureVisitEntity);
                _context.SaveChanges();
            }
        }

        public void UpdateLectureVisit(LectureVisitDTO lectureVisitDTO)
        {
            var lectureVisitEntity = _context.LectureVisits.FirstOrDefault(d => d.Id == lectureVisitDTO.Id);
            if (lectureVisitEntity == null) return;

            lectureVisitEntity.IsVisit = lectureVisitDTO.isVisit;
            lectureVisitEntity.LectureId = lectureVisitDTO.LectureId;
            lectureVisitEntity.StudentId = lectureVisitDTO.StudentId;
            lectureVisitEntity.PointsCount = lectureVisitDTO.PointsCount;

            _context.SaveChanges();
        }
    }
}
