﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniCabinet.Domain.Entities;

namespace UniCabinet.Domain.DTO
{
    public class StudentProgressDTO
    {
        public int Id { get; set; }

        public string StudentId { get; set; }

        public int DisciplineDetailId { get; set; }

        /// <summary>
        /// Сумма баллов за лекций
        /// </summary>
        public int TotalLecturePoints { get; set; }

        /// <summary>
        /// Сумма баллов за практику
        /// </summary>
        public int TotalPracticalPoints { get; set; }

        /// <summary>
        /// Общая сумма баллов
        /// </summary>
        public int TotalPoints { get; set; }

        /// <summary>
        /// Итоговая оценка
        /// </summary>
        public int FinalGrade { get; set; }

        /// <summary>
        /// Требуется пересдача
        /// </summary>
        public bool NeedsRetake { get; set; }
    }
}
