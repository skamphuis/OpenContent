﻿using DotNetNuke.Web.Client;
using DotNetNuke.Web.Client.ClientResourceManagement;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using HandlebarsNet = HandlebarsDotNet;

namespace Satrabel.OpenContent.Components.Handlebars
{
    public class HandlebarsEngine
    {
        public string Execute(string source, dynamic model)
        {
            var hbs = HandlebarsNet.Handlebars.Create();
            RegisterDivideHelper(hbs);
            RegisterMultiplyHelper(hbs);
            RegisterEqualHelper(hbs);
            var template = hbs.Compile(source);
            var result = template(model);
            return result;
        }
        public string Execute(Page page, string sourceFilename, dynamic model)
        {
            string source = File.ReadAllText(System.Web.Hosting.HostingEnvironment.MapPath(sourceFilename));
            string sourceFolder = Path.GetDirectoryName(sourceFilename).Replace("\\", "/") + "/";
            var hbs = HandlebarsNet.Handlebars.Create();
            RegisterDivideHelper(hbs);
            RegisterMultiplyHelper(hbs);
            RegisterEqualHelper(hbs);
            RegisterScriptHelper(hbs);
            RegisterRegisterScriptHelper(hbs, page, sourceFolder);
            RegisterRegisterScriptHelper(hbs, page, sourceFolder);
            var template = hbs.Compile(source);
            var result = template(model);
            return result;
        }
        private void RegisterMultiplyHelper(HandlebarsNet.IHandlebars hbs)
        {
            hbs.RegisterHelper("multiply", (writer, context, parameters) =>
            {
                try
                {
                    int a = int.Parse(parameters[0].ToString());
                    int b = int.Parse(parameters[1].ToString());
                    int c = a * b;
                    HandlebarsNet.HandlebarsExtensions.WriteSafeString(writer, c.ToString());
                }
                catch (Exception)
                {
                    HandlebarsNet.HandlebarsExtensions.WriteSafeString(writer, "0");
                }
            });
        }
        private void RegisterDivideHelper(HandlebarsNet.IHandlebars hbs)
        {
            hbs.RegisterHelper("divide", (writer, context, parameters) =>
            {
                try
                {
                    int a = int.Parse(parameters[0].ToString());
                    int b = int.Parse(parameters[1].ToString());
                    int c = a / b;
                    HandlebarsNet.HandlebarsExtensions.WriteSafeString(writer, c.ToString());
                }
                catch (Exception)
                {
                    HandlebarsNet.HandlebarsExtensions.WriteSafeString(writer, "0");
                }
            });
        }
        private void RegisterEqualHelper(HandlebarsNet.IHandlebars hbs)
        {
            hbs.RegisterHelper("equal", (writer, options, context, arguments) =>
            {
                if (arguments.Length == 2 && arguments[0].Equals(arguments[1]))
                {
                    options.Template(writer, (object)context);
                }
                else
                {
                    options.Inverse(writer, (object)context);
                }
            });
        }
        private void RegisterScriptHelper(HandlebarsNet.IHandlebars hbs)
        {
            hbs.RegisterHelper("script", (writer, options, context, arguments) =>
            {
                HandlebarsNet.HandlebarsExtensions.WriteSafeString(writer, "<script>");
                options.Template(writer, (object)context);
                HandlebarsNet.HandlebarsExtensions.WriteSafeString(writer, "</script>");
            });
            
        }
        private void RegisterRegisterScriptHelper(HandlebarsNet.IHandlebars hbs, Page page, string sourceFolder)
        {
            hbs.RegisterHelper("registerscript", (writer, context, parameters) =>
            {
                if (parameters.Length == 1)
                {
                    string jsfilename = sourceFolder + parameters[0];
                    ClientResourceManager.RegisterScript(page, page.ResolveUrl(jsfilename), FileOrder.Js.DefaultPriority);
                    //writer.WriteSafeString(Page.ResolveUrl(jsfilename));
                }
            });
        }
        private void RegisterRegisterStylesheetHelper(HandlebarsNet.IHandlebars hbs, Page page, string sourceFolder)
        {
            hbs.RegisterHelper("registerstylesheet", (writer, context, parameters) =>
            {
                if (parameters.Length == 1)
                {
                    string cssfilename = sourceFolder + parameters[0];
                    ClientResourceManager.RegisterStyleSheet(page, page.ResolveUrl(cssfilename), FileOrder.Css.PortalCss);
                }
            });
        }
    }

}