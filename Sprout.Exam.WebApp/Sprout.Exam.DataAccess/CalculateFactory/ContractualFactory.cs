using System;
using System.Collections.Generic;
using System.Text;
using Sprout.Exam.DataAccess.Classification;

namespace Sprout.Exam.DataAccess.CalculateFactory
{
    public class ContractualFactory : CalculateFactory
    {
        ContractualEmployee emp = null;
        public ContractualFactory(decimal Days)
        {
            emp = new ContractualEmployee(Days);
        }

        public override EmployeeClass GetFinalSalary()
        {
            emp.ComputeFinalSalary();
            return emp;
        }
    }
}
