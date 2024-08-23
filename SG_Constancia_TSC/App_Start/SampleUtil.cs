using System.Web;
using System.Net.Mail;
using System.Configuration;
using System;
using System.IO;


//using Glimpse.Core.Extensibility;

namespace SG_Constancia_TSC.App_Start
{
    public class SampleUtil
    {
        public static HttpContext Context
        {
            get
            {
                return HttpContext.Current;
            }
        }

        private static SmtpClient ConfigureSmtpClient()
        {
            var host = ConfigurationManager.AppSettings["SMTPNAME"];
            var username = ConfigurationManager.AppSettings["emailServiceUserName"];
            var password = ConfigurationManager.AppSettings["emailServicePassword"];
            var port = int.Parse(ConfigurationManager.AppSettings["SMTPPORT"]);

            SmtpClient smtp = new SmtpClient
            {
                Host = host,
                Port = port,
                EnableSsl = false,
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential(username, password),
                DeliveryMethod = SmtpDeliveryMethod.Network
            };

            return smtp;
        }

        private static MailMessage CreateMailMessage(string from, string to, string subject, string body, string cc = "", MemoryStream stream = null)
        {
            MailMessage message = new MailMessage
            {
                From = new MailAddress(from, "Tribunal Superior de Cuentas"),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            message.To.Add(new MailAddress(to));

            if (!string.IsNullOrEmpty(cc))
            {
                string[] ccId = cc.Split(',');
                foreach (string ccEmail in ccId)
                {
                    message.CC.Add(new MailAddress(ccEmail));
                }
            }

            if (stream != null)
            {
                stream.Seek(0, SeekOrigin.Begin);
                Attachment attachedDoc = new Attachment(stream, "Pre_Registro.pdf", "application/pdf");
                message.Attachments.Add(attachedDoc);
            }

            return message;
        }

        public static string GetFileDirectory()
        {
            // Dim dir As String = "C:\APPS WEB INTERNAS\APPS JHMEDINA\SDCE\Files" ' "C:\inetpub\wwwroot\PAGINA_WEB_TSC\SDCE\Files" 'Context.Server.MapPath("~/Files/")
            //string dir = @"C:\inetpub\wwwroot\APP_TSC\SISEDEC\Files\"; // Context.Server.MapPath("~/Files/")
            string dir = Context.Server.MapPath("~/Files/");
           

            if (!System.IO.Directory.Exists(dir))
                System.IO.Directory.CreateDirectory(dir);
            return dir;
        }

        public static bool EnviarCorreo3(MemoryStream stream, string Subject = "", string ToEmail = "", string Body = "", string cc = "", string attachmentName = "")
        {
            try
            {
                var from = ConfigurationManager.AppSettings["emailServiceUserName"];
                SmtpClient Smtp = ConfigureSmtpClient();

                // Create the main mail message
                MailMessage mainMessage = CreateMailMessage(from, ToEmail, Subject, Body, cc, stream, attachmentName);

                if (!string.IsNullOrEmpty(cc))
                {
                    mainMessage.CC.Add(new MailAddress(cc));
                    mainMessage.Body = "<br/><br/>Por favor vea el adjunto.";
                }
                //Send the email
                Smtp.Send(mainMessage);

                return true;
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                Console.WriteLine(ex.Message);
                return false; // Indicate that the email was not sent successfully
            }
        }

        private static MailMessage CreateMailMessage(string from, string to, string subject, string body, string cc, MemoryStream stream, string attachmentName)
        {
            MailMessage message = new MailMessage
            {
                From = new MailAddress(from, "Tribunal Superior de Cuentas"),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            message.To.Add(new MailAddress(to));

            if (!string.IsNullOrEmpty(cc))
            {
                string[] ccId = cc.Split(',');
                foreach (string ccEmail in ccId)
                {
                    message.CC.Add(new MailAddress(ccEmail));
                }
            }

            if (stream != null)
            {
                stream.Seek(0, SeekOrigin.Begin);
                Attachment attachedDoc = new Attachment(stream, attachmentName, "application/pdf");
                message.Attachments.Add(attachedDoc);
            }

            return message;
        }

        public static bool EnviarCorreo(string Desde, string Subject, string ToEmail, string Body)
        {
            _ = new SmtpClient("SMTPNAME", 2525)
            {
                Credentials = new System.Net.NetworkCredential("sistemastsc@tsc.gob.hn", "515+3m@5TSC1024@"),
                DeliveryMethod = SmtpDeliveryMethod.Network
            };
            _ = new MailMessage("sistemastsc@tsc.gob.hn", ToEmail)
            {
                Subject = Subject,
                Body = Body
            };
            return true;
        }

        public static bool EnviarCorreo1(string Desde = "", string Subject = "", string ToEmail = "", string Body = "")
        {
            if (Desde is null)
            {
                throw new ArgumentNullException(nameof(Desde));
            }

            string from = ConfigurationManager.AppSettings["emailServiceUserName"];
            string tto = ToEmail;
            string host = ConfigurationManager.AppSettings["SMTPNAME"];
            string username = ConfigurationManager.AppSettings["emailServiceUserName"];
            string password = ConfigurationManager.AppSettings["emailServicePassword"];
            MailMessage Message = new MailMessage();
            SmtpClient Smtp = new SmtpClient();
            System.Net.NetworkCredential SmtpUser = new System.Net.NetworkCredential();

            Message.From = new MailAddress(from, "SG_Constancia_TSC");
            Message.To.Add(new MailAddress(tto));
            Message.IsBodyHtml = true;

            Message.Subject = Subject;
            Message.Body = Body;
            SmtpUser.UserName = username;
            SmtpUser.Password = password;
            Smtp.EnableSsl = false;

            Smtp.UseDefaultCredentials = false;
            Smtp.Credentials = SmtpUser;
            Smtp.Host = host;
            Smtp.Port = int.Parse(ConfigurationManager.AppSettings["SMTPPORT"]);
            Smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            Smtp.Send(Message);
            return true;

        } 
        public static bool EnviarCorreo2(MemoryStream stream,string Desde = "", string Subject = "", string ToEmail = "", string Body = "")
        {
            if (Desde is null)
            {
                throw new ArgumentNullException(nameof(Desde));
            }

            

            string from = ConfigurationManager.AppSettings["emailServiceUserName"];
            string tto = ToEmail;
            string host = ConfigurationManager.AppSettings["SMTPNAME"];
            string username = ConfigurationManager.AppSettings["emailServiceUserName"];
            string password = ConfigurationManager.AppSettings["emailServicePassword"];
            MailMessage Message = new MailMessage();
            stream.Seek(0, System.IO.SeekOrigin.Begin);
            Attachment attachedDoc = new Attachment(stream, "Pre-Registro.pdf", "application/pdf");

            SmtpClient Smtp = new SmtpClient();
            System.Net.NetworkCredential SmtpUser = new System.Net.NetworkCredential();

            Message.From = new MailAddress(from, "SG_Constancia_TSC");
            Message.To.Add(new MailAddress(tto));
            Message.IsBodyHtml = true;
            Message.Attachments.Add(attachedDoc);

            Message.Subject = Subject;
            Message.Body = Body;
            SmtpUser.UserName = username;
            SmtpUser.Password = password;
            Smtp.EnableSsl = false;

            Smtp.UseDefaultCredentials = false;
            Smtp.Credentials = SmtpUser;
            Smtp.Host = host;
            Smtp.Port = int.Parse(ConfigurationManager.AppSettings["SMTPPORT"]);
            Smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            Smtp.Send(Message);
            return true;
        }


        public static bool EnviarCorreo(MemoryStream stream, string Subject = "", string ToEmail = "", string Body = "", string cc = "")
        {
            try
            {
                var from = ConfigurationManager.AppSettings["emailServiceUserName"];
                SmtpClient Smtp = ConfigureSmtpClient();
                MailMessage Message = CreateMailMessage(from, ToEmail, Subject, Body, cc, stream);
                Smtp.Send(Message);
                return true;
            }
            catch (Exception)
            {
                return false; // Indica que el correo no se envió con éxito
            }
        }


        private static string GetEmailBody(string token)
        {
            string templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CodigoVerificacion.html");
            string body = File.ReadAllText(templatePath);
            body = body.Replace("{{token}}", token);
            return body;
        }

        public static bool SendToken(string email, string token)
        {
            try
            {
                var from = ConfigurationManager.AppSettings["emailServiceUserName"];
                SmtpClient smtp = ConfigureSmtpClient();
                string body = GetEmailBody(token);

                MailMessage message = CreateMailMessage(from, email, "Su token de verificación", body);
                smtp.Send(message);
                return true;
            }
            catch (Exception)
            {
                return false; // Indica que el correo no se envió con éxito
            }
        }


        public static void SendEmails(string emailList)
        {
            if (string.IsNullOrWhiteSpace(emailList))
            {
                ErrorLogger.LogError(new ArgumentException("La lista de correos no debe ser nula o vacía."));
                return;
            }

            var emails = emailList.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            int maxRetryAttempts = 3;
            int delayBetweenRetries = 2000; // 2000 milliseconds (2 seconds)

            foreach (var email in emails)
            {
                int retryAttempt = 0;
                bool isSuccess = false;

                while (retryAttempt < maxRetryAttempts && !isSuccess)
                {
                    try
                    {
                        if (!SendNotification(email))
                        {
                            throw new InvalidOperationException($"Error al enviar el correo a {email}");
                        }
                        isSuccess = true; // If SendNotification succeeds, mark as success
                    }
                    catch (FormatException formatEx)
                    {
                        ErrorLogger.LogError(new Exception($"Formato de correo inválido: {email}", formatEx));
                        break; // Exit loop on FormatException as it won't succeed on retry
                    }
                    catch (SmtpException smtpEx)
                    {
                        retryAttempt++;
                        if (retryAttempt >= maxRetryAttempts)
                        {
                            ErrorLogger.LogError(new Exception($"Error SMTP al enviar el correo a {email} después de {maxRetryAttempts} intentos", smtpEx));
                        }
                        else
                        {
                            ErrorLogger.LogError(new Exception($"Error SMTP al enviar el correo a {email}. Reintentando... ({retryAttempt}/{maxRetryAttempts})", smtpEx));
                            System.Threading.Thread.Sleep(delayBetweenRetries); // Delay before retrying
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorLogger.LogError(new Exception($"Error desconocido al enviar el correo a {email}", ex));
                        break; // Exit loop on general exception
                    }
                }
            }
        }


        public static bool SendNotification(string email)
        {
            try
            {
                var from = ConfigurationManager.AppSettings["emailServiceUserName"];
                SmtpClient smtp = ConfigureSmtpClient();
                string body = GetBody();

                MailMessage message = CreateMailMessage(from, email, "Notificación de Creación Usuario", body, null, null, null);
                smtp.Send(message);
                return true;
            }
            catch (Exception)
            {
                return false; // Indica que el correo no se envió con éxito
            }
        }

        private static string GetBody()
        {
            string templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MensajeCorreo.html");
            string body = File.ReadAllText(templatePath);
            // Reemplaza cualquier marcador de posición en la plantilla con el mensaje real

            return body;
        }
    }
}