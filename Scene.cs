using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace torc
{
    class Scene
    {
        public Camera main { get; private set; }

        private List<GameObject> objects = new();
        public void Add(GameObject obj)
        {
            objects.Add(obj);
        }

        public void Update()
        {
            foreach (GameObject o in objects)
                o.Update();
        }

        public void Render()
        {
            if (main == null)
            {
                foreach (GameObject o in objects)
                {
                    Camera cam = o.GetComponent<Camera>();
                    if (cam != null)
                        main = cam;
                    break;
                }
            }

            foreach (GameObject o in objects)
                o.Render();
        }
    }
}
