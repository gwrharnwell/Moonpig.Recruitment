namespace Moonpig.PostOffice.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Data;
    using Microsoft.AspNetCore.Mvc;
    using Model;

    [Route("api/[controller]")]
    public class DespatchDateController : Controller
    {
        private readonly IDbContext _cxt;
        public DespatchDateController(IDbContext cxt)
        {
            _cxt = cxt;
        }

        [HttpGet]
        public IActionResult Get(List<int> productIds, DateTime orderDate)
        {
            //Ensure we start calculating from a weekday
            orderDate = EnsureDateIsWeekDay(orderDate);

            foreach (var ID in productIds)
            {
                //Find product by ID
                var product = _cxt.Products.SingleOrDefault(x => x.ProductId == ID);

                //Check if product exists
                if (product == null)
                {
                    //Couldn't find this product, return NotFound message to the client
                    return NotFound($"Product: { ID } doesn't exist");
                }

                //Find supplier by product supplier ID
                var supplier = _cxt.Suppliers.SingleOrDefault(x => x.SupplierId == product.SupplierId);

                //Check supplier exists
                if (supplier == null)
                {
                    //Couldn't find supplier - return NotFound message to client
                    return NotFound("There was a problem with one of our suppliers. We currently can't provide a despatch date.");
                }


                if (orderDate.AddDays(supplier.LeadTime) > orderDate)
                {
                    orderDate = orderDate.AddDays(supplier.LeadTime);
                }
            }

            //Determine whether or not extra days need to be added on to the despatch date due to weekend
            var despatchDate = EnsureDateIsWeekDay(orderDate);

            //Wrap object in OK result and return to client
            return Ok(new DespatchDate(despatchDate));
        }

        private DateTime EnsureDateIsWeekDay(DateTime dt)
        {
            var daysToAdd = 0;

            if (dt.DayOfWeek == DayOfWeek.Saturday)
                daysToAdd = 2;
            else if (dt.DayOfWeek == DayOfWeek.Sunday)
                daysToAdd = 1;

            return dt.AddDays(daysToAdd);
        }
    }
}
