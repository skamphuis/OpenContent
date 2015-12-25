using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Satrabel.OpenContent.Components.Infrastructure
{
    public static class Factories
    {
        public static IDatasource GetDatasource(string source = "")
        {
            IDatasource datasource = null;

            switch (source)
            {
                case "OntoDb":

                default:
                    datasource = new OpenContentController();
                    break;
            }
            return datasource;
        }
    }
}