using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern
{
    class GameRole
    {
        private int vit;
        /// <summary>
        /// 生命力
        /// </summary>
        public int Vitality
        {
            get { return vit; }
            set { vit = value; }
        }

        private int atk;
        /// <summary>
        /// 攻击力
        /// </summary>
        public int Attack
        {
            get { return atk; }
            set { atk = value; }
        }

        private int def;
        /// <summary>
        /// 防御力
        /// </summary>
        public int Defense
        {
            get { return def; }
            set { def = value; }
        }

        public void Display()
        {
            Console.WriteLine("角色当前状态：");
            Console.WriteLine("体力:{0}", Vitality);
            Console.WriteLine("攻击力:{0}", Attack);
            Console.WriteLine("防御力:{0}", Defense);
            Console.WriteLine("");
        }

        public void GetInitState()
        {
            this.vit = 100;
            this.atk = 100;
            this.def = 100;
        }

        public void Fight()
        {
            this.vit = 0;
            this.atk = 0;
            this.def = 0;
        }

        public RoleStateMemento SaveState()
        {
            return new RoleStateMemento(vit, atk, def);
        }

        public void RecoveryState(RoleStateMemento memento)
        {
            this.vit = memento.Vitality;
            this.atk = memento.Attack;
            this.def = memento.Defense;
        }
    }
}
