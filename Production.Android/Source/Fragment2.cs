using Android.OS;
using Android.Views;
using Android.Support.V4.App;

namespace Production.Android.Source
{
    public class Fragment2 : Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            return inflater.Inflate(Resource.Layout.fragment2, container, false);

            // return base.OnCreateView(inflater, container, savedInstanceState);
        }
    }
}