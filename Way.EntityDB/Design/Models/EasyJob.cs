
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;

namespace EJ{


    /// <summary>
	/// 项目
	/// </summary>
    [Way.EntityDB.Attributes.Table("Project","id")]
    public class Project :Way.EntityDB.DataItem
    {

/// <summary>
	/// 
	/// </summary>
public  Project()
        {
        }


System.Nullable<Int32> _id;
/// <summary>
/// id
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="id",Storage = "_id",DbType="int" ,IsPrimaryKey=true,IsDbGenerated=true)]
        public System.Nullable<Int32> id
        {
            get
            {
                return this._id;
            }
            set
            {
                if ((this._id != value))
                {
                    this.SendPropertyChanging("id",this._id,value);
                    this._id = value;
                    this.SendPropertyChanged("id");

                }
            }
        }

String _Name;
/// <summary>
/// Name
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="Name",Storage = "_Name",DbType="varchar(50)")]
        public String Name
        {
            get
            {
                return this._Name;
            }
            set
            {
                if ((this._Name != value))
                {
                    this.SendPropertyChanging("Name",this._Name,value);
                    this._Name = value;
                    this.SendPropertyChanged("Name");

                }
            }
        }
}}
namespace EJ{

/// <summary>
/// 
	/// </summary>
public enum Databases_dbTypeEnum:int
{
    

/// <summary>
/// 
	/// </summary>
SqlServer = 1,

/// <summary>
/// 
	/// </summary>

Sqlite = 2,

/// <summary>
/// 
	/// </summary>

MySql=3,
}


    /// <summary>
	/// 数据库
	/// </summary>
    [Way.EntityDB.Attributes.Table("Databases","id")]
    public class Databases :Way.EntityDB.DataItem
    {

/// <summary>
	/// 
	/// </summary>
public  Databases()
        {
        }


System.Nullable<Int32> _id;
/// <summary>
/// id
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="id",Storage = "_id",DbType="int" ,IsPrimaryKey=true,IsDbGenerated=true)]
        public System.Nullable<Int32> id
        {
            get
            {
                return this._id;
            }
            set
            {
                if ((this._id != value))
                {
                    this.SendPropertyChanging("id",this._id,value);
                    this._id = value;
                    this.SendPropertyChanged("id");

                }
            }
        }

System.Nullable<Int32> _ProjectID;
/// <summary>
/// 项目ID
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="项目ID",Storage = "_ProjectID",DbType="int")]
        public System.Nullable<Int32> ProjectID
        {
            get
            {
                return this._ProjectID;
            }
            set
            {
                if ((this._ProjectID != value))
                {
                    this.SendPropertyChanging("ProjectID",this._ProjectID,value);
                    this._ProjectID = value;
                    this.SendPropertyChanged("ProjectID");

                }
            }
        }

String _Name;
/// <summary>
/// Name
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="Name",Storage = "_Name",DbType="varchar(50)")]
        public String Name
        {
            get
            {
                return this._Name;
            }
            set
            {
                if ((this._Name != value))
                {
                    this.SendPropertyChanging("Name",this._Name,value);
                    this._Name = value;
                    this.SendPropertyChanged("Name");

                }
            }
        }

System.Nullable<Databases_dbTypeEnum> _dbType=(System.Nullable<Databases_dbTypeEnum>)(1);
/// <summary>
/// 数据库类型
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="数据库类型",Storage = "_dbType",DbType="int")]
        public System.Nullable<Databases_dbTypeEnum> dbType
        {
            get
            {
                return this._dbType;
            }
            set
            {
                if ((this._dbType != value))
                {
                    this.SendPropertyChanging("dbType",this._dbType,value);
                    this._dbType = value;
                    this.SendPropertyChanged("dbType");

                }
            }
        }

String _conStr;
/// <summary>
/// 连接字符串
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="连接字符串",Storage = "_conStr",DbType="varchar(200)")]
        public String conStr
        {
            get
            {
                return this._conStr;
            }
            set
            {
                if ((this._conStr != value))
                {
                    this.SendPropertyChanging("conStr",this._conStr,value);
                    this._conStr = value;
                    this.SendPropertyChanged("conStr");

                }
            }
        }

String _dllPath;
/// <summary>
/// dll生成文件夹
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="dll生成文件夹",Storage = "_dllPath",DbType="varchar(100)")]
        public String dllPath
        {
            get
            {
                return this._dllPath;
            }
            set
            {
                if ((this._dllPath != value))
                {
                    this.SendPropertyChanging("dllPath",this._dllPath,value);
                    this._dllPath = value;
                    this.SendPropertyChanged("dllPath");

                }
            }
        }

System.Nullable<Int32> _iLock=0;
/// <summary>
/// iLock
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="iLock",Storage = "_iLock",DbType="int")]
        public System.Nullable<Int32> iLock
        {
            get
            {
                return this._iLock;
            }
            set
            {
                if ((this._iLock != value))
                {
                    this.SendPropertyChanging("iLock",this._iLock,value);
                    this._iLock = value;
                    this.SendPropertyChanged("iLock");

                }
            }
        }

String _NameSpace;
/// <summary>
/// NameSpace
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="NameSpace",Storage = "_NameSpace",DbType="varchar(50)")]
        public String NameSpace
        {
            get
            {
                return this._NameSpace;
            }
            set
            {
                if ((this._NameSpace != value))
                {
                    this.SendPropertyChanging("NameSpace",this._NameSpace,value);
                    this._NameSpace = value;
                    this.SendPropertyChanged("NameSpace");

                }
            }
        }

String _Guid;
/// <summary>
/// 唯一标示ID
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="唯一标示ID",Storage = "_Guid",DbType="varchar(50)")]
        public String Guid
        {
            get
            {
                return this._Guid;
            }
            set
            {
                if ((this._Guid != value))
                {
                    this.SendPropertyChanging("Guid",this._Guid,value);
                    this._Guid = value;
                    this.SendPropertyChanged("Guid");

                }
            }
        }
}}
namespace EJ{

/// <summary>
/// 
	/// </summary>
public enum User_RoleEnum:int
{
    

/// <summary>
/// 
	/// </summary>
开发人员 = 1,

/// <summary>
/// 
	/// </summary>

客户端测试人员 = 1<<1,

/// <summary>
/// 
	/// </summary>

数据库设计师 = 1<<2 | 开发人员,

/// <summary>
/// 
	/// </summary>

管理员 = 数据库设计师 | 1<<3,

/// <summary>
/// 
	/// </summary>

项目经理 = 1<<4 | 开发人员,
}


    /// <summary>
	/// 系统用户
	/// </summary>
    [Way.EntityDB.Attributes.Table("User","id")]
    public class User :Way.EntityDB.DataItem
    {

/// <summary>
	/// 
	/// </summary>
public  User()
        {
        }


System.Nullable<Int32> _id;
/// <summary>
/// id
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="id",Storage = "_id",DbType="int" ,IsPrimaryKey=true,IsDbGenerated=true)]
        public System.Nullable<Int32> id
        {
            get
            {
                return this._id;
            }
            set
            {
                if ((this._id != value))
                {
                    this.SendPropertyChanging("id",this._id,value);
                    this._id = value;
                    this.SendPropertyChanged("id");

                }
            }
        }

System.Nullable<User_RoleEnum> _Role;
/// <summary>
/// 角色
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="角色",Storage = "_Role",DbType="int")]
        public System.Nullable<User_RoleEnum> Role
        {
            get
            {
                return this._Role;
            }
            set
            {
                if ((this._Role != value))
                {
                    this.SendPropertyChanging("Role",this._Role,value);
                    this._Role = value;
                    this.SendPropertyChanged("Role");

                }
            }
        }

String _Name;
/// <summary>
/// Name
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="Name",Storage = "_Name",DbType="varchar(50)")]
        public String Name
        {
            get
            {
                return this._Name;
            }
            set
            {
                if ((this._Name != value))
                {
                    this.SendPropertyChanging("Name",this._Name,value);
                    this._Name = value;
                    this.SendPropertyChanged("Name");

                }
            }
        }

String _Password;
/// <summary>
/// Password
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="Password",Storage = "_Password",DbType="varchar(50)")]
        public String Password
        {
            get
            {
                return this._Password;
            }
            set
            {
                if ((this._Password != value))
                {
                    this.SendPropertyChanging("Password",this._Password,value);
                    this._Password = value;
                    this.SendPropertyChanged("Password");

                }
            }
        }
}}
namespace EJ{

/// <summary>
/// 
	/// </summary>
public enum DBPower_PowerEnum:int
{
    

/// <summary>
/// 
	/// </summary>
只读 = 0,

/// <summary>
/// 
	/// </summary>

修改 = 1,
}


    /// <summary>
	/// 数据库权限
	/// </summary>
    [Way.EntityDB.Attributes.Table("DBPower","id")]
    public class DBPower :Way.EntityDB.DataItem
    {

/// <summary>
	/// 
	/// </summary>
public  DBPower()
        {
        }


System.Nullable<Int32> _id;
/// <summary>
/// id
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="id",Storage = "_id",DbType="int" ,IsPrimaryKey=true,IsDbGenerated=true)]
        public System.Nullable<Int32> id
        {
            get
            {
                return this._id;
            }
            set
            {
                if ((this._id != value))
                {
                    this.SendPropertyChanging("id",this._id,value);
                    this._id = value;
                    this.SendPropertyChanged("id");

                }
            }
        }

System.Nullable<Int32> _UserID;
/// <summary>
/// 用户
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="用户",Storage = "_UserID",DbType="int")]
        public System.Nullable<Int32> UserID
        {
            get
            {
                return this._UserID;
            }
            set
            {
                if ((this._UserID != value))
                {
                    this.SendPropertyChanging("UserID",this._UserID,value);
                    this._UserID = value;
                    this.SendPropertyChanged("UserID");

                }
            }
        }

System.Nullable<DBPower_PowerEnum> _Power;
/// <summary>
/// 权限
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="权限",Storage = "_Power",DbType="int")]
        public System.Nullable<DBPower_PowerEnum> Power
        {
            get
            {
                return this._Power;
            }
            set
            {
                if ((this._Power != value))
                {
                    this.SendPropertyChanging("Power",this._Power,value);
                    this._Power = value;
                    this.SendPropertyChanged("Power");

                }
            }
        }

System.Nullable<Int32> _DatabaseID;
/// <summary>
/// 数据库ID
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="数据库ID",Storage = "_DatabaseID",DbType="int")]
        public System.Nullable<Int32> DatabaseID
        {
            get
            {
                return this._DatabaseID;
            }
            set
            {
                if ((this._DatabaseID != value))
                {
                    this.SendPropertyChanging("DatabaseID",this._DatabaseID,value);
                    this._DatabaseID = value;
                    this.SendPropertyChanged("DatabaseID");

                }
            }
        }
}}
namespace EJ{

/// <summary>
/// 
	/// </summary>
public enum Bug_StatusEnum:int
{
    

/// <summary>
/// 
	/// </summary>
提交给开发人员 = 0,

/// <summary>
/// 
	/// </summary>

反馈给提交者 = 1,

/// <summary>
/// 
	/// </summary>

处理完毕 = 2,
}


    /// <summary>
	/// 错误报告
	/// </summary>
    [Way.EntityDB.Attributes.Table("Bug","id")]
    public class Bug :Way.EntityDB.DataItem
    {

/// <summary>
	/// 
	/// </summary>
public  Bug()
        {
        }


System.Nullable<Int32> _id;
/// <summary>
/// id
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="id",Storage = "_id",DbType="int" ,IsPrimaryKey=true,IsDbGenerated=true)]
        public System.Nullable<Int32> id
        {
            get
            {
                return this._id;
            }
            set
            {
                if ((this._id != value))
                {
                    this.SendPropertyChanging("id",this._id,value);
                    this._id = value;
                    this.SendPropertyChanged("id");

                }
            }
        }

String _Title;
/// <summary>
/// 标题
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="标题",Storage = "_Title",DbType="varchar(50)")]
        public String Title
        {
            get
            {
                return this._Title;
            }
            set
            {
                if ((this._Title != value))
                {
                    this.SendPropertyChanging("Title",this._Title,value);
                    this._Title = value;
                    this.SendPropertyChanged("Title");

                }
            }
        }

System.Nullable<Int32> _SubmitUserID;
/// <summary>
/// 提交者ID
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="提交者ID",Storage = "_SubmitUserID",DbType="int")]
        public System.Nullable<Int32> SubmitUserID
        {
            get
            {
                return this._SubmitUserID;
            }
            set
            {
                if ((this._SubmitUserID != value))
                {
                    this.SendPropertyChanging("SubmitUserID",this._SubmitUserID,value);
                    this._SubmitUserID = value;
                    this.SendPropertyChanged("SubmitUserID");

                }
            }
        }

System.Nullable<DateTime> _SubmitTime;
/// <summary>
/// 提交时间
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="提交时间",Storage = "_SubmitTime",DbType="datetime")]
        public System.Nullable<DateTime> SubmitTime
        {
            get
            {
                return this._SubmitTime;
            }
            set
            {
                if ((this._SubmitTime != value))
                {
                    this.SendPropertyChanging("SubmitTime",this._SubmitTime,value);
                    this._SubmitTime = value;
                    this.SendPropertyChanged("SubmitTime");

                }
            }
        }

System.Nullable<Int32> _HandlerID;
/// <summary>
/// 处理者ID
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="处理者ID",Storage = "_HandlerID",DbType="int")]
        public System.Nullable<Int32> HandlerID
        {
            get
            {
                return this._HandlerID;
            }
            set
            {
                if ((this._HandlerID != value))
                {
                    this.SendPropertyChanging("HandlerID",this._HandlerID,value);
                    this._HandlerID = value;
                    this.SendPropertyChanged("HandlerID");

                }
            }
        }

System.Nullable<DateTime> _LastDate;
/// <summary>
/// 最后反馈时间
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="最后反馈时间",Storage = "_LastDate",DbType="datetime")]
        public System.Nullable<DateTime> LastDate
        {
            get
            {
                return this._LastDate;
            }
            set
            {
                if ((this._LastDate != value))
                {
                    this.SendPropertyChanging("LastDate",this._LastDate,value);
                    this._LastDate = value;
                    this.SendPropertyChanged("LastDate");

                }
            }
        }

System.Nullable<DateTime> _FinishTime;
/// <summary>
/// 处理完毕时间
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="处理完毕时间",Storage = "_FinishTime",DbType="datetime")]
        public System.Nullable<DateTime> FinishTime
        {
            get
            {
                return this._FinishTime;
            }
            set
            {
                if ((this._FinishTime != value))
                {
                    this.SendPropertyChanging("FinishTime",this._FinishTime,value);
                    this._FinishTime = value;
                    this.SendPropertyChanged("FinishTime");

                }
            }
        }

System.Nullable<Bug_StatusEnum> _Status;
/// <summary>
/// 当前状态
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="当前状态",Storage = "_Status",DbType="int")]
        public System.Nullable<Bug_StatusEnum> Status
        {
            get
            {
                return this._Status;
            }
            set
            {
                if ((this._Status != value))
                {
                    this.SendPropertyChanging("Status",this._Status,value);
                    this._Status = value;
                    this.SendPropertyChanged("Status");

                }
            }
        }
}}
namespace EJ{


    /// <summary>
	/// 数据表
	/// </summary>
    [Way.EntityDB.Attributes.Table("DBTable","id")]
    public class DBTable :Way.EntityDB.DataItem
    {

/// <summary>
	/// 
	/// </summary>
public  DBTable()
        {
        }


System.Nullable<Int32> _id;
/// <summary>
/// id
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="id",Storage = "_id",DbType="int" ,IsPrimaryKey=true,IsDbGenerated=true)]
        public System.Nullable<Int32> id
        {
            get
            {
                return this._id;
            }
            set
            {
                if ((this._id != value))
                {
                    this.SendPropertyChanging("id",this._id,value);
                    this._id = value;
                    this.SendPropertyChanged("id");

                }
            }
        }

String _caption;
/// <summary>
/// caption
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="caption",Storage = "_caption",DbType="varchar(50)")]
        public String caption
        {
            get
            {
                return this._caption;
            }
            set
            {
                if ((this._caption != value))
                {
                    this.SendPropertyChanging("caption",this._caption,value);
                    this._caption = value;
                    this.SendPropertyChanged("caption");

                }
            }
        }

String _Name;
/// <summary>
/// Name
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="Name",Storage = "_Name",DbType="varchar(50)")]
        public String Name
        {
            get
            {
                return this._Name;
            }
            set
            {
                if ((this._Name != value))
                {
                    this.SendPropertyChanging("Name",this._Name,value);
                    this._Name = value;
                    this.SendPropertyChanged("Name");

                }
            }
        }

System.Nullable<Int32> _DatabaseID;
/// <summary>
/// DatabaseID
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="DatabaseID",Storage = "_DatabaseID",DbType="int")]
        public System.Nullable<Int32> DatabaseID
        {
            get
            {
                return this._DatabaseID;
            }
            set
            {
                if ((this._DatabaseID != value))
                {
                    this.SendPropertyChanging("DatabaseID",this._DatabaseID,value);
                    this._DatabaseID = value;
                    this.SendPropertyChanged("DatabaseID");

                }
            }
        }

System.Nullable<Int32> _iLock=0;
/// <summary>
/// iLock
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="iLock",Storage = "_iLock",DbType="int")]
        public System.Nullable<Int32> iLock
        {
            get
            {
                return this._iLock;
            }
            set
            {
                if ((this._iLock != value))
                {
                    this.SendPropertyChanging("iLock",this._iLock,value);
                    this._iLock = value;
                    this.SendPropertyChanged("iLock");

                }
            }
        }
}}
namespace EJ{


    /// <summary>
	/// 字段
	/// </summary>
    [Way.EntityDB.Attributes.Table("DBColumn","id")]
    public class DBColumn :Way.EntityDB.DataItem
    {

/// <summary>
	/// 
	/// </summary>
public  DBColumn()
        {
        }


System.Nullable<Int32> _id;
/// <summary>
/// id
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="id",Storage = "_id",DbType="int" ,IsPrimaryKey=true,IsDbGenerated=true)]
        public System.Nullable<Int32> id
        {
            get
            {
                return this._id;
            }
            set
            {
                if ((this._id != value))
                {
                    this.SendPropertyChanging("id",this._id,value);
                    this._id = value;
                    this.SendPropertyChanged("id");

                }
            }
        }

String _caption;
/// <summary>
/// caption
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="caption",Storage = "_caption",DbType="varchar(200)")]
        public String caption
        {
            get
            {
                return this._caption;
            }
            set
            {
                if ((this._caption != value))
                {
                    this.SendPropertyChanging("caption",this._caption,value);
                    this._caption = value;
                    this.SendPropertyChanged("caption");

                }
            }
        }

String _Name;
/// <summary>
/// Name
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="Name",Storage = "_Name",DbType="varchar(50)")]
        public String Name
        {
            get
            {
                return this._Name;
            }
            set
            {
                if ((this._Name != value))
                {
                    this.SendPropertyChanging("Name",this._Name,value);
                    this._Name = value;
                    this.SendPropertyChanged("Name");

                }
            }
        }

System.Nullable<Boolean> _IsAutoIncrement=false;
/// <summary>
/// 自增长
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="自增长",Storage = "_IsAutoIncrement",DbType="bit")]
        public System.Nullable<Boolean> IsAutoIncrement
        {
            get
            {
                return this._IsAutoIncrement;
            }
            set
            {
                if ((this._IsAutoIncrement != value))
                {
                    this.SendPropertyChanging("IsAutoIncrement",this._IsAutoIncrement,value);
                    this._IsAutoIncrement = value;
                    this.SendPropertyChanged("IsAutoIncrement");

                }
            }
        }

System.Nullable<Boolean> _CanNull=true;
/// <summary>
/// 可以为空
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="可以为空",Storage = "_CanNull",DbType="bit")]
        public System.Nullable<Boolean> CanNull
        {
            get
            {
                return this._CanNull;
            }
            set
            {
                if ((this._CanNull != value))
                {
                    this.SendPropertyChanging("CanNull",this._CanNull,value);
                    this._CanNull = value;
                    this.SendPropertyChanged("CanNull");

                }
            }
        }

String _dbType;
/// <summary>
/// 数据库字段类型
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="数据库字段类型",Storage = "_dbType",DbType="varchar(50)")]
        public String dbType
        {
            get
            {
                return this._dbType;
            }
            set
            {
                if ((this._dbType != value))
                {
                    this.SendPropertyChanging("dbType",this._dbType,value);
                    this._dbType = value;
                    this.SendPropertyChanged("dbType");

                }
            }
        }

String _Type;
/// <summary>
/// c#类型
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="c#类型",Storage = "_Type",DbType="varchar(50)")]
        public String Type
        {
            get
            {
                return this._Type;
            }
            set
            {
                if ((this._Type != value))
                {
                    this.SendPropertyChanging("Type",this._Type,value);
                    this._Type = value;
                    this.SendPropertyChanged("Type");

                }
            }
        }

String _EnumDefine;
/// <summary>
/// Enum定义
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="Enum定义",Storage = "_EnumDefine",DbType="varchar(300)")]
        public String EnumDefine
        {
            get
            {
                return this._EnumDefine;
            }
            set
            {
                if ((this._EnumDefine != value))
                {
                    this.SendPropertyChanging("EnumDefine",this._EnumDefine,value);
                    this._EnumDefine = value;
                    this.SendPropertyChanged("EnumDefine");

                }
            }
        }

String _length;
/// <summary>
/// 长度
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="长度",Storage = "_length",DbType="varchar(50)")]
        public String length
        {
            get
            {
                return this._length;
            }
            set
            {
                if ((this._length != value))
                {
                    this.SendPropertyChanging("length",this._length,value);
                    this._length = value;
                    this.SendPropertyChanged("length");

                }
            }
        }

String _defaultValue;
/// <summary>
/// 默认值
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="默认值",Storage = "_defaultValue",DbType="varchar(200)")]
        public String defaultValue
        {
            get
            {
                return this._defaultValue;
            }
            set
            {
                if ((this._defaultValue != value))
                {
                    this.SendPropertyChanging("defaultValue",this._defaultValue,value);
                    this._defaultValue = value;
                    this.SendPropertyChanged("defaultValue");

                }
            }
        }

System.Nullable<Int32> _TableID;
/// <summary>
/// TableID
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="TableID",Storage = "_TableID",DbType="int")]
        public System.Nullable<Int32> TableID
        {
            get
            {
                return this._TableID;
            }
            set
            {
                if ((this._TableID != value))
                {
                    this.SendPropertyChanging("TableID",this._TableID,value);
                    this._TableID = value;
                    this.SendPropertyChanged("TableID");

                }
            }
        }

System.Nullable<Boolean> _IsPKID=false;
/// <summary>
/// 是否是主键
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="是否是主键",Storage = "_IsPKID",DbType="bit")]
        public System.Nullable<Boolean> IsPKID
        {
            get
            {
                return this._IsPKID;
            }
            set
            {
                if ((this._IsPKID != value))
                {
                    this.SendPropertyChanging("IsPKID",this._IsPKID,value);
                    this._IsPKID = value;
                    this.SendPropertyChanged("IsPKID");

                }
            }
        }

System.Nullable<Int32> _orderid=0;
/// <summary>
/// orderid
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="orderid",Storage = "_orderid",DbType="int")]
        public System.Nullable<Int32> orderid
        {
            get
            {
                return this._orderid;
            }
            set
            {
                if ((this._orderid != value))
                {
                    this.SendPropertyChanging("orderid",this._orderid,value);
                    this._orderid = value;
                    this.SendPropertyChanged("orderid");

                }
            }
        }
}}
namespace EJ{


    /// <summary>
	/// 数据表权限
	/// </summary>
    [Way.EntityDB.Attributes.Table("TablePower","id")]
    public class TablePower :Way.EntityDB.DataItem
    {

/// <summary>
	/// 
	/// </summary>
public  TablePower()
        {
        }


System.Nullable<Int32> _id;
/// <summary>
/// id
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="id",Storage = "_id",DbType="int" ,IsPrimaryKey=true,IsDbGenerated=true)]
        public System.Nullable<Int32> id
        {
            get
            {
                return this._id;
            }
            set
            {
                if ((this._id != value))
                {
                    this.SendPropertyChanging("id",this._id,value);
                    this._id = value;
                    this.SendPropertyChanged("id");

                }
            }
        }

System.Nullable<Int32> _UserID;
/// <summary>
/// UserID
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="UserID",Storage = "_UserID",DbType="int")]
        public System.Nullable<Int32> UserID
        {
            get
            {
                return this._UserID;
            }
            set
            {
                if ((this._UserID != value))
                {
                    this.SendPropertyChanging("UserID",this._UserID,value);
                    this._UserID = value;
                    this.SendPropertyChanged("UserID");

                }
            }
        }

System.Nullable<Int32> _TableID;
/// <summary>
/// TableID
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="TableID",Storage = "_TableID",DbType="int")]
        public System.Nullable<Int32> TableID
        {
            get
            {
                return this._TableID;
            }
            set
            {
                if ((this._TableID != value))
                {
                    this.SendPropertyChanging("TableID",this._TableID,value);
                    this._TableID = value;
                    this.SendPropertyChanged("TableID");

                }
            }
        }
}}
namespace EJ{


    /// <summary>
	/// 项目权限
	/// </summary>
    [Way.EntityDB.Attributes.Table("ProjectPower","id")]
    public class ProjectPower :Way.EntityDB.DataItem
    {

/// <summary>
	/// 
	/// </summary>
public  ProjectPower()
        {
        }


System.Nullable<Int32> _id;
/// <summary>
/// id
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="id",Storage = "_id",DbType="int" ,IsPrimaryKey=true,IsDbGenerated=true)]
        public System.Nullable<Int32> id
        {
            get
            {
                return this._id;
            }
            set
            {
                if ((this._id != value))
                {
                    this.SendPropertyChanging("id",this._id,value);
                    this._id = value;
                    this.SendPropertyChanged("id");

                }
            }
        }

System.Nullable<Int32> _ProjectID;
/// <summary>
/// ProjectID
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="ProjectID",Storage = "_ProjectID",DbType="int")]
        public System.Nullable<Int32> ProjectID
        {
            get
            {
                return this._ProjectID;
            }
            set
            {
                if ((this._ProjectID != value))
                {
                    this.SendPropertyChanging("ProjectID",this._ProjectID,value);
                    this._ProjectID = value;
                    this.SendPropertyChanged("ProjectID");

                }
            }
        }

System.Nullable<Int32> _UserID;
/// <summary>
/// UserID
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="UserID",Storage = "_UserID",DbType="int")]
        public System.Nullable<Int32> UserID
        {
            get
            {
                return this._UserID;
            }
            set
            {
                if ((this._UserID != value))
                {
                    this.SendPropertyChanging("UserID",this._UserID,value);
                    this._UserID = value;
                    this.SendPropertyChanged("UserID");

                }
            }
        }
}}
namespace EJ{


    /// <summary>
	/// 数据模块
	/// </summary>
    [Way.EntityDB.Attributes.Table("DBModule","id")]
    public class DBModule :Way.EntityDB.DataItem
    {

/// <summary>
	/// 
	/// </summary>
public  DBModule()
        {
        }


System.Nullable<Int32> _id;
/// <summary>
/// id
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="id",Storage = "_id",DbType="int" ,IsPrimaryKey=true,IsDbGenerated=true)]
        public System.Nullable<Int32> id
        {
            get
            {
                return this._id;
            }
            set
            {
                if ((this._id != value))
                {
                    this.SendPropertyChanging("id",this._id,value);
                    this._id = value;
                    this.SendPropertyChanged("id");

                }
            }
        }

String _Name;
/// <summary>
/// Name
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="Name",Storage = "_Name",DbType="varchar(50)")]
        public String Name
        {
            get
            {
                return this._Name;
            }
            set
            {
                if ((this._Name != value))
                {
                    this.SendPropertyChanging("Name",this._Name,value);
                    this._Name = value;
                    this.SendPropertyChanged("Name");

                }
            }
        }

System.Nullable<Int32> _DatabaseID;
/// <summary>
/// DatabaseID
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="DatabaseID",Storage = "_DatabaseID",DbType="int")]
        public System.Nullable<Int32> DatabaseID
        {
            get
            {
                return this._DatabaseID;
            }
            set
            {
                if ((this._DatabaseID != value))
                {
                    this.SendPropertyChanging("DatabaseID",this._DatabaseID,value);
                    this._DatabaseID = value;
                    this.SendPropertyChanged("DatabaseID");

                }
            }
        }

System.Nullable<Boolean> _IsFolder=false;
/// <summary>
/// IsFolder
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="IsFolder",Storage = "_IsFolder",DbType="bit")]
        public System.Nullable<Boolean> IsFolder
        {
            get
            {
                return this._IsFolder;
            }
            set
            {
                if ((this._IsFolder != value))
                {
                    this.SendPropertyChanging("IsFolder",this._IsFolder,value);
                    this._IsFolder = value;
                    this.SendPropertyChanged("IsFolder");

                }
            }
        }

System.Nullable<Int32> _parentID;
/// <summary>
/// parentID
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="parentID",Storage = "_parentID",DbType="int")]
        public System.Nullable<Int32> parentID
        {
            get
            {
                return this._parentID;
            }
            set
            {
                if ((this._parentID != value))
                {
                    this.SendPropertyChanging("parentID",this._parentID,value);
                    this._parentID = value;
                    this.SendPropertyChanged("parentID");

                }
            }
        }
}}
namespace EJ{


    /// <summary>
	/// 级联删除
	/// </summary>
    [Way.EntityDB.Attributes.Table("DBDeleteConfig","id")]
    public class DBDeleteConfig :Way.EntityDB.DataItem
    {

/// <summary>
	/// 
	/// </summary>
public  DBDeleteConfig()
        {
        }


System.Nullable<Int32> _id;
/// <summary>
/// id
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="id",Storage = "_id",DbType="int" ,IsPrimaryKey=true,IsDbGenerated=true)]
        public System.Nullable<Int32> id
        {
            get
            {
                return this._id;
            }
            set
            {
                if ((this._id != value))
                {
                    this.SendPropertyChanging("id",this._id,value);
                    this._id = value;
                    this.SendPropertyChanged("id");

                }
            }
        }

System.Nullable<Int32> _TableID;
/// <summary>
/// TableID
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="TableID",Storage = "_TableID",DbType="int")]
        public System.Nullable<Int32> TableID
        {
            get
            {
                return this._TableID;
            }
            set
            {
                if ((this._TableID != value))
                {
                    this.SendPropertyChanging("TableID",this._TableID,value);
                    this._TableID = value;
                    this.SendPropertyChanged("TableID");

                }
            }
        }

System.Nullable<Int32> _RelaTableID;
/// <summary>
/// 关联表ID
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="关联表ID",Storage = "_RelaTableID",DbType="int")]
        public System.Nullable<Int32> RelaTableID
        {
            get
            {
                return this._RelaTableID;
            }
            set
            {
                if ((this._RelaTableID != value))
                {
                    this.SendPropertyChanging("RelaTableID",this._RelaTableID,value);
                    this._RelaTableID = value;
                    this.SendPropertyChanged("RelaTableID");

                }
            }
        }

String _RelaTable_Desc;
/// <summary>
/// RelaTable_Desc
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="RelaTable_Desc",Storage = "_RelaTable_Desc",DbType="varchar(50)")]
        public String RelaTable_Desc
        {
            get
            {
                return this._RelaTable_Desc;
            }
            set
            {
                if ((this._RelaTable_Desc != value))
                {
                    this.SendPropertyChanging("RelaTable_Desc",this._RelaTable_Desc,value);
                    this._RelaTable_Desc = value;
                    this.SendPropertyChanged("RelaTable_Desc");

                }
            }
        }

System.Nullable<Int32> _RelaColumID;
/// <summary>
/// 关联字段的ID
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="关联字段的ID",Storage = "_RelaColumID",DbType="int")]
        public System.Nullable<Int32> RelaColumID
        {
            get
            {
                return this._RelaColumID;
            }
            set
            {
                if ((this._RelaColumID != value))
                {
                    this.SendPropertyChanging("RelaColumID",this._RelaColumID,value);
                    this._RelaColumID = value;
                    this.SendPropertyChanged("RelaColumID");

                }
            }
        }

String _RelaColumn_Desc;
/// <summary>
/// RelaColumn_Desc
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="RelaColumn_Desc",Storage = "_RelaColumn_Desc",DbType="varchar(50)")]
        public String RelaColumn_Desc
        {
            get
            {
                return this._RelaColumn_Desc;
            }
            set
            {
                if ((this._RelaColumn_Desc != value))
                {
                    this.SendPropertyChanging("RelaColumn_Desc",this._RelaColumn_Desc,value);
                    this._RelaColumn_Desc = value;
                    this.SendPropertyChanged("RelaColumn_Desc");

                }
            }
        }
}}
namespace EJ{


    /// <summary>
	/// TableInModule
	/// </summary>
    [Way.EntityDB.Attributes.Table("TableInModule","id")]
    public class TableInModule :Way.EntityDB.DataItem
    {

/// <summary>
	/// 
	/// </summary>
public  TableInModule()
        {
        }


System.Nullable<Int32> _id;
/// <summary>
/// id
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="id",Storage = "_id",DbType="int" ,IsPrimaryKey=true,IsDbGenerated=true)]
        public System.Nullable<Int32> id
        {
            get
            {
                return this._id;
            }
            set
            {
                if ((this._id != value))
                {
                    this.SendPropertyChanging("id",this._id,value);
                    this._id = value;
                    this.SendPropertyChanged("id");

                }
            }
        }

System.Nullable<Int32> _TableID;
/// <summary>
/// TableID
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="TableID",Storage = "_TableID",DbType="int")]
        public System.Nullable<Int32> TableID
        {
            get
            {
                return this._TableID;
            }
            set
            {
                if ((this._TableID != value))
                {
                    this.SendPropertyChanging("TableID",this._TableID,value);
                    this._TableID = value;
                    this.SendPropertyChanged("TableID");

                }
            }
        }

System.Nullable<Int32> _ModuleID;
/// <summary>
/// ModuleID
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="ModuleID",Storage = "_ModuleID",DbType="int")]
        public System.Nullable<Int32> ModuleID
        {
            get
            {
                return this._ModuleID;
            }
            set
            {
                if ((this._ModuleID != value))
                {
                    this.SendPropertyChanging("ModuleID",this._ModuleID,value);
                    this._ModuleID = value;
                    this.SendPropertyChanged("ModuleID");

                }
            }
        }

System.Nullable<Int32> _x;
/// <summary>
/// x
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="x",Storage = "_x",DbType="int")]
        public System.Nullable<Int32> x
        {
            get
            {
                return this._x;
            }
            set
            {
                if ((this._x != value))
                {
                    this.SendPropertyChanging("x",this._x,value);
                    this._x = value;
                    this.SendPropertyChanged("x");

                }
            }
        }

System.Nullable<Int32> _y;
/// <summary>
/// y
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="y",Storage = "_y",DbType="int")]
        public System.Nullable<Int32> y
        {
            get
            {
                return this._y;
            }
            set
            {
                if ((this._y != value))
                {
                    this.SendPropertyChanging("y",this._y,value);
                    this._y = value;
                    this.SendPropertyChanged("y");

                }
            }
        }

String _flag;
/// <summary>
/// 临时变量
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="临时变量",Storage = "_flag",DbType="varchar(50)")]
        public String flag
        {
            get
            {
                return this._flag;
            }
            set
            {
                if ((this._flag != value))
                {
                    this.SendPropertyChanging("flag",this._flag,value);
                    this._flag = value;
                    this.SendPropertyChanged("flag");

                }
            }
        }

String _flag2;
/// <summary>
/// flag2
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="flag2",Storage = "_flag2",DbType="varchar(50)")]
        public String flag2
        {
            get
            {
                return this._flag2;
            }
            set
            {
                if ((this._flag2 != value))
                {
                    this.SendPropertyChanging("flag2",this._flag2,value);
                    this._flag2 = value;
                    this.SendPropertyChanged("flag2");

                }
            }
        }
}}
namespace EJ{


    /// <summary>
	/// 唯一值索引
	/// </summary>
    [Way.EntityDB.Attributes.Table("IDXIndex","id")]
    public class IDXIndex :Way.EntityDB.DataItem
    {

/// <summary>
	/// 
	/// </summary>
public  IDXIndex()
        {
        }


System.Nullable<Int32> _id;
/// <summary>
/// id
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="id",Storage = "_id",DbType="int" ,IsPrimaryKey=true,IsDbGenerated=true)]
        public System.Nullable<Int32> id
        {
            get
            {
                return this._id;
            }
            set
            {
                if ((this._id != value))
                {
                    this.SendPropertyChanging("id",this._id,value);
                    this._id = value;
                    this.SendPropertyChanged("id");

                }
            }
        }

System.Nullable<Int32> _TableID;
/// <summary>
/// TableID
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="TableID",Storage = "_TableID",DbType="int")]
        public System.Nullable<Int32> TableID
        {
            get
            {
                return this._TableID;
            }
            set
            {
                if ((this._TableID != value))
                {
                    this.SendPropertyChanging("TableID",this._TableID,value);
                    this._TableID = value;
                    this.SendPropertyChanged("TableID");

                }
            }
        }

String _Keys;
/// <summary>
/// Keys
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="Keys",Storage = "_Keys",DbType="varchar(100)")]
        public String Keys
        {
            get
            {
                return this._Keys;
            }
            set
            {
                if ((this._Keys != value))
                {
                    this.SendPropertyChanging("Keys",this._Keys,value);
                    this._Keys = value;
                    this.SendPropertyChanged("Keys");

                }
            }
        }

System.Nullable<Boolean> _IsUnique=true;
/// <summary>
/// 是否唯一索引
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="是否唯一索引",Storage = "_IsUnique",DbType="bit")]
        public System.Nullable<Boolean> IsUnique
        {
            get
            {
                return this._IsUnique;
            }
            set
            {
                if ((this._IsUnique != value))
                {
                    this.SendPropertyChanging("IsUnique",this._IsUnique,value);
                    this._IsUnique = value;
                    this.SendPropertyChanged("IsUnique");

                }
            }
        }

System.Nullable<Boolean> _IsClustered=false;
/// <summary>
/// 是否聚焦
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="是否聚焦",Storage = "_IsClustered",DbType="bit")]
        public System.Nullable<Boolean> IsClustered
        {
            get
            {
                return this._IsClustered;
            }
            set
            {
                if ((this._IsClustered != value))
                {
                    this.SendPropertyChanging("IsClustered",this._IsClustered,value);
                    this._IsClustered = value;
                    this.SendPropertyChanged("IsClustered");

                }
            }
        }
}}
namespace EJ{


    /// <summary>
	/// Bug处理历史记录
	/// </summary>
    [Way.EntityDB.Attributes.Table("BugHandleHistory","id")]
    public class BugHandleHistory :Way.EntityDB.DataItem
    {

/// <summary>
	/// 
	/// </summary>
public  BugHandleHistory()
        {
        }


System.Nullable<Int32> _id;
/// <summary>
/// 
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="",Storage = "_id",DbType="int" ,IsPrimaryKey=true,IsDbGenerated=true,CanBeNull=false)]
        public System.Nullable<Int32> id
        {
            get
            {
                return this._id;
            }
            set
            {
                if ((this._id != value))
                {
                    this.SendPropertyChanging("id",this._id,value);
                    this._id = value;
                    this.SendPropertyChanged("id");

                }
            }
        }

System.Nullable<Int32> _BugID;
/// <summary>
/// 
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="",Storage = "_BugID",DbType="int")]
        public System.Nullable<Int32> BugID
        {
            get
            {
                return this._BugID;
            }
            set
            {
                if ((this._BugID != value))
                {
                    this.SendPropertyChanging("BugID",this._BugID,value);
                    this._BugID = value;
                    this.SendPropertyChanged("BugID");

                }
            }
        }

System.Nullable<Int32> _UserID;
/// <summary>
/// 发标者ID
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="发标者ID",Storage = "_UserID",DbType="int")]
        public System.Nullable<Int32> UserID
        {
            get
            {
                return this._UserID;
            }
            set
            {
                if ((this._UserID != value))
                {
                    this.SendPropertyChanging("UserID",this._UserID,value);
                    this._UserID = value;
                    this.SendPropertyChanged("UserID");

                }
            }
        }

Byte[] _content;
/// <summary>
/// 内容
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="内容",Storage = "_content",DbType="image")]
        public Byte[] content
        {
            get
            {
                return this._content;
            }
            set
            {
                if ((this._content != value))
                {
                    this.SendPropertyChanging("content",this._content,value);
                    this._content = value;
                    this.SendPropertyChanged("content");

                }
            }
        }

System.Nullable<DateTime> _SendTime;
/// <summary>
/// 发表时间
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="发表时间",Storage = "_SendTime",DbType="datetime")]
        public System.Nullable<DateTime> SendTime
        {
            get
            {
                return this._SendTime;
            }
            set
            {
                if ((this._SendTime != value))
                {
                    this.SendPropertyChanging("SendTime",this._SendTime,value);
                    this._SendTime = value;
                    this.SendPropertyChanged("SendTime");

                }
            }
        }
}}
namespace EJ{


    /// <summary>
	/// Bug附带截图
	/// </summary>
    [Way.EntityDB.Attributes.Table("BugImages","id")]
    public class BugImages :Way.EntityDB.DataItem
    {

/// <summary>
	/// 
	/// </summary>
public  BugImages()
        {
        }


System.Nullable<Int32> _id;
/// <summary>
/// 
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="",Storage = "_id",DbType="int" ,IsPrimaryKey=true,IsDbGenerated=true,CanBeNull=false)]
        public System.Nullable<Int32> id
        {
            get
            {
                return this._id;
            }
            set
            {
                if ((this._id != value))
                {
                    this.SendPropertyChanging("id",this._id,value);
                    this._id = value;
                    this.SendPropertyChanged("id");

                }
            }
        }

System.Nullable<Int32> _BugID;
/// <summary>
/// 
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="",Storage = "_BugID",DbType="int")]
        public System.Nullable<Int32> BugID
        {
            get
            {
                return this._BugID;
            }
            set
            {
                if ((this._BugID != value))
                {
                    this.SendPropertyChanging("BugID",this._BugID,value);
                    this._BugID = value;
                    this.SendPropertyChanged("BugID");

                }
            }
        }

Byte[] _content;
/// <summary>
/// 
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="",Storage = "_content",DbType="image")]
        public Byte[] content
        {
            get
            {
                return this._content;
            }
            set
            {
                if ((this._content != value))
                {
                    this.SendPropertyChanging("content",this._content,value);
                    this._content = value;
                    this.SendPropertyChanged("content");

                }
            }
        }

System.Nullable<Int32> _orderID;
/// <summary>
/// 排序
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="排序",Storage = "_orderID",DbType="int")]
        public System.Nullable<Int32> orderID
        {
            get
            {
                return this._orderID;
            }
            set
            {
                if ((this._orderID != value))
                {
                    this.SendPropertyChanging("orderID",this._orderID,value);
                    this._orderID = value;
                    this.SendPropertyChanged("orderID");

                }
            }
        }
}}
namespace EJ{


    /// <summary>
	/// 引入的dll
	/// </summary>
    [Way.EntityDB.Attributes.Table("DLLImport","id")]
    public class DLLImport :Way.EntityDB.DataItem
    {

/// <summary>
	/// 
	/// </summary>
public  DLLImport()
        {
        }


System.Nullable<Int32> _id;
/// <summary>
/// 
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="",Storage = "_id",DbType="int" ,IsPrimaryKey=true,IsDbGenerated=true,CanBeNull=false)]
        public System.Nullable<Int32> id
        {
            get
            {
                return this._id;
            }
            set
            {
                if ((this._id != value))
                {
                    this.SendPropertyChanging("id",this._id,value);
                    this._id = value;
                    this.SendPropertyChanged("id");

                }
            }
        }

String _path;
/// <summary>
/// dll文件路径
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="dll文件路径",Storage = "_path",DbType="varchar(200)")]
        public String path
        {
            get
            {
                return this._path;
            }
            set
            {
                if ((this._path != value))
                {
                    this.SendPropertyChanging("path",this._path,value);
                    this._path = value;
                    this.SendPropertyChanged("path");

                }
            }
        }

System.Nullable<Int32> _ProjectID;
/// <summary>
/// 
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="",Storage = "_ProjectID",DbType="int")]
        public System.Nullable<Int32> ProjectID
        {
            get
            {
                return this._ProjectID;
            }
            set
            {
                if ((this._ProjectID != value))
                {
                    this.SendPropertyChanging("ProjectID",this._ProjectID,value);
                    this._ProjectID = value;
                    this.SendPropertyChanged("ProjectID");

                }
            }
        }
}}
namespace EJ{


    /// <summary>
	/// 接口设计的目录结构
	/// </summary>
    [Way.EntityDB.Attributes.Table("InterfaceModule","id")]
    public class InterfaceModule :Way.EntityDB.DataItem
    {

/// <summary>
	/// 
	/// </summary>
public  InterfaceModule()
        {
        }


System.Nullable<Int32> _id;
/// <summary>
/// 
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="",Storage = "_id",DbType="int" ,IsPrimaryKey=true,IsDbGenerated=true,CanBeNull=false)]
        public System.Nullable<Int32> id
        {
            get
            {
                return this._id;
            }
            set
            {
                if ((this._id != value))
                {
                    this.SendPropertyChanging("id",this._id,value);
                    this._id = value;
                    this.SendPropertyChanged("id");

                }
            }
        }

System.Nullable<Int32> _ProjectID;
/// <summary>
/// 
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="",Storage = "_ProjectID",DbType="int")]
        public System.Nullable<Int32> ProjectID
        {
            get
            {
                return this._ProjectID;
            }
            set
            {
                if ((this._ProjectID != value))
                {
                    this.SendPropertyChanging("ProjectID",this._ProjectID,value);
                    this._ProjectID = value;
                    this.SendPropertyChanged("ProjectID");

                }
            }
        }

String _Name;
/// <summary>
/// 
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="",Storage = "_Name",DbType="varchar(50)")]
        public String Name
        {
            get
            {
                return this._Name;
            }
            set
            {
                if ((this._Name != value))
                {
                    this.SendPropertyChanging("Name",this._Name,value);
                    this._Name = value;
                    this.SendPropertyChanged("Name");

                }
            }
        }

System.Nullable<Int32> _ParentID=0;
/// <summary>
/// 
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="",Storage = "_ParentID",DbType="int")]
        public System.Nullable<Int32> ParentID
        {
            get
            {
                return this._ParentID;
            }
            set
            {
                if ((this._ParentID != value))
                {
                    this.SendPropertyChanging("ParentID",this._ParentID,value);
                    this._ParentID = value;
                    this.SendPropertyChanged("ParentID");

                }
            }
        }

System.Nullable<Boolean> _IsFolder=false;
/// <summary>
/// 
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="",Storage = "_IsFolder",DbType="bit")]
        public System.Nullable<Boolean> IsFolder
        {
            get
            {
                return this._IsFolder;
            }
            set
            {
                if ((this._IsFolder != value))
                {
                    this.SendPropertyChanging("IsFolder",this._IsFolder,value);
                    this._IsFolder = value;
                    this.SendPropertyChanged("IsFolder");

                }
            }
        }

System.Nullable<Int32> _LockUserId;
/// <summary>
/// 已经被某人锁定
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="已经被某人锁定",Storage = "_LockUserId",DbType="int")]
        public System.Nullable<Int32> LockUserId
        {
            get
            {
                return this._LockUserId;
            }
            set
            {
                if ((this._LockUserId != value))
                {
                    this.SendPropertyChanging("LockUserId",this._LockUserId,value);
                    this._LockUserId = value;
                    this.SendPropertyChanged("LockUserId");

                }
            }
        }
}}
namespace EJ{


    /// <summary>
	/// 
	/// </summary>
    [Way.EntityDB.Attributes.Table("InterfaceInModule","id")]
    public class InterfaceInModule :Way.EntityDB.DataItem
    {

/// <summary>
	/// 
	/// </summary>
public  InterfaceInModule()
        {
        }


System.Nullable<Int32> _id;
/// <summary>
/// 
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="",Storage = "_id",DbType="int" ,IsPrimaryKey=true,IsDbGenerated=true,CanBeNull=false)]
        public System.Nullable<Int32> id
        {
            get
            {
                return this._id;
            }
            set
            {
                if ((this._id != value))
                {
                    this.SendPropertyChanging("id",this._id,value);
                    this._id = value;
                    this.SendPropertyChanged("id");

                }
            }
        }

System.Nullable<Int32> _ModuleID;
/// <summary>
/// 
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="",Storage = "_ModuleID",DbType="int")]
        public System.Nullable<Int32> ModuleID
        {
            get
            {
                return this._ModuleID;
            }
            set
            {
                if ((this._ModuleID != value))
                {
                    this.SendPropertyChanging("ModuleID",this._ModuleID,value);
                    this._ModuleID = value;
                    this.SendPropertyChanged("ModuleID");

                }
            }
        }

System.Nullable<Int32> _x;
/// <summary>
/// 
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="",Storage = "_x",DbType="int")]
        public System.Nullable<Int32> x
        {
            get
            {
                return this._x;
            }
            set
            {
                if ((this._x != value))
                {
                    this.SendPropertyChanging("x",this._x,value);
                    this._x = value;
                    this.SendPropertyChanged("x");

                }
            }
        }

System.Nullable<Int32> _y;
/// <summary>
/// 
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="",Storage = "_y",DbType="int")]
        public System.Nullable<Int32> y
        {
            get
            {
                return this._y;
            }
            set
            {
                if ((this._y != value))
                {
                    this.SendPropertyChanging("y",this._y,value);
                    this._y = value;
                    this.SendPropertyChanged("y");

                }
            }
        }

String _Type;
/// <summary>
/// 
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="",Storage = "_Type",DbType="varchar(100)")]
        public String Type
        {
            get
            {
                return this._Type;
            }
            set
            {
                if ((this._Type != value))
                {
                    this.SendPropertyChanging("Type",this._Type,value);
                    this._Type = value;
                    this.SendPropertyChanged("Type");

                }
            }
        }

String _JsonData;
/// <summary>
/// 
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="",Storage = "_JsonData",DbType="text")]
        public String JsonData
        {
            get
            {
                return this._JsonData;
            }
            set
            {
                if ((this._JsonData != value))
                {
                    this.SendPropertyChanging("JsonData",this._JsonData,value);
                    this._JsonData = value;
                    this.SendPropertyChanged("JsonData");

                }
            }
        }

System.Nullable<Int32> _width;
/// <summary>
/// 
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="",Storage = "_width",DbType="int")]
        public System.Nullable<Int32> width
        {
            get
            {
                return this._width;
            }
            set
            {
                if ((this._width != value))
                {
                    this.SendPropertyChanging("width",this._width,value);
                    this._width = value;
                    this.SendPropertyChanged("width");

                }
            }
        }

System.Nullable<Int32> _height;
/// <summary>
/// 
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="",Storage = "_height",DbType="int")]
        public System.Nullable<Int32> height
        {
            get
            {
                return this._height;
            }
            set
            {
                if ((this._height != value))
                {
                    this.SendPropertyChanging("height",this._height,value);
                    this._height = value;
                    this.SendPropertyChanged("height");

                }
            }
        }
}}
namespace EJ{


    /// <summary>
	/// InterfaceModule权限设定表
	/// </summary>
    [Way.EntityDB.Attributes.Table("InterfaceModulePower","id")]
    public class InterfaceModulePower :Way.EntityDB.DataItem
    {

/// <summary>
	/// 
	/// </summary>
public  InterfaceModulePower()
        {
        }


System.Nullable<Int32> _id;
/// <summary>
/// 
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="",Storage = "_id",DbType="int" ,IsPrimaryKey=true,IsDbGenerated=true,CanBeNull=false)]
        public System.Nullable<Int32> id
        {
            get
            {
                return this._id;
            }
            set
            {
                if ((this._id != value))
                {
                    this.SendPropertyChanging("id",this._id,value);
                    this._id = value;
                    this.SendPropertyChanged("id");

                }
            }
        }

System.Nullable<Int32> _UserID;
/// <summary>
/// 
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="",Storage = "_UserID",DbType="int")]
        public System.Nullable<Int32> UserID
        {
            get
            {
                return this._UserID;
            }
            set
            {
                if ((this._UserID != value))
                {
                    this.SendPropertyChanging("UserID",this._UserID,value);
                    this._UserID = value;
                    this.SendPropertyChanged("UserID");

                }
            }
        }

System.Nullable<Int32> _ModuleID;
/// <summary>
/// 
	/// </summary>
[Way.EntityDB.WayDBColumnAttribute(Comment="",Caption="",Storage = "_ModuleID",DbType="int")]
        public System.Nullable<Int32> ModuleID
        {
            get
            {
                return this._ModuleID;
            }
            set
            {
                if ((this._ModuleID != value))
                {
                    this.SendPropertyChanging("ModuleID",this._ModuleID,value);
                    this._ModuleID = value;
                    this.SendPropertyChanged("ModuleID");

                }
            }
        }
}}

namespace EJ.DB{
    /// <summary>
	/// 
	/// </summary>
    public class EasyJob : Way.EntityDB.DBContext
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="dbType"></param>
        public EasyJob(string connection, Way.EntityDB.DatabaseType dbType): base(connection, dbType)
        {
            if (!setEvented)
            {
                lock (lockObj)
                {
                    if (!setEvented)
                    {
                        Way.EntityDB.Design.DBUpgrade.Upgrade(this, _designData);
                        setEvented = true;
                        Way.EntityDB.DBContext.BeforeDelete += Database_BeforeDelete;
                    }
                }
            }
        }

static object lockObj = new object();
        static bool setEvented = false;
 

        static void Database_BeforeDelete(object sender, Way.EntityDB.DatabaseModifyEventArg e)
        {
            var db =  sender as EJ.DB.EasyJob;
            if (db == null)
                return;


                if (e.DataItem is EJ.Project)
                {
                    var deletingItem = (EJ.Project)e.DataItem;
                    
    var items0 = (from m in db.Databases
                    where m.ProjectID == deletingItem.id
                    select new EJ.Databases
                    {
                        id = m.id
                    });
while(true)
{
    var data2del = items0.Take(100).ToList();
if(data2del.Count() ==0)
break;
            foreach (var t in data2del)
            {
                db.Delete(t);
            }
}

    var items1 = (from m in db.DLLImport
                    where m.ProjectID == deletingItem.id
                    select new EJ.DLLImport
                    {
                        id = m.id
                    });
while(true)
{
    var data2del = items1.Take(100).ToList();
if(data2del.Count() ==0)
break;
            foreach (var t in data2del)
            {
                db.Delete(t);
            }
}

    var items2 = (from m in db.InterfaceModule
                    where m.ProjectID == deletingItem.id
                    select new EJ.InterfaceModule
                    {
                        id = m.id
                    });
while(true)
{
    var data2del = items2.Take(100).ToList();
if(data2del.Count() ==0)
break;
            foreach (var t in data2del)
            {
                db.Delete(t);
            }
}

    var items3 = (from m in db.ProjectPower
                    where m.ProjectID == deletingItem.id
                    select new EJ.ProjectPower
                    {
                        id = m.id
                    });
while(true)
{
    var data2del = items3.Take(100).ToList();
if(data2del.Count() ==0)
break;
            foreach (var t in data2del)
            {
                db.Delete(t);
            }
}

                }

                if (e.DataItem is EJ.Databases)
                {
                    var deletingItem = (EJ.Databases)e.DataItem;
                    
    var items0 = (from m in db.DBPower
                    where m.DatabaseID == deletingItem.id
                    select new EJ.DBPower
                    {
                        id = m.id
                    });
while(true)
{
    var data2del = items0.Take(100).ToList();
if(data2del.Count() ==0)
break;
            foreach (var t in data2del)
            {
                db.Delete(t);
            }
}

    var items1 = (from m in db.DBTable
                    where m.DatabaseID == deletingItem.id
                    select new EJ.DBTable
                    {
                        id = m.id
                    });
while(true)
{
    var data2del = items1.Take(100).ToList();
if(data2del.Count() ==0)
break;
            foreach (var t in data2del)
            {
                db.Delete(t);
            }
}

    var items2 = (from m in db.DBModule
                    where m.DatabaseID == deletingItem.id
                    select new EJ.DBModule
                    {
                        id = m.id
                    });
while(true)
{
    var data2del = items2.Take(100).ToList();
if(data2del.Count() ==0)
break;
            foreach (var t in data2del)
            {
                db.Delete(t);
            }
}

                }

                if (e.DataItem is EJ.User)
                {
                    var deletingItem = (EJ.User)e.DataItem;
                    
    var items0 = (from m in db.InterfaceModulePower
                    where m.UserID == deletingItem.id
                    select new EJ.InterfaceModulePower
                    {
                        id = m.id
                    });
while(true)
{
    var data2del = items0.Take(100).ToList();
if(data2del.Count() ==0)
break;
            foreach (var t in data2del)
            {
                db.Delete(t);
            }
}

    var items1 = (from m in db.DBPower
                    where m.UserID == deletingItem.id
                    select new EJ.DBPower
                    {
                        id = m.id
                    });
while(true)
{
    var data2del = items1.Take(100).ToList();
if(data2del.Count() ==0)
break;
            foreach (var t in data2del)
            {
                db.Delete(t);
            }
}

    var items2 = (from m in db.TablePower
                    where m.UserID == deletingItem.id
                    select new EJ.TablePower
                    {
                        id = m.id
                    });
while(true)
{
    var data2del = items2.Take(100).ToList();
if(data2del.Count() ==0)
break;
            foreach (var t in data2del)
            {
                db.Delete(t);
            }
}

    var items3 = (from m in db.ProjectPower
                    where m.UserID == deletingItem.id
                    select new EJ.ProjectPower
                    {
                        id = m.id
                    });
while(true)
{
    var data2del = items3.Take(100).ToList();
if(data2del.Count() ==0)
break;
            foreach (var t in data2del)
            {
                db.Delete(t);
            }
}

                }

                if (e.DataItem is EJ.Bug)
                {
                    var deletingItem = (EJ.Bug)e.DataItem;
                    
    var items0 = (from m in db.BugHandleHistory
                    where m.BugID == deletingItem.id
                    select new EJ.BugHandleHistory
                    {
                        id = m.id
                    });
while(true)
{
    var data2del = items0.Take(100).ToList();
if(data2del.Count() ==0)
break;
            foreach (var t in data2del)
            {
                db.Delete(t);
            }
}

    var items1 = (from m in db.BugImages
                    where m.BugID == deletingItem.id
                    select new EJ.BugImages
                    {
                        id = m.id
                    });
while(true)
{
    var data2del = items1.Take(100).ToList();
if(data2del.Count() ==0)
break;
            foreach (var t in data2del)
            {
                db.Delete(t);
            }
}

                }

                if (e.DataItem is EJ.DBTable)
                {
                    var deletingItem = (EJ.DBTable)e.DataItem;
                    
    var items0 = (from m in db.IDXIndex
                    where m.TableID == deletingItem.id
                    select new EJ.IDXIndex
                    {
                        id = m.id
                    });
while(true)
{
    var data2del = items0.Take(100).ToList();
if(data2del.Count() ==0)
break;
            foreach (var t in data2del)
            {
                db.Delete(t);
            }
}

    var items1 = (from m in db.DBDeleteConfig
                    where m.TableID == deletingItem.id
                    select new EJ.DBDeleteConfig
                    {
                        id = m.id
                    });
while(true)
{
    var data2del = items1.Take(100).ToList();
if(data2del.Count() ==0)
break;
            foreach (var t in data2del)
            {
                db.Delete(t);
            }
}

    var items2 = (from m in db.DBDeleteConfig
                    where m.RelaTableID == deletingItem.id
                    select new EJ.DBDeleteConfig
                    {
                        id = m.id
                    });
while(true)
{
    var data2del = items2.Take(100).ToList();
if(data2del.Count() ==0)
break;
            foreach (var t in data2del)
            {
                db.Delete(t);
            }
}

    var items3 = (from m in db.DBColumn
                    where m.TableID == deletingItem.id
                    select new EJ.DBColumn
                    {
                        id = m.id
                    });
while(true)
{
    var data2del = items3.Take(100).ToList();
if(data2del.Count() ==0)
break;
            foreach (var t in data2del)
            {
                db.Delete(t);
            }
}

    var items4 = (from m in db.TableInModule
                    where m.TableID == deletingItem.id
                    select new EJ.TableInModule
                    {
                        id = m.id
                    });
while(true)
{
    var data2del = items4.Take(100).ToList();
if(data2del.Count() ==0)
break;
            foreach (var t in data2del)
            {
                db.Delete(t);
            }
}

    var items5 = (from m in db.TablePower
                    where m.TableID == deletingItem.id
                    select new EJ.TablePower
                    {
                        id = m.id
                    });
while(true)
{
    var data2del = items5.Take(100).ToList();
if(data2del.Count() ==0)
break;
            foreach (var t in data2del)
            {
                db.Delete(t);
            }
}

                }

                if (e.DataItem is EJ.DBColumn)
                {
                    var deletingItem = (EJ.DBColumn)e.DataItem;
                    
    var items0 = (from m in db.DBDeleteConfig
                    where m.RelaColumID == deletingItem.id
                    select new EJ.DBDeleteConfig
                    {
                        id = m.id
                    });
while(true)
{
    var data2del = items0.Take(100).ToList();
if(data2del.Count() ==0)
break;
            foreach (var t in data2del)
            {
                db.Delete(t);
            }
}

                }

                if (e.DataItem is EJ.InterfaceModule)
                {
                    var deletingItem = (EJ.InterfaceModule)e.DataItem;
                    
    var items0 = (from m in db.InterfaceInModule
                    where m.ModuleID == deletingItem.id
                    select new EJ.InterfaceInModule
                    {
                        id = m.id
                    });
while(true)
{
    var data2del = items0.Take(100).ToList();
if(data2del.Count() ==0)
break;
            foreach (var t in data2del)
            {
                db.Delete(t);
            }
}

    var items1 = (from m in db.InterfaceModulePower
                    where m.ModuleID == deletingItem.id
                    select new EJ.InterfaceModulePower
                    {
                        id = m.id
                    });
while(true)
{
    var data2del = items1.Take(100).ToList();
if(data2del.Count() ==0)
break;
            foreach (var t in data2del)
            {
                db.Delete(t);
            }
}

                }

        }

/// <summary>
	/// 
	/// </summary>
 /// <param name="modelBuilder"></param>
         protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
   modelBuilder.Entity<EJ.Project>().HasKey(m => m.id);
modelBuilder.Entity<EJ.Databases>().HasKey(m => m.id);
modelBuilder.Entity<EJ.User>().HasKey(m => m.id);
modelBuilder.Entity<EJ.DBPower>().HasKey(m => m.id);
modelBuilder.Entity<EJ.Bug>().HasKey(m => m.id);
modelBuilder.Entity<EJ.DBTable>().HasKey(m => m.id);
modelBuilder.Entity<EJ.DBColumn>().HasKey(m => m.id);
modelBuilder.Entity<EJ.TablePower>().HasKey(m => m.id);
modelBuilder.Entity<EJ.ProjectPower>().HasKey(m => m.id);
modelBuilder.Entity<EJ.DBModule>().HasKey(m => m.id);
modelBuilder.Entity<EJ.DBDeleteConfig>().HasKey(m => m.id);
modelBuilder.Entity<EJ.TableInModule>().HasKey(m => m.id);
modelBuilder.Entity<EJ.IDXIndex>().HasKey(m => m.id);
modelBuilder.Entity<EJ.BugHandleHistory>().HasKey(m => m.id);
modelBuilder.Entity<EJ.BugImages>().HasKey(m => m.id);
modelBuilder.Entity<EJ.DLLImport>().HasKey(m => m.id);
modelBuilder.Entity<EJ.InterfaceModule>().HasKey(m => m.id);
modelBuilder.Entity<EJ.InterfaceInModule>().HasKey(m => m.id);
modelBuilder.Entity<EJ.InterfaceModulePower>().HasKey(m => m.id);
}

System.Linq.IQueryable<EJ.Project> _Project;
 /// <summary>
        /// 项目
        /// </summary>
        public virtual System.Linq.IQueryable<EJ.Project> Project
        {
             get
            {
                if (_Project == null)
                {
                    _Project = new Way.EntityDB.WayQueryable<EJ.Project>(this.Set<EJ.Project>());
                }
                return _Project;
            }
        }

System.Linq.IQueryable<EJ.Databases> _Databases;
 /// <summary>
        /// 数据库
        /// </summary>
        public virtual System.Linq.IQueryable<EJ.Databases> Databases
        {
             get
            {
                if (_Databases == null)
                {
                    _Databases = new Way.EntityDB.WayQueryable<EJ.Databases>(this.Set<EJ.Databases>());
                }
                return _Databases;
            }
        }

System.Linq.IQueryable<EJ.User> _User;
 /// <summary>
        /// 系统用户
        /// </summary>
        public virtual System.Linq.IQueryable<EJ.User> User
        {
             get
            {
                if (_User == null)
                {
                    _User = new Way.EntityDB.WayQueryable<EJ.User>(this.Set<EJ.User>());
                }
                return _User;
            }
        }

System.Linq.IQueryable<EJ.DBPower> _DBPower;
 /// <summary>
        /// 数据库权限
        /// </summary>
        public virtual System.Linq.IQueryable<EJ.DBPower> DBPower
        {
             get
            {
                if (_DBPower == null)
                {
                    _DBPower = new Way.EntityDB.WayQueryable<EJ.DBPower>(this.Set<EJ.DBPower>());
                }
                return _DBPower;
            }
        }

System.Linq.IQueryable<EJ.Bug> _Bug;
 /// <summary>
        /// 错误报告
        /// </summary>
        public virtual System.Linq.IQueryable<EJ.Bug> Bug
        {
             get
            {
                if (_Bug == null)
                {
                    _Bug = new Way.EntityDB.WayQueryable<EJ.Bug>(this.Set<EJ.Bug>());
                }
                return _Bug;
            }
        }

System.Linq.IQueryable<EJ.DBTable> _DBTable;
 /// <summary>
        /// 数据表
        /// </summary>
        public virtual System.Linq.IQueryable<EJ.DBTable> DBTable
        {
             get
            {
                if (_DBTable == null)
                {
                    _DBTable = new Way.EntityDB.WayQueryable<EJ.DBTable>(this.Set<EJ.DBTable>());
                }
                return _DBTable;
            }
        }

System.Linq.IQueryable<EJ.DBColumn> _DBColumn;
 /// <summary>
        /// 字段
        /// </summary>
        public virtual System.Linq.IQueryable<EJ.DBColumn> DBColumn
        {
             get
            {
                if (_DBColumn == null)
                {
                    _DBColumn = new Way.EntityDB.WayQueryable<EJ.DBColumn>(this.Set<EJ.DBColumn>());
                }
                return _DBColumn;
            }
        }

System.Linq.IQueryable<EJ.TablePower> _TablePower;
 /// <summary>
        /// 数据表权限
        /// </summary>
        public virtual System.Linq.IQueryable<EJ.TablePower> TablePower
        {
             get
            {
                if (_TablePower == null)
                {
                    _TablePower = new Way.EntityDB.WayQueryable<EJ.TablePower>(this.Set<EJ.TablePower>());
                }
                return _TablePower;
            }
        }

System.Linq.IQueryable<EJ.ProjectPower> _ProjectPower;
 /// <summary>
        /// 项目权限
        /// </summary>
        public virtual System.Linq.IQueryable<EJ.ProjectPower> ProjectPower
        {
             get
            {
                if (_ProjectPower == null)
                {
                    _ProjectPower = new Way.EntityDB.WayQueryable<EJ.ProjectPower>(this.Set<EJ.ProjectPower>());
                }
                return _ProjectPower;
            }
        }

System.Linq.IQueryable<EJ.DBModule> _DBModule;
 /// <summary>
        /// 数据模块
        /// </summary>
        public virtual System.Linq.IQueryable<EJ.DBModule> DBModule
        {
             get
            {
                if (_DBModule == null)
                {
                    _DBModule = new Way.EntityDB.WayQueryable<EJ.DBModule>(this.Set<EJ.DBModule>());
                }
                return _DBModule;
            }
        }

System.Linq.IQueryable<EJ.DBDeleteConfig> _DBDeleteConfig;
 /// <summary>
        /// 级联删除
        /// </summary>
        public virtual System.Linq.IQueryable<EJ.DBDeleteConfig> DBDeleteConfig
        {
             get
            {
                if (_DBDeleteConfig == null)
                {
                    _DBDeleteConfig = new Way.EntityDB.WayQueryable<EJ.DBDeleteConfig>(this.Set<EJ.DBDeleteConfig>());
                }
                return _DBDeleteConfig;
            }
        }

System.Linq.IQueryable<EJ.TableInModule> _TableInModule;
 /// <summary>
        /// TableInModule
        /// </summary>
        public virtual System.Linq.IQueryable<EJ.TableInModule> TableInModule
        {
             get
            {
                if (_TableInModule == null)
                {
                    _TableInModule = new Way.EntityDB.WayQueryable<EJ.TableInModule>(this.Set<EJ.TableInModule>());
                }
                return _TableInModule;
            }
        }

System.Linq.IQueryable<EJ.IDXIndex> _IDXIndex;
 /// <summary>
        /// 唯一值索引
        /// </summary>
        public virtual System.Linq.IQueryable<EJ.IDXIndex> IDXIndex
        {
             get
            {
                if (_IDXIndex == null)
                {
                    _IDXIndex = new Way.EntityDB.WayQueryable<EJ.IDXIndex>(this.Set<EJ.IDXIndex>());
                }
                return _IDXIndex;
            }
        }

System.Linq.IQueryable<EJ.BugHandleHistory> _BugHandleHistory;
 /// <summary>
        /// Bug处理历史记录
        /// </summary>
        public virtual System.Linq.IQueryable<EJ.BugHandleHistory> BugHandleHistory
        {
             get
            {
                if (_BugHandleHistory == null)
                {
                    _BugHandleHistory = new Way.EntityDB.WayQueryable<EJ.BugHandleHistory>(this.Set<EJ.BugHandleHistory>());
                }
                return _BugHandleHistory;
            }
        }

System.Linq.IQueryable<EJ.BugImages> _BugImages;
 /// <summary>
        /// Bug附带截图
        /// </summary>
        public virtual System.Linq.IQueryable<EJ.BugImages> BugImages
        {
             get
            {
                if (_BugImages == null)
                {
                    _BugImages = new Way.EntityDB.WayQueryable<EJ.BugImages>(this.Set<EJ.BugImages>());
                }
                return _BugImages;
            }
        }

System.Linq.IQueryable<EJ.DLLImport> _DLLImport;
 /// <summary>
        /// 引入的dll
        /// </summary>
        public virtual System.Linq.IQueryable<EJ.DLLImport> DLLImport
        {
             get
            {
                if (_DLLImport == null)
                {
                    _DLLImport = new Way.EntityDB.WayQueryable<EJ.DLLImport>(this.Set<EJ.DLLImport>());
                }
                return _DLLImport;
            }
        }

System.Linq.IQueryable<EJ.InterfaceModule> _InterfaceModule;
 /// <summary>
        /// 接口设计的目录结构
        /// </summary>
        public virtual System.Linq.IQueryable<EJ.InterfaceModule> InterfaceModule
        {
             get
            {
                if (_InterfaceModule == null)
                {
                    _InterfaceModule = new Way.EntityDB.WayQueryable<EJ.InterfaceModule>(this.Set<EJ.InterfaceModule>());
                }
                return _InterfaceModule;
            }
        }

System.Linq.IQueryable<EJ.InterfaceInModule> _InterfaceInModule;
 /// <summary>
        /// 
        /// </summary>
        public virtual System.Linq.IQueryable<EJ.InterfaceInModule> InterfaceInModule
        {
             get
            {
                if (_InterfaceInModule == null)
                {
                    _InterfaceInModule = new Way.EntityDB.WayQueryable<EJ.InterfaceInModule>(this.Set<EJ.InterfaceInModule>());
                }
                return _InterfaceInModule;
            }
        }

System.Linq.IQueryable<EJ.InterfaceModulePower> _InterfaceModulePower;
 /// <summary>
        /// InterfaceModule权限设定表
        /// </summary>
        public virtual System.Linq.IQueryable<EJ.InterfaceModulePower> InterfaceModulePower
        {
             get
            {
                if (_InterfaceModulePower == null)
                {
                    _InterfaceModulePower = new Way.EntityDB.WayQueryable<EJ.InterfaceModulePower>(this.Set<EJ.InterfaceModulePower>());
                }
                return _InterfaceModulePower;
            }
        }

static string _designData = "eyJUYWJsZXMiOlt7IlRhYmxlTmFtZSI6IlNxbGl0ZSIsIlJvd3MiOlt7Ikl0ZW1zIjpbeyJOYW1lIjoiaWQiLCJWYWx1ZSI6MTU4fSx7Ik5hbWUiOiJ0eXBlIiwiVmFsdWUiOiJDcmVhdGVUYWJsZUFjdGlvbiJ9LHsiTmFtZSI6ImNvbnRlbnQiLCJWYWx1ZSI6IntcIlRhYmxlXCI6e1wiaWRcIjozLFwiY2FwdGlvblwiOlwi6aG555uuXCIsXCJOYW1lXCI6XCJQcm9qZWN0XCIsXCJEYXRhYmFzZUlEXCI6MixcImlMb2NrXCI6MH0sXCJDb2x1bW5zXCI6W3tcImlkXCI6MTAsXCJjYXB0aW9uXCI6XCJpZFwiLFwiTmFtZVwiOlwiaWRcIixcIklzQXV0b0luY3JlbWVudFwiOnRydWUsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJFbnVtRGVmaW5lXCI6XCJcIixcImRlZmF1bHRWYWx1ZVwiOlwiXCIsXCJUYWJsZUlEXCI6MyxcIklzUEtJRFwiOnRydWUsXCJvcmRlcmlkXCI6MH0se1wiaWRcIjoxMSxcImNhcHRpb25cIjpcIk5hbWVcIixcIk5hbWVcIjpcIk5hbWVcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcInZhcmNoYXJcIixcIkVudW1EZWZpbmVcIjpcIlwiLFwiZGVmYXVsdFZhbHVlXCI6XCJcIixcIlRhYmxlSURcIjozLFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MX1dLFwiSURYQ29uZmlnc1wiOltdLFwiSURcIjowfSJ9LHsiTmFtZSI6ImRhdGFiYXNlaWQiLCJWYWx1ZSI6Mn1dLCJSb3dTdGF0ZSI6MH0seyJJdGVtcyI6W3siTmFtZSI6ImlkIiwiVmFsdWUiOjE1OX0seyJOYW1lIjoidHlwZSIsIlZhbHVlIjoiQ3JlYXRlVGFibGVBY3Rpb24ifSx7Ik5hbWUiOiJjb250ZW50IiwiVmFsdWUiOiJ7XCJUYWJsZVwiOntcImlkXCI6NCxcImNhcHRpb25cIjpcIuaVsOaNruW6k1wiLFwiTmFtZVwiOlwiRGF0YWJhc2VzXCIsXCJEYXRhYmFzZUlEXCI6MixcImlMb2NrXCI6MH0sXCJDb2x1bW5zXCI6W3tcImlkXCI6MTIsXCJjYXB0aW9uXCI6XCJpZFwiLFwiTmFtZVwiOlwiaWRcIixcIklzQXV0b0luY3JlbWVudFwiOnRydWUsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJFbnVtRGVmaW5lXCI6XCJcIixcImRlZmF1bHRWYWx1ZVwiOlwiXCIsXCJUYWJsZUlEXCI6NCxcIklzUEtJRFwiOnRydWUsXCJvcmRlcmlkXCI6MH0se1wiaWRcIjoxMyxcImNhcHRpb25cIjpcIumhueebrklEXCIsXCJOYW1lXCI6XCJQcm9qZWN0SURcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwiRW51bURlZmluZVwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIlwiLFwiVGFibGVJRFwiOjQsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoyfSx7XCJpZFwiOjE0LFwiY2FwdGlvblwiOlwiTmFtZVwiLFwiTmFtZVwiOlwiTmFtZVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwiRW51bURlZmluZVwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIlwiLFwiVGFibGVJRFwiOjQsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjozfSx7XCJpZFwiOjE1LFwiY2FwdGlvblwiOlwi5pWw5o2u5bqT57G75Z6LXCIsXCJOYW1lXCI6XCJkYlR5cGVcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwiRW51bURlZmluZVwiOlwiU3FsU2VydmVyID0gMSxcXG5TcWxpdGUgPSAyLFxcbk15U3FsPTNcIixcImRlZmF1bHRWYWx1ZVwiOlwiMVwiLFwiVGFibGVJRFwiOjQsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo0fSx7XCJpZFwiOjE2LFwiY2FwdGlvblwiOlwi6L+e5o6l5a2X56ym5LiyXCIsXCJOYW1lXCI6XCJjb25TdHJcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcInZhcmNoYXJcIixcIkVudW1EZWZpbmVcIjpcIlwiLFwibGVuZ3RoXCI6XCIyMDBcIixcImRlZmF1bHRWYWx1ZVwiOlwiXCIsXCJUYWJsZUlEXCI6NCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjV9LHtcImlkXCI6MTcsXCJjYXB0aW9uXCI6XCJkbGznlJ/miJDmlofku7blpLlcIixcIk5hbWVcIjpcImRsbFBhdGhcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcInZhcmNoYXJcIixcIkVudW1EZWZpbmVcIjpcIlwiLFwibGVuZ3RoXCI6XCIxMDBcIixcImRlZmF1bHRWYWx1ZVwiOlwiXCIsXCJUYWJsZUlEXCI6NCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjZ9LHtcImlkXCI6MTgsXCJjYXB0aW9uXCI6XCJpTG9ja1wiLFwiTmFtZVwiOlwiaUxvY2tcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwiRW51bURlZmluZVwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIjBcIixcIlRhYmxlSURcIjo0LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6N30se1wiaWRcIjoxOSxcImNhcHRpb25cIjpcIk5hbWVTcGFjZVwiLFwiTmFtZVwiOlwiTmFtZVNwYWNlXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJFbnVtRGVmaW5lXCI6XCJcIixcImRlZmF1bHRWYWx1ZVwiOlwiXCIsXCJUYWJsZUlEXCI6NCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjh9LHtcImlkXCI6MTk5LFwiY2FwdGlvblwiOlwi5ZSv5LiA5qCH56S6SURcIixcIk5hbWVcIjpcIkd1aWRcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcInZhcmNoYXJcIixcImxlbmd0aFwiOlwiNTBcIixcIlRhYmxlSURcIjo0LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MX1dLFwiSURYQ29uZmlnc1wiOltdLFwiSURcIjowfSJ9LHsiTmFtZSI6ImRhdGFiYXNlaWQiLCJWYWx1ZSI6Mn1dLCJSb3dTdGF0ZSI6MH0seyJJdGVtcyI6W3siTmFtZSI6ImlkIiwiVmFsdWUiOjE2MH0seyJOYW1lIjoidHlwZSIsIlZhbHVlIjoiQ3JlYXRlVGFibGVBY3Rpb24ifSx7Ik5hbWUiOiJjb250ZW50IiwiVmFsdWUiOiJ7XCJUYWJsZVwiOntcImlkXCI6NSxcImNhcHRpb25cIjpcIuezu+e7n+eUqOaIt1wiLFwiTmFtZVwiOlwiVXNlclwiLFwiRGF0YWJhc2VJRFwiOjIsXCJpTG9ja1wiOjB9LFwiQ29sdW1uc1wiOlt7XCJpZFwiOjIwLFwiY2FwdGlvblwiOlwiaWRcIixcIk5hbWVcIjpcImlkXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjp0cnVlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwiRW51bURlZmluZVwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIlwiLFwiVGFibGVJRFwiOjUsXCJJc1BLSURcIjp0cnVlLFwib3JkZXJpZFwiOjB9LHtcImlkXCI6MjEsXCJjYXB0aW9uXCI6XCLop5LoibJcIixcIk5hbWVcIjpcIlJvbGVcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwiRW51bURlZmluZVwiOlwi5byA5Y+R5Lq65ZGYID0gMSxcXG7lrqLmiLfnq6/mtYvor5XkurrlkZggPSAxPDwxLFxcbuaVsOaNruW6k+iuvuiuoeW4iCA9IDE8PDIgfCDlvIDlj5HkurrlkZgsXFxu566h55CG5ZGYID0g5pWw5o2u5bqT6K6+6K6h5biIIHwgMTw8MyxcXG7pobnnm67nu4/nkIYgPSAxPDw0IHwg5byA5Y+R5Lq65ZGYLFwiLFwiZGVmYXVsdFZhbHVlXCI6XCJcIixcIlRhYmxlSURcIjo1LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MX0se1wiaWRcIjoyMixcImNhcHRpb25cIjpcIk5hbWVcIixcIk5hbWVcIjpcIk5hbWVcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcInZhcmNoYXJcIixcIkVudW1EZWZpbmVcIjpcIlwiLFwiZGVmYXVsdFZhbHVlXCI6XCJcIixcIlRhYmxlSURcIjo1LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6Mn0se1wiaWRcIjoyMyxcImNhcHRpb25cIjpcIlBhc3N3b3JkXCIsXCJOYW1lXCI6XCJQYXNzd29yZFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwiRW51bURlZmluZVwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIlwiLFwiVGFibGVJRFwiOjUsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjozfV0sXCJJRFhDb25maWdzXCI6W10sXCJJRFwiOjB9In0seyJOYW1lIjoiZGF0YWJhc2VpZCIsIlZhbHVlIjoyfV0sIlJvd1N0YXRlIjowfSx7Ikl0ZW1zIjpbeyJOYW1lIjoiaWQiLCJWYWx1ZSI6MTYxfSx7Ik5hbWUiOiJ0eXBlIiwiVmFsdWUiOiJDcmVhdGVUYWJsZUFjdGlvbiJ9LHsiTmFtZSI6ImNvbnRlbnQiLCJWYWx1ZSI6IntcIlRhYmxlXCI6e1wiaWRcIjo2LFwiY2FwdGlvblwiOlwi5pWw5o2u5bqT5p2D6ZmQXCIsXCJOYW1lXCI6XCJEQlBvd2VyXCIsXCJEYXRhYmFzZUlEXCI6MixcImlMb2NrXCI6MH0sXCJDb2x1bW5zXCI6W3tcImlkXCI6MjQsXCJjYXB0aW9uXCI6XCJpZFwiLFwiTmFtZVwiOlwiaWRcIixcIklzQXV0b0luY3JlbWVudFwiOnRydWUsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJFbnVtRGVmaW5lXCI6XCJcIixcImRlZmF1bHRWYWx1ZVwiOlwiXCIsXCJUYWJsZUlEXCI6NixcIklzUEtJRFwiOnRydWUsXCJvcmRlcmlkXCI6MH0se1wiaWRcIjoyNSxcImNhcHRpb25cIjpcIueUqOaIt1wiLFwiTmFtZVwiOlwiVXNlcklEXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJpbnRcIixcIkVudW1EZWZpbmVcIjpcIlwiLFwiZGVmYXVsdFZhbHVlXCI6XCJcIixcIlRhYmxlSURcIjo2LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MX0se1wiaWRcIjoyNixcImNhcHRpb25cIjpcIuadg+mZkFwiLFwiTmFtZVwiOlwiUG93ZXJcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwiRW51bURlZmluZVwiOlwi5Y+q6K+7ID0gMCxcXG7kv67mlLkgPSAxXCIsXCJkZWZhdWx0VmFsdWVcIjpcIlwiLFwiVGFibGVJRFwiOjYsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoyfSx7XCJpZFwiOjI3LFwiY2FwdGlvblwiOlwi5pWw5o2u5bqTSURcIixcIk5hbWVcIjpcIkRhdGFiYXNlSURcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwiRW51bURlZmluZVwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIlwiLFwiVGFibGVJRFwiOjYsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjozfV0sXCJJRFhDb25maWdzXCI6W10sXCJJRFwiOjB9In0seyJOYW1lIjoiZGF0YWJhc2VpZCIsIlZhbHVlIjoyfV0sIlJvd1N0YXRlIjowfSx7Ikl0ZW1zIjpbeyJOYW1lIjoiaWQiLCJWYWx1ZSI6MTYyfSx7Ik5hbWUiOiJ0eXBlIiwiVmFsdWUiOiJDcmVhdGVUYWJsZUFjdGlvbiJ9LHsiTmFtZSI6ImNvbnRlbnQiLCJWYWx1ZSI6IntcIlRhYmxlXCI6e1wiaWRcIjo3LFwiY2FwdGlvblwiOlwi6ZSZ6K+v5oql5ZGKXCIsXCJOYW1lXCI6XCJCdWdcIixcIkRhdGFiYXNlSURcIjoyLFwiaUxvY2tcIjowfSxcIkNvbHVtbnNcIjpbe1wiaWRcIjoyOCxcImNhcHRpb25cIjpcImlkXCIsXCJOYW1lXCI6XCJpZFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6dHJ1ZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJpbnRcIixcIkVudW1EZWZpbmVcIjpcIlwiLFwiZGVmYXVsdFZhbHVlXCI6XCJcIixcIlRhYmxlSURcIjo3LFwiSXNQS0lEXCI6dHJ1ZSxcIm9yZGVyaWRcIjowfSx7XCJpZFwiOjI5LFwiY2FwdGlvblwiOlwi5qCH6aKYXCIsXCJOYW1lXCI6XCJUaXRsZVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwiRW51bURlZmluZVwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIlwiLFwiVGFibGVJRFwiOjcsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoxfSx7XCJpZFwiOjMwLFwiY2FwdGlvblwiOlwi5o+Q5Lqk6ICFSURcIixcIk5hbWVcIjpcIlN1Ym1pdFVzZXJJRFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJFbnVtRGVmaW5lXCI6XCJcIixcImRlZmF1bHRWYWx1ZVwiOlwiXCIsXCJUYWJsZUlEXCI6NyxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjJ9LHtcImlkXCI6MzEsXCJjYXB0aW9uXCI6XCLmj5DkuqTml7bpl7RcIixcIk5hbWVcIjpcIlN1Ym1pdFRpbWVcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImRhdGV0aW1lXCIsXCJFbnVtRGVmaW5lXCI6XCJcIixcImRlZmF1bHRWYWx1ZVwiOlwiXCIsXCJUYWJsZUlEXCI6NyxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjN9LHtcImlkXCI6MzIsXCJjYXB0aW9uXCI6XCLlpITnkIbogIVJRFwiLFwiTmFtZVwiOlwiSGFuZGxlcklEXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJpbnRcIixcIkVudW1EZWZpbmVcIjpcIlwiLFwiZGVmYXVsdFZhbHVlXCI6XCJcIixcIlRhYmxlSURcIjo3LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6NH0se1wiaWRcIjozMyxcImNhcHRpb25cIjpcIuacgOWQjuWPjemmiOaXtumXtFwiLFwiTmFtZVwiOlwiTGFzdERhdGVcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImRhdGV0aW1lXCIsXCJFbnVtRGVmaW5lXCI6XCJcIixcImRlZmF1bHRWYWx1ZVwiOlwiXCIsXCJUYWJsZUlEXCI6NyxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjV9LHtcImlkXCI6MTE4LFwiY2FwdGlvblwiOlwi5aSE55CG5a6M5q+V5pe26Ze0XCIsXCJOYW1lXCI6XCJGaW5pc2hUaW1lXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJkYXRldGltZVwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo3LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6Nn0se1wiaWRcIjoxNDMsXCJjYXB0aW9uXCI6XCLlvZPliY3nirbmgIFcIixcIk5hbWVcIjpcIlN0YXR1c1wiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJFbnVtRGVmaW5lXCI6XCLmj5DkuqTnu5nlvIDlj5HkurrlkZggPSAwLFxcbuWPjemmiOe7meaPkOS6pOiAhSA9IDEsXFxu5aSE55CG5a6M5q+VID0gMlwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo3LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6N31dLFwiSURYQ29uZmlnc1wiOltdLFwiSURcIjowfSJ9LHsiTmFtZSI6ImRhdGFiYXNlaWQiLCJWYWx1ZSI6Mn1dLCJSb3dTdGF0ZSI6MH0seyJJdGVtcyI6W3siTmFtZSI6ImlkIiwiVmFsdWUiOjE2M30seyJOYW1lIjoidHlwZSIsIlZhbHVlIjoiQ3JlYXRlVGFibGVBY3Rpb24ifSx7Ik5hbWUiOiJjb250ZW50IiwiVmFsdWUiOiJ7XCJUYWJsZVwiOntcImlkXCI6OCxcImNhcHRpb25cIjpcIuaVsOaNruihqFwiLFwiTmFtZVwiOlwiREJUYWJsZVwiLFwiRGF0YWJhc2VJRFwiOjIsXCJpTG9ja1wiOjB9LFwiQ29sdW1uc1wiOlt7XCJpZFwiOjM0LFwiY2FwdGlvblwiOlwiaWRcIixcIk5hbWVcIjpcImlkXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjp0cnVlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwiRW51bURlZmluZVwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjp0cnVlLFwib3JkZXJpZFwiOjB9LHtcImlkXCI6MzUsXCJjYXB0aW9uXCI6XCJjYXB0aW9uXCIsXCJOYW1lXCI6XCJjYXB0aW9uXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJFbnVtRGVmaW5lXCI6XCJcIixcImRlZmF1bHRWYWx1ZVwiOlwiXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjF9LHtcImlkXCI6MzYsXCJjYXB0aW9uXCI6XCJOYW1lXCIsXCJOYW1lXCI6XCJOYW1lXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJFbnVtRGVmaW5lXCI6XCJcIixcImRlZmF1bHRWYWx1ZVwiOlwiXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjJ9LHtcImlkXCI6MzcsXCJjYXB0aW9uXCI6XCJEYXRhYmFzZUlEXCIsXCJOYW1lXCI6XCJEYXRhYmFzZUlEXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJpbnRcIixcIkVudW1EZWZpbmVcIjpcIlwiLFwiZGVmYXVsdFZhbHVlXCI6XCJcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6M30se1wiaWRcIjozOCxcImNhcHRpb25cIjpcImlMb2NrXCIsXCJOYW1lXCI6XCJpTG9ja1wiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJFbnVtRGVmaW5lXCI6XCJcIixcImRlZmF1bHRWYWx1ZVwiOlwiMFwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo0fV0sXCJJRFhDb25maWdzXCI6W10sXCJJRFwiOjB9In0seyJOYW1lIjoiZGF0YWJhc2VpZCIsIlZhbHVlIjoyfV0sIlJvd1N0YXRlIjowfSx7Ikl0ZW1zIjpbeyJOYW1lIjoiaWQiLCJWYWx1ZSI6MTY0fSx7Ik5hbWUiOiJ0eXBlIiwiVmFsdWUiOiJDcmVhdGVUYWJsZUFjdGlvbiJ9LHsiTmFtZSI6ImNvbnRlbnQiLCJWYWx1ZSI6IntcIlRhYmxlXCI6e1wiaWRcIjo5LFwiY2FwdGlvblwiOlwi5a2X5q61XCIsXCJOYW1lXCI6XCJEQkNvbHVtblwiLFwiRGF0YWJhc2VJRFwiOjIsXCJpTG9ja1wiOjB9LFwiQ29sdW1uc1wiOlt7XCJpZFwiOjM5LFwiY2FwdGlvblwiOlwiaWRcIixcIk5hbWVcIjpcImlkXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjp0cnVlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwiRW51bURlZmluZVwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIlwiLFwiVGFibGVJRFwiOjksXCJJc1BLSURcIjp0cnVlLFwib3JkZXJpZFwiOjB9LHtcImlkXCI6NDAsXCJjYXB0aW9uXCI6XCJjYXB0aW9uXCIsXCJOYW1lXCI6XCJjYXB0aW9uXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJFbnVtRGVmaW5lXCI6XCJcIixcImxlbmd0aFwiOlwiMjAwXCIsXCJkZWZhdWx0VmFsdWVcIjpcIlwiLFwiVGFibGVJRFwiOjksXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoxfSx7XCJpZFwiOjQxLFwiY2FwdGlvblwiOlwiTmFtZVwiLFwiTmFtZVwiOlwiTmFtZVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwiRW51bURlZmluZVwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIlwiLFwiVGFibGVJRFwiOjksXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoyfSx7XCJpZFwiOjQyLFwiY2FwdGlvblwiOlwi6Ieq5aKe6ZW/XCIsXCJOYW1lXCI6XCJJc0F1dG9JbmNyZW1lbnRcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImJpdFwiLFwiRW51bURlZmluZVwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIjBcIixcIlRhYmxlSURcIjo5LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6M30se1wiaWRcIjo0MyxcImNhcHRpb25cIjpcIuWPr+S7peS4uuepulwiLFwiTmFtZVwiOlwiQ2FuTnVsbFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiYml0XCIsXCJFbnVtRGVmaW5lXCI6XCJcIixcImRlZmF1bHRWYWx1ZVwiOlwiMVwiLFwiVGFibGVJRFwiOjksXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo0fSx7XCJpZFwiOjQ0LFwiY2FwdGlvblwiOlwi5pWw5o2u5bqT5a2X5q6157G75Z6LXCIsXCJOYW1lXCI6XCJkYlR5cGVcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcInZhcmNoYXJcIixcIkVudW1EZWZpbmVcIjpcIlwiLFwiZGVmYXVsdFZhbHVlXCI6XCJcIixcIlRhYmxlSURcIjo5LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6NX0se1wiaWRcIjo0NSxcImNhcHRpb25cIjpcImMj57G75Z6LXCIsXCJOYW1lXCI6XCJUeXBlXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJFbnVtRGVmaW5lXCI6XCJcIixcImRlZmF1bHRWYWx1ZVwiOlwiXCIsXCJUYWJsZUlEXCI6OSxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjZ9LHtcImlkXCI6NDYsXCJjYXB0aW9uXCI6XCJFbnVt5a6a5LmJXCIsXCJOYW1lXCI6XCJFbnVtRGVmaW5lXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJFbnVtRGVmaW5lXCI6XCJcIixcImxlbmd0aFwiOlwiMzAwXCIsXCJkZWZhdWx0VmFsdWVcIjpcIlwiLFwiVGFibGVJRFwiOjksXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo3fSx7XCJpZFwiOjQ3LFwiY2FwdGlvblwiOlwi6ZW/5bqmXCIsXCJOYW1lXCI6XCJsZW5ndGhcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcInZhcmNoYXJcIixcIkVudW1EZWZpbmVcIjpcIlwiLFwiZGVmYXVsdFZhbHVlXCI6XCJcIixcIlRhYmxlSURcIjo5LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6OH0se1wiaWRcIjo0OCxcImNhcHRpb25cIjpcIum7mOiupOWAvFwiLFwiTmFtZVwiOlwiZGVmYXVsdFZhbHVlXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJFbnVtRGVmaW5lXCI6XCJcIixcImxlbmd0aFwiOlwiMjAwXCIsXCJkZWZhdWx0VmFsdWVcIjpcIlwiLFwiVGFibGVJRFwiOjksXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo5fSx7XCJpZFwiOjQ5LFwiY2FwdGlvblwiOlwiVGFibGVJRFwiLFwiTmFtZVwiOlwiVGFibGVJRFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJFbnVtRGVmaW5lXCI6XCJcIixcImRlZmF1bHRWYWx1ZVwiOlwiXCIsXCJUYWJsZUlEXCI6OSxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjEwfSx7XCJpZFwiOjUwLFwiY2FwdGlvblwiOlwi5piv5ZCm5piv5Li76ZSuXCIsXCJOYW1lXCI6XCJJc1BLSURcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImJpdFwiLFwiRW51bURlZmluZVwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIjBcIixcIlRhYmxlSURcIjo5LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MTF9LHtcImlkXCI6NTEsXCJjYXB0aW9uXCI6XCJvcmRlcmlkXCIsXCJOYW1lXCI6XCJvcmRlcmlkXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJpbnRcIixcIkVudW1EZWZpbmVcIjpcIlwiLFwiZGVmYXVsdFZhbHVlXCI6XCIwXCIsXCJUYWJsZUlEXCI6OSxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjEyfV0sXCJJRFhDb25maWdzXCI6W10sXCJJRFwiOjB9In0seyJOYW1lIjoiZGF0YWJhc2VpZCIsIlZhbHVlIjoyfV0sIlJvd1N0YXRlIjowfSx7Ikl0ZW1zIjpbeyJOYW1lIjoiaWQiLCJWYWx1ZSI6MTY1fSx7Ik5hbWUiOiJ0eXBlIiwiVmFsdWUiOiJDcmVhdGVUYWJsZUFjdGlvbiJ9LHsiTmFtZSI6ImNvbnRlbnQiLCJWYWx1ZSI6IntcIlRhYmxlXCI6e1wiaWRcIjoxMCxcImNhcHRpb25cIjpcIuaVsOaNruihqOadg+mZkFwiLFwiTmFtZVwiOlwiVGFibGVQb3dlclwiLFwiRGF0YWJhc2VJRFwiOjIsXCJpTG9ja1wiOjB9LFwiQ29sdW1uc1wiOlt7XCJpZFwiOjUyLFwiY2FwdGlvblwiOlwiaWRcIixcIk5hbWVcIjpcImlkXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjp0cnVlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwiRW51bURlZmluZVwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIlwiLFwiVGFibGVJRFwiOjEwLFwiSXNQS0lEXCI6dHJ1ZSxcIm9yZGVyaWRcIjowfSx7XCJpZFwiOjUzLFwiY2FwdGlvblwiOlwiVXNlcklEXCIsXCJOYW1lXCI6XCJVc2VySURcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwiRW51bURlZmluZVwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIlwiLFwiVGFibGVJRFwiOjEwLFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MX0se1wiaWRcIjo1NCxcImNhcHRpb25cIjpcIlRhYmxlSURcIixcIk5hbWVcIjpcIlRhYmxlSURcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwiRW51bURlZmluZVwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIlwiLFwiVGFibGVJRFwiOjEwLFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6Mn1dLFwiSURYQ29uZmlnc1wiOltdLFwiSURcIjowfSJ9LHsiTmFtZSI6ImRhdGFiYXNlaWQiLCJWYWx1ZSI6Mn1dLCJSb3dTdGF0ZSI6MH0seyJJdGVtcyI6W3siTmFtZSI6ImlkIiwiVmFsdWUiOjE2Nn0seyJOYW1lIjoidHlwZSIsIlZhbHVlIjoiQ3JlYXRlVGFibGVBY3Rpb24ifSx7Ik5hbWUiOiJjb250ZW50IiwiVmFsdWUiOiJ7XCJUYWJsZVwiOntcImlkXCI6MTEsXCJjYXB0aW9uXCI6XCLpobnnm67mnYPpmZBcIixcIk5hbWVcIjpcIlByb2plY3RQb3dlclwiLFwiRGF0YWJhc2VJRFwiOjIsXCJpTG9ja1wiOjB9LFwiQ29sdW1uc1wiOlt7XCJpZFwiOjU1LFwiY2FwdGlvblwiOlwiaWRcIixcIk5hbWVcIjpcImlkXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjp0cnVlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwiRW51bURlZmluZVwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIlwiLFwiVGFibGVJRFwiOjExLFwiSXNQS0lEXCI6dHJ1ZSxcIm9yZGVyaWRcIjowfSx7XCJpZFwiOjU2LFwiY2FwdGlvblwiOlwiUHJvamVjdElEXCIsXCJOYW1lXCI6XCJQcm9qZWN0SURcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwiRW51bURlZmluZVwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIlwiLFwiVGFibGVJRFwiOjExLFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MX0se1wiaWRcIjo1NyxcImNhcHRpb25cIjpcIlVzZXJJRFwiLFwiTmFtZVwiOlwiVXNlcklEXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJpbnRcIixcIkVudW1EZWZpbmVcIjpcIlwiLFwiZGVmYXVsdFZhbHVlXCI6XCJcIixcIlRhYmxlSURcIjoxMSxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjJ9XSxcIklEWENvbmZpZ3NcIjpbXSxcIklEXCI6MH0ifSx7Ik5hbWUiOiJkYXRhYmFzZWlkIiwiVmFsdWUiOjJ9XSwiUm93U3RhdGUiOjB9LHsiSXRlbXMiOlt7Ik5hbWUiOiJpZCIsIlZhbHVlIjoxNjd9LHsiTmFtZSI6InR5cGUiLCJWYWx1ZSI6IkNyZWF0ZVRhYmxlQWN0aW9uIn0seyJOYW1lIjoiY29udGVudCIsIlZhbHVlIjoie1wiVGFibGVcIjp7XCJpZFwiOjEzLFwiY2FwdGlvblwiOlwi5pWw5o2u5qih5Z2XXCIsXCJOYW1lXCI6XCJEQk1vZHVsZVwiLFwiRGF0YWJhc2VJRFwiOjIsXCJpTG9ja1wiOjB9LFwiQ29sdW1uc1wiOlt7XCJpZFwiOjYxLFwiY2FwdGlvblwiOlwiaWRcIixcIk5hbWVcIjpcImlkXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjp0cnVlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwiRW51bURlZmluZVwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIlwiLFwiVGFibGVJRFwiOjEzLFwiSXNQS0lEXCI6dHJ1ZSxcIm9yZGVyaWRcIjowfSx7XCJpZFwiOjYyLFwiY2FwdGlvblwiOlwiTmFtZVwiLFwiTmFtZVwiOlwiTmFtZVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwiRW51bURlZmluZVwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIlwiLFwiVGFibGVJRFwiOjEzLFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MX0se1wiaWRcIjo2MyxcImNhcHRpb25cIjpcIkRhdGFiYXNlSURcIixcIk5hbWVcIjpcIkRhdGFiYXNlSURcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwiRW51bURlZmluZVwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIlwiLFwiVGFibGVJRFwiOjEzLFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6Mn0se1wiaWRcIjo2NCxcImNhcHRpb25cIjpcIklzRm9sZGVyXCIsXCJOYW1lXCI6XCJJc0ZvbGRlclwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiYml0XCIsXCJFbnVtRGVmaW5lXCI6XCJcIixcImRlZmF1bHRWYWx1ZVwiOlwiMFwiLFwiVGFibGVJRFwiOjEzLFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6M30se1wiaWRcIjo2NSxcImNhcHRpb25cIjpcInBhcmVudElEXCIsXCJOYW1lXCI6XCJwYXJlbnRJRFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJFbnVtRGVmaW5lXCI6XCJcIixcImRlZmF1bHRWYWx1ZVwiOlwiXCIsXCJUYWJsZUlEXCI6MTMsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo0fV0sXCJJRFhDb25maWdzXCI6W10sXCJJRFwiOjB9In0seyJOYW1lIjoiZGF0YWJhc2VpZCIsIlZhbHVlIjoyfV0sIlJvd1N0YXRlIjowfSx7Ikl0ZW1zIjpbeyJOYW1lIjoiaWQiLCJWYWx1ZSI6MTY4fSx7Ik5hbWUiOiJ0eXBlIiwiVmFsdWUiOiJDcmVhdGVUYWJsZUFjdGlvbiJ9LHsiTmFtZSI6ImNvbnRlbnQiLCJWYWx1ZSI6IntcIlRhYmxlXCI6e1wiaWRcIjoxNCxcImNhcHRpb25cIjpcIue6p+iBlOWIoOmZpFwiLFwiTmFtZVwiOlwiREJEZWxldGVDb25maWdcIixcIkRhdGFiYXNlSURcIjoyLFwiaUxvY2tcIjowfSxcIkNvbHVtbnNcIjpbe1wiaWRcIjo2NixcImNhcHRpb25cIjpcImlkXCIsXCJOYW1lXCI6XCJpZFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6dHJ1ZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJpbnRcIixcIkVudW1EZWZpbmVcIjpcIlwiLFwiZGVmYXVsdFZhbHVlXCI6XCJcIixcIlRhYmxlSURcIjoxNCxcIklzUEtJRFwiOnRydWUsXCJvcmRlcmlkXCI6MH0se1wiaWRcIjo2NyxcImNhcHRpb25cIjpcIlRhYmxlSURcIixcIk5hbWVcIjpcIlRhYmxlSURcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwiRW51bURlZmluZVwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIlwiLFwiVGFibGVJRFwiOjE0LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MX0se1wiaWRcIjo2OCxcImNhcHRpb25cIjpcIuWFs+iBlOihqElEXCIsXCJOYW1lXCI6XCJSZWxhVGFibGVJRFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJFbnVtRGVmaW5lXCI6XCJcIixcImRlZmF1bHRWYWx1ZVwiOlwiXCIsXCJUYWJsZUlEXCI6MTQsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoyfSx7XCJpZFwiOjY5LFwiY2FwdGlvblwiOlwiUmVsYVRhYmxlX0Rlc2NcIixcIk5hbWVcIjpcIlJlbGFUYWJsZV9EZXNjXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJFbnVtRGVmaW5lXCI6XCJcIixcImRlZmF1bHRWYWx1ZVwiOlwiXCIsXCJUYWJsZUlEXCI6MTQsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjozfSx7XCJpZFwiOjcwLFwiY2FwdGlvblwiOlwi5YWz6IGU5a2X5q6155qESURcIixcIk5hbWVcIjpcIlJlbGFDb2x1bUlEXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJpbnRcIixcIkVudW1EZWZpbmVcIjpcIlwiLFwiZGVmYXVsdFZhbHVlXCI6XCJcIixcIlRhYmxlSURcIjoxNCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjR9LHtcImlkXCI6NzEsXCJjYXB0aW9uXCI6XCJSZWxhQ29sdW1uX0Rlc2NcIixcIk5hbWVcIjpcIlJlbGFDb2x1bW5fRGVzY1wiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwiRW51bURlZmluZVwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIlwiLFwiVGFibGVJRFwiOjE0LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6NX1dLFwiSURYQ29uZmlnc1wiOltdLFwiSURcIjowfSJ9LHsiTmFtZSI6ImRhdGFiYXNlaWQiLCJWYWx1ZSI6Mn1dLCJSb3dTdGF0ZSI6MH0seyJJdGVtcyI6W3siTmFtZSI6ImlkIiwiVmFsdWUiOjE2OX0seyJOYW1lIjoidHlwZSIsIlZhbHVlIjoiQ3JlYXRlVGFibGVBY3Rpb24ifSx7Ik5hbWUiOiJjb250ZW50IiwiVmFsdWUiOiJ7XCJUYWJsZVwiOntcImlkXCI6MTUsXCJjYXB0aW9uXCI6XCJUYWJsZUluTW9kdWxlXCIsXCJOYW1lXCI6XCJUYWJsZUluTW9kdWxlXCIsXCJEYXRhYmFzZUlEXCI6MixcImlMb2NrXCI6MH0sXCJDb2x1bW5zXCI6W3tcImlkXCI6NzIsXCJjYXB0aW9uXCI6XCJpZFwiLFwiTmFtZVwiOlwiaWRcIixcIklzQXV0b0luY3JlbWVudFwiOnRydWUsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJFbnVtRGVmaW5lXCI6XCJcIixcImRlZmF1bHRWYWx1ZVwiOlwiXCIsXCJUYWJsZUlEXCI6MTUsXCJJc1BLSURcIjp0cnVlLFwib3JkZXJpZFwiOjB9LHtcImlkXCI6NzMsXCJjYXB0aW9uXCI6XCJUYWJsZUlEXCIsXCJOYW1lXCI6XCJUYWJsZUlEXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJpbnRcIixcIkVudW1EZWZpbmVcIjpcIlwiLFwiZGVmYXVsdFZhbHVlXCI6XCJcIixcIlRhYmxlSURcIjoxNSxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjF9LHtcImlkXCI6NzQsXCJjYXB0aW9uXCI6XCJNb2R1bGVJRFwiLFwiTmFtZVwiOlwiTW9kdWxlSURcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwiRW51bURlZmluZVwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIlwiLFwiVGFibGVJRFwiOjE1LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6Mn0se1wiaWRcIjo3NSxcImNhcHRpb25cIjpcInhcIixcIk5hbWVcIjpcInhcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwiRW51bURlZmluZVwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIlwiLFwiVGFibGVJRFwiOjE1LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6M30se1wiaWRcIjo3NixcImNhcHRpb25cIjpcInlcIixcIk5hbWVcIjpcInlcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwiRW51bURlZmluZVwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIlwiLFwiVGFibGVJRFwiOjE1LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6NH0se1wiaWRcIjo3NyxcImNhcHRpb25cIjpcIuS4tOaXtuWPmOmHj1wiLFwiTmFtZVwiOlwiZmxhZ1wiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwiRW51bURlZmluZVwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIlwiLFwiVGFibGVJRFwiOjE1LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6NX0se1wiaWRcIjo3OCxcImNhcHRpb25cIjpcImZsYWcyXCIsXCJOYW1lXCI6XCJmbGFnMlwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwiRW51bURlZmluZVwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIlwiLFwiVGFibGVJRFwiOjE1LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6Nn1dLFwiSURYQ29uZmlnc1wiOltdLFwiSURcIjowfSJ9LHsiTmFtZSI6ImRhdGFiYXNlaWQiLCJWYWx1ZSI6Mn1dLCJSb3dTdGF0ZSI6MH0seyJJdGVtcyI6W3siTmFtZSI6ImlkIiwiVmFsdWUiOjE3MH0seyJOYW1lIjoidHlwZSIsIlZhbHVlIjoiQ3JlYXRlVGFibGVBY3Rpb24ifSx7Ik5hbWUiOiJjb250ZW50IiwiVmFsdWUiOiJ7XCJUYWJsZVwiOntcImlkXCI6MTYsXCJjYXB0aW9uXCI6XCLllK/kuIDlgLzntKLlvJVcIixcIk5hbWVcIjpcIklEWEluZGV4XCIsXCJEYXRhYmFzZUlEXCI6MixcImlMb2NrXCI6MH0sXCJDb2x1bW5zXCI6W3tcImlkXCI6NzksXCJjYXB0aW9uXCI6XCJpZFwiLFwiTmFtZVwiOlwiaWRcIixcIklzQXV0b0luY3JlbWVudFwiOnRydWUsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJFbnVtRGVmaW5lXCI6XCJcIixcImRlZmF1bHRWYWx1ZVwiOlwiXCIsXCJUYWJsZUlEXCI6MTYsXCJJc1BLSURcIjp0cnVlLFwib3JkZXJpZFwiOjB9LHtcImlkXCI6ODAsXCJjYXB0aW9uXCI6XCJUYWJsZUlEXCIsXCJOYW1lXCI6XCJUYWJsZUlEXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJpbnRcIixcIkVudW1EZWZpbmVcIjpcIlwiLFwiZGVmYXVsdFZhbHVlXCI6XCJcIixcIlRhYmxlSURcIjoxNixcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjF9LHtcImlkXCI6ODEsXCJjYXB0aW9uXCI6XCJLZXlzXCIsXCJOYW1lXCI6XCJLZXlzXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJFbnVtRGVmaW5lXCI6XCJcIixcImxlbmd0aFwiOlwiMTAwXCIsXCJkZWZhdWx0VmFsdWVcIjpcIlwiLFwiVGFibGVJRFwiOjE2LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6Mn0se1wiaWRcIjoxNjQsXCJjYXB0aW9uXCI6XCLmmK/lkKbllK/kuIDntKLlvJVcIixcIk5hbWVcIjpcIklzVW5pcXVlXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJiaXRcIixcImxlbmd0aFwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIjFcIixcIlRhYmxlSURcIjoxNixcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjN9LHtcImlkXCI6MTY1LFwiY2FwdGlvblwiOlwi5piv5ZCm6IGa54SmXCIsXCJOYW1lXCI6XCJJc0NsdXN0ZXJlZFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiYml0XCIsXCJsZW5ndGhcIjpcIlwiLFwiZGVmYXVsdFZhbHVlXCI6XCIwXCIsXCJUYWJsZUlEXCI6MTYsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo0fV0sXCJJRFhDb25maWdzXCI6W10sXCJJRFwiOjB9In0seyJOYW1lIjoiZGF0YWJhc2VpZCIsIlZhbHVlIjoyfV0sIlJvd1N0YXRlIjowfSx7Ikl0ZW1zIjpbeyJOYW1lIjoiaWQiLCJWYWx1ZSI6MTcxfSx7Ik5hbWUiOiJ0eXBlIiwiVmFsdWUiOiJDcmVhdGVUYWJsZUFjdGlvbiJ9LHsiTmFtZSI6ImNvbnRlbnQiLCJWYWx1ZSI6IntcIlRhYmxlXCI6e1wiaWRcIjoyNSxcImNhcHRpb25cIjpcIkJ1Z+WkhOeQhuWOhuWPsuiusOW9lVwiLFwiTmFtZVwiOlwiQnVnSGFuZGxlSGlzdG9yeVwiLFwiRGF0YWJhc2VJRFwiOjIsXCJpTG9ja1wiOjB9LFwiQ29sdW1uc1wiOlt7XCJpZFwiOjEwNyxcIk5hbWVcIjpcImlkXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjp0cnVlLFwiQ2FuTnVsbFwiOmZhbHNlLFwiZGJUeXBlXCI6XCJpbnRcIixcIlRhYmxlSURcIjoyNSxcIklzUEtJRFwiOnRydWUsXCJvcmRlcmlkXCI6MH0se1wiaWRcIjoxMTMsXCJOYW1lXCI6XCJCdWdJRFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjI1LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MX0se1wiaWRcIjoxNDQsXCJjYXB0aW9uXCI6XCLlj5HmoIfogIVJRFwiLFwiTmFtZVwiOlwiVXNlcklEXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJpbnRcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6MjUsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoyfSx7XCJpZFwiOjE0NSxcImNhcHRpb25cIjpcIuWGheWuuVwiLFwiTmFtZVwiOlwiY29udGVudFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW1hZ2VcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6MjUsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjozfSx7XCJpZFwiOjE0NixcImNhcHRpb25cIjpcIuWPkeihqOaXtumXtFwiLFwiTmFtZVwiOlwiU2VuZFRpbWVcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImRhdGV0aW1lXCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjI1LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6NH1dLFwiSURYQ29uZmlnc1wiOltdLFwiSURcIjowfSJ9LHsiTmFtZSI6ImRhdGFiYXNlaWQiLCJWYWx1ZSI6Mn1dLCJSb3dTdGF0ZSI6MH0seyJJdGVtcyI6W3siTmFtZSI6ImlkIiwiVmFsdWUiOjE3Mn0seyJOYW1lIjoidHlwZSIsIlZhbHVlIjoiQ3JlYXRlVGFibGVBY3Rpb24ifSx7Ik5hbWUiOiJjb250ZW50IiwiVmFsdWUiOiJ7XCJUYWJsZVwiOntcImlkXCI6MjYsXCJjYXB0aW9uXCI6XCJCdWfpmYTluKbmiKrlm75cIixcIk5hbWVcIjpcIkJ1Z0ltYWdlc1wiLFwiRGF0YWJhc2VJRFwiOjIsXCJpTG9ja1wiOjB9LFwiQ29sdW1uc1wiOlt7XCJpZFwiOjExNCxcIk5hbWVcIjpcImlkXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjp0cnVlLFwiQ2FuTnVsbFwiOmZhbHNlLFwiZGJUeXBlXCI6XCJpbnRcIixcIlRhYmxlSURcIjoyNixcIklzUEtJRFwiOnRydWUsXCJvcmRlcmlkXCI6MH0se1wiaWRcIjoxMTUsXCJOYW1lXCI6XCJCdWdJRFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjI2LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MX0se1wiaWRcIjoxMTYsXCJOYW1lXCI6XCJjb250ZW50XCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJpbWFnZVwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjoyNixcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjJ9LHtcImlkXCI6MTE3LFwiY2FwdGlvblwiOlwi5o6S5bqPXCIsXCJOYW1lXCI6XCJvcmRlcklEXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJpbnRcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6MjYsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjozfV0sXCJJRFhDb25maWdzXCI6W10sXCJJRFwiOjB9In0seyJOYW1lIjoiZGF0YWJhc2VpZCIsIlZhbHVlIjoyfV0sIlJvd1N0YXRlIjowfSx7Ikl0ZW1zIjpbeyJOYW1lIjoiaWQiLCJWYWx1ZSI6MTczfSx7Ik5hbWUiOiJ0eXBlIiwiVmFsdWUiOiJDcmVhdGVUYWJsZUFjdGlvbiJ9LHsiTmFtZSI6ImNvbnRlbnQiLCJWYWx1ZSI6IntcIlRhYmxlXCI6e1wiaWRcIjoyNyxcImNhcHRpb25cIjpcIuW8leWFpeeahGRsbFwiLFwiTmFtZVwiOlwiRExMSW1wb3J0XCIsXCJEYXRhYmFzZUlEXCI6MixcImlMb2NrXCI6MH0sXCJDb2x1bW5zXCI6W3tcImlkXCI6MTE5LFwiTmFtZVwiOlwiaWRcIixcIklzQXV0b0luY3JlbWVudFwiOnRydWUsXCJDYW5OdWxsXCI6ZmFsc2UsXCJkYlR5cGVcIjpcImludFwiLFwiVGFibGVJRFwiOjI3LFwiSXNQS0lEXCI6dHJ1ZSxcIm9yZGVyaWRcIjowfSx7XCJpZFwiOjEyMCxcImNhcHRpb25cIjpcImRsbOaWh+S7tui3r+W+hFwiLFwiTmFtZVwiOlwicGF0aFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCIyMDBcIixcIlRhYmxlSURcIjoyNyxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjF9LHtcImlkXCI6MTIxLFwiTmFtZVwiOlwiUHJvamVjdElEXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJpbnRcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6MjcsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoyfV0sXCJJRFhDb25maWdzXCI6W10sXCJJRFwiOjB9In0seyJOYW1lIjoiZGF0YWJhc2VpZCIsIlZhbHVlIjoyfV0sIlJvd1N0YXRlIjowfSx7Ikl0ZW1zIjpbeyJOYW1lIjoiaWQiLCJWYWx1ZSI6MTc0fSx7Ik5hbWUiOiJ0eXBlIiwiVmFsdWUiOiJDcmVhdGVUYWJsZUFjdGlvbiJ9LHsiTmFtZSI6ImNvbnRlbnQiLCJWYWx1ZSI6IntcIlRhYmxlXCI6e1wiaWRcIjoyOCxcImNhcHRpb25cIjpcIuaOpeWPo+iuvuiuoeeahOebruW9lee7k+aehFwiLFwiTmFtZVwiOlwiSW50ZXJmYWNlTW9kdWxlXCIsXCJEYXRhYmFzZUlEXCI6MixcImlMb2NrXCI6MH0sXCJDb2x1bW5zXCI6W3tcImlkXCI6MTIyLFwiTmFtZVwiOlwiaWRcIixcIklzQXV0b0luY3JlbWVudFwiOnRydWUsXCJDYW5OdWxsXCI6ZmFsc2UsXCJkYlR5cGVcIjpcImludFwiLFwiVGFibGVJRFwiOjI4LFwiSXNQS0lEXCI6dHJ1ZSxcIm9yZGVyaWRcIjowfSx7XCJpZFwiOjEyMyxcIk5hbWVcIjpcIlByb2plY3RJRFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjI4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MX0se1wiaWRcIjoxMjQsXCJOYW1lXCI6XCJOYW1lXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjUwXCIsXCJUYWJsZUlEXCI6MjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoyfSx7XCJpZFwiOjEyNSxcIk5hbWVcIjpcIlBhcmVudElEXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJpbnRcIixcImxlbmd0aFwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIjBcIixcIlRhYmxlSURcIjoyOCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjN9LHtcImlkXCI6MTMyLFwiTmFtZVwiOlwiSXNGb2xkZXJcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImJpdFwiLFwibGVuZ3RoXCI6XCJcIixcImRlZmF1bHRWYWx1ZVwiOlwiMFwiLFwiVGFibGVJRFwiOjI4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6NH0se1wiaWRcIjoxMzgsXCJjYXB0aW9uXCI6XCLlt7Lnu4/ooqvmn5DkurrplIHlrppcIixcIk5hbWVcIjpcIkxvY2tVc2VySWRcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjoyOCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjV9XSxcIklEWENvbmZpZ3NcIjpbXSxcIklEXCI6MH0ifSx7Ik5hbWUiOiJkYXRhYmFzZWlkIiwiVmFsdWUiOjJ9XSwiUm93U3RhdGUiOjB9LHsiSXRlbXMiOlt7Ik5hbWUiOiJpZCIsIlZhbHVlIjoxNzV9LHsiTmFtZSI6InR5cGUiLCJWYWx1ZSI6IkNyZWF0ZVRhYmxlQWN0aW9uIn0seyJOYW1lIjoiY29udGVudCIsIlZhbHVlIjoie1wiVGFibGVcIjp7XCJpZFwiOjI5LFwiTmFtZVwiOlwiSW50ZXJmYWNlSW5Nb2R1bGVcIixcIkRhdGFiYXNlSURcIjoyLFwiaUxvY2tcIjowfSxcIkNvbHVtbnNcIjpbe1wiaWRcIjoxMjYsXCJOYW1lXCI6XCJpZFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6dHJ1ZSxcIkNhbk51bGxcIjpmYWxzZSxcImRiVHlwZVwiOlwiaW50XCIsXCJUYWJsZUlEXCI6MjksXCJJc1BLSURcIjp0cnVlLFwib3JkZXJpZFwiOjB9LHtcImlkXCI6MTI3LFwiTmFtZVwiOlwiTW9kdWxlSURcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjoyOSxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjF9LHtcImlkXCI6MTI4LFwiTmFtZVwiOlwieFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjI5LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6Mn0se1wiaWRcIjoxMjksXCJOYW1lXCI6XCJ5XCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJpbnRcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6MjksXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjozfSx7XCJpZFwiOjEzMCxcIk5hbWVcIjpcIlR5cGVcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcInZhcmNoYXJcIixcImxlbmd0aFwiOlwiMTAwXCIsXCJUYWJsZUlEXCI6MjksXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo0fSx7XCJpZFwiOjEzMSxcIk5hbWVcIjpcIkpzb25EYXRhXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ0ZXh0XCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjI5LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6NX0se1wiaWRcIjoxMzMsXCJOYW1lXCI6XCJ3aWR0aFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjI5LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6Nn0se1wiaWRcIjoxMzQsXCJOYW1lXCI6XCJoZWlnaHRcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjoyOSxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjd9XSxcIklEWENvbmZpZ3NcIjpbXSxcIklEXCI6MH0ifSx7Ik5hbWUiOiJkYXRhYmFzZWlkIiwiVmFsdWUiOjJ9XSwiUm93U3RhdGUiOjB9LHsiSXRlbXMiOlt7Ik5hbWUiOiJpZCIsIlZhbHVlIjoxNzZ9LHsiTmFtZSI6InR5cGUiLCJWYWx1ZSI6IkNyZWF0ZVRhYmxlQWN0aW9uIn0seyJOYW1lIjoiY29udGVudCIsIlZhbHVlIjoie1wiVGFibGVcIjp7XCJpZFwiOjMwLFwiY2FwdGlvblwiOlwiSW50ZXJmYWNlTW9kdWxl5p2D6ZmQ6K6+5a6a6KGoXCIsXCJOYW1lXCI6XCJJbnRlcmZhY2VNb2R1bGVQb3dlclwiLFwiRGF0YWJhc2VJRFwiOjIsXCJpTG9ja1wiOjB9LFwiQ29sdW1uc1wiOlt7XCJpZFwiOjEzNSxcIk5hbWVcIjpcImlkXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjp0cnVlLFwiQ2FuTnVsbFwiOmZhbHNlLFwiZGJUeXBlXCI6XCJpbnRcIixcIlRhYmxlSURcIjozMCxcIklzUEtJRFwiOnRydWUsXCJvcmRlcmlkXCI6MH0se1wiaWRcIjoxMzYsXCJOYW1lXCI6XCJVc2VySURcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjozMCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjF9LHtcImlkXCI6MTM3LFwiTmFtZVwiOlwiTW9kdWxlSURcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjozMCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjJ9XSxcIklEWENvbmZpZ3NcIjpbXSxcIklEXCI6MH0ifSx7Ik5hbWUiOiJkYXRhYmFzZWlkIiwiVmFsdWUiOjJ9XSwiUm93U3RhdGUiOjB9XSwiQ29sdW1ucyI6W3siQ29sdW1uTmFtZSI6ImlkIiwiRGF0YVR5cGUiOiJTeXN0ZW0uSW50NjQifSx7IkNvbHVtbk5hbWUiOiJ0eXBlIiwiRGF0YVR5cGUiOiJTeXN0ZW0uU3RyaW5nIn0seyJDb2x1bW5OYW1lIjoiY29udGVudCIsIkRhdGFUeXBlIjoiU3lzdGVtLlN0cmluZyJ9LHsiQ29sdW1uTmFtZSI6ImRhdGFiYXNlaWQiLCJEYXRhVHlwZSI6IlN5c3RlbS5JbnQ2NCJ9XX1dLCJEYXRhU2V0TmFtZSI6IjdGNEM0QjYwLTU2QkItNEI4Mi1BOTMxLTkzMDBCNDA3NUE3NiJ9";}}
