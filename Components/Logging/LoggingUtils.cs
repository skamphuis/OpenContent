﻿using System;
using System.Web;
using DotNetNuke.Web.Api;
using Satrabel.OpenContent.Components.Dnn;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Modules;
using System.Web.UI;

namespace Satrabel.OpenContent.Components.Logging
{
    public static class LoggingUtils
    {
        private static string PrepareErrorMessage(ModuleInfo module, Exception exc)
        {
            var ps = PortalSettings.Current;
            string friendlyMessage = string.Format("Alias: {3} \nTab: {4} - {5} \nModule: {0} \nContext: {2} \nError: {1}",
                module.ModuleID,
                exc.Message,
                LoggingUtils.HttpRequestLogInfo(HttpContext.Current),
                ps.PortalAlias.HTTPAlias,
                ps.ActiveTab.TabID,
                DnnUrlUtils.NavigateUrl(ps.ActiveTab.TabID)
                );
            Exception lastExc = exc;
            while (lastExc.InnerException != null)
            {
                lastExc = lastExc.InnerException;
                friendlyMessage += "\n" + lastExc.Message;
            }
            return friendlyMessage;
        }
        private static string PrepareErrorMessage(DotNetNuke.Web.Razor.RazorModuleBase ctrl, Exception exc)
        {
            string friendlyMessage = string.Format("Alias: {3} \nTab: {4} - {5} \nModule: {0} \nContext: {2} \nError: {1}",
                ctrl.ModuleContext.ModuleId,
                exc.Message,
                LoggingUtils.HttpRequestLogInfo(HttpContext.Current),
                ctrl.ModuleContext.PortalAlias.HTTPAlias,
                ctrl.ModuleContext.TabId,
                DnnUrlUtils.NavigateUrl(ctrl.ModuleContext.TabId)
                );
            Exception lastExc = exc;
            while (lastExc.InnerException != null)
            {
                lastExc = lastExc.InnerException;
                friendlyMessage += "\n" + lastExc.Message;
            }
            return friendlyMessage;
        }
        private static string PrepareErrorMessage(DnnApiController ctrl, Exception exc)
        {
            string friendlyMessage = string.Format("PortalId: {3} \nTab: {4} - {5} \nModule: {0} \nContext: {2} \nError: {1}",
                ctrl.ActiveModule.ModuleID,
                exc.Message,
                LoggingUtils.HttpRequestLogInfo(HttpContext.Current),
                ctrl.ActiveModule.PortalID,
                ctrl.ActiveModule.TabID,
                DnnUrlUtils.NavigateUrl(ctrl.ActiveModule.TabID)
                );
            Exception lastExc = exc;
            while (lastExc.InnerException != null)
            {
                lastExc = exc.InnerException;
                friendlyMessage += "\n" + lastExc.Message;
            }
            return friendlyMessage;
        }

        public static void ProcessLogFileException(DotNetNuke.Web.Razor.RazorModuleBase ctrl, Exception exc)
        {
            string friendlyMessage = PrepareErrorMessage(ctrl, exc);
            Log.Logger.Error(friendlyMessage);
        }
        public static void ProcessLogFileException(Control ctrl, ModuleInfo module, Exception exc)
        {
            string friendlyMessage = PrepareErrorMessage(module, exc);
            Log.Logger.Error(friendlyMessage);
        }

        public static void ProcessApiLoadException(DnnApiController ctrl, Exception exc)
        {
            string friendlyMessage = PrepareErrorMessage(ctrl, exc);
            Log.Logger.Error(friendlyMessage);
        }

        public static void ProcessModuleLoadException(DotNetNuke.Web.Razor.RazorModuleBase ctrl, Exception exc)
        {
            string friendlyMessage = PrepareErrorMessage(ctrl, exc);
            DotNetNuke.Services.Exceptions.Exceptions.ProcessModuleLoadException(friendlyMessage, ctrl, exc);
        }
        public static void ProcessModuleLoadException(Control ctrl, ModuleInfo module, Exception exc)
        {
            string friendlyMessage = PrepareErrorMessage(module, exc);
            DotNetNuke.Services.Exceptions.Exceptions.ProcessModuleLoadException(friendlyMessage, ctrl, exc);
        }
        public static string HttpRequestLogInfo(HttpContext context)
        {
            string url = "-unknown-";
            string referrer = "-unknown-";
            if (context != null)
            {
                url = context.Request.Url.AbsoluteUri;
                referrer = context.Request.UrlReferrer == null ? "???" : context.Request.UrlReferrer.AbsoluteUri;
            }
            string retval = string.Format("Called from {0}. Referrer: {1}.", url, referrer);

            return retval;
        }
    }
}