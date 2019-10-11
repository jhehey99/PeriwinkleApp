using System.Collections.Generic;
using PeriwinkleApp.Core.Sources.CommonInterfaces;
using PeriwinkleApp.Core.Sources.Extensions;

namespace PeriwinkleApp.Core.Sources.Models.Domain
{
    public class MbesResponse : IDebugString
    {
        public int? MbesResponseId { get; set; }
        public int AttemptId { get; set; }
        public List<int> QuestionIds { get; set; }
        public List<int> ScaleValues { get; set; }

        private string jsonString;

        public MbesResponse() { }

        public MbesResponse (MbesResponse mbesResponse)
        {
            MbesResponseId = mbesResponse.MbesResponseId;
            AttemptId = mbesResponse.AttemptId;
            QuestionIds = mbesResponse.QuestionIds;
            ScaleValues = mbesResponse.ScaleValues;
        }

        public string ToDebug ()
        {
            return jsonString ?? (jsonString = this.PrettySerialize());
        }
    }
}
