using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using ESW19Backup2.Models;
using ESW19Backup2.Services;

namespace ESW19Backup2.Services
{
    public static class EmailSenderExtensions
    {
        public static Task SendEmailConfirmationAsync(this IEmailSender emailSender, string email, string link)
        {
            return emailSender.SendEmailAsync(email, "Confirm your email",
                $"Please confirm your account by clicking this link: <a href='{HtmlEncoder.Default.Encode(link)}'>link</a>");
        }

        public static Task SendEmailForgotPasswordAsync(this IEmailSender emailSender, string email, string link)
        {
            return emailSender.SendEmailAsync(email, "Recupera��o de Password",
                $"Recebemos o seu pedido de recupera��o de password. <br /> " +
                $"Pode proceder atrav�s do seguinte link: <a href='{HtmlEncoder.Default.Encode(link)}'>RECUPERAR PASSWORD</a>" +
                $"<br /><br />Caso n�o tenha feito nenhum pedido de recupera��o de password, ignore este e-mail");

        }

       
    }
}
