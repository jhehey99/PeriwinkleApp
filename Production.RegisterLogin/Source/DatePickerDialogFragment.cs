﻿using System;
using Android.App;
using Android.Content;
using Android.OS;
using DialogFragment = Android.Support.V4.App.DialogFragment;


namespace Production.RegisterLogin.Source
{
    public class DatePickerDialogFragment : DialogFragment
    {
        private readonly Context context;
        private readonly DateTime date;
        private readonly DatePickerDialog.IOnDateSetListener listener;

        public DatePickerDialogFragment (Context context, DateTime date, DatePickerDialog.IOnDateSetListener listener)
        {
            this.context = context;
            this.date = date;
            this.listener = listener;
        }

        public override Dialog OnCreateDialog (Bundle savedInstanceState)
        {
            return new DatePickerDialog(context, listener, date.Year, date.Month - 1, date.Day);
        }
    }
}
