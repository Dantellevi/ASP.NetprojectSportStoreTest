using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;


namespace SportsStore.Domain.Concrete
{
    public class EmailSettings
    {
        public string MailToAdress = "melkipotrashitel003@gmail.com";
        public string MailFromAdress = "sportsstore@gmail.com";
        public bool UseSel = true;
        public string UserName = "MySmtpUserName";
        public string Password = "MySmtpPassword";
        public string ServerName = "smtp.examle.com";
        public int ServerPort = 587;
        public bool WriteAsFile = false;
        public string FileLocation = @"C:\sports_store_emails";


    }







  public  class EmailOrderProcessor : IOrderProcessor
    {
        public EmailSettings emailsettings;
        public EmailOrderProcessor(EmailSettings settings)
        {
            emailsettings = settings;

        }
        public void Processor(Cart cart, ShippingDetails shippingInfo)
        {
            using (var smtpClient=new SmtpClient())
            {
                smtpClient.EnableSsl = emailsettings.UseSel;
                smtpClient.Host = emailsettings.ServerName;
                smtpClient.Port = emailsettings.ServerPort;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(emailsettings.UserName, emailsettings.Password);

                if (emailsettings.WriteAsFile)
                {
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    smtpClient.PickupDirectoryLocation = emailsettings.FileLocation;
                    smtpClient.EnableSsl = false;
                }

                StringBuilder body = new StringBuilder()
                    .AppendLine("Новый заказ подтвержден!!!")
                    .AppendLine("-----------------")
                    .AppendLine("Элементы:");

                foreach(var line in cart.Lines)
                {
                    var subtotal = line.Product.price * line.Quantity;
                    body.AppendFormat("{0} x {1} (subtutorial: {2:c}", line.Quantity, line.Product.Name, subtotal);

                }

                body.AppendFormat("Общие заказ :{0:c}", cart.ComputerTotalValue())
                    .AppendLine("--------------------")
                    .AppendLine("Адрес получателя:")
                    .AppendLine(shippingInfo.Name)
                    .AppendLine(shippingInfo.Line1)
                    .AppendLine(shippingInfo.Line2 ?? "")
                    .AppendLine(shippingInfo.Line3 ?? "")
                    .AppendLine(shippingInfo.city)
                    .AppendLine(shippingInfo.State ?? "")
                    .AppendLine(shippingInfo.Country)
                    .AppendLine(shippingInfo.Zip)
                    .AppendLine("-----------------------")
                    .AppendFormat("Подарочная упаковка: {0}", shippingInfo.GiftWrap ? "Yes" : "NO");

                MailMessage mailMessage = new MailMessage(
                    emailsettings.MailFromAdress,   //от
                    emailsettings.MailToAdress, //кому
                    "Новый заказ подтвержден!!!",   //тема
                    body.ToString() //тело
                    );


                if (emailsettings.WriteAsFile)
                {
                    mailMessage.BodyEncoding = Encoding.UTF8;

                }

                smtpClient.Send(mailMessage);

                    
                




            }
        }
    }
}
