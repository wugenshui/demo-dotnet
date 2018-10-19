using System;

namespace DesignPattern
{
    class SleepingState : State
    {
        public override void WriteProgram(Work work)
        {
            Console.WriteLine("当前时间：{0}点 不行了，睡着了", work.Hour);
        }
    }
}