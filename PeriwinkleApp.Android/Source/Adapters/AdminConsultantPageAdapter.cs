using Android.Support.V4.App;
using PeriwinkleApp.Android.Source.Views.Fragments.AdminFragments;

namespace PeriwinkleApp.Android.Source.Adapters
{
    public class AdminConsultantPageAdapter : FragmentStatePagerAdapter
    {
        public override int Count { get; }

        public AdminConsultantPageAdapter (FragmentManager fm, int tabCount) : base (fm)
        {
            Count = tabCount;
        }

        public override Fragment GetItem (int position)
        {
            Fragment fragment;
            switch (position)
            {
                case 0:
                    fragment = new AdminConsultantProfileAboutView();
                    break;
                case 1:
                    fragment = new AdminConsultantProfileClientsView();
                    break;
                default:
                    fragment = null;
                    break;
            }
            return fragment;
        }

        //TODO: Dictionary nalang, tas gawa tayo ng abstract base class para dito
        // kasama nya adminclientpageadapter

    }
}
