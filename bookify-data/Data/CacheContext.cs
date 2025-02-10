using bookify_data.Helper;
using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace bookify_data.Data
{
	public class CacheContext 
	{
		public readonly IDbConnection _connection;
		public readonly IConfiguration config;

		public CacheContext()
		{
			_connection = new SqlConnection(Consts.String.RCPconnectionString);
			//SimpleCRUD.SetDialect(SimpleCRUD.Dialect.SQLServer);
		}
		public CacheContext(IDbConnection db)
		{
			_connection = db;
			//SimpleCRUD.SetDialect(SimpleCRUD.Dialect.SQLServer);
		}
		public void Dispose()
		{
			_connection.Dispose();
			_connection.Close();
		}

		public IEnumerable<TModel> Query<TModel>(string sql, object param = null)
		{

			if (_connection.State == ConnectionState.Closed)
				_connection.Open();

			return _connection.Query<TModel>(sql, param).AsEnumerable();
		}
		public async Task<IEnumerable<TModel>> QueryAsync<TModel>(string sql, object param = null)
		{

			if (_connection.State == ConnectionState.Closed)
				_connection.Open();

			return await _connection.QueryAsync<TModel>(sql, param);
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


		public T Get<T>(object id) where T : class
		{
			try
			{
				string sql = $@"select * from dbo.[{TableName<T>()}] where {ObjectHelper.PrimaryKey<T>()}='{id}'";
				if (_connection.State == ConnectionState.Closed)
					_connection.Open();

				var tmp = _connection.Query<T>(sql, new { id = id }).FirstOrDefault();

				return tmp;
			}
			catch (Exception ex)
			{

				throw;
			}

		}
		public T Find<T>(string condition, object obj = null) where T : class
		{
			string sql = $@"select * from dbo.{TableName<T>()} where {condition}";
			if (_connection.State == ConnectionState.Closed)
				_connection.Open();

			return _connection.Query<T>(sql, obj).FirstOrDefault();
		}

		public IEnumerable<T> GetAll<T>(string conditions = null, object parms = null, string orderBy = "", int? page = null, int? limit = Consts.Num.Limit) where T : class
		{
			string sql = $@"select * from dbo.{TableName<T>()} WITH(NOLOCK)";
			if (!string.IsNullOrWhiteSpace(conditions))
			{
				sql += $@" where {conditions}";
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
			return _connection.Query<T>(sql, parms).ToList();
		}
		public int Count(string from, string condition = null, object parms = null)
		{
			string sql = $@"select count(1) from dbo.{from} ";
			if (!string.IsNullOrWhiteSpace(condition))
			{
				sql += $@" where {condition}";
			}

			if (_connection.State == ConnectionState.Closed)
				_connection.Open();
			return _connection.Query<int>(sql, parms).FirstOrDefault();
		}

		public IEnumerable<T> GetAll<T>(string select, string from, string conditions = null, object parms = null, string orderBy = "", int? page = null, int? limit = 50) where T : class
		{
			if (string.IsNullOrWhiteSpace(from))
			{
				from = TableName<T>() + " with(nolock)";
			}
			string sql = $@"select {select} from {from} ";

			if (!string.IsNullOrWhiteSpace(conditions))
			{
				sql += $@" where {conditions}";
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
			return _connection.Query<T>(sql, parms).ToList();
		}
		public int Execute(string sp, object parms, IDbTransaction transaction = null)
		{
			int row = 0;
			try
			{
				if (_connection.State == ConnectionState.Closed)
					_connection.Open();

				if (transaction != null)
				{
					try
					{
						row = _connection.Execute(sp, parms, transaction);
					}
					catch (Exception ex)
					{
						transaction.Rollback();
						throw ex;
					}
					finally
					{
						transaction.Dispose();
					}
				}
				else
				{
					try
					{
						row = _connection.Execute(sp, parms);
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

			return row;
		}

		public long Insert<T>(T parms, IDbTransaction transaction = null) where T : class
		{
			long result = 0;
			try
			{
				//string sql = string.Format(ObjectHelper.Insert<T>(), TableName<T>());

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

		public IEnumerable<T> Query<T>(string sql, object parms, IDbTransaction transaction = null) where T : class
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

		public IEnumerable<T> Query<T>(string sql, object parms, int? page = null, int? limit = 50) where T : class
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

		public bool Update<T>(T parms, IDbTransaction transaction = null) where T : class
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

		public bool Deactivate<T>(object id) where T : class
		{
			var result = false;
			try
			{
				var sql = $@"update [" + TableName<T>() + "] set IsActive = 0 where ID = @ID";
				var resultDb = _connection.Execute(sql, new { ID = id });
				result = true;
			}
			catch (Exception ex)
			{

			}
			return result;
		}

		public async Task<bool> DeactivateAsync<T>(object id) where T : class
		{
			var result = false;
			try
			{
				var sql = $@"update [" + TableName<T>() + "] set IsActive = 0 where ID = @ID";
				var resultDb = await _connection.ExecuteAsync(sql, new { ID = id });
				result = true;
			}
			catch (Exception ex)
			{

			}
			return result;
		}

		public bool Delete<T>(object id) where T : class
		{
			var result = false;
			try
			{
				string sql = $@"delete {TableName<T>()} where ID=@id";
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

		public async Task<bool> DeleteAsync<T>(object id) where T : class
		{
			var result = false;
			try
			{
				string sql = $@"delete {TableName<T>()} where ID=@id";
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
		protected string TableName<T>()
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


		public bool InsertMany<T>(IEnumerable<T> parms, IDbTransaction transaction = null) where T : class
		{
			bool result = false;
			try
			{
				DataTable dt = parms.ToDataTable();
				using (SqlBulkCopy sqlBulk = new SqlBulkCopy((SqlConnection)_connection))
				{
					if (_connection.State != ConnectionState.Open)
					{
						_connection.Open();
					}
					sqlBulk.DestinationTableName = TableName<T>();
					foreach (DataColumn column in dt.Columns)
					{
						sqlBulk.ColumnMappings.Add(column.ColumnName, column.ColumnName);
					}
					sqlBulk.WriteToServer(dt);
				}
				return true;
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

		public bool UpdateMany<T>(IEnumerable<T> parms, IDbTransaction transaction = null) where T : class
		{
			bool result = false;
			try
			{
				DataTable dt = parms.ToDataTable();
				using (SqlConnection con = new SqlConnection(Consts.String.RCPconnectionString))
				{
					using (SqlCommand command = new SqlCommand("", con))
					{
						try
						{
							con.Open();
							//Creating temp table on database
							command.CommandText = "CREATE TABLE #TmpTable(...)";
							command.ExecuteNonQuery();

							using (SqlBulkCopy sqlBulk = new SqlBulkCopy(con))
							{
								//con.Open();
								sqlBulk.DestinationTableName = "#TmpTable";
								foreach (DataColumn column in dt.Columns)
								{
									sqlBulk.ColumnMappings.Add(column.ColumnName, column.ColumnName);
								}
								sqlBulk.WriteToServer(dt);
							}

							string sql = $@"{ObjectHelper.UpdateFromTable<T>()} FROM {TableName<T>()} AS P INNER JOIN #TmpTable AS T ON P.[ID] = T.[ID] ;DROP TABLE #TmpTable;";
							command.CommandTimeout = 3000;
							command.CommandText = sql;
							command.ExecuteNonQuery();
						}
						catch (Exception ex) { }
						finally
						{
							if (con.State == ConnectionState.Open)
								con.Close();
						}
					}
				}
				//var rows = _connection.BulkUpdate(parms);
				result = true;

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
			return true;
		}

		public Task<IEnumerable<string>> InsertsAsync<T>(IEnumerable<T> parms, IDbTransaction transaction = null)
		{
			throw new NotImplementedException();
		}
	}
}
