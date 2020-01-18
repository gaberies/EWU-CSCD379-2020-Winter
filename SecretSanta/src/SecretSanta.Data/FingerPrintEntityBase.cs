using System;
using System.Collections.Generic;
using System.Text;

namespace SecretSanta.Data
{
	public class FingerPrintEntityBase : EntityBase
	{
		string CreatedBy { get; set; }
		DateTime CreatedOn { get; set; }
		string ModifiedBy { get; set; }
		DateTime ModifiedOn { get; set; }
	}
}
