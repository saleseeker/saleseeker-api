namespace saleseeker_data
{
    // Unit
    public class Unit
    {
        public int UnitId { get; set; } // UnitID (Primary key)
        public string UnitName { get; set; } // UnitName (length: 30)

        // Reverse navigation

        /// <summary>
        /// Child Packs where [Pack].[ItemQuantityUnitID] point to this entity (FK_Pack_Unit)
        /// </summary>
        public virtual ICollection<Pack> Packs { get; set; } // Pack.FK_Pack_Unit

        public Unit()
        {
            Packs = new List<Pack>();
        }
    }

}
