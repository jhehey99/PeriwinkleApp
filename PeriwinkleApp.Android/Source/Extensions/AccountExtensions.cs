using PeriwinkleApp.Android.Source.AdapterModels;
using PeriwinkleApp.Core.Sources.Models.Domain;
using System.Collections.Generic;

namespace PeriwinkleApp.Android.Source.Extensions
{
    public static class AccountExtensions
    {
        public static (string, string) ToCardView(this Account account)
        {
            // TODO: Ung Picture kung meron
            return (account.FirstName + account.LastName, account.Email);
        }

        public static List<(string, string)> ListToCardView<T> (this List<T> tList)
            where T: Account
        {
            List<(string, string)> list = new List<(string, string)>();

            foreach(var t in tList)
                list.Add((t.ToCardView()));

            return list;
        }

        public static AccountAdapterModel ToAccountAdapterModel(this Account account)
        {
            return new AccountAdapterModel()
            {
                Name = account.FirstName + " " + account.LastName,
                Email = account.Email
            };
        }

        public static List<AccountAdapterModel> ToListAccountAdapterModel<T> (this List<T> accounts)
            where T: Account
        {
            List<AccountAdapterModel> list = new List<AccountAdapterModel>();

            foreach (var account in accounts)
                list.Add(account.ToAccountAdapterModel());

            return list;
        }

        public static string[] ToBundleStringArray(this Account account)
        {
            return new string[] { account.FirstName + " " + account.LastName, account.Username };
        }

    }
}