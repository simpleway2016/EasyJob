﻿using ECWeb.Database.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECWeb.Database.Actions
{
    public class CreateTableAction : Action
    {
        public EJ.DBTable Table
        {
            get;
            set;
        }
        public EJ.DBColumn[] Columns
        {
            get;
            set;
        }
        public IndexInfo[] IDXConfigs
        {
            get;
            set;
        }

         public CreateTableAction()
        {
        }
        public CreateTableAction(EJ.DBTable table , EJ.DBColumn[] columns
            , IndexInfo[] idxConfigs)
        {
            this.Table = table;
            this.Columns = columns;

            this.IDXConfigs = idxConfigs;
        }
        internal override void BeforeSave()
        {
            foreach (var c in this.Columns)
            {
                c.ChangedProperties.Clear();
            }
        }
        public override void Invoke( EntityDB.IDatabaseService invokingDB)
        {
  
             ITableService service = DBHelper.CreateInstance<ITableService>(invokingDB.GetType().Name);
             service.CreateTable(invokingDB, this.Table, this.Columns, IDXConfigs);
        }
        public override string ToString()
        {
            return string.Format("Create Table {0}", this.Table.Name);
        }
    }
}