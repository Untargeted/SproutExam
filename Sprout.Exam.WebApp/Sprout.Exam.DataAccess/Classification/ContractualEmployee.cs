using System;
using System.Collections.Generic;
using System.Text;

namespace Sprout.Exam.DataAccess.Classification
{
    public class ContractualEmployee: EmployeeClass
    {
        private decimal BasicSalary_;
        private decimal FinalSalary_;
        private decimal WorkingDays_;

        public ContractualEmployee(decimal WorkingDays)
        {
            WorkingDays_ = WorkingDays;
        }
        public override decimal FinalSalary
        {
            get { return FinalSalary_; }
            set { FinalSalary_ = value; }
        }

        public void ComputeFinalSalary()
        {
            decimal result = 500 * WorkingDays_;
            FinalSalary_ =  Math.Round((result), 2);
        }
    }
}
