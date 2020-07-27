using Finances.Model;
using SQLite;
using SQLitePCL;
using System;
using System.IO;
using System.Windows;

namespace Finances
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string DataBasePath;
        public static IServiceProvider Services = Dependencies.GetServiceProvider();

        public static void Init()
        {
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            DataBasePath = Path.Combine(folderPath, "Finances", "Finances.db");
            var exists = File.Exists(DataBasePath);
            var isFirstInit = !exists;

            if (isFirstInit)
            {
                //Make sure folder exists
                folderPath = Path.GetDirectoryName(DataBasePath);
                Directory.CreateDirectory(folderPath);
                File.CreateText(DataBasePath).Dispose();
            }

            if (isFirstInit)
            {
                var db = new SQLiteConnection(DataBasePath);
                //Create schema
                db.CreateTable<Parcel>();
                db.CreateTable<Bill>();
            }
        }
    }
}