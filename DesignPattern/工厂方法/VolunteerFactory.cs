using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern
{
    class VolunteerFactory : IFactory
    {
        public LeiFeng CreateLeiFeng()
        {
            return new Volunteer();
        }
    }
}
