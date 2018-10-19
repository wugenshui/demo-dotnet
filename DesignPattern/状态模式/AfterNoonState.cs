using System;

namespace DesignPattern
{
    class AfterNoonState : State
    {
        public override void WriteProgram(Work work)
        {
            if (work.Hour <= 18)
            {
                Console.WriteLine("当前时间：{0}点 下午状态还不错，继续努力", work.Hour);
            }
            else
            {
                work.SetState(new EveningState());
                work.WriteProgram();
            }
        }
    }
}