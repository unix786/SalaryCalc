using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalaryApp.Models;
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

        public HomeController(MSSqlLocalDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

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
                field = nameof(Employee.Position),
                size = "200px",
                //editable = 
            };

            public static readonly GridColumn Name = new GridColumn
            {
                field = nameof(Employee.Name),
                text = "Full Name",
                editable = new FieldOptions { type = "text" },
                size = "200px",
            };

            internal static readonly GridColumn Employed = new GridColumn
            {
                field = nameof(Employee.Employed),
                text = "Year Employed",
                editable = new FieldOptions { type = "int" },
                hidden = true,
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
                new GridColumn
                {
                    field = nameof(Employee.YearsEmployed),
                    text = "Years Worked",
                    hidden = true,
                    size = "80px",
                },
                new GridColumn
                {
                    field = nameof(Employee.Salary),
                    render = "int",
                    size = "80px",
                }
            },
        };

        private static object ToGridRecord(Employee e)
        {
            var rec = new Dictionary<string, object>
            {
                [Columns.ID.field] = e.Id,
                [Columns.Name.field] = e.Name,
                [Columns.Employed.field] = e.Employed,
                [nameof(Employee.YearsEmployed)] = e.YearsEmployed,
                [nameof(Employee.Salary)] = e.Salary,
            };
            if (e.Position != null)
                rec[Columns.Position.field] = new FieldItem
                {
                    id = e.PositionId,
                    text = e.Position.Name
                };
            return rec;
        }

        public async Task<IActionResult> Index()
        {
            var positions = await dbContext.Position.ToListAsync();
            Columns.Position.editable = new FieldOptions
            {
                type = "enum",
                items = positions.Select(p => new FieldItem
                {
                    id = p.Id,
                    text = p.Name
                }).ToArray()
            };
            return View(new EmployeeList
            {
                GridOptions = gridOptions,
            });
        }

        [HttpPost]
        public async Task<string> GridGet([FromForm(Name = "request")] string requestString)
        {
            try
            {
                var request = GridRequest.Deserialize(requestString);
                var count = dbContext.Employee.CountAsync();
                IEnumerable<Employee> records = await dbContext.Employee
                    .Include(e => e.Position)
                    .Skip(request.Offset).Take(request.Limit)
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

        [HttpPost]
        public GridResponse GridSave([FromForm(Name = "request")] string requestString)
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
                        if (change.TryGetValue(Columns.Name.field, out object name)) employee.Name = name.ToString();
                        if (change.TryGetValue(Columns.Employed.field, out object employed)) employee.Employed = Convert.ToInt16(employed);
                    }
                    //records.Add(ToGridRecord(employee));
                }
                dbContext.SaveChanges();
                //return new GridResponse { records = records };
                return GridResponse.Success; // The grid seems to just reload anyway.
            }
            catch (Exception ex)
            {
                return GridResponse.Error(ex.Message);
            }
        }

        [HttpPost]
        public GridResponse GridDelete([FromForm(Name = "request")] string requestString)
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
                return GridResponse.Error(ex.Message);
            }
        }
    }
}