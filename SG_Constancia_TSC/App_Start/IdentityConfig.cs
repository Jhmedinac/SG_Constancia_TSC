using System;
using System.Configuration;
using System.Diagnostics;
using System.Net.Mail;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using SG_Constancia_TSC.Models;
using System.Text;
using System.Net.Mime;

namespace SG_Constancia_TSC
{
    // Servicio de correo 
    public class EmailService : IIdentityMessageService
    {
        public async Task SendAsync(IdentityMessage message)
        {
            var mensaje = new MailMessage();

            // Destinatario y remitente
            mensaje.To.Add(message.Destination);
            mensaje.From = new MailAddress(
                ConfigurationManager.AppSettings["emailServiceUserName"],
                "Tribunal Superior de Cuentas",
                Encoding.UTF8
            );

            var htmlBody = $@"
    <!DOCTYPE html>
    <html>
    <head>
        <meta charset=""UTF-8"">
        <title>{System.Net.WebUtility.HtmlEncode(message.Subject)}</title>
    </head>
    <body style=""font-family:Arial, Helvetica, sans-serif; font-size:14px; color:#333;"">
        {message.Body}
    </body>
    </html>";

            mensaje.Subject = message.Subject;
            mensaje.SubjectEncoding = Encoding.UTF8;
            mensaje.BodyEncoding = Encoding.UTF8;
            mensaje.HeadersEncoding = Encoding.UTF8;
            mensaje.IsBodyHtml = true;

            var htmlView = AlternateView.CreateAlternateViewFromString(
                htmlBody, Encoding.UTF8, MediaTypeNames.Text.Html
            );

            mensaje.AlternateViews.Clear();
            mensaje.AlternateViews.Add(htmlView);

            using (var smtp = new SmtpClient())
            {
                var credentials = new NetworkCredential(
                    ConfigurationManager.AppSettings["emailServiceUserName"],
                    ConfigurationManager.AppSettings["emailServicePassword"]
                );

                smtp.Host = ConfigurationManager.AppSettings["SMTPNAME"];
                smtp.Port = int.Parse(ConfigurationManager.AppSettings["SMTPPORT"]);
                smtp.EnableSsl = false; // ajusta si tu servidor requiere SSL
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = credentials;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                await smtp.SendMailAsync(mensaje);
            }
        }
    }

    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
           
            return Task.FromResult(0);
        }
    }

    

    public class PasswordValidatorEs : PasswordValidator
    {
        public override Task<IdentityResult> ValidateAsync(string password)
        {
            var errors = new System.Collections.Generic.List<string>();

            if (string.IsNullOrEmpty(password) || password.Length < RequiredLength)
            {
                errors.Add($"La contraseña debe tener al menos {RequiredLength} caracteres.");
            }

            if (RequireNonLetterOrDigit && password != null && password.All(char.IsLetterOrDigit))
            {
                errors.Add("La contraseña debe contener al menos un carácter especial.");
            }

            if (RequireDigit && (password == null || !password.Any(char.IsDigit)))
            {
                errors.Add("La contraseña debe contener al menos un dígito ('0'-'9').");
            }

            if (RequireLowercase && (password == null || !password.Any(char.IsLower)))
            {
                errors.Add("La contraseña debe contener al menos una letra minúscula ('a'-'z').");
            }

            if (RequireUppercase && (password == null || !password.Any(char.IsUpper)))
            {
                errors.Add("La contraseña debe contener al menos una letra mayúscula ('A'-'Z').");
            }

            if (errors.Any())
                return Task.FromResult(IdentityResult.Failed(errors.ToArray()));

            return Task.FromResult(IdentityResult.Success);
        }
    }

   
    public class UserValidatorEs : UserValidator<ApplicationUser>
    {
        public UserValidatorEs(UserManager<ApplicationUser> manager) : base(manager) { }

        public override async Task<IdentityResult> ValidateAsync(ApplicationUser user)
        {
            var baseResult = await base.ValidateAsync(user);
            if (baseResult.Succeeded) return baseResult;

            var spanish = baseResult.Errors.Select(e =>
            {
                var lower = (e ?? string.Empty).ToLowerInvariant();

                if (lower.Contains("is already taken") || lower.Contains("already taken") || lower.Contains("already exists"))
                    return "El nombre de usuario o correo electrónico ya está en uso.";

                if (lower.Contains("is not a valid email") || lower.Contains("is invalid"))
                    return "El correo electrónico no es válido.";

                if (lower.Contains("is invalid") || lower.Contains("is not a valid"))
                    return "El nombre de usuario no es válido.";

                return e;
            }).ToArray();

            return IdentityResult.Failed(spanish);
        }
    }

    // ----------------- ApplicationUserManager -----------------
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>()));

           
            manager.UserValidator = new UserValidatorEs(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            manager.PasswordValidator = new PasswordValidatorEs
            {
                RequiredLength = 6, 
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<ApplicationUser>
            {
                MessageFormat = "Su código de seguridad es {0}"
            });
            manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<ApplicationUser>
            {
                Subject = "Código de seguridad",
                BodyFormat = "Su código de seguridad es {0}"
            });

            // Lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();

            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }

            return manager;
        }

        
        private IdentityResult TraducirIdentityResult(IdentityResult result)
        {
            if (result == null) return IdentityResult.Failed(new[] { "Error desconocido." });
            if (result.Succeeded) return result;

            var traducidos = result.Errors.Select(e =>
            {
                if (string.IsNullOrEmpty(e)) return "Error desconocido.";
                var lower = e.ToLowerInvariant();

                if (lower.Contains("passwords must have at least")) return "La contraseña no cumple los requisitos mínimos.";
                if (lower.Contains("at least one non letter or digit")) return "La contraseña debe contener al menos un carácter especial.";
                if (lower.Contains("at least one uppercase")) return "La contraseña debe contener al menos una letra mayúscula.";
                if (lower.Contains("at least one digit")) return "La contraseña debe contener al menos un dígito.";
                if (lower.Contains("is already taken") || lower.Contains("already taken") || lower.Contains("already exists")) return "El nombre de usuario o correo electrónico ya está en uso.";
                if (lower.Contains("is not a valid email") || lower.Contains("is invalid")) return "El correo electrónico no es válido.";

                return e;
            }).ToArray();

            return IdentityResult.Failed(traducidos);
        }

        // Overrides comunes — si tu versión de UserManager los permite
        public override async Task<IdentityResult> CreateAsync(ApplicationUser user)
        {
            var baseResult = await base.CreateAsync(user);
            return TraducirIdentityResult(baseResult);
        }

        public override async Task<IdentityResult> ChangePasswordAsync(string userId, string currentPassword, string newPassword)
        {
            var baseResult = await base.ChangePasswordAsync(userId, currentPassword, newPassword);
            return TraducirIdentityResult(baseResult);
        }

        public override async Task<IdentityResult> AddPasswordAsync(string userId, string password)
        {
            var baseResult = await base.AddPasswordAsync(userId, password);
            return TraducirIdentityResult(baseResult);
        }

        public override async Task<IdentityResult> ResetPasswordAsync(string userId, string token, string newPassword)
        {
            var baseResult = await base.ResetPasswordAsync(userId, token, newPassword);
            return TraducirIdentityResult(baseResult);
        }

        public override async Task<IdentityResult> UpdateAsync(ApplicationUser user)
        {
            var baseResult = await base.UpdateAsync(user);
            return TraducirIdentityResult(baseResult);
        }
    }

    // ----------------- SignInManager -----------------
    public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager) :
            base(userManager, authenticationManager)
        { }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }
}