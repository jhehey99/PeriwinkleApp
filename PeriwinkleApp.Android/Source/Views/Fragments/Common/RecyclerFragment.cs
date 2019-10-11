using System;
using System.Collections.Generic;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using JP.Wasabeef.Recyclerview.Adapters;
using JP.Wasabeef.Recyclerview.Animators;
using PeriwinkleApp.Android.Source.Adapters;
using PeriwinkleApp.Android.Source.Factories;

namespace PeriwinkleApp.Android.Source.Views.Fragments.Common
{
    public abstract class RecyclerFragment<TRecyclerAdapter, TAdapterModel> : Fragment
		where TRecyclerAdapter : BaseRecyclerAdapter <TAdapterModel>, new()
        where TAdapterModel : class, new()
    {
        #region Fields

        protected LinearLayout RfLinearLayout;

        protected ProgressBar RfProgressBar;

        protected RecyclerView RfRecyclerView;

        protected RecyclerView.LayoutManager RfLayoutManager;

		protected FloatingActionButton RfFab;

		protected TRecyclerAdapter RfRecyclerAdapter;

		protected bool IsAnimated;
		
        #endregion

        #region OnCreate

        protected abstract void OnCreateInitialize ();

        public override void OnCreate (Bundle savedInstanceState)
        {
            base.OnCreate (savedInstanceState);
            OnCreateInitialize();
        }

        #endregion

        #region OnCreateView

        protected abstract int ResourceLayout { get; }

        public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return inflater.Inflate (ResourceLayout, container, false);
        }

        #endregion

        #region OnViewCreated

        protected virtual int? ResourceIdProgressBar { get; } = null;

        protected virtual void InitProgressBar ()
        {
            if (ResourceIdProgressBar == null)
                return;

            RfProgressBar = View.FindViewById <ProgressBar> ((int) ResourceIdProgressBar);
			ShowProgressBar ();
		}

		protected void ShowProgressBar ()
		{
			if(RfProgressBar != null)
				RfProgressBar.Visibility = ViewStates.Visible;
			
            RfLinearLayout?.SetGravity(GravityFlags.Center);
        }

		protected void HideProgressBar ()
		{
			if (RfProgressBar != null)
				RfProgressBar.Visibility = ViewStates.Gone;

			RfLinearLayout?.SetGravity(GravityFlags.Center | GravityFlags.Top);
        }

        protected abstract int? ResourceIdLinearLayout { get; }

        protected virtual void InitLinearLayout ()
        {
            if(ResourceIdLinearLayout == null)
                return;

            RfLinearLayout = View.FindViewById <LinearLayout> ((int) ResourceIdLinearLayout);
        }

        protected abstract int? ResourceIdRecyclerView { get; }

        protected virtual void InitRecyclerView ()
        {
            if (ResourceIdRecyclerView == null)
                return;

            RfRecyclerView = View.FindViewById <RecyclerView> ((int) ResourceIdRecyclerView);
            RfRecyclerView.NestedScrollingEnabled = false;
        }

        protected virtual void InitLayoutManager ()
        {
            RfLayoutManager = new LinearLayoutManager (View.Context);
            RfRecyclerView.SetLayoutManager (RfLayoutManager);
        }

		protected virtual int? ResourceIdFab { get; } = null;

		protected virtual void InitFab ()
		{
			if (ResourceIdFab == null)
				return;

			RfFab = View.FindViewById <FloatingActionButton> ((int) ResourceIdFab);
			RfFab.Click += OnFloatingActionButtonClicked;
			RfFab.Visibility = ViewStates.Visible;
        }

        protected virtual void OnFloatingActionButtonClicked (object sender, EventArgs e) { }

		protected virtual void InitSetAdapter ()
		{
			RfRecyclerAdapter = new TRecyclerAdapter();
			RfRecyclerAdapter.ItemClick += OnItemClick;
			RfRecyclerView.SetAdapter (RfRecyclerAdapter);
		}

		protected virtual void InitAnimation ()
		{
			if (!IsAnimated)
				return;

			var animationAdapter = AnimationFactory.CreateRecyclerAnimation(RfRecyclerAdapter);
			var animator = AnimationFactory.CreateRecyclerAnimator();
			
			RfRecyclerView.SetAdapter(animationAdapter);
			RfRecyclerView.SetItemAnimator(animator);
        }
		
		protected abstract void LoadInitialDataSet();

        protected virtual void SetViewReferences (View view) { }

        public override void OnViewCreated (View view, Bundle savedInstanceState)
        {
            base.OnViewCreated (view, savedInstanceState);

            InitProgressBar ();
            InitLinearLayout ();
            InitRecyclerView ();
            InitLayoutManager ();
			InitFab ();
            InitSetAdapter ();
			InitAnimation ();
            LoadInitialDataSet ();
            SetViewReferences(view);
        }

        #endregion

        protected virtual void OnItemClick (object sender, int position)
        {
            int itemNum = position + 1;
            Toast.MakeText(this.Activity, "This is item number " + itemNum, ToastLength.Short).Show();
        }

		protected virtual void UpdateAdapterDataSet (List <TAdapterModel> dataSet)
		{
			if(dataSet != null)
				RfRecyclerAdapter.UpdateList (dataSet);
		}
    }
}
