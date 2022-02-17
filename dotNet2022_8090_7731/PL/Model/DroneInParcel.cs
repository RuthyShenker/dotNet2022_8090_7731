namespace PO
{
    /// <summary>
    /// A class of DroneInParcel that contains:
    /// Id
    /// BatteryStatus
    ///CurrLocation
    /// </summary>
    public class DroneInParcel
    {
        /// <summary>
        /// this field is init.
        /// </summary>
        public int Id { get; init; }
        public double BatteryStatus { get; set; }
        public Location CurrLocation { get; set; }
    }
}