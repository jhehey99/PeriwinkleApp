// ReSharper disable InconsistentNaming

using System;
using System.Net.NetworkInformation;

namespace PeriwinkleApp.Core.Sources.Services
{
    public abstract class PeriwinkleHttpService
    {
        public static string Tag => "PeriwinkleHttpService";
        
    #region Properties

        protected static readonly HttpService httpService;
        
    #endregion

    #region Static Constructor

        static PeriwinkleHttpService ()
        {
            httpService = new HttpService ();
        }
        
    #endregion
    }
}
