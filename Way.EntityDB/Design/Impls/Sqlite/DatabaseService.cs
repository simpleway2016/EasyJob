﻿
using Way.EntityDB.Design.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;


namespace Way.EntityDB.Design.Database.Sqlite
{
    [EntityDB.Attributes.DatabaseTypeAttribute(DatabaseType.Sqlite)]
    public class DatabaseService : IDatabaseDesignService
    {

        public void Create(EJ.Databases database)
        {

            var db = EntityDB.DBContext.CreateDatabaseService(database.conStr, EntityDB.DatabaseType.Sqlite);
            CreateEasyJobTable(db);
        }

        public void CreateEasyJobTable(EntityDB.IDatabaseService db)
        {
            bool exists = true;
            try
            {
                db.ExecSqlString("select * from __WayEasyJob");
            }
            catch
            {
                exists = false;
            }
            if (!exists)
            {
                db.ExecSqlString("CREATE TABLE [__WayEasyJob](contentConfig TEXT  NOT NULL)");
                var dbconfig = new DataBaseConfig();
                try
                {
                    dbconfig.LastUpdatedID = Convert.ToInt32( db.ExecSqlString("select lastID from __EasyJob"));
                }
                catch
                {
                }
                db.ExecSqlString("insert into __WayEasyJob (contentConfig) values (@p0)", dbconfig.ToJsonString());
               
            }

            //try
            //{
            //    db.ExecSqlString("CREATE TABLE  [] ([lastID] integer  NOT NULL)");
            //    db.ExecSqlString("insert into  (lastID) values (0)");
            //}
            //catch
            //{
            //}
        }

        public void ChangeName(EJ.Databases database, string newName, string newConnectString)
        {
            var dbnameMatch_old = System.Text.RegularExpressions.Regex.Match(database.conStr, @"Data Source=(?<dname>(\w|\:|\\)+)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            var dbnameMatch = System.Text.RegularExpressions.Regex.Match(newConnectString, @"Data Source=(?<dname>(\w|\:|\\)+)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            string newfilepath = dbnameMatch.Groups["dname"].Value;
            string oldfilepath = dbnameMatch_old.Groups["dname"].Value;
            System.IO.File.Move(oldfilepath, newfilepath);
        }

        public void GetViews(EntityDB.IDatabaseService db, out List<EJ.DBTable> tables, out List<EJ.DBColumn> columns)
        {
            tables = new List<EJ.DBTable>();
            columns = new List<EJ.DBColumn>();
        }


        public string GetObjectFormat()
        {
            return "[{0}]";
        }
    }
}