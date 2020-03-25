using System;

namespace SalaryApp.Services
{
    internal class SystemTimeService : ITime
    {
        public virtual DateTime Now => DateTime.Now;
    }

    internal sealed class OffsetSystemTimeService : SystemTimeService
    {
        int offset;
        public override DateTime Now => base.Now.AddYears(offset);

        public void SetYear(int year) => offset = year - base.Now.Year;
        public void AddYears(int years) => offset += years;
    }
}
