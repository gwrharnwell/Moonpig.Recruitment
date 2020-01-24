# Moonpig Engineering recruitment test

Welcome to the Moonpig engineering test. Here at Moonpig we really value
quality code, and this test has been designed to allow you to show us how 
you think quality code should be written. 

To allow you to focus on the design and implementation of the code we have 
added all the use cases we expect you to implement to the bottom of the 
instructions. In return we ask that you make sure your implementation 
follows all the best practices you are aware of, and that at the end of it, 
the code you submit, is code you are proud of. 

We have not set a timelimit, we prefer that you spend some extra time to get
it right and write the highest quality code you can. Please feel free to make
any changes you want to the solution, add classes, remove projects etc. If you
change the request or response please update the example in the section below.

For bonus points, commit regularly and include the .git folder in your
submission. This will allow us to follow the evolution of your solution.

When complete please upload your solution and answers in a .zip to the google
drive link provided to you by the recruiter.

## Programming Exercise - Moonpig Post Office

You have been tasked with creating a service that calculates the estimated 
despatch dates of customers' orders. 

An order consists of an order date and a collection of products that a 
customer has added to their shopping basket. 

Each of these products is supplied to Moonpig on demand through a number of 
3rd party suppliers.

As soon as an order is received by a supplier, the supplier will start 
processing the order. The supplier has an agreed lead time in which to 
process the order before delivering it to the Moonpig Post Office.

Once the Moonpig Post Office has received all products in an order it is 
despatched to the customer.

**Assumptions**:

1. Suppliers start processing an order on the same day that the order is 
	received. For example, a supplier with a lead time of one day, receiving
	an order today will send it to Moonpig tomorrow.


2. For the purposes of this exercise we are ignoring time i.e. if a 
	supplier has a lead time of 1 day then an order received any time on 
	Tuesday would arrive at Moonpig on the Wednesday.

3. Once all products for an order have arrived at Moonpig from the suppliers, 
	they will be despatched to the customer on the same day.

### Part 1 

When the /api/DespatchDate endpoint is hit return the despatch date of that 
order.

### Part 2

Moonpig Post Office staff are getting complaints from customers expecting 
packages to be delivered on the weekend. You find out that the Moonpig post
office is shut over the weekend. Packages received from a supplier on a weekend 
will be despatched the following Monday.

Modify the existing code to ensure that any orders received from a supplier
on the weekend are despatched on the following Monday.

### Part 3

The Moonpig post office is still getting complaints... It turns out suppliers 
don't work during the weekend as well, i.e. if an order is received on the 
Friday with a lead time of 2 days, Moonpig would receive and dispatch on the 
Tuesday.

Modify the existing code to ensure that any orders that would have been 
processed during the weekend resume processing on Monday.

---

Parts 1 & 2 have already been completed albeit lacking in quality. Please review
the code, document the problems you find (see question 1), and refactor into
what you would consider quality code. 

Once you have completed the refactoring, extend your solution to capture the 
requirements listed in part 3.

Please note, the provided DbContext is a stubbed class which provides test 
data. Please feel free to use this in your implementation and tests but do 
keep in mind that it would be switched for something like an EntityFramework 
DBContext backed by a real database in production.

While completing the exercise please answer the questions listed below. 
We are not looking for essay length answers. You can add the answers in this 
document.

## Questions

Q1. What 'code smells' / anti-patterns did you find in the existing implemention of part 1 & 2?
* The DateTime _mlt variable should probably just be a local variable and not defined at the class level as it’s only used/only relevant to the Get method
* The DespatchDateController was creating a new instance over the DbContext class and was therefore tightly coupled to the DbContext class, rather than implementing the IDbContext interface. This would make it painful when switching out data providers. 
* The Product and Supplier models were defined inside the data project, but they’d likely be used elsewhere in the solution and therefore any other project wishing to use the models would need to be coupled to the data project.
* The GET method uses the LINQ Single() method multiple times, which throws an exception if an element doesn’t exist in a collection. The code then also tries to access properties of the fetched elements (SupplierID, LeadTime) without first checking if the elements aren’t null
* GET method was returning in multiple places when checking the day of the week. Better to return once where possible.
* The code was adding all of the supplier lead times on to the order date. So if there was a product with a lead time of 1 day, and a product with a lead time of 2 days then 3 days would have been added on to the order date instead of 2. 
Added additional fields to the DespatchDate object to inform users of any errors - whilst keeping the original structure intact (open for extension)/
* The test methods were using dynamic days (DateTime.Now) and expecting a result by just adding a day which is unpredictable. I’ve changed them to use the dates laid out in the Acceptance Criteria
Added extra unit tests to support Parts 2 and 3.

Q2. What best pracices have you used while implementing your solution?
* Dependency Inversion Principle - It’s important that solutions use interfaces where possible, to avoid tightly coupled code.
* Added Instance Constructor to DespatchDate. Better to have one place to create new instances of an object.
* I needed to use my EnsureDateIsWeekDay functionality multiple times so I extracted this to its own helper class, adhering to the DRY principal. 
* Clearly laid out code
* Refrained from writing long, complex methods and tried to refactor where possible.
* Consistent use of camel case for local variables, and starting private members with an underscore. Variables and method names are also clearly named.
* Clearly commented code (both inline and XML comments on new methods).
* Cleared unused code


Q3. What further steps would you take to improve the solution given more time?
* I’d introduce a Services layer that would sit between the Controllers and the Data Access classes. These services would take care of the business logic, such as looking up products and suppliers and calculating despatch dates. This way the controller would just route the query to a service, keeping the business logic in the controller actions to a minimum. 
* The solution should also take public holidays into account. It could discover these using an external API (a gov.uk dataset, for example).
* Could probably extend the standard DateTime class and add my datetime calculation methods for adding business days and ensuring a day is a weekday. 
* GET method’s return signature is DespatchDate. I prefer to always return an ActionResult or ActionResult<T> to take advantage of the built in status code returns. For example I’ve would return a NotFound() response if a product id was provided which doesn’t exist.

Q4. What's a technology that you're excited about and where do you see this 
    being applicable? (Your answer does not have to be related to this problem)
* The number one thing I’m excited about right now is Blazor. Blazor allows C# developers to be able to write dynamic and interactive front end web apps, in a way similar to the likes of React, Vue.js and so on, with very minimal javascript knowledge. The ability to be able to create cutting edge, single page web applications whilst still writing C# code is fascinating to me. I’ve been writing Blazor applications in my own time for about a year now and I’ve been closely following the roadmap and the features the team have been bringing out in each release. It’s clear Microsoft are listening to the dev community and they realise there’s a real need for .NET developers to be able to write front end, REST API driven web applications. I’m confident it’s going to improve the speed and quality of the applications I write going forward, and actually make front end development more enjoyable! 

## Request and Response Examples

Please see examples for how to make requests and the expected response below.

### Request

The service is setup as a Web API and takes a request in the following format

~~~~ 
GET /api/DespatchDate?ProductIds={product_id}&orderDate={order_date} 
~~~~

e.g.

~~~~ 
GET /api/DespatchDate?ProductIds=1&orderDate=2018-01-29T00:00:00
GET /api/DespatchDate?ProductIds=2&ProductIds=3&orderDate=2018-01-29T00:00:00 
~~~~

### Response

The response will be a JSON object with a date property set to the resulting 
Despatch Date

~~~~ 
{
    "date" : "2018-01-30T00:00:00"
}
~~~~ 

## Acceptance Criteria

### Lead time added to despatch date  

**Given** an order contains a product from a supplier with a lead time of 1 day  
**And** the order is place on a Monday - 01/01/2018  
**When** the despatch date is calculated  
**Then** the despatch date is Tuesday - 02/01/2018  

**Given** an order contains a product from a supplier with a lead time of 2 days  
**And** the order is place on a Monday - 01/01/2018  
**When** the despatch date is calculated  
**Then** the despatch date is Wednesday - 03/01/2018  

### Supplier with longest lead time is used for calculation

**Given** an order contains a product from a supplier with a lead time of 1 day  
**And** the order also contains a product from a different supplier with a lead time of 2 days  
**And** the order is place on a Monday - 01/01/2018  
**When** the despatch date is calculated  
**Then** the despatch date is Wednesday - 03/01/2018  

### Lead time is not counted over a weekend

**Given** an order contains a product from a supplier with a lead time of 1 day  
**And** the order is place on a Friday - 05/01/2018  
**When** the despatch date is calculated  
**Then** the despatch date is Monday - 08/01/2018  

**Given** an order contains a product from a supplier with a lead time of 1 day  
**And** the order is place on a Saturday - 06/01/18  
**When** the despatch date is calculated  
**Then** the despatch date is Tuesday - 09/01/2018  

**Given** an order contains a product from a supplier with a lead time of 1 days  
**And** the order is place on a Sunday - 07/01/2018  
**When** the despatch date is calculated  
**Then** the despatch date is Tuesday - 09/01/2018  

### Lead time over multiple weeks

**Given** an order contains a product from a supplier with a lead time of 6 days  
**And** the order is place on a Friday - 05/01/2018  
**When** the despatch date is calculated  
**Then** the despatch date is Monday - 15/01/2018  

**Given** an order contains a product from a supplier with a lead time of 11 days  
**And** the order is place on a Friday - 05/01/2018  
**When** the despatch date is calculated  
**Then** the despatch date is Monday - 22/01/2018