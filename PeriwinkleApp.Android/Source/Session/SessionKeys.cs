using System.Collections.Generic;
using PeriwinkleApp.Core.Sources.Models.Domain;

namespace PeriwinkleApp.Android.Source.Session
{
    public static class SessionKeys
    {
        public static string AdminConsultantProfileClientsKey { get; } = "SKConCli";
        public static string AdminConsultantsKey { get; } = "SKConsul";
        public static string AdminClientsKey { get; } = "SKClient";
        public static string LoginKey { get; } = "SKLogin";
        public static string LoggedClient { get; } = "SKLogCli";
        public static string LoggedConsultant { get; } = "SKLogCon";
		public static string ViewClient { get; } = "SKVCli";
    }
}
