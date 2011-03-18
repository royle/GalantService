using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Threading;

namespace GLTWarter.ExternalData
{
    public delegate void WriterProgressHandler(int progress);

    interface IExcelExporter
    {
        /// <summary>
        /// 导出文件路径
        /// </summary>
        string Filename
        {
            get;
        }

        SynchronizationContext Context
        {
            get;
            set;
        }
    }

    public class ExcelExporterBase : BackgroundWorker, IExcelExporter
    {
        public string Filename
        {
            get;
            set;
        }

        public SynchronizationContext Context
        {
            get;
            set;
        }

        protected void RaiseProgress(int progress)
        {
            if (Context != null)
            {
                Context.Post((SendOrPostCallback)delegate(object state)
                {
                    this.OnProgressChanged(new ProgressChangedEventArgs(progress, null));
                }, null);
            }
            else
            {
                this.OnProgressChanged(new ProgressChangedEventArgs(progress, null));
            }
        }
    }
}
