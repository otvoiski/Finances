using Finances.Model;
using SQLite;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace Finances.Service
{
    public class SqlService : ISqlService
    {
        public int Delete(object obj)
        {
            using SQLiteConnection connection = Factory();
            return connection
                .Delete(obj);
        }

        public int Delete<T>(Expression<Func<T, bool>> predExpr) where T : new()
        {
            using SQLiteConnection connection = Factory();
            return connection
                .Table<T>()
                .Delete(predExpr);
        }

        public int GetLastInsertRowId()
        {
            using SQLiteConnection connection = Factory();
            return connection
                .FindWithQuery<int>("SELECT last_insert_rowid()");
        }

        public int InsertOrReplace(object obj)
        {
            using SQLiteConnection connection = Factory();

            return connection
                .InsertOrReplace(obj);
        }

        public IList<T> Query<T>(string query, params object[] args) where T : new()
        {
            using SQLiteConnection connection = Factory();
            return connection
                .Query<T>(query, args);
        }

        public IList<T> ToList<T>() where T : new()
        {
            using SQLiteConnection connection = Factory();
            return connection
                .Table<T>()
                .ToList();
        }

        public IList<T> ToList<T>(Expression<Func<T, bool>> predExpr) where T : new()
        {
            using SQLiteConnection connection = Factory();
            return connection
                .Table<T>()
                .Where(predExpr)
                .ToList();
        }

        private SQLiteConnection Factory()
            => new SQLiteConnection(App.DataBasePath);
    }

    public interface ISqlService
    {
        int GetLastInsertRowId();

        int InsertOrReplace(object obj);

        int Delete(object obj);

        IList<T> Query<T>(string query, params object[] args) where T : new();

        IList<T> ToList<T>() where T : new();

        IList<T> ToList<T>(Expression<Func<T, bool>> predExpr) where T : new();

        int Delete<T>(Expression<Func<T, bool>> predExpr) where T : new();
    }
}