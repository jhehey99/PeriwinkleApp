using Android.Support.V4.App;

namespace ViewPagerTabs
{
    public class PageAdapter : FragmentPagerAdapter
    {
        private int tabCount;

        public override int Count => tabCount;

        public PageAdapter(FragmentManager fm, int tabcount) : base(fm)
        {
            tabCount = tabcount;
        }
        
        public override Fragment GetItem(int position)
        {
            switch(position)
            {
                case 0:
                    return new ChatFragment();
                case 1:
                    return new StatusFragment();
                case 2:
                    return new CallFragment();
                default:
                    return null;
            }
        }
    }
}
