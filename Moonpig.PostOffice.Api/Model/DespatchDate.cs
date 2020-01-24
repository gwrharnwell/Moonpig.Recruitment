namespace Moonpig.PostOffice.Api.Model
{
    using System;

    public class DespatchDate
    {
        public DespatchDate(DateTime date, String error = null)
        {
            Date = date;
            Error = error;
            Success = String.IsNullOrEmpty(error); //IF an error has been provided, the call isn't a success
        }

        public DateTime Date { get; set; }
        public String Error { get; set; }
        public Boolean Success { get; set; }
    }
}