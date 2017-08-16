using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apothecary
{
    class Relations
    {

        private Dictionary<string, List<EssentialOil>> relations;

        public Relations(Dictionary<string, List<EssentialOil>> relations)
        {
            this.relations = relations;
        }
    }
}
