using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Satrabel.OpenContent.Components.Infrastructure
{
    public interface IRepository
    {
        T GetById<T>(Guid id) where T : IAggregate;
        //T GetById<T, K>(Guid id)
        //    where T : IAggregate
        //    where K : AbstractIndexCreationTask, new();
        void Save<T>(T item) where T : IAggregate;
    }
}
