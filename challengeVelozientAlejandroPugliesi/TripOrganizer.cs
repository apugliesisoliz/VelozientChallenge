using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace challengeVelozientAlejandroPugliesi
{
    /// <summary>
    /// Class in charge of finding the best organization for Drone trips/deliveries
    /// </summary>
    internal class TripOrganizer
    {
        private readonly string _inputFileName = "/Input.txt";
        private string _erroMessage = "";
        private List<Configuration> _dronConfig = new List<Configuration>();
        private List<Configuration> _locationConfig;
        private List<TripResult> _tripResult;
        /// <summary>
        /// I fill the private variables _dronConfig and _locationConfig reading from the txt file
        /// popular the _errorMessage when the file is missing
        /// </summary>
        public TripOrganizer()
        {
            _dronConfig = new List<Configuration>();
            _locationConfig = new List<Configuration>();
            _tripResult = new List<TripResult>();
            if (File.Exists(System.Environment.CurrentDirectory + _inputFileName))
            {
                string[] lines = File.ReadAllLines(System.Environment.CurrentDirectory + _inputFileName);
                fillDroneConfig(lines[0]);
                fillLocationConfig(lines);
            }
            else 
            {
                _erroMessage += "Input file missing. at " + System.Environment.CurrentDirectory + _inputFileName;
            }
        }
        
        /// <summary>
        /// this just return the error string
        /// </summary>
        /// <returns></returns>
        public string getErrors()
        {
            return _erroMessage;
        }
        /// <summary>
        /// main function of the class, call another funtion in charge of organizing the trips. 
        /// returns an rodered list of Trip Results
        /// </summary>
        /// <returns></returns>
        public List<TripResult> calculateTrips()
        {
            foreach (var drone in _dronConfig)
            {
                getAllPossibleTrips(drone);
            }
            return _tripResult.OrderBy(x => x.DroneName).ToList();
        }
        /// <summary>
        /// organize the trips for drones based on this assumptions (extracted from the PDF document):
        /// 1.- a drone could not have associated trips
        /// 2.- we need to find the minimal trips for each drone
        /// 3.- there is no restriction for maxtrips for drones
        /// 4.- on each trip we need to reach the maximum quantity of locations
        /// 
        /// also I inferred other assumptions
        /// 5.- based on the list above, a drone could take all the trips by itself
        /// 6.- based on reaching a maximum quantity of locations, drones with more capacity had more priority on auto assignment
        /// </summary>
        /// <param name="drone">Drone config variable provided</param>
        private void getAllPossibleTrips(Configuration drone)
        {
            var result = new List<TripResult>();
            var droneNotUsed = true;
            var tripCount = 1;
            foreach (var loc in _locationConfig)
            {
                if (!_tripResult.Any(x => x.Locations.Any(x=> x.Equals(loc.Name))))
                {
                    if (loc.MaxWeight <= drone.MaxWeight)
                    {
                        droneNotUsed = false;
                        var trip = new TripResult();
                        trip.DroneName = drone.Name;
                        trip.TripNumber = tripCount;
                        trip.Locations.Add(loc.Name);
                        var sumWeight = loc.MaxWeight;
                        while (sumWeight <= drone.MaxWeight)
                        {
                            // I try to find the max/min (maximun value of the minimun) of the unassigned locations that match the remaining capacity of the trip
                            // also we validates if the location was not schedulled before
                            var nextLoc = _locationConfig.Where(x => x.MaxWeight <= (drone.MaxWeight - sumWeight) && !trip.Locations.Any(y => y.Equals(x.Name))
                                            && !_tripResult.Any(z=> z.Locations.Any(t=> t.Equals(x.Name))))
                                            .OrderByDescending(y => y.MaxWeight).FirstOrDefault();
                            if (nextLoc != null)
                            {
                                sumWeight += nextLoc.MaxWeight;
                                trip.Locations.Add(nextLoc.Name);
                            }
                            else 
                            {
                                break;
                            }
                        }
                        _tripResult.Add(trip);
                        tripCount++;
                    }
                }
            }
            // if drone had no assignations, just add a blank trip to the list
            if (droneNotUsed)
            {
                _tripResult.Add(new TripResult { 
                    DroneName = drone.Name,
                    TripNumber = 0,
                    Locations = new List<string> ()
                });
            }
        }
        /// <summary>
        /// this function populates the private variable _locationConfigs
        /// </summary>
        /// <param name="lines">list of configuration for location</param>
        private void fillLocationConfig(string[] lines)
        {
            for (int i = 1; i < lines.Length; i++)
            {
                var configs = lines[i].Split(",");
                _locationConfig.Add(new Configuration
                {
                    Name = configs[0].Trim(),
                    MaxWeight = Convert.ToDouble(configs[1].Trim().Replace("[", "").Replace("]", ""))
                });
            }
        }
        /// <summary>
        /// this function populates the private variable _droneConfig
        /// </summary>
        /// <param name="v">list of configuration for drones</param>
        private void fillDroneConfig(string v)
        {
            var configs = v.Split(",");
            for(int i = 0; i < configs.Length; i = i + 2)
            {
                _dronConfig.Add(new Configuration
                {
                    Name = configs[i].Trim(),
                    MaxWeight = Convert.ToDouble(configs[i+1].Trim().Replace("[","").Replace("]",""))
                });
            }
            _dronConfig = _dronConfig.OrderByDescending(x=> x.MaxWeight).ToList();
        }
    }
    /// <summary>
    /// this unique class serve to define the configuration for both drone and locations
    /// </summary>
    public class Configuration
    { 
        public string Name { get; set; }
        public double MaxWeight { get; set; }
    }
    /// <summary>
    /// this class helpus to organize a better result response to the main program
    /// </summary>
    public class TripResult
    {
        public TripResult()
        {
            Locations = new List<string>();
        }
        public string DroneName { get; set; }
        public int TripNumber { get; set; }
        public List<string> Locations{ get; set; }
    }
}

