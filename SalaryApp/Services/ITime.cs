using System;

namespace SalaryApp.Services
{
    public interface ITime
    {
        DateTime Now { get; }
        //int Year => Now.Year;
    }
}
