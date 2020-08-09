using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace aepistle.Web.Marketing
{
    public class MarketingService
    {
        static readonly string[] Scopes = { SheetsService.Scope.Spreadsheets };
        static readonly string ApplicationName = "aepistle";
        static readonly string sheet = "Sheet1";
        static readonly string SpreadsheetId = "10tbJ5hpYS1YbxRxb4i7GtEBbyxZc05zrrJq4FwWnjBg";
        static SheetsService service;
      public MarketingService(string email)
        {
            Init();
            WriteSheet(email);
            UserCredential credential;

        }
        static void Init()
        {
            GoogleCredential credential;
            //Reading Credentials File...
            using (var stream = new FileStream("D:/UPWORK/Projects/WEBMarketing/aepistle.Web.Marketing/google-credentials.json", FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream)
                    .CreateScoped(Scopes);
            }
            // Creating Google Sheets API service...
            service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
        }
        static void ReadSheet()
        {
            // Specifying Column Range for reading...
            var range = $"{sheet}!A:E";
            SpreadsheetsResource.ValuesResource.GetRequest request =
                    service.Spreadsheets.Values.Get(SpreadsheetId, range);
            // Ecexuting Read Operation...
            var response = request.Execute();
            // Getting all records from Column A to E...
            IList<IList<object>> values = response.Values;
            if (values != null && values.Count > 0)
            {
                foreach (var row in values)
                {
                    // Writing Data on Console...
                    Console.WriteLine("{0} ", row[0]);
                }
            }
            else
            {
                Console.WriteLine("No data found.");
            }
        }
        static void WriteSheet(string email)
        {
            // Specifying Column Range for reading...
            var range = $"{sheet}!A:E";
            List<IList<Object>> objNewRecords = new List<IList<Object>>();
            IList<Object> obj = new List<Object>();
            obj.Add(email);
            obj.Add(DateTime.UtcNow);
            objNewRecords.Add(obj);

            SpreadsheetsResource.ValuesResource.AppendRequest request =
              service.Spreadsheets.Values.Append(new ValueRange() { Values = objNewRecords }, SpreadsheetId, range);
            request.InsertDataOption = SpreadsheetsResource.ValuesResource.AppendRequest.InsertDataOptionEnum.INSERTROWS;
            request.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.RAW;
            var response = request.Execute();
        }
    }
}
