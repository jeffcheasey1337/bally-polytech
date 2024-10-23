﻿using Microsoft.EntityFrameworkCore;
using UniCabinet.Application.Interfaces.Repository;
using UniCabinet.Domain.DTO;
using UniCabinet.Domain.Entities;
using UniCabinet.Infrastructure.Data;

namespace UniCabinet.Infrastructure.Repository
{
    public class LectureRepository : ILectureRepository
    {
        private readonly ApplicationDbContext _context;
        public LectureRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public LectureDTO GetLectureById(int id)
        {
            var lectureEntity = _context.Lectures.Find(id);
            if (lectureEntity == null) return null;

            return new LectureDTO
            {
                Date = lectureEntity.Date,
                DisciplineDetailId = lectureEntity.DisciplineDetailId,
                Number = lectureEntity.Number,
            };
        }

        public IEnumerable<LectureDTO> GetLectureListByDisciplineDetailId(int id)
        {
            var lectureListEntity = _context.Lectures
                .Where(l => l.DisciplineDetailId == id);

            var disciplineDetailEntity = _context.DisciplineDetails.Find(id);
            var disciplineEntity = _context.Disciplines.Find(disciplineDetailEntity.DisciplineId);

            return lectureListEntity
                .Select(l => new LectureDTO
                {
                    Id = l.Id,
                    Date = l.Date,
                    DisciplineDetailId = l.DisciplineDetailId,
                    Number = l.Number,
                }).ToList();
        }

        public List<LectureDTO> GetAllLectures()
        {
            var lectureEntity = _context.Lectures.ToList();

            return lectureEntity.Select(d => new LectureDTO
            {
                Id = d.Id,
                Date = d.Date,
                DisciplineDetailId = d.DisciplineDetailId,
                Number = d.Number,
            }).ToList();
        }

        public void AddLecture(LectureDTO lectureDTO)
        {
            var lectureEntity = new Lecture
            {
                Date = lectureDTO.Date,
                DisciplineDetailId = lectureDTO.DisciplineDetailId,
                Number = lectureDTO.Number,
            };

            _context.Lectures.Add(lectureEntity);
            _context.SaveChanges();
        }

        public async Task DeleteLecture(int id)
        {
            var lectureEntity = await _context.Lectures.FindAsync(id);
            if (lectureEntity != null)
            {
                _context.Lectures.Remove(lectureEntity);
                await _context.SaveChangesAsync();
            }
        }

        public void UpdateLecture(LectureDTO lectureDTO)
        {
            var lectureEntity = _context.Lectures.FirstOrDefault(d => d.Id == lectureDTO.Id);
            if (lectureEntity == null) return;

            lectureEntity.Number = lectureDTO.Number;
            lectureEntity.DisciplineDetailId = lectureDTO.DisciplineDetailId;
            lectureEntity.Date = lectureDTO.Date;

            _context.SaveChanges();
        }
    }
}
