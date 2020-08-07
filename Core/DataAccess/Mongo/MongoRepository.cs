using Core.CrossCuttingConcern.Logging.Serilog.Loggers;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.DataAccess.Mongo
{
	public class MongoRepository<T> : IMongoRepository<T> where T : class
	{
		public IMongoDatabase _database { get; set; }
		public IMongoCollection<T> _collection { get; private set; }
		private readonly ILoggerService _logger;
		private const string _outputTemplate = "[{Timestamp:HH:mm:ss} {Level:u3}] MONGO {Message:lj}{NewLine}{Exception}";

		public MongoRepository(/*ILoggerService logger*/)
		{
			var config = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
				.Build();
			var mongoClient = new MongoClient(config.GetSection("MongoConnection:ConnectionString").Value);

			_database = mongoClient.GetDatabase(config.GetSection("MongoConnection:Database").Value);
			_collection = _database.GetCollection<T>(MapModelNameToDatabase(typeof(T).Name.ToLower()));
			//_logger = logger;
		}

		private static string MapModelNameToDatabase(string modelName)
		{
			if (modelName.EndsWith("y"))
			{
				modelName = modelName.Replace("y", "ies");
			}
			else if (modelName.EndsWith("s"))
			{
				modelName += "es";
			}
			else
			{
				modelName += "s";
			}

			return modelName;
		}

		public async Task<T> Add(T entity)
		{
			try
			{
				await _collection.InsertOneAsync(entity);

				return entity;
			}
			catch (Exception ex)
			{
				//_logger.Write(LogEventLevel.Error, ex, _outputTemplate);
				return null;
			}
		}

		public async Task<bool> Delete(string id, bool wipeRecord = false)
		{
			var result = true;

			try
			{
				if (wipeRecord)
				{
					var filter = Builders<T>.Filter.Eq("_id", ObjectId.Parse(id));
					await _collection.DeleteOneAsync(filter);
				}
				else
				{
					var filter = Builders<T>.Filter.Eq("_id", ObjectId.Parse(id));
					var current = await _collection.Find(filter).FirstOrDefaultAsync();
					var updated = Builders<T>.Update
						.Set("IsActive", false)
						.Set("IsDeleted", true)
						.Set("DateUpdated", DateTime.Now);

					await _collection.UpdateOneAsync(filter, updated);
				}
			}
			catch (Exception ex)
			{
				_logger.Write(LogEventLevel.Error, ex, _outputTemplate);
				result = false;
			}

			return result;
		}

		public async Task<bool> DeleteAll(bool wipeRecord = false)
		{
			var result = true;

			try
			{
				if (wipeRecord)
				{
					await _collection.DeleteManyAsync(FilterDefinition<T>.Empty);
				}
				else
				{
					var list = await _collection.AsQueryable().ToListAsync();

					//foreach (var record in list)
					//{
					//	await Delete(record.Id, false);
					//}
				}
			}
			catch (Exception ex)
			{
				//_logger.Write(LogEventLevel.Error, ex, _outputTemplate);
				result = false;
			}

			return result;
		}

		public async Task<T> Find(string id)
		{
			var filter = Builders<T>.Filter.Eq("_id", ObjectId.Parse(id));
			return await _collection.Find(filter).FirstOrDefaultAsync();
		}

		public IAsyncCursor<T> FindByFilter(FilterDefinition<T> exp, FindOptions<T> options)
		{
			return _collection.FindAsync(exp, options).Result;
		}

		public IAggregateFluent<T> Aggregate()
		{
			return _collection.Aggregate();
		}

		public IAsyncCursor<T> FindByFilter(FilterDefinition<T> exp)
		{
			return _collection.FindAsync(exp).Result;
		}

		public async Task<IList<T>> QueryList(Expression<Func<T, bool>> expression = null)
		{
			return expression != null ?
				await _collection.AsQueryable().Where(expression).ToListAsync()
				:
				await _collection.AsQueryable().ToListAsync();
		}

		public IMongoQueryable<T> Query(Expression<Func<T, bool>> expression = null)
		{
			return expression != null ?
				 _collection.AsQueryable().Where(expression)
				:
				 _collection.AsQueryable();
		}

		public async Task<T> Update(string id, T entity)
		{
			try
			{
				var options = new FindOneAndReplaceOptions<T>
				{
					ReturnDocument = ReturnDocument.After
				};
				var filter = Builders<T>.Filter.Eq("_id", ObjectId.Parse(id.Trim()));

				return await _collection.FindOneAndReplaceAsync(filter, entity, options);
			}
			catch (Exception ex)
			{
				//_logger.Write(LogEventLevel.Error, ex, _outputTemplate);
				return null;
			}
		}
	}
	}
