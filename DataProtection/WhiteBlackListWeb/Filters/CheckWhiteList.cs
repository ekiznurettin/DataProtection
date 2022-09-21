﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WhiteBlackListWeb.Middleware;

namespace WhiteBlackListWeb.Filters
{
    public class CheckWhiteList:ActionFilterAttribute
    {
        //Method ve controller katmanında yapılan ip kontrolü
        private readonly IPList _ipList;

        public CheckWhiteList(IOptions< IPList> ipList)
        {
            _ipList = ipList.Value;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var reqIpAddress = context.HttpContext.Connection.RemoteIpAddress;

            var isWhiteList = _ipList.WhiteList.Where(x => IPAddress.Parse(x).Equals(reqIpAddress)).Any();
            if (!isWhiteList)
            {
                context.Result = new StatusCodeResult((int)HttpStatusCode.Forbidden);
                return;
            }
            
            base.OnActionExecuting(context);
        }
    }
}
