using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Globalization;

namespace GLTWarter.Util
{
    public sealed class ExcelCultureBlock : IDisposable
    {
        CultureInfo backupCulture;
        public ExcelCultureBlock()
        {
            backupCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        }

        public void Dispose()
        {
            Thread.CurrentThread.CurrentCulture = backupCulture;
        }
    }
}
