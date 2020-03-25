using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SalaryApp.Models;
using SalaryApp.Services;
using SalaryApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalaryApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly MSSqlLocalDBContext dbContext;
        private readonly ITime time;

        public HomeController(MSSqlLocalDBContext dbContext, ITime time)
        {
            this.dbContext = dbContext;
            this.time = time ?? new SystemTimeService();
        }

        #region Grid
        /// <summary>
        /// Columns for w2ui grid.
        /// </summary>
        private static class Columns
        {
            public static readonly GridColumn ID = new GridColumn
            {
                field = GridOptions.DefaultIDField,
                text = "ID",
                hidden = true,
            };

            public static readonly GridColumn Position = new GridColumn
            {
                field = "Position",
                size = "200px",
                //editable = 
                searchable = "text"
            };

            public static readonly GridColumn Name = new GridColumn
            {
                field = "Name",
                text = "Full Name",
                editable = new FieldOptions { type = "text" },
                size = "200px",
                searchable = "text"
            };

            internal static readonly GridColumn Employed = new GridColumn
            {
                field = "Employed",
                text = "Year Employed",
                editable = new FieldOptions { type = "text" },
                hidden = true,
                size = "80px",
            };

            internal static readonly GridColumn Rating = new GridColumn
            {
                field = "Rating",
                text = "Last rating",
                render = "number:1",
                hidden = true,
                size = "80px",
            };

            internal static readonly GridColumn RatingYear = new GridColumn
            {
                field = "RatingYear",
                text = "Year of last rating",
                hidden = true,
                size = "80px",
            };

            internal static readonly GridColumn Salary = new GridColumn
            {
                field = nameof(Employee.Salary),
                render = "int",
                size = "80px",
            };
        }

        private static readonly GridOptions gridOptions = new GridOptions
        {
            Columns = new List<GridColumn>
            {
                Columns.ID,
                Columns.Name,
                Columns.Position,
                Columns.Employed,
                Columns.Rating,
                Columns.RatingYear,
                Columns.Salary,
            },
            show = new Dictionary<string, object>
            {
                ["toolbarSearch"] = false,
            }
        };

        private static object ToGridRecord(Employee e)
        {
            var rec = new Dictionary<string, object>
            {
                [Columns.ID.field] = e.Id,
                [Columns.Name.field] = e.Name,
                [Columns.Employed.field] = e.Employed,
                [Columns.Rating.field] = RatingToFloat(e.Rating),
                [Columns.RatingYear.field] = e.Employed + e.YearsEmployed + EmployeeDetail.RatingYearOffset,
                [Columns.Salary.field] = e.Salary,
            };
            if (e.Position != null)
                rec[Columns.Position.field] = new FieldItem
                {
                    id = e.PositionId,
                    text = e.Position.Name
                };
            return rec;
        }

        public IActionResult Index()
        {
            // "select" type editing is poorly implemented. "enum" is not implemented at all.
            //var positions = await dbContext.Position.ToListAsync();
            //Columns.Position.editable = new FieldOptions
            //{
            //    type = "select",
            //    items = positions.Select(p => new FieldItem
            //    {
            //        id = p.Id,
            //        text = p.Name
            //    }).ToArray()
            //};
            return View(new EmployeeList
            {
                GridOptions = gridOptions,
            });
        }

        private class FromGridRequestAttribute : FromFormAttribute
        {
            public FromGridRequestAttribute()
            {
                Name = "request";
            }
        }

        [HttpPost]
        public async Task<string> GridGet([FromGridRequest] string requestString)
        {
            try
            {
                var request = GridRequest.Deserialize(requestString);

                IQueryable<Employee> query = dbContext.Employee;

                // https://github.com/dotnet/efcore/issues/11488
                //foreach (var search in request.Search)
                //{
                //    if (search.value == null) continue;

                //    Func<string, bool> containsFunc = ...;
                //    if (search.field == Columns.Name.field)
                //        query = query.Where(e => containsFunc(e.Name));
                //}

                var count = query.CountAsync();

                IEnumerable<Employee> records = await query
                    .Skip(request.Offset).Take(request.Limit)
                    .Include(e => e.Position)
                    .ToListAsync();
                return new GridResponse
                {
                    total = await count,
                    records = records.Select(ToGridRecord)
                }.Serialize();
            }
            catch (Exception ex)
            {
                return GridResponse.Error(ex.Message).Serialize();
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public GridResponse GridSave([FromGridRequest] string requestString)
        {
            try
            {
                var request = GridRequest.Deserialize(requestString);
                //var records = new List<object>();
                foreach (var change in request.Changes)
                {
                    var id = Convert.ToInt32(change[Columns.ID.field]);
                    var employee = dbContext.Employee.Find(id);
                    if (employee != null)
                    {
                        var viewModel = GetEmployeeViewModel(employee);
                        if (change.TryGetValue(Columns.Name.field, out object name)) viewModel.Name = name.ToString();
                        if (change.TryGetValue(Columns.Employed.field, out object employed)) viewModel.Employed = Convert.ToInt16(employed);
                        if (change.TryGetValue(Columns.Rating.field, out object rating)) viewModel.Rating = Convert.ToSingle(rating);
                        if (change.TryGetValue(Columns.RatingYear.field, out object ratingYear)) viewModel.RatingYear = Convert.ToInt32(ratingYear);
                        UpdateEmployeeFromView(employee, viewModel);
                    }
                    //records.Add(ToGridRecord(employee));
                }
                dbContext.SaveChanges();
                //return new GridResponse { records = records };
                return GridResponse.Success; // The grid seems to just reload anyway.
            }
            catch (Exception ex)
            {
                return GridResponse.Error(GetMessage(ex));
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public GridResponse GridDelete([FromGridRequest] string requestString)
        {
            try
            {
                var request = GridRequest.Deserialize(requestString);
                var range = dbContext.Employee.Where(e => request.Selected.Contains(e.Id));
                dbContext.Employee.RemoveRange(range);
                dbContext.SaveChanges();
                return GridResponse.Success;
            }
            catch (Exception ex)
            {
                return GridResponse.Error(GetMessage(ex));
            }
        }
        #endregion

        #region Overlay
        private static float RatingToFloat(int rating) => rating / 10f;
        private static int RatingToInt(float rating) => (int)Math.Round(rating * 10, 0);

        private int GetCurrentYear() => time.Now.Year;
        private const int ratingYearOffset = EmployeeDetail.RatingYearOffset;

        private EmployeeDetail GetEmployeeViewModel(Employee data) => GetEmployeeViewModel(data, GetCurrentYear());

        internal static EmployeeDetail GetEmployeeViewModel(Employee data, int currentYear)
        {
            int dataYear = data.Employed + data.YearsEmployed;
            int dataRatingYear = dataYear + ratingYearOffset;

            var view = new EmployeeDetail
            {
                ID = data.Id,
                Name = data.Name,
                Employed = data.Employed,
                Position = data.PositionId,
                Salary = data.Salary,
                Manager = data.Manager?.Name,

                Rating = RatingToFloat(data.Rating),
                CurrentYear = currentYear,
                PrevRating1 = RatingToFloat(data.PrevRating1),
                PrevRating1Year = dataRatingYear - 1,
                PrevRating2 = RatingToFloat(data.PrevRating2),
                PrevRating2Year = dataRatingYear - 2,
            };

            if (currentYear != dataYear)
            {
                view.PrevRating3 = view.PrevRating2;
                view.PrevRating3Year = view.PrevRating2Year;
                view.PrevRating2 = view.PrevRating1;
                view.PrevRating2Year = view.PrevRating1Year;
                view.PrevRating1 = view.Rating.Value;
                view.PrevRating1Year = dataRatingYear;
                view.Rating = null;
            }

            return view;
        }

        internal static EmployeeDetail GetNewEmployeeViewModel(int year)
        {
            return new EmployeeDetail
            {
                Employed = year,
                RatingYear = year + ratingYearOffset,
                Rating = 0,
            };
        }

        public async Task<IActionResult> Detail(int? id)
        {
            EmployeeDetail model;
            if (id.HasValue)
            {
                var employee = await dbContext.Employee.FindAsync(id);
                if (employee == null) return NotFound(id);
                model = GetEmployeeViewModel(employee);
            }
            else
            {
                model = GetNewEmployeeViewModel(GetCurrentYear());
            }

            SetSelectLists(model);
            return View(model);
        }

        private static void UpdateEmployeeFromView(Employee dataModel, EmployeeDetail viewModel)
        {
            dataModel.Name = viewModel.Name;
            dataModel.PositionId = viewModel.Position;

            int dataCurrentYear = dataModel.Employed + dataModel.YearsEmployed;
            dataModel.Employed = (short)viewModel.Employed;

            if (viewModel.Rating.HasValue)
            {
                dataModel.Rating = (byte)RatingToInt(viewModel.Rating.Value);
                dataModel.YearsEmployed = (byte)viewModel.YearsEmployed;

                if (dataCurrentYear < viewModel.CurrentYear)
                {
                    // The rating is being entered for the next year (may also have skipped some years).
                    dataModel.PrevRating2 = viewModel.CurrentYear - dataCurrentYear == 1 ?
                        dataModel.PrevRating1
                        : dataModel.Rating;
                    dataModel.PrevRating1 = dataModel.Rating;
                }
            }
            else
            {
                dataModel.YearsEmployed = (byte)(dataCurrentYear - dataModel.Employed);
            }
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(EmployeeDetail viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int currentYear = GetCurrentYear();
                    // Not allowing to enter ratings for the past
                    if (viewModel.Rating.HasValue && !(viewModel.CurrentYear == currentYear || viewModel.CurrentYear + 1 == currentYear))
                        return BadRequest();

                    if (viewModel.YearsEmployed < 0) // This still allows entering future dates.
                        return BadRequest();

                    Employee dataModel;
                    if (viewModel.IsNew)
                    {
                        dataModel = new Employee();

                        dataModel.Name = viewModel.Name;
                        dataModel.PositionId = viewModel.Position;
                        dataModel.Rating = (byte)RatingToInt(viewModel.Rating.Value);
                        dataModel.Employed = (short)viewModel.Employed;
                        dataModel.YearsEmployed = (byte)viewModel.YearsEmployed;
                        if (dataModel.YearsEmployed > 0) dataModel.PrevRating1 = dataModel.Rating;
                        if (dataModel.YearsEmployed > 1) dataModel.PrevRating2 = dataModel.Rating;
                    }
                    else
                    {
                        dataModel = dbContext.Employee.Find(viewModel.ID);
                        if (dataModel == null) return NotFound(viewModel.ID);
                        UpdateEmployeeFromView(dataModel, viewModel);
                    }

                    if (viewModel.IsNew) dbContext.Employee.Add(dataModel);
                    await dbContext.SaveChangesAsync();
                    return RedirectToAction(nameof(Detail), new { id = dataModel.Id });
                }
                catch (Exception ex)
                {
                    viewModel.Error = GetMessage(ex);
                }
            }

            SetSelectLists(viewModel);
            return View(nameof(Detail), viewModel);
        }

        private static string GetMessage(Exception ex) => (ex.GetBaseException() ?? ex).Message;

        private void SetSelectLists(EmployeeDetail model)
        {
            model.Positions = new SelectList(dbContext.Position, nameof(Position.Id), nameof(Position.Name), model.Position);
        }
        #endregion
    }
}