using System;

namespace DesignPattern
{
    class EveningState : State
    {
        public override void WriteProgram(Work work)
        {
            if (work.TaskFinish)
            {
                work.SetState(new RestState());
                work.WriteProgram();
            }
            else
            {
                if (work.Hour < 21)
                {
                    Console.WriteLine("当前时间：{0}点 加班哦，疲累之极", work.Hour);
                }
                else
                {
                    work.SetState(new SleepingState());
                    work.WriteProgram();
                }
            }

        }
    }
}