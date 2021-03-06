﻿namespace Moonpig.PostOffice.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Data;
    using Microsoft.AspNetCore.Mvc;
    using Model;
    using Moonpig.PostOffice.Api.Helpers;

    [Route("api/[controller]")]
    public class DespatchDateController : Controller
    {
        private IDbContext _cxt;
        public DespatchDateController(IDbContext cxt)
        {
            _cxt = cxt;
        }

        [HttpGet]
        public DespatchDate Get(List<int> productIds, DateTime orderDate)
        {
            //Ensure at least one product has been specified
            if (productIds == null || !productIds.Any())
            {
                return new DespatchDate(orderDate, "Please provide at least one valid product ID");
            }

            //Ensure we start calculating from a weekday
            orderDate = DateCalculationHelper.EnsureDateIsWeekDay(orderDate);

            int longestLeadTime = 0;
            foreach (var ID in productIds)
            {
                //Find product by ID
                var product = _cxt.Products.SingleOrDefault(x => x.ProductId == ID);

                //Check if product exists
                if (product == null)
                {
                    //Couldn't find this product, return NotFound message to the client
                    return new DespatchDate(orderDate, $"Product: { ID } doesn't exist");
                }

                //Find supplier by product supplier ID
                var supplier = _cxt.Suppliers.SingleOrDefault(x => x.SupplierId == product.SupplierId);

                //Check supplier exists
                if (supplier == null)
                {
                    //Couldn't find supplier - return NotFound message to client
                    return new DespatchDate(orderDate, "There was a problem with one of our suppliers. We currently can't provide a despatch date.");
                }

                //Check if this supplier has the longest lead time
                if (supplier.LeadTime > longestLeadTime)
                {
                    longestLeadTime = supplier.LeadTime;
                }
            }

            //Calculate final dispatch date
            var dispatchDate = DateCalculationHelper.AddBusinessDaysToDate(orderDate, longestLeadTime);

            return new DespatchDate(dispatchDate);
        }

    }
}
