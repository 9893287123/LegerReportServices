
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Kernel.Colors;
using iText.Layout.Properties;
using iText.Kernel.Geom;
using iText.Kernel.Pdf.Canvas;
using iText.IO.Image;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Pdf.Colorspace;
using iText.Kernel.Pdf.Extgstate;
using System.Threading;
using System.Net.Mail;
using System.Net;
using System.IO;
using System.Configuration;
using System.Data.SqlClient;

namespace LegerReportServices
{
    public partial class Service1 : ServiceBase
    {
        System.Timers.Timer StatusRecheckTimer;
        FetchDataFrmDatabase _fetchDataFrmDatabase = new FetchDataFrmDatabase();
        public Service1()
        {
            InitializeComponent();
            testing();
        }

        protected override void OnStart(string[] args)
        {
            EventLog.WriteEntry("Ledger ON Start !!!!");
            int interval = Convert.ToInt32(ConfigurationManager.AppSettings["TimerInterval"]); // In minutes
            StatusRecheckTimer = new System.Timers.Timer();
            StatusRecheckTimer.Elapsed += new System.Timers.ElapsedEventHandler(timer1_Tick);
            StatusRecheckTimer.Interval = 60000; //10000 =  10sec
            StatusRecheckTimer.Interval = interval * 10000; //10000 =  10sec
            StatusRecheckTimer.Enabled = true;
            StatusRecheckTimer.AutoReset = true;
            StatusRecheckTimer.Start();
        }

        protected override void OnStop()
        {
        }
        public void timer1_Tick(object sender, System.Timers.ElapsedEventArgs args)
        {
            string MISReport = _fetchDataFrmDatabase.GetparameterDefaultvalue("MISLedgerReportActive");
            EventLog.WriteEntry(" Get Ledger MIS report!!!");
            if (MISReport == "1")
            {
                EventLog.WriteEntry("Active Ledger MIS report!!!!");
                string MISLedgerReportHourOfexec = _fetchDataFrmDatabase.GetparameterDefaultvalue("MISLedgerReportHourOfexec");
                if (DateTime.Now.Hour >= Convert.ToInt32(MISLedgerReportHourOfexec))
                {
                    EventLog.WriteEntry(" Match time!!!!");
                    SMSBO _objCheckSMS = new SMSBO();
                    _objCheckSMS = _fetchDataFrmDatabase.GETsmsExecutionDetails("GETLEDGERMIS");
                    if (_objCheckSMS.IsSMSExecuted == 0)
                    {
                        EventLog.WriteEntry(" is executed!!!!");
                        testing();
                        _fetchDataFrmDatabase.SaveExecutionDetails(1, "SAVELEDGERMIS");
                    }
                    else
                    {
                        EventLog.WriteEntry(" not executed!!!!");
                    }
                }
                else
                {
                    EventLog.WriteEntry(" not match Match time!!!!");
                }

            }
            EventLog.WriteEntry("Fetch Active User List!!!!");
            
        }
        public void testing()
        {
            DeleteFile("\\Users\\Dell_Owner\\source\\repos\\LegerReportServices\\LegerReportServices\\pdffile");
            DateTime today = DateTime.Now;
            string previousDate = today.AddDays(-1).Day.ToString();
            string previousMonth = today.AddDays(-1).Month.ToString();
            string previousyear = today.AddDays(-1).Year.ToString();
            string Titlepdf = "Leger Report of " + previousDate + '-' + previousMonth + '-' + previousyear;
            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            DataTable dtrecharge = new DataTable();
            string _resultOut = "NA";
            dt = _fetchDataFrmDatabase.GetAllActiveUser();

            //string pdfPath = @"D:\Testingservices\Testingservices\Testingcs\designed_pdf.pdf";
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    //EventLog.WriteEntry("DMT loop started");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string AtishayvendorId = dt.Rows[i]["AtishayVendorID"].ToString();
                        string EmailID = dt.Rows[i]["PersonEmailID"].ToString();
                        //AtishayvendorId = "UP4362449RT";
                        string Name = dt.Rows[i]["NameOnPANCard"].ToString();
                        string Mobileno = dt.Rows[i]["MobileNo"].ToString();
                        string[] namedata = Name.Split(' ');
                        //string filePath = "\\Testingservices\\Testingservices\\Testingcs\\" + namedata[0] + "-" + i+1 + "-" + previousDate + '-' + previousMonth + '-' + previousyear + "_legderReport.xlsx";
                        string PdfPath1 = "\\Users\\Dell_Owner\\source\\repos\\LegerReportServices\\LegerReportServices\\pdffile\\" + namedata[0] + "-" + i + 1 + "-" + previousDate + '-' + previousMonth + '-' + previousyear + "_legderReport.pdf";
                        using (PdfWriter writer = new PdfWriter(PdfPath1))
                        {
                            // Initialize the PDF document
                            PdfDocument pdf = new PdfDocument(writer);
                            //Document document = new Document(pdf, PageSize.A4);
                            Document document = new Document(pdf, iText.Kernel.Geom.PageSize.A4);
                            // Set document margins
                            document.SetMargins(50, 50, 50, 50);


                            // Add a title with styling
                            AddStyledTitle(document, Titlepdf);

                            // Add shapes (rectangle and line) to the PDF
                            //AddShapes(pdf);

                            // Add an image to the PDF
                            AddImage(document);

                            // Add some formatted text
                            AddFormattedText(document, Name, Titlepdf);
                            Addlegerreport(document, AtishayvendorId);
                            AddRechargeReport(document, AtishayvendorId, "Rechargedetail", "RECHARGE DETAILS");
                            AddRechargeReport(document, AtishayvendorId, "AEPS", "AEPS DETAILS");
                            //AddWatercolorBackground(pdf);
                            AddTextWatermark(pdf, "Atishay Limited");
                            // Close the document
                            document.Close();
                            SentMail(PdfPath1, EmailID, Name);
                        }
                        //dt1 = _fetchDataFrmDatabase.GetUserTransactionPreviesDate("CH5922999RT");
                        //dtrecharge = _fetchDataFrmDatabase.GetRechargeDetails(AtishayvendorId, "Rechargedetail");
                        //ConvertExcelToPdf(PdfPath1, Titlepdf, dtrecharge, dt1);
                        //  string excelfile=   SentBalanceSheetbyExcel(dt1, EmailID,Name,Mobileno,i, dtrecharge);
                        Thread.Sleep(2000);


                    }
                    // EventLog.WriteEntry("DMT loop Complete");
                }
                //else
                //{
                //    EventLog.WriteEntry("Data Not found for pending!!");
                //    _resultOut = "NA";
                //}
            }
            else
            {
                EventLog.WriteEntry("Data Not found!!");
                _resultOut = "NA";
            }

           
        }
      
  public void Addlegerreport(Document document,string AtishayvendorId)
        {
            DataTable legerdb = new DataTable();
            legerdb = _fetchDataFrmDatabase.GetUserTransactionPreviesDate(AtishayvendorId);
            Table pdfTable = new Table(6);
            pdfTable.SetWidth(UnitValue.CreatePercentValue(100)); // Set table width to 100% of the page width

            // Header row with bold text and a background color
            string[] headers = { "Opening Balance", "Credit", "Debit", "Closing Balance", "TransactionDate", "Narration" };
            foreach (var header in headers)
            {
                Cell cell = new Cell().Add(new Paragraph(header).SetBold());
                cell.SetBackgroundColor(ColorConstants.LIGHT_GRAY);
                cell.SetTextAlignment(TextAlignment.CENTER);
                cell.SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                pdfTable.AddCell(cell);
            }
            if (legerdb != null)
            {
                if (legerdb.Rows.Count > 0)
                {

                    //EventLog.WriteEntry("DMT loop started");
                    for (int i = 0; i < legerdb.Rows.Count; i++)
                    {
                        //  j = 3 + i;
                        string OpeningBalance = legerdb.Rows[i]["OpeningBalance"].ToString();
                        string Credit = legerdb.Rows[i]["Credit"].ToString();
                        string Debit = legerdb.Rows[i]["Debit"].ToString();
                        string ClosingBalance = legerdb.Rows[i]["ClosingBalance"].ToString();
                        string TransactionDate = legerdb.Rows[i]["TransactionDate"].ToString();
                        string Narration = legerdb.Rows[i]["Narration"].ToString();
                        pdfTable.AddCell(new Cell().Add(new Paragraph(OpeningBalance)).SetTextAlignment(TextAlignment.LEFT));
                        pdfTable.AddCell(new Cell().Add(new Paragraph(Credit)).SetTextAlignment(TextAlignment.LEFT));
                        pdfTable.AddCell(new Cell().Add(new Paragraph(Debit)).SetTextAlignment(TextAlignment.LEFT));
                        pdfTable.AddCell(new Cell().Add(new Paragraph(ClosingBalance)).SetTextAlignment(TextAlignment.LEFT));
                        pdfTable.AddCell(new Cell().Add(new Paragraph(TransactionDate)).SetTextAlignment(TextAlignment.LEFT));
                        pdfTable.AddCell(new Cell().Add(new Paragraph(Narration)).SetTextAlignment(TextAlignment.LEFT));


                    }
                    // EventLog.WriteEntry("DMT loop Complete");
                }
            }

            document.Add(pdfTable);
        }
        public void AddRechargeReport(Document document,string AtishayvendorId,string type,string title)
        {
            DataTable dtrecharge = new DataTable();
        
            dtrecharge = _fetchDataFrmDatabase.GetRechargeDetails(AtishayvendorId, type);
            if (dtrecharge != null)
            {
                if (dtrecharge.Rows.Count > 0)
                {
                    if (type == "Rechargedetail")
            {
                Paragraph Rechargetitle = new Paragraph(title)
    .SetFontSize(10)
    .SetBold()
    .SetTextAlignment(TextAlignment.CENTER)
    .SetFontColor(ColorConstants.BLUE)
    .SetMarginBottom(20);  // Spacing below the title
                document.Add(Rechargetitle);
            }
            else
            {
                Paragraph Rechargetitle = new Paragraph(title)
    .SetFontSize(10)
    .SetBold()
    .SetTextAlignment(TextAlignment.CENTER)
    .SetFontColor(ColorConstants.YELLOW)
    .SetMarginBottom(20);
                document.Add(Rechargetitle);
            }
           

            Table RechargeTable = new Table(6);
            RechargeTable.SetWidth(UnitValue.CreatePercentValue(100));
            //string[] headerRecharge = { "TransactionID", "Time", "MobileNo", "Amount", "Commission Amount","TDS","Final Commission","Status"};
                    string[] headerRecharge = { "TransactionID", "Time", "MobileNo", "Amount",  "Commission", "Status" };
                    foreach (var header in headerRecharge)
            {
                Cell cell = new Cell().Add(new Paragraph(header).SetBold());
                if (type == "Rechargedetail")
                {
                    cell.SetBackgroundColor(ColorConstants.GRAY);
                }
                else
                {
                    cell.SetBackgroundColor(ColorConstants.CYAN);
                }
                   
                cell.SetTextAlignment(TextAlignment.CENTER);
                cell.SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                RechargeTable.AddCell(cell);
            }
           

                    //EventLog.WriteEntry("DMT loop started");
                    for (int i = 0; i < dtrecharge.Rows.Count; i++)
                    {
                        //  j = 3 + i;
                       string ServiceType = dtrecharge.Rows[i]["ServiceType"].ToString();
                        string OperatorName = dtrecharge.Rows[i]["OperatorName"].ToString();
                        string TransactionID = dtrecharge.Rows[i]["TransactionID"].ToString();
                        string Trn_Time = dtrecharge.Rows[i]["Trn_Time"].ToString();
                        string CustomerMobileNumber = dtrecharge.Rows[i]["CustomerMobileNumber"].ToString();
                        string Amount = dtrecharge.Rows[i]["Amount"].ToString();
                        string TotalCommissionAmt = dtrecharge.Rows[i]["TotalCommissionAmt"].ToString();
                        string TDSAmt = dtrecharge.Rows[i]["TDSAmt"].ToString();
                        string FinalCommission = dtrecharge.Rows[i]["FinalCommission"].ToString();
                        string Trn_Status = dtrecharge.Rows[i]["Trn_Status"].ToString();
                        //string RefundedDate = dtrecharge.Rows[i]["RefundedDate"].ToString();


                        string tran = TransactionID + (ServiceType, OperatorName);
                        RechargeTable.AddCell(new Cell().Add(new Paragraph(tran)).SetTextAlignment(TextAlignment.LEFT));
                        RechargeTable.AddCell(new Cell().Add(new Paragraph(Trn_Time)).SetTextAlignment(TextAlignment.LEFT));
                        RechargeTable.AddCell(new Cell().Add(new Paragraph(CustomerMobileNumber)).SetTextAlignment(TextAlignment.LEFT));
                        RechargeTable.AddCell(new Cell().Add(new Paragraph(Amount)).SetTextAlignment(TextAlignment.LEFT));
                        //RechargeTable.AddCell(new Cell().Add(new Paragraph(TotalCommissionAmt)).SetTextAlignment(TextAlignment.LEFT));
                        //RechargeTable.AddCell(new Cell().Add(new Paragraph(TDSAmt)).SetTextAlignment(TextAlignment.LEFT));
                        RechargeTable.AddCell(new Cell().Add(new Paragraph(FinalCommission)).SetTextAlignment(TextAlignment.LEFT));
                        RechargeTable.AddCell(new Cell().Add(new Paragraph(Trn_Status)).SetTextAlignment(TextAlignment.LEFT));
                        //RechargeTable.AddCell(new Cell().Add(new Paragraph(RefundedDate)).SetTextAlignment(TextAlignment.LEFT));


                    }
                    // EventLog.WriteEntry("DMT loop Complete");
                    document.Add(RechargeTable);
                }
            }
            
        }
    static void AddWatercolorBackground(PdfDocument pdf)
        {

           
            // Iterate over the pages (for this example, we add the background to one page)
            int numberOfPages = pdf.GetNumberOfPages();
            for (int i = 1; i <= numberOfPages; i++)  // Adjust the loop for multiple pages as needed
            {
                // Add a new page
                //pdf.AddNewPage();

                // Get the page canvas
                PdfCanvas canvas = new PdfCanvas(pdf.GetPage(i));
                PdfExtGState extGState = new PdfExtGState().SetFillOpacity(0.5f);
                // Set the fill color with transparency to simulate watercolor
                Color watercolorColor = new DeviceRgb(128, 128, 255); // Light Blue (can be changed for different colors)
                canvas.SaveState();
                canvas.SetExtGState(extGState);

                canvas.SetFillColor(watercolorColor)
                    .Rectangle(0, 0, pdf.GetDefaultPageSize().GetWidth(), pdf.GetDefaultPageSize().GetHeight())
                    .Fill()  // Set opacity for translucent (watercolor) effect
                    
                    .RestoreState();
            }
        }
        static void AddStyledTitle(Document document,string Titlepdf)
        {
            // Title with custom font size, color, and alignment
          
         
            Paragraph title = new Paragraph(Titlepdf)
                .SetFontSize(12)
                .SetBold()
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFontColor(ColorConstants.BLUE)
                .SetMarginBottom(20);  // Space below the title

            document.Add(title);
        }

        static void AddShapes(PdfDocument pdf)
        {
            // Get the PdfCanvas from the first page
            PdfCanvas canvas = new PdfCanvas(pdf.AddNewPage());

            // Draw a rectangle with a background color
            canvas.SaveState()
                .SetFillColor(ColorConstants.LIGHT_GRAY)
                .Rectangle(50, 700, 500, 100)
                .Fill()
                .RestoreState();

            // Draw a line under the title
            canvas.MoveTo(50, 675)
                .LineTo(550, 675)
                .SetLineWidth(2)
                .Stroke();
        }

        static void AddImage(Document document)
        {
            // Load an image from a file path and add it to the document
            string imagePath = @"D:\Testingservices\Testingservices\Testingcs\test.png";
            ImageData imageData = ImageDataFactory.Create(imagePath);
            Image pdfImage = new Image(imageData);

            // Resize the image
            pdfImage.SetWidth(150);
            pdfImage.SetHeight(100);

            // Center-align the image
            pdfImage.SetHorizontalAlignment(HorizontalAlignment.CENTER);
            pdfImage.SetMarginBottom(20);  // Add some space after the image

            document.Add(pdfImage);
        }

        static void AddFormattedText(Document document,string name,string Titlepdf)
        {
            // Add a paragraph with styled text and background color
            Paragraph paragraph = new Paragraph("Good Morning " + name + "sir" )
                .SetFontSize(12)
                .SetFontColor(ColorConstants.DARK_GRAY)
                .SetBackgroundColor(ColorConstants.YELLOW);
                //.SetPadding(10)
                //.SetTextAlignment(TextAlignment.JUSTIFIED);

            document.Add(paragraph);
        }

        static void AddTextWatermark(PdfDocument pdf, string watermarkText)
        {
            int numberOfPages = pdf.GetNumberOfPages();

            for (int i = 1; i <= numberOfPages; i++)
            {
                PdfCanvas canvas = new PdfCanvas(pdf.GetPage(i));

                // Set the font and size for the watermark
                canvas.SaveState();
                canvas.BeginText()
                    .SetFontAndSize(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD), 30)
                    .SetColor(ColorConstants.GRAY, true)
                    .SetTextRenderingMode(PdfCanvasConstants.TextRenderingMode.FILL)
                    .MoveText(150, 400) // Adjust position of the watermark
                    .SetTextMatrix(2, 2)   // Rotation angle for diagonal text (optional)
                    .ShowText(watermarkText)
                    .EndText();

                // Restore state after drawing the watermark
                canvas.RestoreState();
            }
        }

        public void SentMail(string PdfPath1, string EmailId, string Username)
        {
            try
            {
                DateTime today = DateTime.Now;
                EmailId = "abhishek.rohila@atishay.com";
                string formattedDate = today.AddDays(-1).ToString("dd-MM-yyyy");
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                SmtpServer.EnableSsl = true;
                mail.From = new MailAddress("support@zapurse.com");
                mail.To.Add(EmailId); // Sending MailTo
                mail.Subject = "Legder Report" + formattedDate; // Mail Subject  
                mail.Body = "Hello  " + Username + System.Environment.NewLine + "*This is an automatically generated email, please do not reply*";
                System.Net.Mail.Attachment attachment;
                attachment = new System.Net.Mail.Attachment(PdfPath1); //Attaching File to Mail  
                mail.Attachments.Add(attachment);
                SmtpServer.Port = Convert.ToInt32("587"); //PORT  
                SmtpServer.EnableSsl = true;
                SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new NetworkCredential("support@zapurse.com", "Atishay@2024");
                SmtpServer.Send(mail);
                //DeleteFile(filePath);
                //DeleteFile(PdfPath1);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void DeleteFile(string filePath)
        {
            string[] files = Directory.GetFiles(filePath);

            // Loop through each file and delete it
            foreach (string file in files)
            {
                File.Delete(file);
                Console.WriteLine($"Deleted: {file}");
            }
        }


        
    }
}
