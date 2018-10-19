using System;

namespace DesignPattern
{
    class NoonState : State
    {
        public override void WriteProgram(Work work)
        {
            if (work.Hour < 13)
            {
                Console.WriteLine("当前时间：{0}点 饿了，吃午饭；犯困，午休", work.Hour);
            }
            else
            {
                work.SetState(new AfterNoonState());
                work.WriteProgram();
            }
        }
    }
}