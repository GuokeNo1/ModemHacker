using System;
using System.Collections.Generic;
using System.Text;

namespace LibModemBase
{
    public abstract class ModemBase
    {
        /// <summary>
        /// 模板类
        /// </summary>
        protected string username = "useradmin";
        protected string passwd;
        protected string host;
        public static string modem = "Base";
        public static bool needUser = false;
        public ModemBase(string host, string passwd)
        {
            this.host = host;
            this.passwd = passwd;
        }
        public ModemBase(string host, string username, string passwd) : this(host, passwd)
        {
            this.username = username;
        }
        public abstract string getModemInfo();
    }
}
