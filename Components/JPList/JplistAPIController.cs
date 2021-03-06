﻿using DotNetNuke.Entities.Modules;
using DotNetNuke.Security;
using DotNetNuke.Web.Api;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DotNetNuke.Entities.Portals;
using Satrabel.OpenContent.Components.Datasource;
using Satrabel.OpenContent.Components.Alpaca;
using Satrabel.OpenContent.Components.Logging;

namespace Satrabel.OpenContent.Components.JPList
{
    [SupportedModules("OpenContent")]
    public class JplistAPIController : DnnApiController
    {
        //[ValidateAntiForgeryToken] to work with output caching
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [HttpPost]
        public HttpResponseMessage List(RequestDTO req)
        {
            try
            {
                OpenContentSettings settings = ActiveModule.OpenContentSettings();
                ModuleInfo module = ActiveModule;
                if (settings.ModuleId > 0)
                {
                    ModuleController mc = new ModuleController();
                    module = mc.GetModule(settings.ModuleId, settings.TabId, false);
                }
                var manifest = settings.Template.Manifest;
                var templateManifest = settings.Template;
                JObject reqOptions = null;
                if (!string.IsNullOrEmpty(req.options))
                {
                    reqOptions = JObject.Parse(req.options);
                }
                //string editRole = manifest.GetEditRole();
                bool listMode = templateManifest != null && templateManifest.IsListTemplate;
                if (listMode)
                {

                    var indexConfig = OpenContentUtils.GetIndexConfig(settings.Template.Key.TemplateDir);
                    QueryBuilder queryBuilder = new QueryBuilder(indexConfig);
                    bool isEditable = ActiveModule.CheckIfEditable(PortalSettings);//portalSettings.UserMode != PortalSettings.Mode.Edit;
                    queryBuilder.Build(settings.Query, !isEditable, UserInfo.UserID, DnnLanguageUtils.GetCurrentCultureCode(), UserInfo.Social.Roles);

                    JplistQueryBuilder.MergeJpListQuery(indexConfig, queryBuilder.Select, req.StatusLst, DnnLanguageUtils.GetCurrentCultureCode());
                    IDataItems dsItems;
                    if (queryBuilder.DefaultNoResults && queryBuilder.Select.IsQueryEmpty)
                    {
                        dsItems = new DefaultDataItems()
                        {
                            Items = new List<DefaultDataItem>(),
                            Total = 0
                        };
                    }
                    else
                    {
                        var ds = DataSourceManager.GetDataSource(manifest.DataSource);
                        var dsContext = new DataSourceContext()
                        {
                            ModuleId = module.ModuleID,
                            ActiveModuleId = ActiveModule.ModuleID,
                            UserId = UserInfo.UserID,
                            TemplateFolder = settings.TemplateDir.FolderPath,
                            Config = manifest.DataSourceConfig,
                            Options = reqOptions
                        };
                        dsItems = ds.GetAll(dsContext, queryBuilder.Select);
                    }
                    int mainTabId = settings.DetailTabId > 0 ? settings.DetailTabId : settings.TabId;
                    ModelFactory mf = new ModelFactory(dsItems.Items, ActiveModule, PortalSettings, mainTabId);
                    mf.Options = reqOptions;
                    var model = mf.GetModelAsJson(false);

                    //model["luceneQuery"] = dsItems.DebugInfo;
                    if (LogContext.IsLogActive)
                    {
                        LogContext.Log(ActiveModule.ModuleID, "RequestContext", "IsEditable", isEditable);
                        LogContext.Log(ActiveModule.ModuleID, "RequestContext", "UserRoles", PortalSettings.UserInfo.Social.Roles.Select(r => r.RoleName));
                        LogContext.Log(ActiveModule.ModuleID, "RequestContext", "CurrentUserId", PortalSettings.UserId);
                        var logKey = "Query";
                        LogContext.Log(ActiveModule.ModuleID, logKey, "select", queryBuilder.Select);
                        LogContext.Log(ActiveModule.ModuleID, logKey, "result", dsItems);
                        LogContext.Log(ActiveModule.ModuleID, logKey, "model", model);
                        model["Logs"] = JToken.FromObject(LogContext.Current.ModuleLogs(ActiveModule.ModuleID));
                    }
                    var res = new ResultDTO()
                    {
                        data = model,
                        count = dsItems.Total
                    };
                    return Request.CreateResponse(HttpStatusCode.OK, res);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "not supported because not in multi items template ");
                }
            }
            catch (Exception exc)
            {
                Log.Logger.Error(exc);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exc);
            }
        }

    }
}
