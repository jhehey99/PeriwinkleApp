using Android.Support.V4.App;
using PeriwinkleApp.Android.Source.Views.Fragments.AdminFragments;

namespace PeriwinkleApp.Android.Source.Adapters
{
    public class AdminClientPageAdapter : FragmentStatePagerAdapter
    {
        public override int Count { get; }

        public AdminClientPageAdapter(FragmentManager fm, int tabCount) : base(fm)
        {
            this.Count = tabCount;
        }

        public override Fragment GetItem(int position)
        {
            Fragment fragment;
            switch (position)
            {
                case 0:
                    fragment = new AdminClientProfileAboutView();
                    break;
                case 1:
                    fragment = new AdminClientProfileActivityView();
                    break;
                case 2:
                    fragment = new AdminClientProfileSurveyView();
                    break;
                default:
                    fragment = null;
                    break;
            }
            return fragment;
        }

    }
}
