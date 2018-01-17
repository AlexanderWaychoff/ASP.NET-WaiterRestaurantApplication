using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Twilio;
using Twilio.AspNet.Mvc;
using Twilio.Rest.Api.V2010.Account;
using Twilio.TwiML;
using WaiterRestaurantApplication.Models;

namespace WaiterRestaurantApplication.Controllers
{
    public class MessengerController : TwilioController
    {
        private string accountSid = "AC723d3d5a612a56560748da63634012d6";
        private string authToken = "f261d85394f7213a2ac3bc294f64802c";
        private string TwilioAccountPhoneNumber = "+14144090727";
        private ApplicationDbContext db = new ApplicationDbContext();


        public MessengerController()
        {
            TwilioClient.Init(accountSid, authToken);
        }
        // GET: Messenger
        public ActionResult Index()
        {
            return View();
        }

        public bool SendSMSMessage(string recipientPhoneNumber, string messageBody)
        {
            try
            {
                MessageResource.Create(
                    from: new Twilio.Types.PhoneNumber("+14144090727"),
                    to: new Twilio.Types.PhoneNumber(recipientPhoneNumber),
                     body: messageBody);

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                return false;
            }
        }

        public ActionResult RecieveSMSMessage(string From, string Body)
        {
            var responseMessage = new MessagingResponse();
            responseMessage.Message("Hi, Thank you for using waiter! We Apreciate your feedback :)");

            From = From.Remove(0, 2);

            TableVisit currentTableVisit = db.TableVisits
                .Where(r => r.DinerPhone == From)
                .FirstOrDefault();

            if(Body == "y")
            {
                currentTableVisit.IsSatisfied = true;

                db.SaveChanges();
            }
            if(Body == "n")
            {
                currentTableVisit.IsSatisfied = false;
                db.SaveChanges();
            }

            return TwiML(responseMessage);
        }
    }
}