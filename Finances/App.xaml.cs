using Finances.Model;
using SQLite;
using System;
using System.Windows;

namespace Finances
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly string dataBaseName = "Finances.db";
        private static readonly string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        public static string DataBasePath = System.IO.Path.Combine(folderPath, "Finances", dataBaseName);
        public static IServiceProvider Services = Dependencies.GetServiceProvider();

        public static void Seed()
        {
            // Seed
            var db = new SQLiteConnection(DataBasePath);
            db.CreateTable<Bill>();
        }
    }
}