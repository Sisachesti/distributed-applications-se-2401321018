using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learn2Gether.Domain.Entities
{
    // Base entity class with common properties for all entities
    public class BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
    }
}
