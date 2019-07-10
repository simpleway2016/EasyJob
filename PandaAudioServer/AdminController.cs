﻿using System;
using System.Collections.Generic;
using System.Text;
using Way.Lib.ScriptRemoting;
using System.Linq;

namespace PandaAudioServer
{
    public class AdminController : BaseController
    {
        public class RegInfo:SysDB.SystemRegCode
        {
            public string MakerName { get; set; }
            public bool Used { get; set; }
            public string UserDesc { get; set; }
        }

        public SysDB.AdminUser_RoleEnum UserRole
        {
            get
            {
                return (SysDB.AdminUser_RoleEnum)this.Session["AdminUserRole"];
            }
        }

        public override int UserId
        {
            get
            {
                if (this.Session["AdminUserId"] == null)
                    throw new Exception("请先登录");
                return (int)this.Session["AdminUserId"];
            }
        }

        public IQueryable<object> RegCodes
        {
            get
            {
                if (this.UserId == 0)
                    return null;
                if (this.UserRole == SysDB.AdminUser_RoleEnum.普通员工)
                {
                    return from m in this.db.SystemRegCode
                           where m.MakerUserId == this.UserId
                           orderby m.UserId
                           select new RegInfo
                           {
                               id = m.id,
                               RegGuid = m.RegGuid,
                               MakerName = (from u in db.AdminUser where u.id == m.MakerUserId select u.UserName).FirstOrDefault(),
                               MakeTime = m.MakeTime,
                               Used = m.UserId > 0,
                               UserDesc = (from u in db.UserInfo where u.id == m.UserId select u.PhoneNumber).FirstOrDefault()
                           };
                }
                else
                {
                    return from m in this.db.SystemRegCode
                           orderby m.UserId
                           select new RegInfo
                           {
                               id = m.id,
                               RegGuid = m.RegGuid,
                               MakerName = (from u in db.AdminUser where u.id == m.MakerUserId select u.UserName).FirstOrDefault(),
                               MakeTime = m.MakeTime,
                               Used = m.UserId > 0,
                               UserDesc = (from u in db.UserInfo where u.id == m.UserId select u.PhoneNumber).FirstOrDefault()
                           };
                }
            }
        }

        /// <summary>
        /// 生成注册码
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        [RemotingMethod]
        public bool MakeRegCode(int count)
        {
            SysDB.AdminUser user = null;
            if(this.UserRole == SysDB.AdminUser_RoleEnum.普通员工)
            {
                user = this.db.AdminUser.FirstOrDefault(m => m.id == this.UserId);
                if (user.CreatedCount + count > user.MaxCount)
                    throw new Exception("你最多只能创建" + user.MaxCount + "个注册码，你已经创建了"+user.CreatedCount+"个");
            }
            for(int i = 0; i < count; i ++)
            {
                var regitem = new SysDB.SystemRegCode()
                {
                    MakerUserId = this.UserId,
                    MakeTime = DateTime.Now,
                    RegGuid = Guid.NewGuid().ToString("N"),
                };
                this.db.Insert(regitem);

                if(user != null)
                {
                    user.CreatedCount += count;
                    this.db.Update(user);
                }
            }
            return true;
        }


        /// <summary>
        /// 发送手机验证码
        /// </summary>
        /// <param name="phoneNumber"></param>
        [RemotingMethod]
        public bool SendPhoneVerifyCode(string phoneNumber)
        {
            string ip = this.Request.RemoteEndPoint.ToString().Split(':')[0];
            if (IPError.IsIPLocked(ip))
                throw new Exception("禁止访问");

            if (this.db.AdminUser.Any(m => m.PhoneNumber == phoneNumber) == false)
            {
                var iperror = IPError.GetInstance(ip);
                this.Session["iperror"] = iperror;
                iperror.MarkError();
                throw new Exception("手机号错误");
            }
            else
            {
                if (this.Session["iperror"] != null)
                {
                    ((IPError)this.Session["iperror"]).Clear();
                    this.Session.Remove("iperror");
                }
                var sms = Factory.GetService<ISms>();
                this.Session["admin_phoneCode"] = new Random().Next(1000, 9999).ToString();
                var msg = sms.Format("{0}", this.Session["admin_phoneCode"].ToString());
                sms.Send(phoneNumber, msg);
            }
            return true;
        }

        [RemotingMethod]
        public bool Login(string phoneNumber, string password,string verifyCode)
        {
            string ip = this.Request.RemoteEndPoint.ToString().Split(':')[0];
            if (IPError.IsIPLocked(ip))
                throw new Exception("禁止访问");
#if DEBUG
#else
            if (string.IsNullOrEmpty(verifyCode) || (string)this.Session["admin_phoneCode"] != verifyCode)
            {
                var iperror = IPError.GetInstance(ip);
                iperror.MarkError();
                this.Session["iperror"] = iperror;
                if (iperror.ErrorCount < 7)
                    throw new Exception("验证码不正确");
                else
                    throw new Exception($"验证码不正确，剩余{iperror.GetChance()}次机会！");
            }
#endif

            var user = this.db.AdminUser.FirstOrDefault(m => m.PhoneNumber == phoneNumber && m.Password == password);
            if (user == null)
            {
                var iperror = IPError.GetInstance(ip);
                iperror.MarkError();
                this.Session["iperror"] = iperror;
                if (iperror.ErrorCount < 7)
                    throw new Exception("用户名或密码错误");
                else
                    throw new Exception($"用户名或密码错误，剩余{iperror.GetChance()}次机会！");
            }

            if (this.Session["iperror"] != null)
            {
                ((IPError)this.Session["iperror"]).Clear();
                this.Session.Remove("iperror");
            }

            this.Session["AdminUserId"] = user.id;
            this.Session["AdminUserRole"] = user.Role;
            return true;
        }

        [RemotingMethod]
        public int GetUserCount()
        {
            return this.db.UserInfo.Count();
        }

        [RemotingMethod]
        public int GetOnlineUserCount()
        {
            DateTime time = DateTime.Now.AddSeconds(-20);
            return this.db.UserInfo.Where(m=>m.LastCheckTime > time).Count();
        }

        /// <summary>
        /// 注销注册码
        /// </summary>
        [RemotingMethod]
        public void DestoryGuid(string guid)
        {
            var item = this.db.SystemRegCode.FirstOrDefault(m => m.RegGuid == guid);
            if (item != null)
            {
                if(item.MakerUserId != this.UserId && this.UserRole != SysDB.AdminUser_RoleEnum.超级管理员)
                {
                    throw new Exception("你无权注销这个注册码");
                }
                this.db.Delete(item);
                this.db.Delete(this.db.UserInfo.Where(m => m.id == item.UserId));
            }
            else
            {
                throw new Exception("该注册码不存在");
            }
        }

        /// <summary>
        /// 获取昨天登陆总数
        /// </summary>
        /// <returns></returns>
        [RemotingMethod]
        public int GetYesterdayLoginCount()
        {
            DateTime startTime = Convert.ToDateTime(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"));
            DateTime endTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));

            var count = (from m in this.db.UserLoginRecord
                         where m.LoginTime >= startTime && m.LoginTime < endTime
                         group m by m.UserId into g
                         select g.Key).Count();
            return count;
        }

        [RemotingMethod]
        public int GetTodayLoginCount()
        {
            DateTime startTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
            DateTime endTime = DateTime.Now;

            var count = (from m in this.db.UserLoginRecord
                         where m.LoginTime >= startTime && m.LoginTime < endTime
                         group m by m.UserId into g
                         select g.Key).Count();
            return count;
        }
    }
}
