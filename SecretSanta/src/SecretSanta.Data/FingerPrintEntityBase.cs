using System;
using System.Collections.Generic;
using System.Text;

namespace SecretSanta.Data
{
    public class FingerPrintEntityBase : EntityBase
    {
        private string _CreatedBy = "<Invalid>";
        public string CreatedBy
        {
            get => _CreatedBy;
            set => _CreatedBy = value;
        }
        private DateTime _CreatedOn;
        public DateTime CreatedOn
        {
            get => _CreatedOn;
            set => _CreatedOn = value;
        }
        private string _ModifiedBy = "<Invalid>";
        public string ModifiedBy
        {
            get => _ModifiedBy;
            set => _ModifiedBy = value;
        }
        private DateTime _ModifiedOn;
        public DateTime ModifiedOn
        {
            get => _ModifiedOn;
            set => _ModifiedOn = value;
        }
    }
}
