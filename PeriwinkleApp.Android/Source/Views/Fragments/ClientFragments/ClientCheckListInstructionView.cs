using System;
using System.Collections.Generic;
using System.Globalization;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using PeriwinkleApp.Android.Source.Presenters.ClientPresenters;
using PeriwinkleApp.Android.Source.Views.Fragments.Common;
using PeriwinkleApp.Core.Sources.Utils;
using CultureInfo = System.Globalization.CultureInfo;

namespace PeriwinkleApp.Android.Source.Views.Fragments.ClientFragments
{
    public interface IClientCheckListInstructionView
    {
        void DisplayInstructions (IList <string> instructions);
        void DisplayInvalidHeightWeight (string message);
    }

    public class ClientCheckListInstructionView : HideSoftInputFragment, IClientCheckListInstructionView
    {
        private EditText txtClientInsHeight, 
						 txtCliInsHeightFt, 
						 txtCliInsHeightIn, 
						 txtClientInsWeight;
		private TextView txtClientIns1, txtClientIns2;
        private Button btnClientInsProceed;
		private Spinner spinHeight, spinWeight;
		private LinearLayout linCliInsFtin;

        private IClientCheckListInstructionPresenter presenter;

		private string heightStr, weightStr;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
            presenter = new ClientCheckListInstructionPresenter (this, Context);

        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            return inflater.Inflate(Resource.Layout.client_checklist_instructions, container, false);
        }

        public override void OnViewCreated (View view, Bundle savedInstanceState)
        {
            base.OnViewCreated (view, savedInstanceState);

            // Edit Texts
            txtClientInsHeight = view.FindViewById<EditText>(Resource.Id.txt_cli_ins_height);
			txtCliInsHeightFt = view.FindViewById<EditText>(Resource.Id.txt_cli_ins_height_ft);
			txtCliInsHeightIn = view.FindViewById<EditText>(Resource.Id.txt_cli_ins_height_in);
            txtClientInsWeight = view.FindViewById<EditText>(Resource.Id.txt_cli_ins_weight);

            // Linear Layout
			linCliInsFtin = view.FindViewById<LinearLayout>(Resource.Id.lin_cli_ins_ftin);

            // Text Views
            txtClientIns1 = view.FindViewById<TextView>(Resource.Id.txt_cli_ins1);
            txtClientIns2 = view.FindViewById<TextView>(Resource.Id.txt_cli_ins2);

            // Proceed Button
            btnClientInsProceed = view.FindViewById <Button> (Resource.Id.btn_cli_ins_proceed);
            btnClientInsProceed.Click += OnProceedClicked;

			spinHeight = view.FindViewById<Spinner>(Resource.Id.spin_height);
			spinWeight = view.FindViewById<Spinner>(Resource.Id.spin_weight);
			spinHeight.ItemSelected += OnHeightSelected;
			spinWeight.ItemSelected += OnWeightSelected;

            // load instruction and display it
            presenter.LoadInstructions ();
        }
		
		private void OnHeightSelected (object sender, AdapterView.ItemSelectedEventArgs e)
		{
            // i-store natin sya in cm
            // m, cm, ft. in.
			int position = e.Position;

            // eto ung default para sa first 2 items
			txtClientInsHeight.Visibility = ViewStates.Visible; 
            linCliInsFtin.Visibility = ViewStates.Gone;

			if (position == 0)
				txtClientInsHeight.Hint = "Meters";
			else if (position == 1)
				txtClientInsHeight.Hint = "Centimeters";
			else if (position == 2)
			{
				txtClientInsHeight.Visibility = ViewStates.Gone;
				linCliInsFtin.Visibility = ViewStates.Visible;
			}
		}

		private void OnWeightSelected(object sender, AdapterView.ItemSelectedEventArgs e)
		{
			// i-store natin sya in kg
            // kg, lbs
			int position = e.Position;

			if (position == 0)
				txtClientInsWeight.Hint = "Kilograms";
			else if (position == 1)
				txtClientInsWeight.Hint = "Pounds";
		}

		private void GetHeightString ()
		{
			int position = spinHeight.SelectedItemPosition;

			if (position == 0) // m
			{
				// str to float
				string mStr = txtClientInsHeight.Text;
				if (!float.TryParse(mStr, out float m))
					return; // todo error

				// m to cm
				const float mToCmDiv = 100f;
				float cm = m / mToCmDiv;

				heightStr = cm.ToString(CultureInfo.InvariantCulture);
			}
			else if (position == 1) // cm
			{
				heightStr = txtClientInsHeight.Text;
			}
			else if (position == 2) // ft. in
			{
				string ftStr = txtCliInsHeightFt.Text;
				string inStr = txtCliInsHeightIn.Text;

				if (!float.TryParse(ftStr, out float ft) || !float.TryParse(inStr, out float inch))
					return; // todo error

				// ft to cm
				const float ftToCmMul = 30.48f;
				float cm = ft * ftToCmMul;

				// in to cm, then add
				const float inToCmMul = 2.54f;
				cm += inch * inToCmMul;

				heightStr = cm.ToString(CultureInfo.InvariantCulture);
			}
        }

		private void GetWeightString ()
		{
			int position = spinWeight.SelectedItemPosition;

			if (position == 0)
				weightStr = txtClientInsWeight.Text;
			else if (position == 1)
			{
				// str to float
				string lbsStr = txtClientInsWeight.Text;
				if (!float.TryParse (lbsStr, out float lbs))
					return; // TODO Error

				// lbs to kg
				const float lbsToKgDiv = 2.205f;
				float kg = lbs / lbsToKgDiv;

				weightStr = kg.ToString (CultureInfo.InvariantCulture);
			}
		}

        private void OnProceedClicked (object sender, EventArgs e)
        {
            //todo baguhin to//get height and weight
//            heightStr = txtClientInsHeight.Text;
//            weightStr = txtClientInsWeight.Text;

			GetHeightString ();
			GetWeightString ();

            presenter.UpdateClientHeightWeight (heightStr, weightStr);

			HideSoftInput ();
            StartQuestionsFragment ();
        }

        private void StartQuestionsFragment ()
        {
            // proceed to questions
            FragmentTransaction ft = Activity.SupportFragmentManager.BeginTransaction();
            Fragment fragment = new ClientCheckListQuestionView();
            
            ft.Replace(Resource.Id.fragment_container, fragment);
            ft.AddToBackStack(null);
            ft.Commit();
        }

    #region IClientCheckListInstructionView

        public void DisplayInstructions (IList <string> instructions)
        {
            if (instructions == null || instructions.Count < 2)
                return;
            
            txtClientIns1.Text = instructions[0];
            txtClientIns2.Text = instructions[1];
        }

        public void DisplayInvalidHeightWeight (string message)
        {
            throw new NotImplementedException ();
        }
		
    #endregion
        }
    }
