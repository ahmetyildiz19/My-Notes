﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;

namespace Notlarim102.Common
{
    public class DefaultCommon : Icommon
    {
        public string GetCurrentUsername()
        {
            return "system";
        }
    }
}