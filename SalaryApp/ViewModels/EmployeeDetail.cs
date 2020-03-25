using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SalaryApp.ViewModels
{
    /// <summary>
    /// ViewModel for <see cref="Models.Employee"/>.
    /// </summary>
    public class EmployeeDetail
    {
        public int? ID { get; set; }
        public bool IsNew { get => ID.HasValue == false; }

        [Required, MinLength(2)]
        [DisplayName("Full name")]
        public string Name { get; set; }

        [Required]
        public int Position { get; set; }
        public SelectList Positions { get; set; }

        [Range(0, 9999)]
        [DisplayName("Year when employed")]
        public int Employed { get; set; }

        public int RatingYear { get; set; }

        /// <summary>Assuming that <see cref="Rating"/> is for the year before now.</summary>
        internal const int RatingYearOffset = -1;

        /// <summary><see cref="RatingYear"/> without <see cref="RatingYearOffset"/>.</summary>
        internal int CurrentYear
        {
            get => RatingYear - RatingYearOffset;
            set => RatingYear = value + RatingYearOffset;
        }

        [DisplayName("Years worked")]
        public int YearsEmployed { get => CurrentYear - Employed; }

        [Range(0, 5)]
        public float? Rating { get; set; }

        public float PrevRating1 { get; set; }

        public int PrevRating1Year { get; set; }

        public float PrevRating2 { get; set; }

        public int PrevRating2Year { get; set; }

        public float? PrevRating3 { get; set; }

        public int PrevRating3Year { get; set; }

        public int? Salary { get; set; }

        public string Manager { get; set; }

        public string Error { get; set; }
    }
}
