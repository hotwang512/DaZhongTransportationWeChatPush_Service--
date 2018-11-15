using System;
using System.ServiceProcess;
using System.Threading;
using Service;
using ServiceLog;


namespace WeChatPushWindowsService
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        Thread pushThread = null;
        Thread deleteThread = null;

        private Thread UpdateThread;
        protected override void OnStart(string[] args)
        {
            try
            {
                //LogManager.WriteLog(LogFile.Trace, DateTime.Now.ToString("yyyy-MM-dd") + "开始启动推送服务");
                pushThread = new Thread(Service.MainFunction.PushContentFunction);
                pushThread.Start();
                //  LogManager.WriteLog(LogFile.Trace, DateTime.Now.ToString("yyyy-MM-dd") + "推送服务启动完成");


                deleteThread = new Thread(Service.MainFunction.UpdatePersonData);//同步人员数据
                deleteThread.Start();

                UpdateThread = new Thread(MainFunction.UpdatePersonStatus);
                UpdateThread.Start();
            }
            catch (Exception exp)
            {
                LogManager.WriteLog(LogFile.Error, exp.ToString());
            }
        }

        protected override void OnStop()
        {
            //关闭Host
            if (pushThread != null)
            {
                pushThread.Abort();
            }
            if (deleteThread != null)
            {
                deleteThread.Abort();
            }
            if (UpdateThread != null)
            {
                UpdateThread.Abort();
            }

        }
    }
}
