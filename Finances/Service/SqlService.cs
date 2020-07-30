using Finances.Data;
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
        private SQLiteConnection Factory()
            => new SQLiteConnection(App.DataBasePath);

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

        public int Update(object obj)
        {
            using SQLiteConnection connection = Factory();

            return connection
                .Update(obj);
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

        public int Insert(object obj)
        {
            using SQLiteConnection connection = Factory();
            return connection
                .Insert(obj);
        }

        public SQLiteConnection BeginTransaction()
        {
            var connection = Factory();
            connection.BeginTransaction();
            return connection;
        }
    }

    public interface ISqlService
    {
        SQLiteConnection BeginTransaction();

        int Update(object obj);

        int Insert(object obj);

        int Delete(object obj);

        IList<T> Query<T>(string query, params object[] args) where T : new();

        IList<T> ToList<T>() where T : new();

        IList<T> ToList<T>(Expression<Func<T, bool>> predExpr) where T : new();

        int Delete<T>(Expression<Func<T, bool>> predExpr) where T : new();
    }
}