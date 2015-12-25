using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Satrabel.OpenContent.Components.Infrastructure
{
    public class DefaultRepository : IRepository
    {
        #region Queries

        public T GetById<T>(Guid id) where T : IAggregate
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Commands

        public void Save<T>(T item) where T : IAggregate
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}