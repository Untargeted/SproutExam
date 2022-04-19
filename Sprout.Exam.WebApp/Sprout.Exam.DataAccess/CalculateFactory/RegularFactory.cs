using System;
using System.Collections.Generic;
using System.Text;
using Sprout.Exam.DataAccess.Classification;

namespace Sprout.Exam.DataAccess.CalculateFactory
{
    public class RegularFactory: CalculateFactory
    {
        RegularEmployee emp = null;
        public RegularFactory(decimal Days)
        {
            emp = new RegularEmployee(Days);
        }

        public override EmployeeClass GetFinalSalary()
        {
            emp.ComputeFinalSalary();
            return emp;
        }
    }
}
