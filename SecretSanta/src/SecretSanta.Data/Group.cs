using System;
using System.Collections.Generic;
using System.Text;

namespace SecretSanta.Data
{
	public class Group : FingerPrintEntityBase
	{
		string Name;
		public ICollection<UserGroupRelationship> Relationship { get; set; }
	}
}
