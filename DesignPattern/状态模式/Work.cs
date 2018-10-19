using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPattern
{
    class Work
    {
        private State state;
        private int hour;
        public int Hour
        {
            get { return hour; }
            set { hour = value; }
        }

        private bool finish;
        public bool TaskFinish
        {
            get { return finish; }
            set { finish = value; }
        }

        public Work()
        {
            this.state = new ForenoonState();
        }

        internal void SetState(State state)
        {
            this.state = state;
        }

        internal void WriteProgram()
        {
            state.WriteProgram(this);
        }
    }
}
