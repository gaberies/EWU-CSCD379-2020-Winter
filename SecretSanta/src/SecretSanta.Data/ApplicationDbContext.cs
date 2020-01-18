using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Text;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;

namespace SecretSanta.Data
{
	public class ApplicationDbContext : DbContext
	{
		private IHttpContextAccessor Accessor;
		public Microsoft.EntityFrameworkCore.DbSet<User> UserProp { get; set; }
		public Microsoft.EntityFrameworkCore.DbSet<Gift> GiftProp { get; set; }
		


		public ApplicationDbContext(DbContextOptions options) : base(options)
		{
		}

		public ApplicationDbContext(DbContextOptions options, IHttpContextAccessor accessor) : base(options)
		{
			Accessor = accessor;
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<User>()
				.HasMany<Gift>(s => s.Gifts)
				.WithMany(c => c.Users)
				.Map(cs =>
						{
							cs.MapLeftKey("UserRefId");
							cs.MapRightKey("GiftRefId");
							cs.ToTable("SecretSanta");
						});
		}
	}
}
