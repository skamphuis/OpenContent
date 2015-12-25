using System.Collections;
using System.Collections.Generic;
using Satrabel.OpenContent.Components.Lucene.Config;
using Satrabel.OpenContent.Components.Manifest;

namespace Satrabel.OpenContent.Components.Infrastructure
{
    public interface IDatasource
    {
        void AddContent(OpenContentInfo content, bool index, FieldConfig indexConfig);
        OpenContentInfo GetFirstContent(int moduleId);
        OpenContentInfo GetContent(int parse);
        void UpdateContent(OpenContentInfo content, bool index, FieldConfig indexConfig);
        void DeleteContent(OpenContentInfo content, bool index);
        IEnumerable<OpenContentInfo> GetContents(int moduleid);
    }
}
