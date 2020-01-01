using Microsoft.EntityFrameworkCore;
using sampleTest.model.context;
using SampleTest.infrastructure.Repository;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace spock.infrastructure.UnitOfWork
{
	public class UnitOfWork : IUnitOfWork
	{
		private bool disposed = false;
		protected readonly SampleTestContext _context;

		public UnitOfWork(SampleTestContext context)
		{
			_context = context;
		}

		public IRepository<T> Repository<T>() where T : class
		{
			return new Repository<T>(_context);
		}

		public int SaveChanges()
		{
			try
			{			
				return _context.SaveChanges();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public async Task<int> SaveChangesAsync()
		{
			try
			{
				if (_context.ChangeTracker.HasChanges())
				{
					var modifications = new System.Collections.Generic.Dictionary<Microsoft.EntityFrameworkCore.Metadata.IProperty, object>();
					var changes = _context.ChangeTracker.Entries();

					foreach (var changedItem in changes)
					{
						if (changedItem.State != EntityState.Unchanged)
						{
							var entity = changedItem.Entity;
							// log this one
						}
					}




					//var changes = _context.ChangeTracker.Entries()
					//	.Select(x => new
					//	{
					//		OriginalValues = x.State != EntityState.Added ? x.OriginalValues.Properties.ToDictionary(c => c, c => x.OriginalValues[c]) : null,
					//		CurrentValues = x.CurrentValues.Properties.ToDictionary(c => c, c => x.CurrentValues[c]),
					//		x.State,
					//		DateInserted = DateTime.Now
					//	})
					//.FirstOrDefault();

					//if (changes.State == EntityState.Modified)
					//{
					//	modifications = changes.OriginalValues;
					//}
					//else
					//{
					//	modifications = changes.CurrentValues;
					//}

					//foreach (var item in modifications)
					//{
					//	var keyName = item.Key.Name;
					//	var value = item.Value;

					//	var modified = changes.CurrentValues[item.Key];
					//}
				}

				return await _context.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!disposed && disposing)
			{
				_context.Dispose();
			}

			disposed = true;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}
