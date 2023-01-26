

using challengeVelozientAlejandroPugliesi;

Console.WriteLine("Hi, this a small program that will help you to organize your deliveries, ");
Console.WriteLine();
Console.WriteLine("reading the information needed from the \"Input\" config file, saved on %SolutionDirectory%/bin/Debug/net6.0/Input.txt ");
Console.WriteLine();
var trips = new TripOrganizer();
if (trips.getErrors().Length > 0)
{
    Console.WriteLine(trips.getErrors());
    Console.WriteLine("Please, provide the required information and restart the program.");
    Console.WriteLine();
}
else 
{
    var result = trips.calculateTrips();
    Console.WriteLine("Calculation Successfull!!!!!....");
    Console.WriteLine("Listing the recomendation below:");
    Console.WriteLine();

    foreach (var item in result)
    {
        Console.WriteLine("Drone:" + item.DroneName + ", #Trip:" + item.TripNumber.ToString() + ", Locations:" + string.Join(",", item.Locations));
    }
}
Console.ReadKey();
