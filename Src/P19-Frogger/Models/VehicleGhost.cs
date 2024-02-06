namespace GameManager.Models
{
    /// <summary>
    /// The vehicles are drawn twice for most rows. In the case of the sports car
    /// that's not true!
    /// </summary>
    enum VehicleGhost
    {
        /// <summary>
        /// Do not draw 'ghost' cars.
        /// </summary>
        NoGhost,
        
        /// <summary>
        /// Draw 'ghost' cars.
        /// </summary>
        Ghost
    }
}
