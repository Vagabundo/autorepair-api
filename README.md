# autorepair-api

Back end:

I will build a server that receives information about a repair job as a json.
It will be an array of jobs (item and number of pieces), a total of labour hours
and a total price.

I use Postman for the requests as the web is not build yet.

I start the web API using Visual Studio Code 1.38.1 and .Net Core version 2.2.203

I build the job and request models in order to test we are getting the information successfully.

Then I create the ResponseManager which will receive the sheet, delegate into other managers to check the rules and return an answer accordingly.

I have created a manager class for each type of job we have a rule to check, so it can be increased if necessary.

In the first place I check the rules and then the labour hours and price. For each check, an error message is added to the answer if something went wrong. The code could have been cleaner avoiding the messages “feature”, but it is already done.

The request is responded asynchronously.

• Approve - If the total price is within 10% of the reference price.
• Refer - If the total price is between 10% and 15% of the reference price.
• Decline - If the total price exceeds 15% of the reference price.

Those % made no sense to me and I spent a while trying to figure out if I missed something. There were two possibilities that made sense to me: a 0 was missed (100% and 150%) or you meant to exceeds that % of the reference (110% and 115%). I chose the second one, but any the difference is just change a number in the code.

About the data, I have added structure to use a database named autorepair. The connection was added in the config file and the dbcontext was added to the services in startup.cs. The entity models are created and the tables are named in the RepairJobsRepository class. All the mocked stuff can be replaced for the dbsets once the DB is created and connected.

For the tests I used Nunit and Moq. As I say in a test class, the db is already mocked but I use Moq to show I know how to use it. Actually I have more experience using Rhino mock and in .Net framework, but once you know the rules, all you have to do is learn how to do with other framework.

To run the server, open the project run command dotnet run. To run the tests, open the test project and run dotnet test
