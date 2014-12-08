using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace MFMElectronique.Models
{
    public class Courriel
    {
        private SmtpClient _smtp;
        private MailMessage _mcourriel;


        /// <summary>
        /// Auteur: Mathew Lemonde
        /// Description : Contructeur : permet d'initialiser les objets du Courriel et client smtp boum
        /// </summary>
        public Courriel(string Email, string SName)
        {
            MailAddress mEnvoyer = new MailAddress("MamanAHugo@gmail.com", "Maman de Hugo");
            MailAddress mReceveur = new MailAddress(Email, SName);


            _mcourriel = new MailMessage(mEnvoyer, mReceveur);

            _smtp = new SmtpClient(ConfigurationManager.AppSettings["AdresseSMTP"].ToString(), 587);

            _smtp.UseDefaultCredentials = false;

            _smtp.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["SMTPLogin"].ToString(), ConfigurationManager.AppSettings["SMTPPass"].ToString());
            _smtp.EnableSsl = true;

        }


        public void CourrielHTMLAsyncReset(string slink)
        {
            _mcourriel.Subject = "Récupération de Mot de Passe";
            _mcourriel.Body = "<HTML><BODY>Bonjour, <br /> Suite à une demande de votre part, voici le lien pour choisir un nouveau Mot de passe. <br /> <a href=" + slink + ">" + slink + "</a> <br /> Si vous n'avez pas fais cette demande, veuillez ignorer ce mail </BODY> </HTML>";
            _mcourriel.IsBodyHtml = true;


            _smtp.SendCompleted += _smtp_SendCompleted;
            //try
            //{
            //    await _smtp.SendMailAsync(_mcourriel);
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e.Message);
            //}
            _smtp.Send(_mcourriel);
        }

        public void CourrielHTMLAsyncValidate(string slink)
        {
            _mcourriel.Subject = "Validation d'email";
            _mcourriel.Body = "<HTML><BODY> Bonjour, <br /> suite à votre inscription sur le site, nous vous demandons de cliquer sur ce lien pour activer votre compte. <br /> <a href=" + slink + ">" + slink + "</a>  </BODY> </HTML>";
            _mcourriel.IsBodyHtml = true;
            _smtp.SendCompleted += _smtp_SendCompleted;

            _smtp.Send(_mcourriel);
        }

        public void CourrielHTMLModified(Order order)
        {
            _mcourriel.Subject = "Validation d'email";
            _mcourriel.Body = "<HTML><BODY> Bonjour, <br /> votre commande a été modifier. <br /> Le nouveau statut est : " + order.OrderStatut + "</" + "BODY> </HTML>";
            _mcourriel.IsBodyHtml = true;
            _smtp.SendCompleted += _smtp_SendCompleted;

            _smtp.Send(_mcourriel);
        }

        void _smtp_SendCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if (e.Cancelled)
                Console.WriteLine("Le courriel n'a pas été envoyé");
            else if (e.Error != null)
                Console.WriteLine("une erreur est survenue");
            else
                Console.WriteLine("Success");
        }

    }
}