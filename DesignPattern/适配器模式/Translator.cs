using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern
{
    class Translator : Player
    {
        private ForeignCenter foreignCenter;

        public Translator(string name) : base(name)
        {
            this.foreignCenter = new ForeignCenter(name);
        }

        public override void Attack()
        {
            foreignCenter.进攻();
        }

        public override void Defence()
        {
            foreignCenter.防守();
        }
    }
}
