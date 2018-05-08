using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ZonaFl.Models
{
    [Table("ToDo")] // Table name
    public class Category
    {
        [Key] // Primary key
        public int Id { get; set; }
        [Column("Name", TypeName = "ntext")]
        public string  Name { get; set; }
       
        [Column("SubCategories")]
        public List<Category> SubCategories { get; set; }
        
        public List<Skill> Skills { get; set; }
    }
}