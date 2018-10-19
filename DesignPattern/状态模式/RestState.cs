using System;

namespace DesignPattern
{
    class RestState : State
    {
        public override void WriteProgram(Work work)
        {
            Console.WriteLine("当前时间：{0}点 下班回家咯", work.Hour);
        }
    }
}