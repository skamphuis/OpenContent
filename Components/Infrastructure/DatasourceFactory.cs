using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace Satrabel.OpenContent.Components.Infrastructure
{
    public static class Factories
    {

        //static readonly Func<IDatasource> YCreator = Expression.Lambda<Func<IDatasource>>(Expression.New(typeof(Y).GetConstructor(Type.EmptyTypes))).Compile();
        //public static IDatasource CreateY_CompiledExpression()
        //{
        //    return YCreator();
        //}
        public static IDatasource GetDatasource(string source = "")
        {
            IDatasource datasource = null;

            switch (source)
            {
                case "OntoDb":
                    datasource = Activator.CreateInstance("Satrabel.OpenData", "Satrabel.OpenData.OntoDb.Datasource") as IDatasource;
                    break;
                default:
                    datasource = new OpenContentController();
                    break;
            }
            return datasource;
        }
    }
}