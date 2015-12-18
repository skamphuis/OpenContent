﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using DotNetNuke.Entities.Modules;

namespace Satrabel.OpenContent.Components.Manifest
{
    public class TemplateInfo
    {
        public TemplateInfo()
        {
            SettingsJson = "";
            DataJson = "";
            OutputString = "";
            Template = null;
            //Manifest = null;
            Files = null;
        }

        #region Public Properties

        public int DetailItemId { get; set; }
        public string OutputString { get; set; }

        #endregion

        #region Data & Settings

        public void ResetData()
        {
            DataJson = "";
            SettingsJson = "";
        }

        public void SetData(string dataJson, string settingsData)
        {
            DataJson = dataJson;
            SettingsJson = settingsData;
            DataExist = !string.IsNullOrWhiteSpace(dataJson);
        }

        public void SetData(IEnumerable<OpenContentInfo> getContents, string settingsData)
        {
            DataList = getContents;
            SettingsJson = settingsData;
            DataExist = (getContents != null && getContents.Any());
        }

        public string DataJson { get; private set; }
        public string SettingsJson { get; private set; }

        public IEnumerable<OpenContentInfo> DataList { get; private set; }
        private bool DataExist { get; set; }
        public bool ShowInitControl { get { return !DataExist || SettingsJson == null && Template.SettingsNeeded(); } }


        #endregion

        #region DataSource Module information

        public void SetDataSourceModule(int tabId, int moduleId, ModuleInfo getModule, TemplateManifest template, string data)
        {
            TabId = tabId;
            ModuleId = moduleId;
            Module = getModule;
            OtherModuleTemplate = template;
            OtherModuleSettingsJson = data;
        }

        public int TabId { get; private set; }
        public int ModuleId { get; private set; }
        public ModuleInfo Module { get; private set; }
        public TemplateManifest OtherModuleTemplate { get; private set; }
        public string OtherModuleSettingsJson { get; private set; }

        #endregion

        public void SetSelectedTemplate(TemplateManifest template)
        {
            Template = template;
        }

        public TemplateManifest Template { get; private set; }

        #region ReadOnly

        public bool IsOtherModule { get { return TabId > 0 && ModuleId > 0; } }

        #endregion

        public TemplateFiles Files { get; set; }

        //public FileUri Template { get;private set; }
        //public Manifest Manifest { get; private set; }
    }
}