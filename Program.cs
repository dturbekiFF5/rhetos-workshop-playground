using ConsoleDump;
using Rhetos;
using Rhetos.Dom.DefaultConcepts;
using Rhetos.Logging;
using Rhetos.Utilities;

void Main() {
    ConsoleLogger.MinLevel = EventType.Info; // Use EventType.Trace for more detailed log.
    string rhetosHostAssemblyPath = @"..\..\..\..\rhetos-workshop\bin\Debug\net6.0\Bookstore.Service.dll";
    using (var scope = LinqPadRhetosHost.CreateScope(rhetosHostAssemblyPath)) {
        var context = scope.Resolve<Common.ExecutionContext>();
        var repository = context.Repository;

        // Query data from the `Common.Claim` table:

        // Creating a "playground" console app
        // https://github.com/Rhetos/Rhetos/wiki/Creating-a-playground-console-app
        //var claims = repository.Common.Claim.Query()
        //    .Where(c => c.ClaimResource.StartsWith("Common.") && c.ClaimRight == "New")
        //    .ToSimple(); // Removes ORM navigation properties from the loaded objects.

        //claims.ToString().Dump("Common.Claims SQL query");
        //claims.Dump("Common.Claims items");

        //// Add and remove a `Common.Principal`:

        //var testUser = new Common.Principal { Name = "Test123", ID = Guid.NewGuid() };
        //repository.Common.Principal.Insert(new[] { testUser });
        //repository.Common.Principal.Delete(new[] { testUser });

        //// Print logged events for the `Common.Principal`:

        //repository.Common.LogReader.Query()
        //    .Where(log => log.TableName == "Common.Principal" && log.ItemId == testUser.ID)
        //    .ToList()
        //    .Dump("Common.Principal log");

        //Console.WriteLine("Done.");


        // 3) 
        //var allBooks = repository.Bookstore.Book.Load();
        //foreach (var book in allBooks) {
        //    var author = repository.Bookstore.Person.Load(new[] { book.AuthorID }).Single().Dump();
        //    Console.WriteLine(book.Title + ", " + author.Name);
        //}

        // 4)
        var query = repository.Bookstore.Book.Query()
            .Select(book => new { book.Title, book.Author.Name });
        var items = query.ToList();
        items.Dump();
        foreach (var item in items) {
            Console.WriteLine(item);
        }
        // 5)
        query.ToString().Dump();

        // 6)
        var actionParameter = new Bookstore.InsertManyBooks {
            NumberOfBooks = 3,
            TitlePrefix = "A Song of Ice and Fire"
        };
        // repository.Bookstore.InsertManyBooks.Execute(actionParameter);

        // Day 3
        var filterParameter = new Bookstore.LongBooks3 {
            MinimumPages = 100,
            ForeignBooksOnly = true
        };
        var queryFilter = repository.Bookstore.Book.Query(filterParameter);
        queryFilter.ToString().Dump(); // Print the SQL query.
        queryFilter.ToSimple().ToList().Dump(); // Load and print the books.

        // repository.Bookstore.Disposal.Insert(
        // new Bookstore.Disposal { BookID = new Guid("712c2146-a6fc-4550-a8ec-ee15df2a4b85") });

        // scope.CommitAndClose(); // Database transaction is rolled back by default.
    }
}
Main();
