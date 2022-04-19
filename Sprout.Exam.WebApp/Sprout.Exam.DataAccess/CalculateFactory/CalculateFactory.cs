using System;
using System.Collections.Generic;
using System.Text;
using Sprout.Exam.DataAccess.Classification;

namespace Sprout.Exam.DataAccess.CalculateFactory
{
    public abstract class CalculateFactory
    {
        public abstract EmployeeClass GetFinalSalary();
    }
}
