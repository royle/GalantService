using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.IO;
using System.Windows;

namespace GLTWarter.Printings
{
    public class Printing
    {
        public Printer Label { get; set; }
        public Printer BigLabel { get; set; }
        public Printer Report { get; set; }

        public double LabelLeftPadding
        {
            get { return DeploymentSettings.Default.PrintLabelLeftPadding; }
            set
            {
                DeploymentSettings.Default.PrintLabelLeftPadding = value;
                DeploymentSettings.Default.Save();
            }
        }

        public double LabelRightPadding
        {
            get { return DeploymentSettings.Default.PrintLabelRightPadding; }
            set
            {
                DeploymentSettings.Default.PrintLabelRightPadding = value;
                DeploymentSettings.Default.Save();
            }
        }

        public Thickness LabelPadding
        {
            get
            {
                // Convert Millimeter to 
                return new Thickness(MillimeterToDip(LabelLeftPadding), 0, MillimeterToDip(LabelRightPadding), 0);
            }
        }

        static double MillimeterToDip(double value)
        {
            return (value / 25.4) * 96.0;
        }

        public Printing()
        {
            // Initialize Label Printer object
            Label = new Printer(
                DeploymentSettings.Default.PrintLabelServer,
                DeploymentSettings.Default.PrintLabelQueue,
                DeploymentSettings.Default.PrintLabelSettings,
                DeploymentSettings.Default.PrintLabelIsEnable,
                PrintModes.Full
                );
            Label.PropertyChanged += new PropertyChangedEventHandler(Label_PropertyChanged);

            // Initialize Label Printer object
            BigLabel = new Printer(
                DeploymentSettings.Default.PrintBigLabelServer,
                DeploymentSettings.Default.PrintBigLabelQueue,
                DeploymentSettings.Default.PrintBigLabelSettings,
                DeploymentSettings.Default.PrintBigLabelIsEnable,
                PrintModes.Full
                );
            BigLabel.PropertyChanged += new PropertyChangedEventHandler(BigLabel_PropertyChanged);

            // Get print mode for Report. Default to Summary
            PrintModes reportPrintMode;
            try
            {
                reportPrintMode = (PrintModes)Enum.Parse(typeof(PrintModes), DeploymentSettings.Default.PrintReportMode);
            }
            catch (ArgumentException)
            {
                reportPrintMode = PrintModes.Summary;
            }
            Report = new Printer(
                DeploymentSettings.Default.PrintReportServer,
                DeploymentSettings.Default.PrintReportQueue,
                DeploymentSettings.Default.PrintReportSettings,
                DeploymentSettings.Default.PrintReportIsEnable,
                reportPrintMode);
            Report.PropertyChanged += new PropertyChangedEventHandler(Report_PropertyChanged);
        }

        void Label_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "ServerName":
                    DeploymentSettings.Default.PrintLabelServer = Label.ServerName;
                    break;
                case "QueueName":
                    DeploymentSettings.Default.PrintLabelQueue = Label.QueueName;
                    break;
                case "PrintTicket":
                    using (MemoryStream ms = Label.PrintTicket.GetXmlStream())
                    {
                        using (StreamReader sr = new StreamReader(ms))
                        {
                            DeploymentSettings.Default.PrintLabelSettings = sr.ReadToEnd();
                        }
                    }
                    break;
                case "IsEnable":
                    DeploymentSettings.Default.PrintLabelIsEnable = Label.IsEnable;
                    break;
                default:
                    return;
            }
            DeploymentSettings.Default.Save();
        }

        void BigLabel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "ServerName":
                    DeploymentSettings.Default.PrintBigLabelServer = BigLabel.ServerName;
                    break;
                case "QueueName":
                    DeploymentSettings.Default.PrintBigLabelQueue = BigLabel.QueueName;
                    break;
                case "PrintTicket":
                    using (MemoryStream ms = BigLabel.PrintTicket.GetXmlStream())
                    {
                        using (StreamReader sr = new StreamReader(ms))
                        {
                            DeploymentSettings.Default.PrintBigLabelSettings = sr.ReadToEnd();
                        }
                    }
                    break;
                case "IsEnable":
                    DeploymentSettings.Default.PrintBigLabelIsEnable = BigLabel.IsEnable;
                    break;
                default:
                    return;
            }
            DeploymentSettings.Default.Save();
        }

        void Report_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "ServerName":
                    DeploymentSettings.Default.PrintReportServer = Report.ServerName;
                    break;
                case "QueueName":
                    DeploymentSettings.Default.PrintReportQueue = Report.QueueName;
                    break;
                case "PrintTicket":
                    using (MemoryStream ms = Report.PrintTicket.GetXmlStream())
                    {
                        using (StreamReader sr = new StreamReader(ms))
                        {
                            DeploymentSettings.Default.PrintReportSettings = sr.ReadToEnd();
                        }
                    }
                    break;
                case "IsEnable":
                    DeploymentSettings.Default.PrintReportIsEnable = Report.IsEnable;
                    break;
                case "PrintMode":
                    DeploymentSettings.Default.PrintReportMode = Report.PrintMode.ToString();
                    break;
                default:
                    return;
            }
            DeploymentSettings.Default.Save();
        }
    }
}
