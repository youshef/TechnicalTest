using System;
using System.Collections.Generic;
using System.Text;

namespace TechnicalTest.Data.Models
{
    public interface IEntity
    {
        Guid Id { get; set; }
    }
}
