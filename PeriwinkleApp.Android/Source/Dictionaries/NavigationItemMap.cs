using System;
using System.Collections.Generic;
using Android.Support.V4.App;
using PeriwinkleApp.Android.Source.Views.Fragments.AdminFragments;
using PeriwinkleApp.Android.Source.Views.Fragments.ClientFragments;
using PeriwinkleApp.Android.Source.Views.Fragments.ConsultantFragments;
using PeriwinkleApp.Core.Sources.Models.Domain;

namespace PeriwinkleApp.Android.Source.Dictionaries
{
    public class NavigationItemMap
    {
        private readonly IDictionary<int, (int, Fragment)> navItemMap;
        
        public NavigationItemMap (AccountType type)
        {
            switch (type)
            {
                case AccountType.Admin:
                    navItemMap = new Dictionary<int, (int, Fragment)>()
                                 {
                                     // { activity_main_drawer.xml, (strings.xml, Fragments/Admin) }
                                     { Resource.Id.menu_nav_home, (Resource.String.menu_home_title, new AdminHome()) },
                                     { Resource.Id.menu_nav_clients, (Resource.String.menu_cli_title, new AdminClientsView()) },
                                     { Resource.Id.menu_nav_consultants, (Resource.String.menu_con_title, new AdminConsultantsView()) },
                                     { Resource.Id.menu_nav_pending, (Resource.String.menu_pend_title, new AdminPendingView()) }
                                 };
                    break;

                case AccountType.Consultant:
                    navItemMap = new Dictionary <int, (int, Fragment)> ()
                                 {
                                     { Resource.Id.con_menu_home, (Resource.String.con_menu_home_title, new ConsultantHomeView()) },
                                     { Resource.Id.con_menu_new, (Resource.String.con_menu_new_title, new ConsultantNewClientView()) },
                                     { Resource.Id.con_menu_clients, (Resource.String.con_menu_clients_title, new ConsultantClientsView()) }
                                 };
                    break;

                case AccountType.Client:
                    navItemMap = new Dictionary<int, (int, Fragment)>()
                                 {
                                     { Resource.Id.cli_menu_home, (Resource.String.cli_menu_home_title, new ClientHomeView()) },
                                     { Resource.Id.cli_menu_record, (Resource.String.cli_menu_record_title, new ClientRecordView()) }, // ClientDeviceView
                                     { Resource.Id.cli_menu_device, (Resource.String.cli_menu_device_title, new ClientMbesListView()) }, // ClientDeviceView
									 { Resource.Id.cli_menu_behavior, (Resource.String.cli_menu_behavior_title, new ClientBehaviorView ()) },
									 { Resource.Id.cli_menu_accelerometer, (Resource.String.cli_menu_accelerometer_title, new ClientBehaviorView ()) },
                                     { Resource.Id.cli_menu_journal, (Resource.String.cli_menu_journal_title, new ClientJournalListView ()) },
                                     { Resource.Id.cli_menu_consultant, (Resource.String.cli_menu_consultant_title, new ClientMyConsultantView ()) },
                                     { Resource.Id.cli_menu_entertainment, (Resource.String.cli_menu_entertainment_title, new ClientPlaylistView ()) }
                                 };
			break;
                
                default: throw new ArgumentOutOfRangeException (nameof (type), type, null);
            }
        }
        
        public (int, Fragment) GetNavItem(int id)
        {
            if (!navItemMap.ContainsKey(id))
                throw new System.NotImplementedException("Id = " + id);
            
            return navItemMap[id];
        }
    }
}
