using System.Collections.Generic;
using Android.Support.V7.Widget;
using Android.Views.Animations;
using JP.Wasabeef.Recyclerview.Adapters;
using JP.Wasabeef.Recyclerview.Animators;
using PeriwinkleApp.Android.Source.Adapters;

namespace PeriwinkleApp.Android.Source.Factories
{
    public class AnimationFactory
    {
        public static ScaleInAnimationAdapter CreateRecyclerAnimation (RecyclerView.Adapter adapter)
        {
			ScaleInAnimationAdapter animAdapter = new ScaleInAnimationAdapter(new SlideInBottomAnimationAdapter(adapter));
//            animAdapter.SetDuration (500);
            animAdapter.SetFirstOnly (false);
            return animAdapter;
        }

        public static SlideInRightAnimator CreateRecyclerAnimator ()
        {
            return new SlideInRightAnimator(new OvershootInterpolator())
                   {
                       AddDuration = 500,
                       RemoveDuration = 500,
                       MoveDuration = 500,
                       ChangeDuration = 500
                   };
        }

//		public static SlideInRightAnimationAdapter CreateSlideInRightAnimationAdapter (RecyclerView.Adapter adapter)
//		{
//			SlideInRightAnimationAdapter animAdapter = new SlideInRightAnimationAdapter (adapter);
//			animAdapter.SetDuration (500);
//			animAdapter.SetFirstOnly ();
//		}
//
//		public static SlideInRightAnimator CreateSlideInRightAnimator ()
//		{
//			return new SlideInRightAnimator()
//				   {
//					   AddDuration = 500,
//					   RemoveDuration = 500,
//					   MoveDuration = 500,
//					   ChangeDuration = 500
//				   };
//        }

    }
}
