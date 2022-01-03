namespace BO
{
    /// <summary>
    /// An Enum of WeightCategories contains:
    /// Light, Medium, Heavy.
    /// </summary>
    public enum WeightCategories { Light = 1, Medium, Heavy };

    /// <summary>
    /// An Enum of UrgencyStatuses contains:
    /// Normal, Fast, Emergency.
    /// </summary>
    public enum Priority { Normal = 1, Fast, Emergency };

    /// <summary>
    /// An Enum of DroneStatuses contains:
    /// Free, Maintenance, Delivery.
    /// </summary>
    public enum DroneStatus { Free = 1, Maintenance, Delivery };

    /// <summary>
    /// An Enum of ParcelStatus contains:
    /// made, belonged, collected,InDestination.
    /// </summary>
    public enum ParcelStatus { made = 1, belonged, collected, InDestination }
}

