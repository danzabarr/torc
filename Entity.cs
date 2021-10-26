using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace torc
{
    class Entity
    {
        public Transform Transform => (Transform)components[0];
        private List<Component> components;
        
        public Entity()
        {
            components = new List<Component>();
        }

    }
}
