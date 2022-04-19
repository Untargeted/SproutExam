using System;
using System.Collections.Generic;
using System.Text;

namespace Sprout.Exam.DataAccess.Classification
{
    public class RegularEmployee : EmployeeClass
    {
        private decimal FinalSalary_;
        private decimal AbsentDays_;

        public RegularEmployee(decimal AbsentDays)
        {
            AbsentDays_ = AbsentDays;
        }
        public override decimal FinalSalary
        {
            get { return FinalSalary_; }
            set { FinalSalary_ = value; }
        }

        public void ComputeFinalSalary()
        {
            double deductAbsent = (double)AbsentDays_ * (double)(20000.00 / 22.00);
            double result = (double)20000.00 - (double)deductAbsent - ((double)20000.00 * (double)0.12);
            FinalSalary_ =  (decimal)Math.Round(result, 2);
        }

    }
}
