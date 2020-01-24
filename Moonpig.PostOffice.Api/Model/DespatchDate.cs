namespace Moonpig.PostOffice.Api.Model
{
    using System;

    public class DespatchDate
    {
        public DespatchDate(DateTime date)
        {
            Date = date;
        }

        public DateTime Date { get; set; }
    }
}