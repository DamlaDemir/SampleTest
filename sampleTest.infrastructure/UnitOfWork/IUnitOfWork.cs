using SampleTest.infrastructure.Repository;
using System;
using System.Threading.Tasks;

namespace spock.infrastructure.UnitOfWork
{
	public interface IUnitOfWork : IDisposable
	{
		IRepository<T> Repository<T>() where T : class;
		int SaveChanges();
		Task<int> SaveChangesAsync();
	}
}
