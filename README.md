Here are some notes for your consideration.

About the Code and Framework:
-The project used a command prompt solution with .net core 6.
-I used Visual Studio 2022 to develop it.
-Only .net core 6 default libraries and components were used on this solution.
-TripOrganizer.cs is the main class in charge of calculate/organize the trips for each drone
-program.cs is just the class that executes the TripOrganizer class and shows the results.
-inside the project is a Input.txt file, pls copy that file on %SolutionDirectory%/bin/Debug/net6.0/Input.txt

About the solution provided:

I organize the trips for drones based on this assumptions (extracted from the PDF document):

-A drone could not have associated trips
-We need to find the minimal trips for each drone
-There is no restriction for max trips for drones
-On each trip we need to reach the maximum quantity of locations

Also I inferred other assumptions:

-Based on the list above, a drone could take all the trips by itself
-Based on reaching a maximum quantity of locations, drones with more capacity had more priority on auto assignment
-For more optimization on the assignment I try to find the max/min (maximum value of the minimum) of the unassigned locations that match the remaining capacity of the trip.

Similar details explaining the solution provided could be found in the comments of the project. Just the most meaningful text was shown on the command prompt to assure 
a light and straightforward user experience.
