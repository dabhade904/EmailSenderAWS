using Amazon.SQS.Model;
using Amazon.SQS;
using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Mail;
using Amazon.Runtime;
using Amazon;

namespace AmazonSESSample
{
    class Program
    {
        public static AmazonSQSConfig SqsConfig { get; private set; }
        public static AmazonSQSClient SqsClient { get; private set; }
        public static async Task Main(string[] args)
        {
            // This address must be verified with Amazon SES.
            string FROM = "dabhade904@gmail.com";
            string FROMNAME = "Sachin Dabhade";
            string AWSAccessKey = "AKIA4X2OL4WZHFXTAUL3";
            string AWSSecretKey = "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";
            string AWSRegion = "ap-south-1";
            string path = "C:\\Users\\kamlesh\\Desktop\\ApplicationEmailSender\\EmailData.csv";
            string[] TO = System.IO.File.ReadAllLines(path);
            string SMTP_USERNAME = "AKIA4X2OL4WZE3YC5WVR";
            string SMTP_PASSWORD = "BEcvh2mOWW4+abPqlaH0GY5OPNQng5KRA/31lH9O+1rx";
            string HOST = "email-smtp.ap-south-1.amazonaws.com";
            string messageBody; ;

            BasicAWSCredentials creds = new BasicAWSCredentials(AWSAccessKey, AWSSecretKey);
            RegionEndpoint region = RegionEndpoint.GetBySystemName(AWSRegion);
            string queueUrl = "https://sqs.ap-south-1.amazonaws.com/875800815026/Myque";
            SqsClient = new AmazonSQSClient(creds, region);
            int PORT = 587;
            // The subject line of the email
            string SUBJECT ="Covid-19 Warning";
            // The body of the email
            string BODY =
                "<h1>Follow the Goverment instructions </h1>" +
                "<p>Stay at home,stay safe try to save our country .follow  social distancing </p>";

            // Create and build a new MailMessage object
            MailMessage message = new MailMessage();
            message.IsBodyHtml = true;
            message.From = new MailAddress(FROM, FROMNAME);
            Console.WriteLine("Attempting to send email...");

            foreach (String s in TO)
            {
                message.To.Add(new MailAddress(s));
                messageBody = "sent mail" + " to " + s;
                Console.WriteLine(messageBody);
                SendMessageRequest request = new SendMessageRequest
                {
                    MessageBody = messageBody,
                    QueueUrl = queueUrl,
                };
                await SqsClient.SendMessageAsync(request);
            }
            Console.WriteLine($"Successfully sent message in queue ");

            message.Subject = SUBJECT;
            message.Body = BODY;
        
            using (var client = new System.Net.Mail.SmtpClient(HOST, PORT))
            {
                // Pass SMTP credentials
                client.Credentials =new NetworkCredential(SMTP_USERNAME, SMTP_PASSWORD);
                client.EnableSsl = true;
                client.Send(message);
            }
        }
    }
}


/*using System;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace ApplicationEmailSender
{
    class Program
    {
        static void Main(string[] args)
        {
            String FROM = "dabhade904@gmail.com";
            String FROMNAME = "Sachin Dabhade";
            string path = "C:\\Users\\kamlesh\\Desktop\\ApplicationEmailSender\\EmailData.csv";
            String SMTP_USERNAME = "AKIA4X2OL4WZOQ2KVRXT";
            String SMTP_PASSWORD = "BA5777N/fWRZdEa9NnPZIa+Wes+9alVnjqgERRUc+QOn";
            String[] TO = System.IO.File.ReadAllLines(path);
            String HOST = "email-smtp.ap-south-1.amazonaws.com";
            int PORT = 587;

            String SUBJECT ="Amazon SES test (SMTP interface accessed using C#)";
            String BODY =
                "<h1>Amazon SES Test</h1>" +
                "<p>This email was sent through the " +
                "<a href='https://aws.amazon.com/ses'>Amazon SES</a> SMTP interface " +
                "using the .NET System.Net.Mail library.</p>";

            // Create and build a new MailMessage object
            MailMessage message = new MailMessage();
            message.IsBodyHtml = true;
            message.From = new MailAddress(FROM, FROMNAME);
            foreach(String s in TO)
            {
                message.To.Add(new MailAddress(s));
            }
            message.Subject = SUBJECT;
            message.Body = BODY;
          

            using (var client = new System.Net.Mail.SmtpClient(HOST, PORT))
            {
                // Pass SMTP credentials
                client.Credentials =
                    new NetworkCredential(SMTP_USERNAME, SMTP_PASSWORD);

                // Enable SSL encryption
                client.EnableSsl = true;

                // Try to send the message. Show status in console.
                try
                {
                    Console.WriteLine("Attempting to send email...");
                    client.Send(message);
                    Console.WriteLine("Email sent!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("The email was not sent.");
                    Console.WriteLine("Error message: " + ex.Message);
                }
            }
        }
    }
}*/