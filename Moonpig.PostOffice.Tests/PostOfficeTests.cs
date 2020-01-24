namespace Moonpig.PostOffice.Tests
{
    using System;
    using System.Collections.Generic;
    using Api.Controllers;
    using Moonpig.PostOffice.Data;
    using Shouldly;
    using Xunit;

    public class PostOfficeTests
    {
        [Fact]
        public void OneProductWithLeadTimeOfOneDay()
        {
            IDbContext cxt = new DbContext();
            DespatchDateController controller = new DespatchDateController(cxt);
            var date = controller.Get(new List<int>() { 1 }, new DateTime(2018, 01, 01));
            date.Date.Date.ShouldBe(new DateTime(2018, 01, 02));
        }

        [Fact]
        public void OneProductWithLeadTimeOfTwoDay()
        {
            IDbContext cxt = new DbContext();
            DespatchDateController controller = new DespatchDateController(cxt);
            var date = controller.Get(new List<int>() { 2 }, new DateTime(2018, 01, 01));
            date.Date.Date.ShouldBe(new DateTime(2018, 01, 03));
        }

        [Fact]
        public void OneProductWithLeadTimeOfThreeDay()
        {
            IDbContext cxt = new DbContext();
            DespatchDateController controller = new DespatchDateController(cxt);
            var date = controller.Get(new List<int>() { 3 }, new DateTime(2018, 01, 01));
            date.Date.Date.ShouldBe(new DateTime(2018, 01, 04));
        }

        [Fact]
        public void OneProductWithLeadTimeOfSixDay()
        {
            IDbContext cxt = new DbContext();
            DespatchDateController controller = new DespatchDateController(cxt);
            var date = controller.Get(new List<int>() { 9 }, new DateTime(2018, 01, 05));
            date.Date.Date.ShouldBe(new DateTime(2018, 01, 15));
        }

        [Fact]
        public void SaturdayHasExtraTwoDays()
        {
            IDbContext cxt = new DbContext();
            DespatchDateController controller = new DespatchDateController(cxt);
            var date = controller.Get(new List<int>() { 1 }, new DateTime(2018, 01, 26));
            date.Date.ShouldBe(new DateTime(2018, 01, 29));
        }

        [Fact]
        public void SundayHasExtraDay()
        {
            IDbContext cxt = new DbContext();
            DespatchDateController controller = new DespatchDateController(cxt);
            var date = controller.Get(new List<int>() { 3 }, new DateTime(2018, 1, 25));
            date.Date.ShouldBe(new DateTime(2018, 01, 30));
        }

        [Fact]
        public void TwoProductsWithHighestLeadTimeOfTwoDays()
        {
            IDbContext cxt = new DbContext();
            DespatchDateController controller = new DespatchDateController(cxt);
            var date = controller.Get(new List<int>() { 1, 2 }, new DateTime(2018, 01, 01));
            date.Date.ShouldBe(new DateTime(2018, 01, 03));
        }

        [Fact]
        public void InvalidProduct()
        {
            IDbContext cxt = new DbContext();
            DespatchDateController controller = new DespatchDateController(cxt);
            var date = controller.Get(new List<int>() { 100 }, new DateTime(2018, 01, 01));
            date.Success.ShouldBeFalse();
        }
    }
}
