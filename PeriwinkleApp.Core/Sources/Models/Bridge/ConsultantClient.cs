using PeriwinkleApp.Core.Sources.Models.Domain;

namespace PeriwinkleApp.Core.Sources.Models.Bridge
{
    public class ConsultantClient
    {
        public Consultant CcConsultant { get; set; }
        public Client CcClient { get; set; }

        public ConsultantClient ()
        {
        }
    }
}
