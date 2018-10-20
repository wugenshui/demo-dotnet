namespace DesignPattern
{
    internal class RoleStateMemento
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

        public RoleStateMemento(int vit, int atk, int def)
        {
            this.vit = vit;
            this.atk = atk;
            this.def = def;
        }
    }
}