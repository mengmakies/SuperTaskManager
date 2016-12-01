using System;
using Quartz;

namespace Super.Service.Tasks.Jobs
{   
    public class TestJob : BaseJob
    {
        public override void ExcuteTask()
        {
            base.ExcuteTask();
            try
            {
                Log.Info("测试任务,当前系统时间:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            catch (Exception ex)
            {
                JobExecutionException e2 = new JobExecutionException(ex);
                Log.Info("测试任务异常", ex);
                //1.立即重新执行任务 
                e2.RefireImmediately = true;
                //2 立即停止所有相关这个任务的触发器
                //e2.UnscheduleAllTriggers=true; 
            }
        }
    }
}
