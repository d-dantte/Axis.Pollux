using Axis.Pollux.Common.Services;
using System;
using System.Linq;
using Axis.Luna.Operation;
using Axis.Pollux.Common.Models.Email;
using CAIRO.ElasticEmail;
using System.Net.Mail;
using Axis.Luna.Extensions;

namespace Axis.Pollux.Utils.ElasticEmail
{
    public class ElasticMailClient : IEmailClient
    {
        private readonly string ApiKey = null;
        public ElasticMailClient(string apiKey)
        {
            if (string.IsNullOrWhiteSpace(apiKey)) throw new Exception("invalid api key");

            ApiKey = apiKey;
        }

        public IOperation<EmailStatus> QueryStatus(Guid emailId)
        => LazyOp.Try(() =>
        {
            //var client = new ElasticemailWebApi(ApiKey);
            //var status = client.GetDeliveryStatus(emailId);

            return new NotSupportedException().Throw<EmailStatus>();
        });

        public IOperation<Guid?> Send(Payload payload)
        => LazyOp.Try(() =>
        {
            var msg = new ElasticemailMessage
            {
                Body = payload.Message.Body,
                IsBodyHtml = payload.Message.IsBodyHtml,
                From = new MailAddress(payload.Sender),
                Subject = payload.Message.Subject,
                ReplyTo = new MailAddress(payload.Sender)
            };
            msg.To.AddRange(payload.Recipients
                .Select(_r => new MailAddress(_r))
                .Concat(payload.CcRecipients
                .Select(_r => new MailAddress(_r))));

            payload.Message.Attachments
                .ToArray()
                .ForAll(_att => msg.Attachments.Add(_att.Name, _att.Data));

            var result = new ElasticemailWebApi(ApiKey).Send(msg);

            if (result.ResultType == ResultType.Success) return result.TransactionId;
            else throw new Exception(result.ErrorMessage ?? "Something went wrong while sending the email");
        });
    }
}
