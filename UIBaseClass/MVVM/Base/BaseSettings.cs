using UIBaseClass.MVVM.Base.Interface;
using BaseLogger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIBaseClass.MessageBox;

namespace UIBaseClass.MVVM.Base
{
    public class BaseSettings : IBase
    {
        public LogWriter Logger { get; set; }
        public string? ConfigPath { get; set; }
        public string? LoggedOnUser { get; set; }
        public string? LoggedOnUserGroup { get; set; }
        public CustomMessageBox Messagebox { get; set; }

        public BaseSettings()
        {
        }
    }
}
