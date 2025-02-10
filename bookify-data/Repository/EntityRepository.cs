using bookify_data.Helper;
using bookify_data.Interfaces;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib;
using Dapper.Contrib.Extensions;
namespace bookify_data.Repository
{
	/// <summary>
	/// RCP
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class EntityRepository<T> : IEntityRepository<T> where T : class
	{
		public readonly IDbConnection _connection;
		public EntityRepository()
		{
			_connection = new SqlConnection(Consts.String.RCPconnectionString);
		}
		public EntityRepository(IDbConnection db)
		{
			_connection = db;
		}

		public void Dispose()
		{
			_connection.Dispose();
			_connection.Close();
		}
		public IEnumerable<T> Query(string sql, object param = null)
		{
			try
			{
				if (_connection.State == ConnectionState.Closed)
					_connection.Open();

				return _connection.Query<T>(sql, param).AsEnumerable();
			}
			catch (Exception ex)
			{
				throw;
			}
			//finally
			//{
			//    _connection.Close();
			//}

		}

		public async Task<IEnumerable<T>> QueryAsync(string sql, object param = null)
		{

			if (_connection.State == ConnectionState.Closed)
				_connection.Open();

			return await _connection.QueryAsync<T>(sql, param);
		}

		public void Execute(string sql, object param = null)
		{

			if (_connection.State == ConnectionState.Closed)
				_connection.Open();

			_connection.Execute(sql, param);
		}
		public async Task ExecuteAsync(string sql, object param = null)
		{

			if (_connection.State == ConnectionState.Closed)
				_connection.Open();

			await _connection.ExecuteAsync(sql, param);
		}

		public async Task<T> Get(object id)
		{
			string sql = $@"select * from {TableName()}  WITH(NOLOCK) where {ObjectHelper.PrimaryKey<T>()}=@id";
			if (_connection.State == ConnectionState.Closed)
				_connection.Open();

			return (await _connection.QueryAsync<T>(sql, new { id = id })).FirstOrDefault();
		}

		public async Task<T> Find(string condition, object parms)
		{
			string sql = $@"select * from {TableName()}  WITH(NOLOCK)";
			if (!string.IsNullOrWhiteSpace(condition))
			{
				sql += $@" where {condition}";
			}
			var result = (await _connection.QueryAsync<T>(sql, parms)).FirstOrDefault();
			return result;
		}

		public async Task<IEnumerable<T>> GetAll(string condition = null, object parms = null, string orderBy = "", int? page = null, int? limit = Consts.Num.Limit)
		{
			string sql = $@"select * from {TableName()} WITH(NOLOCK)";
			if (!string.IsNullOrWhiteSpace(condition))
			{
				sql += $@" where {condition}";
			}
			if (!string.IsNullOrWhiteSpace(orderBy))
			{
				sql += $@" Order By {orderBy}";
			}

			if (page >= 0)
			{
				sql += $@" OFFSET {page * limit} ROWS
                        FETCH NEXT  {limit} ROWS ONLY";
			}
			if (_connection.State == ConnectionState.Closed)
				_connection.Open();
			return await _connection.QueryAsync<T>(sql, parms);
		}
		public async Task<IEnumerable<T>> GetAll(string select, string from, string condition = null,
		   object parms = null, string orderBy = "", int? page = null, int? limit = 50)
		{
			if (string.IsNullOrWhiteSpace(from))
			{
				from = TableName() + " with(nolock)";
			}
			string sql = $@"select {select} from {from} ";

			if (!string.IsNullOrWhiteSpace(condition))
			{
				sql += $@" where {condition}";
			}
			if (!string.IsNullOrWhiteSpace(orderBy))
			{
				sql += $@" Order By {orderBy}";
			}
			if (page >= 0)
			{
				sql += $@" OFFSET {page * limit} ROWS
                        FETCH NEXT  {limit} ROWS ONLY";
			}

			if (_connection.State == ConnectionState.Closed)
				_connection.Open();
			return await _connection.QueryAsync<T>(sql, parms);
		}

		public async Task<int> Count(string from, string condition = null, object parms = null)
		{
			from = string.IsNullOrWhiteSpace(from) ? TableName() : from;
			string sql = $@"select count(1) from {from} WITH(NOLOCK)";
			if (!string.IsNullOrWhiteSpace(condition))
			{
				sql += $@" where {condition}";
			}

			if (_connection.State == ConnectionState.Closed)
				_connection.Open();
			return (await _connection.QueryAsync<int>(sql, parms)).FirstOrDefault();
		}

		public long Inserts(T parms, IDbTransaction transaction = null)
		{
			long result;
			//string sql = string.Format(ObjectHelper.Insert<T>(), TableName());
			try
			{
				if (_connection.State == ConnectionState.Closed)
					_connection.Open();

				if (transaction != null)
				{
					try
					{
						result = _connection.Insert(parms, transaction);
					}
					catch (Exception ex)
					{
						transaction.Rollback();
						throw ex;
					}
				}
				else
				{
					try
					{
						result = _connection.Insert(parms);

					}
					catch (Exception ex)
					{
						throw ex;
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				if (_connection.State == ConnectionState.Open)
					_connection.Close();
			}

			return result;
		}

		public bool Updates(T parms, IDbTransaction transaction = null)
		{
			bool result;
			try
			{
				if (_connection.State == ConnectionState.Closed)
					_connection.Open();
				if (transaction != null)
				{
					try
					{
						result = _connection.Update(parms, transaction);
						transaction.Commit();
					}
					catch (Exception ex)
					{
						transaction.Rollback();
						throw ex;
					}
				}
				else
				{
					try
					{
						result = _connection.Update(parms);
					}
					catch (Exception ex)
					{
						throw ex;
					}
				}

			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				if (_connection.State == ConnectionState.Open)
					_connection.Close();
			}

			return result;
		}

		public bool Delete(object id)
		{
			var result = false;
			try
			{
				string sql = $@"delete {TableName()} where ID=@id";
				if (_connection.State == ConnectionState.Closed)
					_connection.Open();

				var rows = _connection.Execute(sql, new { id = id });
				result = rows > 0 ? true : false;
			}
			catch (Exception ex)
			{

			}
			return result;
		}

		public async Task<bool> DeleteAsync(object id)
		{
			var result = false;
			try
			{
				string sql = $@"delete {TableName()} where ID=@id";
				if (_connection.State == ConnectionState.Closed)
					_connection.Open();

				var rows = await _connection.ExecuteAsync(sql, new { id = id });
				result = rows > 0 ? true : false;
			}
			catch (Exception ex)
			{

			}
			return result;
		}

		protected string TableName()
		{
			// Check if we've already set our custom table mapper to TableNameMapper.
			if (SqlMapperExtensions.TableNameMapper != null)
				return SqlMapperExtensions.TableNameMapper(typeof(T));

			// If not, we can use Dapper default method "SqlMapperExtensions.GetTableName(Type type)" which is unfortunately private, that's why we have to call it via reflection.
			string getTableName = "GetTableName";
			MethodInfo getTableNameMethod = typeof(SqlMapperExtensions).GetMethod(getTableName, BindingFlags.NonPublic | BindingFlags.Static);

			if (getTableNameMethod == null)
				throw new ArgumentOutOfRangeException($"Method '{getTableName}' is not found in '{nameof(SqlMapperExtensions)}' class.");

			return getTableNameMethod.Invoke(null, new object[] { typeof(T) }) as string;
		}

		public bool Deactivate(object id)
		{
			throw new NotImplementedException();
		}

		public Task<bool> DeactivateAsync(object id)
		{
			throw new NotImplementedException();
		}


		public IEnumerable<T> Query<T>(string sql, object parms, IDbTransaction transaction = null)
		{
			IEnumerable<T> result;

			try
			{
				if (_connection.State == ConnectionState.Closed)
					_connection.Open();
				if (transaction != null)
				{
					try
					{
						result = _connection.Query<T>(sql, parms, transaction).AsEnumerable();
						transaction.Commit();
					}
					catch (Exception ex)
					{
						transaction.Rollback();
						throw ex;
					}
				}
				else
				{
					try
					{
						result = _connection.Query<T>(sql, parms).AsEnumerable();
					}
					catch (Exception ex)
					{
						throw ex;
					}
				}

			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				if (_connection.State == ConnectionState.Open)
					_connection.Close();
			}

			return result;
		}

		public IEnumerable<T> Query<T>(string sql, object parms, int? page = null, int? limit = 50)
		{
			IEnumerable<T> result;

			try
			{
				if (page >= 0)
				{
					sql += $@" OFFSET {page * limit} ROWS
                        FETCH NEXT  {limit} ROWS ONLY";
				}

				if (_connection.State == ConnectionState.Closed)
					_connection.Open();

				try
				{
					result = _connection.Query<T>(sql, parms).AsEnumerable();
				}
				catch (Exception ex)
				{
					throw ex;
				}


			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				if (_connection.State == ConnectionState.Open)
					_connection.Close();
			}

			return result;
		}

	}
}
