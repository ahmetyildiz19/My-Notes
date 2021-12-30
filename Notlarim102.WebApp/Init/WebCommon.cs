﻿using Notlarim102.Common;
using Notlarim102.Entity;
using Notlarim102.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Notlarim102.WebApp.Init
{
    public class WebCommon : Icommon
    {
        public string GetCurrentUsername()
        {
            if (CurrentSession.User != null)
            {
                NotlarimUser user = CurrentSession.User as NotlarimUser;
                return user.Username;
            }
            return "system";
        }
    }
}