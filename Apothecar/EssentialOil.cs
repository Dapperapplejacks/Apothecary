using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apothecary
{

    class EssentialOil
    {
        private string name;
        private List<string> descriptors;
        private List<EssentialOil> relations;

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        public List<string> Descriptors
        {
            get
            {
                return descriptors;
            }
            private set
            {
                descriptors = value;
            }
        }

        public List<EssentialOil> Relations
        {
            get
            {
                return relations;
            }
            private set
            {
                relations = value;
            }
        }

        public EssentialOil(string name)
        {
            this.name = name;
            descriptors = new List<string>();
        }

        public void AddDescriptor(string desc)
        {
            descriptors.Add(desc);
        }

        public void RemoveDescriptor(string desc)
        {
            if (descriptors.Contains(desc))
            {
                descriptors.Remove(desc);
            }
        }

        public void AddRelation(EssentialOil secondOil)
        {
            if (!relations.Contains(secondOil))
            {
                relations.Add(secondOil);
            }
        }

        public void RemoveRelation(EssentialOil secondOil)
        {
            if (relations.Contains(secondOil))
            {
                relations.Remove(secondOil);
            }
        }

        
    }
}
