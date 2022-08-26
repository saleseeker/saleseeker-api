namespace saleseeker_data
{
    // Category
    public class Category
    {
        public int CategoryId { get; set; } // CategoryID (Primary key)
        public int? ParentCategoryId { get; set; } // ParentCategoryId
        public string CategoryName { get; set; } // CategoryName (length: 100)
        public string CategoryDescription { get; set; } // CategoryDescription (length: 200)

        // Reverse navigation

        /// <summary>
        /// Child Categories where [Category].[ParentCategoryID] point to this entity (FK_Category_Category)
        /// </summary>
        public virtual ICollection<Category> Categories { get; set; } // Category.FK_Category_Category

        public Category()
        {
            Categories = new List<Category>();
        }
    }
}
